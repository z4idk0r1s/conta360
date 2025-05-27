using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;

namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services
{
    public class XmlValidator
    {
        public void ValidateXml(string xmlPath, string xsdPath)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += (sender, e) =>
            {
                Console.WriteLine($"❌ ERROR de validación: {e.Message}");
            };

            using var reader = XmlReader.Create(xmlPath, settings);
            while (reader.Read()) { }

            Console.WriteLine($"✅ XML '{Path.GetFileName(xmlPath)}' validado contra XSD '{Path.GetFileName(xsdPath)}'");
        }

        public void ValidateXmlWithMultipleSchemas(string xmlPath, List<string> xsdPaths)
        {
            var schemas = new XmlSchemaSet();
            foreach (var xsd in xsdPaths)
            {
                schemas.Add(null, xsd);
            }

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas
            };

            settings.ValidationEventHandler += (sender, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                    Console.WriteLine($"❌ ERROR: {e.Message}");
                else
                    Console.WriteLine($"⚠️ ADVERTENCIA: {e.Message}");
            };

            using var reader = XmlReader.Create(xmlPath, settings);
            while (reader.Read()) { }

            Console.WriteLine("✅ Validación completa con múltiples XSDs.");
        }
    }
}
