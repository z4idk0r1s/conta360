using Conta360.Application.Interfaces;

namespace Conta360.Infrastructure.Postgres
{
    public class UnitOfWorkPostgres : IUnitOfWork
    {
        private readonly IApplicationDbContext _context;

        public UnitOfWorkPostgres(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync(CancellationToken.None);
        }

        public void Rollback()
        {
            // Not typically implemented in EF Core, changes are discarded if not committed
            // If using transactions, you might rollback here.
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // DbContext is typically disposed by DI container
            // For direct usage, you would dispose _context here
        }
    }
}