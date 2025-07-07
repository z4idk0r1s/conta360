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
    /// <summary>
    /// Provee métodos para parsear los diferentes archivos de una taxonomía XBRL (XSD, Label Linkbase, Presentation Linkbase).
    /// </summary>
    public class PgcTaxonomyParser
    {
        private readonly ILogger<PgcTaxonomyParser> _logger;

        public PgcTaxonomyParser(ILogger<PgcTaxonomyParser> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parsea un archivo XSD para extraer los códigos de los elementos que son conceptos XBRL (sustituyen a xbrli:item).
        /// </summary>
        /// <param name="xsdPath">Ruta al archivo XSD.</param>
        /// <returns>Una lista de cadenas con los nombres de los elementos (códigos de cuenta).</returns>
        public async Task<List<string>> ParseCodesFromXsd(string xsdPath)
        {
            _logger.LogDebug("[PgcTaxonomyParser.ParseCodesFromXsd] Iniciando parseo de códigos desde XSD: '{XsdPath}'", xsdPath);
            var codes = new HashSet<string>();
            
            var xbrliItemQName = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

            try
            {
                var settings = new XmlReaderSettings
                {
                    XmlResolver = new XmlUrlResolver(),
                    DtdProcessing = DtdProcessing.Parse,
                    ValidationType = ValidationType.Schema
                };

                XmlSchema? mainSchema;
                using (var reader = XmlReader.Create(xsdPath, settings))
                {
                    mainSchema = await Task.Run(() => XmlSchema.Read(reader, (sender, e) =>
                    {
                        if (e.Severity == XmlSeverityType.Error)
                        {
                            _logger.LogError("[PgcTaxonomyParser.ParseCodesFromXsd] Error de validación del XSD '{XsdPath}': {Message}", xsdPath, e.Message);
                        }
                        else
                        {
                            _logger.LogWarning("[PgcTaxonomyParser.ParseCodesFromXsd] Advertencia de validación del XSD '{XsdPath}': {Message}", xsdPath, e.Message);
                        }
                    }));
                }

                if (mainSchema == null)
                    throw new InvalidDataException($"No se pudo leer el esquema XSD: {xsdPath}");

                var schemaSet = new XmlSchemaSet();
                schemaSet.Add(mainSchema);
                schemaSet.Compile();

                _logger.LogInformation("[PgcTaxonomyParser.ParseCodesFromXsd] Número de esquemas resueltos en SchemaSet para '{XsdPath}': {Count}", xsdPath, schemaSet.Count);

                foreach (XmlSchema s in schemaSet.Schemas())
                {
                    _logger.LogDebug("[PgcTaxonomyParser.ParseCodesFromXsd] Procesando elementos del esquema con TargetNamespace: {TargetNamespace}", s.TargetNamespace);
                    foreach (XmlSchemaElement element in s.Elements.Values)
                    {
                        if (element.SubstitutionGroup != null && element.SubstitutionGroup.Equals(xbrliItemQName))
                        {
                            if (!string.IsNullOrWhiteSpace(element.Name))
                            {
                                codes.Add(element.Name.Trim());
                            }
                        }
                    }
                }
            }
            catch (XmlSchemaException ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyParser.ParseCodesFromXsd] Error de esquema XML al parsear XSD '{XsdPath}': {Message}", xsdPath, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyParser.ParseCodesFromXsd] Error inesperado al parsear XSD '{XsdPath}': {Message}", xsdPath, ex.Message);
                throw;
            }

            _logger.LogInformation("[PgcTaxonomyParser.ParseCodesFromXsd] Se encontraron {Count} códigos únicos en el XSD completo '{XsdPath}'.", codes.Count, xsdPath);
            return codes.ToList();
        }

        /// <summary>
        /// Parsea un archivo XBRL Label Linkbase (.lbl) para extraer las etiquetas (nombres) de los conceptos.
        /// </summary>
        /// <param name="labelPath">Ruta al archivo Label Linkbase.</param>
        /// <returns>Un diccionario donde la clave es el código del concepto y el valor es su etiqueta.</returns>
        public async Task<Dictionary<string, string>> ParseLabels(string labelPath)
        {
            _logger.LogDebug("[PgcTaxonomyParser.ParseLabels] Iniciando parseo de etiquetas desde archivo: '{LabelPath}'", labelPath);
            var conceptLabels = new Dictionary<string, string>();
            try
            {
                var xmlContent = await File.ReadAllTextAsync(labelPath);
                var doc = XDocument.Parse(xmlContent);

                XNamespace xlink = "http://www.w3.org/1999/xlink";
                XNamespace link = "http://www.xbrl.org/2003/linkbase";
                // XNamespace xbrli = "http://www.xbrl.org/2003/instance"; // No es estrictamente necesario aquí si no se usa directamente para buscar elementos/atributos

                // Paso 1: Mapear los locators (en este caso, los href de los conceptos a sus xlink:label)
                var locators = doc.Descendants(link + "loc")
                    .Select(e => new
                    {
                        Label = e.Attribute(xlink + "label")?.Value,
                        Code = e.Attribute(xlink + "href")?.Value?.Split('#').Last()
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Label) && !string.IsNullOrEmpty(x.Code))
                    .ToDictionary(x => x.Label!, x => x.Code!);

                // Paso 2: Recolectar las etiquetas por su xlink:label y asegurar el idioma español
                // CORRECTED: Buscar el atributo 'lang' con el namespace 'http://www.w3.org/XML/1998/namespace'
                var labelsByXlinkLabel = doc.Descendants(link + "label")
                    .Where(e => 
                    {
                        var langAttribute = e.Attribute(XNamespace.Xml + "lang"); // Correcto para xml:lang
                        return langAttribute?.Value?.ToLower() == "es";
                    })
                    .Select(e => new
                    {
                        XlinklLabel = e.Attribute(xlink + "label")?.Value,
                        Text = e.Value.Trim()
                    })
                    .Where(x => !string.IsNullOrEmpty(x.XlinklLabel) && !string.IsNullOrEmpty(x.Text))
                    .ToDictionary(x => x.XlinklLabel!, x => x.Text);

                // Paso 3: Usar los labelArc para conectar los locators con las etiquetas
                foreach (var labelArc in doc.Descendants(link + "labelArc"))
                {
                    string? fromLabel = labelArc.Attribute(xlink + "from")?.Value;
                    string? toLabel = labelArc.Attribute(xlink + "to")?.Value;

                    if (string.IsNullOrWhiteSpace(fromLabel) || string.IsNullOrWhiteSpace(toLabel))
                    {
                        _logger.LogWarning("[PgcTaxonomyParser.ParseLabels] labelArc incompleto encontrado en '{LabelPath}'. Se omitirá.", labelPath);
                        continue;
                    }

                    if (!locators.TryGetValue(fromLabel, out string? conceptCode) || conceptCode == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyParser.ParseLabels] Origen de labelArc '{FromLabel}' no encontrado en locators para '{LabelPath}'. Se omitirá esta relación de etiqueta.", fromLabel, labelPath);
                        continue;
                    }

                    if (!labelsByXlinkLabel.TryGetValue(toLabel, out string? labelText) || labelText == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyParser.ParseLabels] Destino de labelArc '{ToLabel}' no encontrado en etiquetas para '{LabelPath}'. Se omitirá esta relación de etiqueta.", toLabel, labelPath);
                        continue;
                    }

                    conceptLabels[conceptCode] = labelText;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyParser.ParseLabels] Error al parsear archivo de etiquetas '{LabelPath}': {Message}", labelPath, ex.Message);
                throw;
            }
            _logger.LogInformation("[PgcTaxonomyParser.ParseLabels] Se encontraron {Count} etiquetas en el archivo '{LabelPath}'.", conceptLabels.Count, labelPath);
            return conceptLabels;
        }

        /// <summary>
        /// Parsea un archivo XBRL Presentation Linkbase (.pre) para extraer las relaciones padre-hijo de la jerarquía de cuentas.
        /// </summary>
        /// <param name="presentationPath">Ruta al archivo Presentation Linkbase.</param>
        /// <returns>Un diccionario donde la clave es el código del hijo y el valor es el código del padre.</returns>
        public async Task<Dictionary<string, string?>> ParseHierarchy(string presentationPath)
        {
            _logger.LogDebug("[PgcTaxonomyParser.ParseHierarchy] Iniciando parseo de jerarquía desde archivo: '{PresentationPath}'", presentationPath);
            var childToParent = new Dictionary<string, string?>();
            try
            {
                var xmlContent = await File.ReadAllTextAsync(presentationPath);
                var doc = XDocument.Parse(xmlContent);

                XNamespace xlink = "http://www.w3.org/1999/xlink";
                XNamespace link = "http://www.xbrl.org/2003/linkbase";

                var locators = doc.Descendants(link + "loc")
                    .Select(e => new
                    {
                        Label = e.Attribute(xlink + "label")?.Value,
                        Code = e.Attribute(xlink + "href")?.Value?.Split('#').Last()
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Label) && !string.IsNullOrEmpty(x.Code))
                    .ToDictionary(x => x.Label!, x => x.Code!);

                var arcs = doc.Descendants(link + "presentationArc")
                    .Select(e => new
                    {
                        ParentLabel = e.Attribute(xlink + "from")?.Value,
                        ChildLabel = e.Attribute(xlink + "to")?.Value
                    })
                    .Where(x => !string.IsNullOrEmpty(x.ParentLabel) && !string.IsNullOrEmpty(x.ChildLabel))
                    .ToList();

                foreach (var arc in arcs)
                {
                    if (!locators.TryGetValue(arc.ParentLabel!, out string? parentCode) || parentCode == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyParser.ParseHierarchy] Label padre '{ParentLabel}' no encontrado en locators para presentación '{PresentationPath}'. Se omitirá esta relación.", arc.ParentLabel, presentationPath);
                        continue;
                    }
                    if (!locators.TryGetValue(arc.ChildLabel!, out string? childCode) || childCode == null)
                    {
                        _logger.LogWarning("[PgcTaxonomyParser.ParseHierarchy] Label hijo '{ChildLabel}' no encontrado en locators para presentación '{PresentationPath}'. Se omitirá esta relación.", arc.ChildLabel, presentationPath);
                        continue;
                    }

                    childToParent[childCode] = parentCode;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PgcTaxonomyParser.ParseHierarchy] Error al parsear archivo de presentación '{PresentationPath}': {Message}", presentationPath, ex.Message);
                throw;
            }
            _logger.LogInformation("[PgcTaxonomyParser.ParseHierarchy] Se encontraron {Count} relaciones padre-hijo en el archivo '{PresentationPath}'.", childToParent.Count, presentationPath);
            return childToParent;
        }
    }
}