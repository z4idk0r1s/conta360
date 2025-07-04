using Conta360.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.Sqlite.Repositories
{
    public class UnitOfWorkSqlite : IUnitOfWork
    {
        private readonly IApplicationDbContext _context;

        public UnitOfWorkSqlite(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            // Este método será usado por operaciones no-bulk que necesiten SaveChanges.
            // Para BulkInsertOrUpdateAsync, el SaveChanges es interno a la extensión.
            return await _context.SaveChangesAsync(CancellationToken.None);
        }

        public void Rollback()
        {
            // En EF Core, si no se llama a SaveChanges, los cambios no se persisten.
            // Si se usaran transacciones explícitas de DbContext (BeginTransaction),
            // aquí iría un dbContext.RollbackTransaction().
            // Dado que BulkExtensions maneja la suya, y SaveChangesAsync no inicia una
            // transacción explícita por defecto para operaciones unitarias,
            // esta NotImplementedException es razonable a menos que se implemente
            // un BeginTransaction/Commit/Rollback a nivel de IUnitOfWork que envuelva
            // TODAS las operaciones de DbContext.
            throw new NotImplementedException("Rollback no implementado para el UnitOfWork implícito de EF Core. Para transacciones explícitas, usa DbContext.Database.BeginTransaction.");
        }

        public void Dispose()
        {
            // El DbContext normalmente es gestionado por el contenedor DI con Scoped lifetime.
            // Si se usara directamente (no recomendado para DI en la capa de infraestructura),
            // se dispondría aquí.
            // (_context as DbContext)?.Dispose(); // Descomentar si el DbContext fuera Disposable y gestionado directamente.
        }
    }
}