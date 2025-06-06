using Conta360.Domain.Entities;

namespace Conta360.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        // Add account-specific methods if any
    }
}