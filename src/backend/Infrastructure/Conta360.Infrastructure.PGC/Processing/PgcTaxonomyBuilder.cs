using Conta360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Conta360.Infrastructure.PGC.Processing
{
    public class PgcTaxonomyBuilder
    {
        public PgcTaxonomyBuilder()
        {
            // Constructor vacío, sin dependencias de persistencia directa.
        }

        /// <summary>
        /// Importa cuentas del PGC con jerarquía (ParentCode y Level) usando XSD, label-es.xml y presentation.xml.
        /// Devuelve la lista de cuentas en lugar de persistirlas directamente.
        /// </summary>
        public async Task<List<PgcAccount>> BuildAccountsFromXsdLabelPresentation(
            string xsdPath,
            string labelPath,
            string presentationPath)
        {
            // Validaciones robustas: Aseguramos que los archivos existan antes de intentar leerlos.
            // Las verificaciones File.Exists no tienen una versión async, así que se mantienen síncronas.
            if (!File.Exists(xsdPath))
            {
                throw new FileNotFoundException($"El archivo XSD no fue encontrado: {xsdPath}");
            }
            if (!File.Exists(labelPath))
            {
                throw new FileNotFoundException($"El archivo de etiquetas no fue encontrado: {labelPath}");
            }
            if (!File.Exists(presentationPath))
            {
                throw new FileNotFoundException($"El archivo de presentación no fue encontrado: {presentationPath}");
            }

            var codes = await ParseCodesFromXsd(xsdPath);
            var labels = await ParseLabels(labelPath);
            var hierarchy = await ParseHierarchy(presentationPath);
            var levels = CalculateLevelsFromHierarchy(hierarchy); // Este sigue siendo síncrono, no hace E/S

            var accounts = new List<PgcAccount>();

            // Paso 1: Crear cuentas con Id asignado
            foreach (var code in codes)
            {
                // TryGetValue devuelve un string?
                labels.TryGetValue(code, out var label);
                var name = string.IsNullOrWhiteSpace(label) ? code : label;

                // TryGetValue devuelve un string?
                hierarchy.TryGetValue(code, out var parent);
                var parentCode = parent; // Puede ser null

                levels.TryGetValue(code, out var lvl); // TryGetValue devuelve el valor predeterminado (0 para int) si no se encuentra
                // Asignar 1 si no se encuentra en 'levels' (no tiene nivel calculado, ergo es raíz o aislado).
                // Si levels.ContainsKey(code) es true pero lvl es 0, significa que su nivel es 0, lo cual es inusual si la lógica es raíz=1.
                // Sin embargo, si la lógica original es 0 para raíces y luego se incrementa, se mantiene.
                // Pero, dado que se busca baseLevel + path.Count + 1, un 0 inicial resultaría en 1 para las raíces.
                // Así que `lvl` ya debería ser correcto o `0` si no se encontró y luego se corrige.
                // Simplificando: si no está en levels, su nivel inicial es 1.
                var level = levels.ContainsKey(code) ? lvl : 1; 


                accounts.Add(new PgcAccount
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = name,
                    ParentCode = parentCode,
                    Level = level,
                    IsMovable = level >= 3 // Lógica de ejemplo para IsMovable
                });
            }

            // Paso 2: Asignar ParentId usando ParentCode
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account.ParentCode))
                {
                    var parent = accounts.FirstOrDefault(a => a.Code == account.ParentCode);
                    if (parent != null)
                    {
                        account.ParentId = parent.Id;
                    }
                }
            }

            // Paso 3: Retornar las cuentas.
            return accounts;
        }

        private async Task<List<string>> ParseCodesFromXsd(string xsdPath)
        {
            var codes = new List<string>();
            await using (var fs = new FileStream(xsdPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var reader = XmlReader.Create(fs, new XmlReaderSettings { Async = true })) 
            {
                // Esta lectura es síncrona. No hay una versión asíncrona de XmlSchema.Read.
                var schema = XmlSchema.Read(reader, null); 
                if (schema == null)
                    throw new InvalidDataException($"No se pudo leer el esquema XSD: {xsdPath}");

                foreach (var item in schema.Items)
                {
                    if (item is XmlSchemaElement element)
                    {
                        if (!string.IsNullOrWhiteSpace(element.Name))
                        {
                            var code = element.Name.Trim();
                            // Asume que los códigos son siempre numéricos.
                            if (int.TryParse(code, out _))
                            {
                                codes.Add(code);
                            }
                        }
                    }
                }
            }
            return codes;
        }

        private async Task<Dictionary<string, string>> ParseLabels(string labelPath)
        {
            var dict = new Dictionary<string, string>();
            var xmlContent = await File.ReadAllTextAsync(labelPath);
            var doc = XDocument.Parse(xmlContent); // El parseo de la cadena es síncrono

            XNamespace xlink = "http://www.w3.org/1999/xlink";

            foreach (var labelElem in doc.Descendants().Where(e => e.Name.LocalName == "label"))
            {
                string? xlinkLabel = labelElem.Attribute(xlink + "label")?.Value;
                
                if (!string.IsNullOrWhiteSpace(xlinkLabel) && xlinkLabel.StartsWith("label_", StringComparison.OrdinalIgnoreCase))
                {
                    var code = xlinkLabel.Substring("label_".Length);
                    var text = labelElem.Value.Trim();
                    if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(text))
                    {
                        dict[code] = text;
                    }
                }
            }
            return dict;
        }

        /// <summary>
        /// Devuelve un diccionario código->ParentCode según el árbol de presentation.xml.
        /// </summary>
        private async Task<Dictionary<string, string?>> ParseHierarchy(string presentationPath)
        {
            var xmlContent = await File.ReadAllTextAsync(presentationPath);
            var doc = XDocument.Parse(xmlContent); // El parseo de la cadena es síncrono

            XNamespace xlink = "http://www.w3.org/1999/xlink";

            // 1. Relaciona label->código XSD (usando xlink:href)
            var locators = doc.Descendants()
                .Where(e => e.Name.LocalName == "loc")
                .Select(e => new
                {
                    Label = e.Attribute(xlink + "label")?.Value,
                    Code = e.Attribute(xlink + "href")?.Value?.Split('#').Last()
                })
                .Where(x => !string.IsNullOrEmpty(x.Label) && !string.IsNullOrEmpty(x.Code))
                .ToDictionary(x => x.Label!, x => x.Code!); 

            // 2. Lee los arcs padre-hijo
            var arcs = doc.Descendants()
                .Where(e => e.Name.LocalName == "presentationArc")
                .Select(e => new
                {
                    ParentLabel = e.Attribute(xlink + "from")?.Value,
                    ChildLabel = e.Attribute(xlink + "to")?.Value
                })
                .Where(x => !string.IsNullOrEmpty(x.ParentLabel) && !string.IsNullOrEmpty(x.ChildLabel))
                .ToList();

            // 3. Construye diccionario código->ParentCode
            var childToParent = new Dictionary<string, string?>();

            foreach (var arc in arcs)
            {
                // Usamos TryGetValue y comprobamos el resultado para evitar nulidad.
                if (!locators.TryGetValue(arc.ParentLabel!, out string? parentLabelCode) || parentLabelCode == null) continue;
                if (!locators.TryGetValue(arc.ChildLabel!, out string? childLabelCode) || childLabelCode == null) continue;

                string parentCode = parentLabelCode; 
                string childCode = childLabelCode;   

                if (!int.TryParse(childCode, out _)) continue;

                string? finalParentCode = null;
                // Solo si el ParentCode es numérico lo consideramos válido.
                if (int.TryParse(parentCode, out _))
                {
                    finalParentCode = parentCode;
                }
                
                childToParent[childCode] = finalParentCode;
            }

            return childToParent;
        }

        /// <summary>
        /// Calcula el nivel jerárquico de cada código (raíz=1).
        /// Implementación más robusta para evitar ciclos y garantizar el cálculo para todos los nodos.
        /// Este método no realiza E/S, por lo que permanece síncrono.
        /// </summary>
        private Dictionary<string, int> CalculateLevelsFromHierarchy(Dictionary<string, string?> hierarchy)
        {
            var levels = new Dictionary<string, int>();

            // Filtrar los valores nulos directamente en la enumeración
            var allNodes = hierarchy.Keys
                                    .Concat(hierarchy.Values.Where(v => v != null).Select(v => v!)) 
                                    .Distinct()
                                    .ToList();

            foreach (string code in allNodes) 
            {
                // Si el nodo ya ha sido procesado (su nivel ya está calculado), saltar.
                if (levels.ContainsKey(code)) continue;

                string? currentCode = code;
                var path = new Stack<string>();
                var visitedInPath = new HashSet<string>(); // Detectar ciclos en la ruta actual

                while (currentCode != null && !levels.ContainsKey(currentCode) && !visitedInPath.Contains(currentCode))
                {
                    path.Push(currentCode);
                    visitedInPath.Add(currentCode);

                    // Obtener el padre. Si no existe en la jerarquía, es una raíz o un nodo aislado.
                    if (!hierarchy.TryGetValue(currentCode, out string? parent))
                    {
                        parent = null; 
                    }
                    currentCode = parent;
                }

                int baseLevel = 0;
                if (currentCode == null)
                {
                    // Si llegamos a un 'null' significa que es una raíz de la jerarquía (o un nodo sin padre).
                    baseLevel = 0; 
                }
                else if (levels.TryGetValue(currentCode, out int existingLevel))
                {
                    // Si ya calculamos el nivel de un nodo superior, usamos ese nivel como base.
                    baseLevel = existingLevel;
                }
                else 
                {
                    // Esto indica un ciclo o un nodo aislado no conectado a una raíz ya procesada. 
                    // Para evitar bucles infinitos en el cálculo, asumimos un nivel base de 1.
                    baseLevel = 1; 
                }

                while (path.Any())
                {
                    var node = path.Pop(); 
                    if (!levels.ContainsKey(node)) 
                    {
                        // El nivel es el nivel base + la cantidad de nodos restantes en el camino (que son sus descendientes) + 1 (el propio nodo).
                        levels[node] = baseLevel + path.Count + 1;
                    }
                }
            }
            return levels;
        }
    }
}