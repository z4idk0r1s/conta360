using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
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
        private readonly IPgcAccountRepository _accountRepository;

        public PgcTaxonomyBuilder(IPgcAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Importa cuentas del PGC con jerarquía (ParentCode y Level) usando XSD, label-es.xml y presentation.xml.
        /// </summary>
        public async Task<List<PgcAccount>> ParseAndPersistAccountsFromXsdLabelPresentationAsync(
            string xsdPath,
            string labelPath,
            string presentationPath)
        {
            var codes = ParseCodesFromXsd(xsdPath);
            var labels = ParseLabels(labelPath);
            var hierarchy = ParseHierarchy(presentationPath);
            var levels = CalculateLevelsFromHierarchy(hierarchy);

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
                    IsMovable = level >= 3
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

            // Paso 3: Persistir en BD
            foreach (var account in accounts)
            {
                await _accountRepository.AddAsync(account);
            }

            return accounts;
        }


        private List<string> ParseCodesFromXsd(string xsdPath)
        {
            var codes = new List<string>();
            using (var fs = File.OpenRead(xsdPath))
            using (var reader = XmlReader.Create(fs))
            {
                var schema = XmlSchema.Read(reader, null);
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

        private Dictionary<string, string> ParseLabels(string labelPath)
        {
            var dict = new Dictionary<string, string>();
            var doc = XDocument.Load(labelPath);
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
        private Dictionary<string, string?> ParseHierarchy(string presentationPath)
        {
            var doc = XDocument.Load(presentationPath);
            XNamespace link = "http://www.xbrl.org/2003/linkbase";
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
                if (!int.TryParse(parentCode, out _)) parentCode = null;
                childToParent[childCode] = parentCode;
            }

            return childToParent;
        }

        /// <summary>
        /// Calcula el nivel jerárquico de cada código (raíz=1).
        /// </summary>
        private Dictionary<string, int> CalculateLevelsFromHierarchy(Dictionary<string, string?> hierarchy)
        {
            var levels = new Dictionary<string, int>();
            foreach (var code in hierarchy.Keys)
            {
                var level = 1;
                var parent = hierarchy[code];
                while (!string.IsNullOrEmpty(parent))
                {
                    level++;
                    if (!hierarchy.TryGetValue(parent, out parent)) break;
                }
                levels[code] = level;
            }
            return levels;
        }
    }
}