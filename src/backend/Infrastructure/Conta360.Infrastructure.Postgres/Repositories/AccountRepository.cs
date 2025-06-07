using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Infrastructure.Persistence.Contexts;
using System.Linq.Expressions;
using Conta360.Domain.Interfaces;

namespace Conta360.Infrastructure.Postgres.Repositories
{
    public class AccountRepository : IAccountRepository
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

        public async Task<IReadOnlyList<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<IReadOnlyList<Account>> GetAsync(Expression<Func<Account, bool>> predicate)
        {
            return await _context.Accounts.Where(predicate).ToListAsync();
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

        public async Task<int> CountAsync(Expression<Func<Account, bool>> predicate)
        {
            return await _context.Accounts.CountAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Account, bool>> predicate)
        {
            return await _context.Accounts.AnyAsync(predicate);
        }
    }
}