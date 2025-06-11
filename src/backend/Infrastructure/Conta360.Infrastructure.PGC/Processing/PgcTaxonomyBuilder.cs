using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
using System.Xml;
using System.Xml.Schema;

namespace Conta360.Infrastructure.PGC.Processing
{
    public class PgcTaxonomyBuilder
    {
        private readonly IAccountRepository _accountRepository;

        public PgcTaxonomyBuilder(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<PgcAccount>> ParseAndPersistAccountsFromXsdAsync(string path)
        {
            var accounts = new List<PgcAccount>();
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(null, path);
            schemaSet.Compile();

            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                foreach (XmlSchemaElement element in schema.Elements.Values)
                {
                    if (element.QualifiedName.Namespace.Contains("pgc07"))
                    {
                        var name = element.QualifiedName.Name;
                        var description = element.Annotation?.Items
                            .OfType<XmlSchemaDocumentation>()
                            .FirstOrDefault()?.Markup?.FirstOrDefault()?.Value;

                        if (int.TryParse(name, out var code))
                        {
                            accounts.Add(new PgcAccount
                            {
                                Code = code.ToString("D3"),
                                Description = description ?? name
                            });
                        }
                    }
                }
            }

            foreach (var acc in accounts)
            {
                await _accountRepository.AddAsync(acc);
            }

            return accounts;
        }
    }
}
