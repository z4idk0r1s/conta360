using Conta360.Application.DTOs;
using Conta360.Domain.Entities;

namespace Conta360.Application.Services
{
    public static class PgcAccountTreeBuilder
    {
        public static List<PgcAccountTreeDto> BuildTree(List<PgcAccount> flatAccounts)
        {
            var lookup = flatAccounts.ToDictionary(a => a.Code, a => new PgcAccountTreeDto
            {
                Code = a.Code,
                Description = a.Description
            });

            var rootNodes = new List<PgcAccountTreeDto>();

            foreach (var account in flatAccounts.OrderBy(a => a.Code))
            {
                var code = account.Code;

                string? parentCode = code.Length switch
                {
                    >= 5 => code.Substring(0, 4),
                    4 => code.Substring(0, 3),
                    3 => code.Substring(0, 2),
                    2 => code.Substring(0, 1),
                    _ => null
                };

                if (parentCode != null && lookup.ContainsKey(parentCode))
                {
                    lookup[parentCode].Children.Add(lookup[code]);
                }
                else
                {
                    rootNodes.Add(lookup[code]);
                }
            }

            return rootNodes;
        }
    }
}
