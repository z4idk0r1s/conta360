using Conta360.Application.Interfaces;

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
            return await _context.SaveChangesAsync(CancellationToken.None);
        }

        public void Rollback()
        {
            // En EF Core, los cambios no se persisten si no se llama a SaveChanges.
            // Si usaras transacciones explícitas, aquí iría el Rollback.
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // El DbContext normalmente es gestionado por el contenedor DI.
            // Si se usara directamente (no recomendado para DI), se dispondría aquí.
        }
    }
}