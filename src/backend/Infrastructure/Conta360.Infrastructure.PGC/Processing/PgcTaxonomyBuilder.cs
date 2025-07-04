using Conta360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks; // ¡Nuevo using necesario!
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
        // ¡El método principal ahora es ASÍNCRONO!
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

            // ¡Ahora usamos 'await' para llamar a los métodos asíncronos!
            var codes = await ParseCodesFromXsd(xsdPath);
            var labels = await ParseLabels(labelPath);
            var hierarchy = await ParseHierarchy(presentationPath);
            var levels = CalculateLevelsFromHierarchy(hierarchy); // Este sigue siendo síncrono, no hace E/S

            var accounts = new List<PgcAccount>();

            // Paso 1: Crear cuentas con Id asignado
            foreach (var code in codes)
            {
                var name = labels.TryGetValue(code, out var label) ? label : code;
                var parentCode = hierarchy.TryGetValue(code, out var parent) ? parent : null;
                var level = levels.TryGetValue(code, out var lvl) ? lvl : 1;

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
                        account.ParentId = parent.Id;
                }
            }

            // Paso 3: Retornar las cuentas.
            return accounts;
        }

        // Método ahora ASÍNCRONO
        private async Task<List<string>> ParseCodesFromXsd(string xsdPath)
        {
            var codes = new List<string>();
            // Usamos FileStream para abrir de forma asíncrona y luego XmlReader.Create
            // XmlSchema.Read no tiene una versión async directa, así que la lectura de esquema
            // se hará síncrona sobre el stream ya abierto asíncronamente.
            await using (var fs = new FileStream(xsdPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var reader = XmlReader.Create(fs, new XmlReaderSettings { Async = true })) // Es importante configurar Async = true
            {
                var schema = XmlSchema.Read(reader, null); // Esta lectura es síncrona
                if (schema == null)
                    throw new InvalidDataException($"No se pudo leer el esquema XSD: {xsdPath}");

                foreach (var item in schema.Items)
                {
                    if (item is XmlSchemaElement element && !string.IsNullOrWhiteSpace(element.Name))
                    {
                        var code = element.Name.Trim();
                        // Solo códigos numéricos típicos de cuentas
                        if (int.TryParse(code, out _))
                            codes.Add(code);
                    }
                }
            }
            return codes;
        }

        // Método ahora ASÍNCRONO
        private async Task<Dictionary<string, string>> ParseLabels(string labelPath)
        {
            var dict = new Dictionary<string, string>();
            // XDocument.Load no tiene un LoadAsync directo.
            // La forma más simple es leer todo el contenido del archivo asíncronamente
            // y luego parsear la cadena síncronamente.
            var xmlContent = await File.ReadAllTextAsync(labelPath);
            var doc = XDocument.Parse(xmlContent); // Parseo de la cadena es síncrono

            XNamespace xlink = "http://www.w3.org/1999/xlink";

            foreach (var labelElem in doc.Descendants().Where(e => e.Name.LocalName == "label"))
            {
                var xlinkLabel = labelElem.Attribute(xlink + "label")?.Value ?? "";
                if (xlinkLabel.StartsWith("label_"))
                {
                    var code = xlinkLabel.Substring("label_".Length);
                    var text = labelElem.Value.Trim();
                    if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(text))
                        dict[code] = text;
                }
            }
            return dict;
        }

        /// <summary>
        /// Devuelve un diccionario código->ParentCode según el árbol de presentation.xml.
        /// </summary>
        // Método ahora ASÍNCRONO
        private async Task<Dictionary<string, string?>> ParseHierarchy(string presentationPath)
        {
            // Similar a ParseLabels, leemos el contenido asíncronamente.
            var xmlContent = await File.ReadAllTextAsync(presentationPath);
            var doc = XDocument.Parse(xmlContent); // Parseo de la cadena es síncrono

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
                if (!locators.TryGetValue(arc.ParentLabel!, out var parentCode)) continue;
                if (!locators.TryGetValue(arc.ChildLabel!, out var childCode)) continue;

                // Solo añade si es un código numérico típico de cuenta
                if (!int.TryParse(childCode, out _)) continue;
                // Si el parentCode no es numérico, lo tratamos como nulo
                if (!int.TryParse(parentCode, out _)) parentCode = null;
                
                // Añadir al diccionario, sobrescribiendo si ya existe (el último arc gana si hay duplicados).
                childToParent[childCode] = parentCode;
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

            foreach (string code in hierarchy.Keys.Concat(hierarchy.Values.Where(v => v != null)).Distinct().Cast<string>())
            {
                if (levels.ContainsKey(code)) continue;

                var currentCode = code;
                var path = new Stack<string>();
                var visitedInPath = new HashSet<string>();

                while (!string.IsNullOrEmpty(currentCode) && !levels.ContainsKey(currentCode) && !visitedInPath.Contains(currentCode))
                {
                    path.Push(currentCode);
                    visitedInPath.Add(currentCode);

                    if (!hierarchy.TryGetValue(currentCode, out var parent))
                    {
                        parent = null;
                    }
                    currentCode = parent;
                }

                int baseLevel = 0;
                if (currentCode == null)
                {
                    baseLevel = 0;
                }
                else if (currentCode != null && levels.ContainsKey(currentCode))
                {
                    baseLevel = levels[currentCode];
                }
                else if (currentCode != null && visitedInPath.Contains(currentCode))
                {
                    baseLevel = 1;
                }

                while (path.Any())
                {
                    var node = path.Pop();
                    if (!string.IsNullOrEmpty(node) && !levels.ContainsKey(node))
                    {
                        levels[node] = baseLevel + path.Count + 1;
                    }
                }
            }
            return levels;
        }
    }
}