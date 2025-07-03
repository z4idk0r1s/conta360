using Conta360.Application.Interfaces;
using Conta360.Domain.Entities;
using Conta360.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Sqlite;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Conta360.Infrastructure.Sqlite.Repositories
{
    public class AccountRepositorySqlite : IPgcAccountRepository
    {
        private readonly IApplicationDbContext _context;

        public AccountRepositorySqlite(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PgcAccount?> GetByIdAsync(Guid id)
        {
            return await _context.PgcAccounts.FindAsync(id);
        }

        public async Task<IReadOnlyList<PgcAccount>> GetAllAsync()
        {
            return await _context.PgcAccounts.ToListAsync();
        }

        public async Task<IReadOnlyList<PgcAccount>> GetAsync(System.Linq.Expressions.Expression<Func<PgcAccount, bool>> predicate)
        {
            return await _context.PgcAccounts.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(PgcAccount entity)
        {
            await _context.PgcAccounts.AddAsync(entity);
        }

        public void Update(PgcAccount entity)
        {
            _context.PgcAccounts.Update(entity);
        }

        public void Delete(PgcAccount entity)
        {
            _context.PgcAccounts.Remove(entity);
        }

        public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<PgcAccount, bool>> predicate)
        {
            return await _context.PgcAccounts.CountAsync(predicate);
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<PgcAccount, bool>> predicate)
        {
            return await _context.PgcAccounts.AnyAsync(predicate);
        }

        public async Task<List<PgcAccount>> GetTreeStructureAsync()
        {
            var allAccounts = await _context.PgcAccounts.ToListAsync();
            return BuildTree(allAccounts, null);
        }

        public async Task<List<PgcAccount>> GetByParentCodeAsync(string? parentCode)
        {
            return await _context.PgcAccounts
                .Where(a => a.ParentCode == parentCode)
                .ToListAsync();
        }

        public async Task<List<PgcAccount>> GetRootAccountsAsync()
        {
            return await _context.PgcAccounts
                .Where(a => string.IsNullOrEmpty(a.ParentCode))
                .ToListAsync();
        }

        // Helper interno para construir jerarquía recursiva
        private List<PgcAccount> BuildTree(List<PgcAccount> allAccounts, string? parentCode)
        {
            return allAccounts
                .Where(a => a.ParentCode == parentCode)
                .Select(a =>
                {
                    a.Children = BuildTree(allAccounts, a.Code);
                    return a;
                })
                .ToList();
        }
    }
}
