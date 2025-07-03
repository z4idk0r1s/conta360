using Conta360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Conta360.Domain.Interfaces
{
    public interface IPgcAccountRepository
    {
        Task<PgcAccount?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<PgcAccount>> GetAllAsync();
        Task<IReadOnlyList<PgcAccount>> GetAsync(Expression<Func<PgcAccount, bool>> predicate);
        Task AddAsync(PgcAccount entity);
        void Update(PgcAccount entity);
        void Delete(PgcAccount entity);
        Task<int> CountAsync(Expression<Func<PgcAccount, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<PgcAccount, bool>> predicate);
        // Otros métodos específicos de dominio
    }
}