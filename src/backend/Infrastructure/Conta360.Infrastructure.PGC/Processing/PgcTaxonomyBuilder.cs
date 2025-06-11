using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
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
            var concepts = xbrlDoc.Schemas.First().Concepts;

            var accounts = new List<PgcAccount>();

            foreach (var concept in concepts)
            {
                if (concept.Name.StartsWith("pgc"))
                    continue;

                if (!int.TryParse(concept.Name, out var code))
                    continue;

                var label = concept.Labels
                    .Where(l => l.Role.Contains("label"))
                    .FirstOrDefault()?.Text ?? concept.Name;

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
