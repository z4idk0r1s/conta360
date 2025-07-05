using Conta360.Domain.Entities;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PgcTaxonomyBuilder> _logger;

        public PgcTaxonomyBuilder(ILogger<PgcTaxonomyBuilder> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Construye una lista de objetos PgcAccount a partir de los archivos de taxonomía XBRL (XSD, Label, Presentation).
        /// </summary>
        /// <param name="xsdPath">Ruta al archivo XSD principal de la taxonomía (e.g., pgc07-normal.xsd).</param>
        /// <param name="labelPath">Ruta al archivo XBRL Label Linkbase (.lbl) que contiene los nombres de las cuentas.</param>
        /// <param name="presentationPath">Ruta al archivo XBRL Presentation Linkbase (.pre) que define la jerarquía padre-hijo de las cuentas. Puede ser nulo.</param>
        /// <returns>Una lista de objetos PgcAccount con los códigos, nombres, jerarquía y niveles calculados.</returns>
        /// <exception cref="FileNotFoundException">Se lanza si alguno de los archivos obligatorios no se encuentra.</exception>
        /// <exception cref="InvalidDataException">Se lanza si el esquema XSD no se puede leer.</exception>
        public async Task<List<PgcAccount>> BuildAccountsFromXsdLabelPresentation(
            string xsdPath,
            string labelPath,
            string? presentationPath)
        {
            if (!File.Exists(xsdPath))
            {
                _logger.LogError("[PgcTaxonomyBuilder] El archivo XSD no fue encontrado: {XsdPath}", xsdPath);
                throw new FileNotFoundException($"El archivo XSD no fue encontrado: {xsdPath}");
            }
            if (!File.Exists(labelPath))
            {
                _logger.LogError("[PgcTaxonomyBuilder] El archivo de etiquetas no fue encontrado: {LabelPath}", labelPath);
                throw new FileNotFoundException($"El archivo de etiquetas no fue encontrado: {labelPath}");
            }

            // Paso 1: Parsear los códigos de las cuentas desde el XSD
            var codes = await ParseCodesFromXsd(xsdPath);
            // Paso 2: Parsear las etiquetas (nombres) de las cuentas
            var labels = await ParseLabels(labelPath);

            // Paso 3: Parsear la jerarquía padre-hijo (opcional)
            Dictionary<string, string?> hierarchy;
            if (presentationPath != null && File.Exists(presentationPath))
            {
                hierarchy = await ParseHierarchy(presentationPath);
            }
            else
            {
                _logger.LogInformation("[PgcTaxonomyBuilder] No se proporcionó o no se encontró archivo de presentación en '{PresentationPath}'. La jerarquía se calculará sin relaciones padre-hijo del presentation linkbase.", presentationPath ?? "NULO");
                hierarchy = new Dictionary<string, string?>();
            }

            // Paso 4: Calcular los niveles jerárquicos de las cuentas
            var levels = CalculateLevelsFromHierarchy(hierarchy);

            var accounts = new List<PgcAccount>();

            // Paso 5: Construir los objetos PgcAccount
            foreach (var code in codes)
            {
                labels.TryGetValue(code, out var label);
                var name = string.IsNullOrWhiteSpace(label) ? code : label;

                hierarchy.TryGetValue(code, out var parent);
                var parentCode = parent;

                levels.TryGetValue(code, out var lvl);
                var level = levels.ContainsKey(code) ? lvl : 1; // Asigna un nivel predeterminado si no se encuentra en la jerarquía

                accounts.Add(new PgcAccount
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = name,
                    ParentCode = parentCode,
                    Level = level,
                    IsMovable = level >= 3 // Lógica definida para IsMovable (ej. a partir del nivel 3)
                });
            }

            // Paso 6: Asignar los ParentId una vez que todas las cuentas han sido creadas
            foreach (var account in accounts)
            {
                if (!string.IsNullOrEmpty(account.ParentCode))
                {
                    var parent = accounts.FirstOrDefault(a => a.Code == account.ParentCode);
                    if (parent != null)
                    {
                        account.ParentId = parent.Id;
                    }
                    else
                    {
                        _logger.LogWarning("[PgcTaxonomyBuilder] Cuenta padre con código '{ParentCode}' no encontrada para la cuenta '{ChildCode}'. Esto puede indicar un problema en la jerarquía o conceptos no definidos.", account.ParentCode, account.Code);
                    }
                }
            }

            _logger.LogInformation("[PgcTaxonomyBuilder] Se construyeron {Count} cuentas del PGC.", accounts.Count);
            return accounts;
        }

        /// <summary>
        /// Parsea un archivo XSD para extraer los códigos de los elementos que son conceptos XBRL (sustituyen a xbrli:item).
        /// </summary>
        /// <param name="xsdPath">Ruta al archivo XSD.</param>
        /// <returns>Una lista de cadenas con los nombres de los elementos (códigos de cuenta).</returns>
        private async Task<List<string>> ParseCodesFromXsd(string xsdPath)
        {
            var codes = new HashSet<string>();
            // Define el XmlQualifiedName para xbrli:item.
            // Es CRÍTICO que este namespace sea EXACTO al usado en los XSD de la taxonomía XBRL.
            // Según tu fragmento, xbrli apunta a "http://www.xbrl.org/2003/instance".
            var xbrliItemQName = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

            try
            {
                var settings = new XmlReaderSettings
                {
                    XmlResolver = new XmlUrlResolver(), // Permite resolver importaciones/inclusiones de XSD locales o remotas
                    DtdProcessing = DtdProcessing.Parse, // Necesario para procesar DTD si existen en el XSD
                    ValidationType = ValidationType.Schema // Habilita la validación contra el esquema
                };

                XmlSchema? mainSchema;
                using (var reader = XmlReader.Create(xsdPath, settings))
                {
                    mainSchema = await Task.Run(() => XmlSchema.Read(reader, (sender, e) =>
                    {
                        // Callback para capturar errores y advertencias durante la lectura del esquema
                        if (e.Severity == XmlSeverityType.Error)
                        {
                            _logger.LogError("[ParseCodesFromXsd] Error de validación del XSD '{XsdPath}': {Message}", xsdPath, e.Message);
                        }
                        else
                        {
                            _logger.LogWarning("[ParseCodesFromXsd] Advertencia de validación del XSD '{XsdPath}': {Message}", xsdPath, e.Message);
                        }
                    }));
                }

                if (mainSchema == null)
                    throw new InvalidDataException($"No se pudo leer el esquema XSD: {xsdPath}");

                var schemaSet = new XmlSchemaSet();
                // Añade el esquema principal. Esto instruye a XmlSchemaSet a resolver todas sus importaciones e inclusiones.
                schemaSet.Add(mainSchema);
                // Compila el schemaSet. Este paso es crucial para que todos los esquemas importados y sus elementos sean accesibles.
                schemaSet.Compile();

                _logger.LogInformation("[ParseCodesFromXsd] Número de esquemas resueltos en SchemaSet: {Count}", schemaSet.Count);

                // Itera sobre TODOS los esquemas resueltos y cargados en el SchemaSet.
                // Esto incluye el esquema principal y todos los esquemas que importa (directa o indirectamente).
                foreach (XmlSchema s in schemaSet.Schemas())
                {
                    _logger.LogInformation("[ParseCodesFromXsd] Procesando elementos del esquema con TargetNamespace: {TargetNamespace}", s.TargetNamespace);
                    // Itera sobre todos los elementos globales definidos en el esquema actual.
                    foreach (XmlSchemaElement element in s.Elements.Values)
                    {
                        // Comprueba si el elemento es un concepto XBRL (es decir, su substitutionGroup es 'xbrli:item').
                        // Este es el filtro clave para identificar las cuentas contables.
                        if (element.SubstitutionGroup != null && element.SubstitutionGroup.Equals(xbrliItemQName))
                        {
                            if (!string.IsNullOrWhiteSpace(element.Name))
                            {
                                codes.Add(element.Name.Trim());
                                // Opcional: Para depuración, puedes habilitar este log en el nivel Debug
                                // _logger.LogDebug("[ParseCodesFromXsd] Código encontrado: {Code}", element.Name.Trim());
                            }
                        }
                        // Opcional: Para depuración, puedes loggear elementos que no son xbrli:item
                        // else
                        // {
                        //     _logger.LogDebug("[ParseCodesFromXsd] Elemento '{Name}' (Namespace: '{Namespace}') NO es xbrli:item. SubstitutionGroup: '{SubGroup}'",
                        //         element.Name, element.QualifiedName.Namespace, element.SubstitutionGroup?.ToString() ?? "N/A");
                        // }
                    }
                }
            }
            catch (XmlSchemaException ex)
            {
                // Captura errores específicos de validación de esquemas XML
                _logger.LogError(ex, "[ParseCodesFromXsd] Error de esquema XML al parsear XSD '{XsdPath}': {Message}", xsdPath, ex.Message);
                throw; // Relanza la excepción para que el llamador la maneje
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error inesperado
                _logger.LogError(ex, "[ParseCodesFromXsd] Error inesperado al parsear XSD '{XsdPath}': {Message}", xsdPath, ex.Message);
                throw; // Relanza la excepción
            }

            _logger.LogInformation("[ParseCodesFromXsd] Se encontraron {Count} códigos únicos en el XSD completo '{XsdPath}'.", codes.Count, xsdPath);
            return codes.ToList();
        }

        /// <summary>
        /// Parsea un archivo XBRL Label Linkbase (.lbl) para extraer las etiquetas (nombres) de los conceptos.
        /// </summary>
        /// <param name="labelPath">Ruta al archivo Label Linkbase.</param>
        /// <returns>Un diccionario donde la clave es el código del concepto y el valor es su etiqueta.</returns>
        private async Task<Dictionary<string, string>> ParseLabels(string labelPath)
        {
            var dict = new Dictionary<string, string>();
            try
            {
                var xmlContent = await File.ReadAllTextAsync(labelPath);
                var doc = XDocument.Parse(xmlContent);

                // Define el namespace XLink, que es común en los linkbase XBRL
                XNamespace xlink = "http://www.w3.org/1999/xlink";

                // Busca todos los elementos 'label' en el documento
                foreach (var labelElem in doc.Descendants().Where(e => e.Name.LocalName == "label"))
                {
                    // Extrae el valor del atributo 'xlink:label', que se usa para enlazar con el concepto
                    string? xlinkLabel = labelElem.Attribute(xlink + "label")?.Value;

                    // Verifica que el label exista y comience con "label_" (convención común en taxonomías XBRL)
                    if (!string.IsNullOrWhiteSpace(xlinkLabel) && xlinkLabel.StartsWith("label_", StringComparison.OrdinalIgnoreCase))
                    {
                        // El código de la cuenta se extrae después de "label_"
                        var code = xlinkLabel.Substring("label_".Length);
                        var text = labelElem.Value.Trim(); // El texto del elemento es la etiqueta de la cuenta

                        if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(text))
                        {
                            dict[code] = text; // Almacena el código y su etiqueta
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ParseLabels] Error al parsear archivo de etiquetas '{LabelPath}': {Message}", labelPath, ex.Message);
                throw;
            }
            _logger.LogInformation("[ParseLabels] Se encontraron {Count} etiquetas en el archivo '{LabelPath}'.", dict.Count, labelPath);
            return dict;
        }

        /// <summary>
        /// Parsea un archivo XBRL Presentation Linkbase (.pre) para extraer las relaciones padre-hijo de la jerarquía de cuentas.
        /// </summary>
        /// <param name="presentationPath">Ruta al archivo Presentation Linkbase.</param>
        /// <returns>Un diccionario donde la clave es el código del hijo y el valor es el código del padre.</returns>
        private async Task<Dictionary<string, string?>> ParseHierarchy(string presentationPath)
        {
            var childToParent = new Dictionary<string, string?>();
            try
            {
                var xmlContent = await File.ReadAllTextAsync(presentationPath);
                var doc = XDocument.Parse(xmlContent);

                XNamespace xlink = "http://www.w3.org/1999/xlink";

                // Paso 1: Mapear los 'locators' (nodos que enlazan etiquetas a conceptos XSD)
                var locators = doc.Descendants()
                    .Where(e => e.Name.LocalName == "loc") // Busca elementos <loc>
                    .Select(e => new
                    {
                        Label = e.Attribute(xlink + "label")?.Value, // Atributo xlink:label (ej., 'concepto_activo')
                        Code = e.Attribute(xlink + "href")?.Value?.Split('#').Last() // Atributo xlink:href (ej., 'pgc07-n-m1-balance.xsd#ActivoCorrienteTotal')
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Label) && !string.IsNullOrEmpty(x.Code))
                    .ToDictionary(x => x.Label!, x => x.Code!); // Crea un diccionario Label -> Code

                // Paso 2: Mapear los 'presentationArc' (nodos que definen las relaciones jerárquicas)
                var arcs = doc.Descendants()
                    .Where(e => e.Name.LocalName == "presentationArc") // Busca elementos <presentationArc>
                    .Select(e => new
                    {
                        ParentLabel = e.Attribute(xlink + "from")?.Value, // Atributo xlink:from (el padre)
                        ChildLabel = e.Attribute(xlink + "to")?.Value // Atributo xlink:to (el hijo)
                    })
                    .Where(x => !string.IsNullOrEmpty(x.ParentLabel) && !string.IsNullOrEmpty(x.ChildLabel))
                    .ToList();

                // Paso 3: Traducir las relaciones de labels a códigos y construir la jerarquía
                foreach (var arc in arcs)
                {
                    // Obtiene el código del padre usando el locator
                    if (!locators.TryGetValue(arc.ParentLabel!, out string? parentCode) || parentCode == null)
                    {
                        _logger.LogWarning("[ParseHierarchy] Label padre '{ParentLabel}' no encontrado en locators para presentación '{PresentationPath}'.", arc.ParentLabel, presentationPath);
                        continue;
                    }
                    // Obtiene el código del hijo usando el locator
                    if (!locators.TryGetValue(arc.ChildLabel!, out string? childCode) || childCode == null)
                    {
                        _logger.LogWarning("[ParseHierarchy] Label hijo '{ChildLabel}' no encontrado en locators para presentación '{PresentationPath}'.", arc.ChildLabel, presentationPath);
                        continue;
                    }

                    // Almacena la relación hijo -> padre
                    childToParent[childCode] = parentCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ParseHierarchy] Error al parsear archivo de presentación '{PresentationPath}': {Message}", presentationPath, ex.Message);
                throw;
            }
            _logger.LogInformation("[ParseHierarchy] Se encontraron {Count} relaciones padre-hijo en el archivo '{PresentationPath}'.", childToParent.Count, presentationPath);
            return childToParent;
        }

        /// <summary>
        /// Calcula el nivel jerárquico de cada concepto a partir de las relaciones padre-hijo.
        /// </summary>
        /// <param name="hierarchy">Diccionario de relaciones hijo -> padre.</param>
        /// <returns>Un diccionario donde la clave es el código del concepto y el valor es su nivel (empezando por 1 para los nodos raíz).</returns>
        private Dictionary<string, int> CalculateLevelsFromHierarchy(Dictionary<string, string?> hierarchy)
        {
            var levels = new Dictionary<string, int>();

            // Obtener todos los códigos únicos presentes en la jerarquía (tanto padres como hijos)
            var allNodes = hierarchy.Keys
                                     .Concat(hierarchy.Values.Where(v => v != null).Select(v => v!))
                                     .Distinct()
                                     .ToList();

            // Iterar sobre cada nodo para calcular su nivel
            foreach (string code in allNodes)
            {
                // Si el nivel ya fue calculado (por ejemplo, a través de otro camino), saltar
                if (levels.ContainsKey(code)) continue;

                string? currentCode = code;
                var path = new Stack<string>(); // Usar una pila para reconstruir el camino del nodo a la raíz
                var visitedInPath = new HashSet<string>(); // Para detectar ciclos en la jerarquía

                // Recorrer la jerarquía hacia arriba hasta encontrar un nodo raíz o un nivel ya calculado
                while (currentCode != null && !levels.ContainsKey(currentCode) && !visitedInPath.Contains(currentCode))
                {
                    path.Push(currentCode); // Añadir el nodo actual a la pila
                    visitedInPath.Add(currentCode); // Marcarlo como visitado en esta ruta

                    // Moverse al padre
                    if (!hierarchy.TryGetValue(currentCode, out string? parent))
                    {
                        parent = null; // No tiene padre conocido en la jerarquía
                    }
                    currentCode = parent;
                }

                int baseLevel = 0;
                if (currentCode == null)
                {
                    // Si llegamos a un nodo sin padre (la "raíz" de este camino), su nivel base es 0
                    baseLevel = 0;
                }
                else if (levels.TryGetValue(currentCode, out int existingLevel))
                {
                    // Si el padre ya tiene un nivel calculado, usarlo como base
                    baseLevel = existingLevel;
                }
                else
                {
                    // Esto indica un ciclo o un nodo que no pudo ser resuelto a una raíz conocida
                    _logger.LogWarning("[CalculateLevelsFromHierarchy] Detectado posible ciclo o nodo aislado en la jerarquía con código '{Code}'. Asignando nivel base de 0 para evitar recursión infinita.", currentCode);
                    baseLevel = 0;
                }

                // Asignar niveles a los nodos en el camino de vuelta desde la raíz
                while (path.Any())
                {
                    var node = path.Pop();
                    if (!levels.ContainsKey(node)) // Evita sobrescribir si el nivel ya fue determinado por un camino más corto o alternativo
                    {
                        // El nivel es el nivel base del ancestro + la distancia desde ese ancestro
                        levels[node] = baseLevel + path.Count + 1;
                    }
                }
            }
            _logger.LogInformation("[CalculateLevelsFromHierarchy] Se calcularon niveles para {Count} códigos.", levels.Count);
            return levels;
        }
    }
}