/*using Conta360.Application.Interfaces;
using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Conta360.Infrastructure.Sqlite.Repositories
{
    public class AccountRepository : IPgcAccountRepository
    {
        private readonly IApplicationDbContext _context;

        public AccountRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task AddAsync(Account entity)
        {
            await _context.Accounts.AddAsync(entity);
        }

        public void Update(Account entity)
        {
            _context.Accounts.Update(entity);
        }

        public void Delete(Account entity)
        {
            _context.Accounts.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Account, bool>> predicate)
        {
            return await _context.Accounts.AnyAsync(predicate);
        }

        public Task<IReadOnlyList<Account>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Account>> GetAsync(Expression<Func<Account, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Account, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
*/