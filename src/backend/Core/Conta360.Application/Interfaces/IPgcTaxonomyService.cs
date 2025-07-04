using Conta360.Domain.Entities;
using Conta360.Core.Common; // Asegúrate de que esta referencia exista si ValidationResult se usa aquí
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conta360.Application.Interfaces
{
    public interface IPgcTaxonomyService
    {
        Task<OperationResult<List<PgcAccount>>> RunAndGetAccountsAsync();
    }
}