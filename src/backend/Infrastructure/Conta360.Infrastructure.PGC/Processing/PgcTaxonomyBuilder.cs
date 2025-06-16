using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Conta360.Application.DTOs;
using JeffFerguson.Gepsio;
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
            var xbrlDoc = new XbrlDocument();
            xbrlDoc.Load(path);
            var schema = xbrlDoc.Schemas.FirstOrDefault();

            if (schema == null)
                throw new InvalidOperationException("No se encontraron esquemas XBRL en el documento.");

            var concepts = schema.Concepts;
            var accounts = new List<PgcAccountDto>();

            foreach (var concept in concepts)
            {
                if (concept.Name.StartsWith("pgc"))
                    continue;

                if (!int.TryParse(concept.Name, out var code))
                    continue;

                var label = concept.Labels?
                    .FirstOrDefault(l => l.Role != null && l.Role.Contains("label"))?
                    .Text ?? concept.Name;

                accounts.Add(new PgcAccount
                {
                    Code = code.ToString("D3"),
                    Description = label,
                    IsAbstract = concept.IsAbstract,
                    Balance = concept.BalanceType?.ToString(),
                    Namespace = concept.Namespace,
                    ConceptId = concept.Id
                });
            }

            foreach (var acc in accounts)
            {
                await _accountRepository.AddAsync(acc);
            }

            return accounts;
        }
    }
}
