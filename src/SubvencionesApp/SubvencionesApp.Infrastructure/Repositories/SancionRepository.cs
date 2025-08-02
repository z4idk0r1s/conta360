using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SancionRepository : GenericRepository<Sancion>, ISancionRepository
    {
        public SancionRepository(AppDbContext context) : base(context)
        {
        }
    }
}