// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Data;




namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services
{
    public class PgcTaxonomyBuilder
    {
        private static readonly HttpClient _httpClient = new();

        public async Task BuildConsolidatedXmlAsync(string zipUrl, string localZipPath, string outputXml)
        {
            using (var response = await _httpClient.GetAsync(zipUrl))
            {
                response.EnsureSuccessStatusCode();
                await using var fs = new FileStream(localZipPath, FileMode.Create, FileAccess.Write);
                await response.Content.CopyToAsync(fs);
            }

            using var zip = ZipFile.OpenRead(localZipPath);
            var doc = new XDocument(new XElement("xbrli:xbrl",
                new XAttribute(XNamespace.Xmlns + "xbrli", "http://www.xbrl.org/2003/instance")));

            var xsdOutputDir = Path.Combine("Data", "Xsd");
            Directory.CreateDirectory(xsdOutputDir); // ← Asegura que exista

            foreach (var entry in zip.Entries)
            {
                if (entry.FullName.EndsWith(".xsd"))
                {
                    var xsdPath = Path.Combine(xsdOutputDir, Path.GetFileName(entry.FullName));
                    using var stream = entry.Open();
                    using var outFile = new FileStream(xsdPath, FileMode.Create, FileAccess.Write);
                    await stream.CopyToAsync(outFile);
                }

                if (entry.FullName.EndsWith(".xsd") || entry.FullName.Contains("Linkbase"))
                {
                    using var stream = entry.Open();
                    var partRoot = XDocument.Load(stream).Root;
                    foreach (var el in partRoot.Descendants()
                        .Where(e => e.Name.LocalName == "element" || e.Name.LocalName.EndsWith("Link")))
                    {
                        doc.Root.Add(el);
                    }
                }
            }

            doc.Save(outputXml);
        }
    }
}
