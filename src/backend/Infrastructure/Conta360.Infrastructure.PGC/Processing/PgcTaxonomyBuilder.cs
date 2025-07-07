using Conta360.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.PGC.Processing
{
    /// <summary>
    /// Construye una lista de objetos PgcAccount a partir de los archivos de taxonomía XBRL (XSD, Label, Presentation).
    /// </summary>
    public class PgcTaxonomyBuilder
    {
        private readonly ILogger<PgcTaxonomyBuilder> _logger;
        private readonly PgcTaxonomyParser _parser; // Inyectamos la nueva clase de parseo

        public PgcTaxonomyBuilder(ILogger<PgcTaxonomyBuilder> logger, PgcTaxonomyParser parser)
        {
            _logger = logger;
            _parser = parser;
        }

        /// <summary>
        /// Construye una lista de objetos PgcAccount a partir de los archivos de taxonomía XBRL (XSD, Label, Presentation).
        /// </summary>
        /// <param name="xsdPath">Ruta al archivo XSD principal de la taxonomía (e.g., pgc07-normal-completo.xsd). Este debe ser el XSD que importa todos los conceptos.</param>
        /// <param name="labelPath">Ruta al archivo XBRL Label Linkbase (.lbl) que contiene los nombres de las cuentas.</param>
        /// <param name="presentationPaths">Lista de rutas a los archivos XBRL Presentation Linkbase (.pre) que definen la jerarquía padre-hijo de las cuentas. Puede estar vacía o ser nula.</param>
        /// <returns>Una lista de objetos PgcAccount con los códigos, nombres, jerarquía y niveles calculados.</returns>
        /// <exception cref="FileNotFoundException">Se lanza si alguno de los archivos obligatorios (XSD, Label) no se encuentra.</exception>
        /// <exception cref="InvalidDataException">Se lanza si el esquema XSD no se puede leer.</exception>
        public async Task<List<PgcAccount>> BuildAccountsFromXsdLabelPresentation(
            string xsdPath,
            string labelPath,
            List<string>? presentationPaths)
        {
            _logger.LogInformation("[PgcTaxonomyBuilder] Iniciando construcción de cuentas PGC para XSD: '{XsdPath}', Label: '{LabelPath}' y {PresentationCount} archivos de presentación.",
                xsdPath, labelPath, presentationPaths?.Count ?? 0);

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

            // Paso 1: Parsear los códigos de las cuentas desde el XSD principal
            var codes = await _parser.ParseCodesFromXsd(xsdPath);
            // Paso 2: Parsear las etiquetas (nombres) de las cuentas
            var labels = await _parser.ParseLabels(labelPath);

            // Paso 3: Parsear la jerarquía padre-hijo de TODOS los archivos de presentación
            var combinedHierarchy = new Dictionary<string, string?>();
            if (presentationPaths != null && presentationPaths.Any())
            {
                foreach (var path in presentationPaths)
                {
                    if (File.Exists(path))
                    {
                        _logger.LogInformation("[PgcTaxonomyBuilder] Parseando jerarquía desde archivo de presentación: '{PresentationPath}'", path);
                        var currentHierarchy = await _parser.ParseHierarchy(path);
                        // Combinar las jerarquías. Si hay duplicados, la última definición prevalece.
                        foreach (var entry in currentHierarchy)
                        {
                            combinedHierarchy[entry.Key] = entry.Value;
                        }
                    }
                    else
                    {
                        _logger.LogWarning("[PgcTaxonomyBuilder] El archivo de presentación no fue encontrado y será omitido: {PresentationPath}", path);
                    }
                }
            }
            else
            {
                _logger.LogWarning("[PgcTaxonomyBuilder] No se proporcionaron archivos de presentación válidos. La jerarquía se calculará solo a partir de los códigos XSD (sin relaciones padre-hijo del linkbase).");
            }
            
            _logger.LogInformation("[PgcTaxonomyBuilder] Se combinaron {Count} relaciones padre-hijo de todos los archivos de presentación.", combinedHierarchy.Count);

            // Paso 4: Calcular los niveles jerárquicos de las cuentas
            var levels = CalculateLevelsFromHierarchy(combinedHierarchy);

            var accounts = new List<PgcAccount>();

            // Paso 5: Construir los objetos PgcAccount
            foreach (var code in codes)
            {
                labels.TryGetValue(code, out var label);
                var name = string.IsNullOrWhiteSpace(label) ? code : label;

                combinedHierarchy.TryGetValue(code, out var parent);
                var parentCode = parent;

                levels.TryGetValue(code, out var lvl);
                var level = levels.ContainsKey(code) ? lvl : 1; 

                accounts.Add(new PgcAccount
                {
                    Id = Guid.NewGuid(),
                    Code = code,
                    Name = name,
                    ParentCode = parentCode,
                    Level = level,
                    IsMovable = level >= 3 
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
                        _logger.LogWarning("[PgcTaxonomyBuilder] Cuenta padre con código '{ParentCode}' no encontrada en la lista de cuentas procesadas para la cuenta '{ChildCode}'. Esto puede indicar un problema en la jerarquía o conceptos no definidos.", account.ParentCode, account.Code);
                    }
                }
            }

            _logger.LogInformation("[PgcTaxonomyBuilder] Finalizada la construcción. Se generaron {Count} cuentas del PGC.", accounts.Count);
            return accounts;
        }

        /// <summary>
        /// Calcula el nivel jerárquico de cada concepto a partir de las relaciones padre-hijo.
        /// </summary>
        /// <param name="hierarchy">Diccionario de relaciones hijo -> padre.</param>
        /// <returns>Un diccionario donde la clave es el código del concepto y el valor es su nivel (empezando por 1 para los nodos raíz).</returns>
        private Dictionary<string, int> CalculateLevelsFromHierarchy(Dictionary<string, string?> hierarchy)
        {
            _logger.LogDebug("[CalculateLevelsFromHierarchy] Iniciando cálculo de niveles para {Count} relaciones.", hierarchy.Count);
            var levels = new Dictionary<string, int>();

            var allNodes = new HashSet<string>();
            foreach (var kvp in hierarchy)
            {
                allNodes.Add(kvp.Key);
                if (kvp.Value != null)
                {
                    allNodes.Add(kvp.Value);
                }
            }

            var rootNodes = allNodes.Where(node => !hierarchy.ContainsKey(node)).ToList();

            var queue = new Queue<string>();

            foreach (var root in rootNodes)
            {
                levels[root] = 1;
                queue.Enqueue(root);
            }
            _logger.LogDebug("[CalculateLevelsFromHierarchy] Identificados {Count} nodos raíz.", rootNodes.Count);

            foreach (var node in allNodes)
            {
                if (!levels.ContainsKey(node))
                {
                    string? currentNode = node;
                    HashSet<string> visitedPath = new HashSet<string>();

                    while (currentNode != null && hierarchy.ContainsKey(currentNode) && !visitedPath.Contains(currentNode))
                    {
                        visitedPath.Add(currentNode);
                        currentNode = hierarchy[currentNode];
                    }

                    if (currentNode == null || !levels.ContainsKey(currentNode))
                    {
                        if (!levels.ContainsKey(node))
                        {
                            levels[node] = 1;
                            queue.Enqueue(node);
                        }
                    }
                }
            }

            while (queue.Any())
            {
                string parentCode = queue.Dequeue();
                int parentLevel = levels[parentCode];

                var children = hierarchy.Where(kv => kv.Value == parentCode).Select(kv => kv.Key).ToList();

                foreach (var childCode in children)
                {
                    if (!levels.ContainsKey(childCode) || levels[childCode] > parentLevel + 1)
                    {
                        levels[childCode] = parentLevel + 1;
                        queue.Enqueue(childCode);
                    }
                }
            }

            _logger.LogInformation("[CalculateLevelsFromHierarchy] Se calcularon niveles para {Count} códigos.", levels.Count);
            return levels;
        }
    }
}