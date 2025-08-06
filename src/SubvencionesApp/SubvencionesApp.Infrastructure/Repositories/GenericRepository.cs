using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            // La entidad T tiene una propiedad de clave primaria llamada "Id" de tipo Guid.
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var body = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return await _dbSet.FirstOrDefaultAsync(lambda);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> ExistsAsync(Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize, Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public virtual async Task<T?> GetByExternalIdAsync(int externalId)
        {
            // La entidad tiene una propiedad 'ExternalId'
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "ExternalId");
            var constant = Expression.Constant(externalId);
            var body = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
            return await _dbSet.FirstOrDefaultAsync(lambda);
        }
    }
}