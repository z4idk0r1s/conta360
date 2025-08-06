using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SancionDetalleRepository : GenericRepository<SancionDetalle>, ISancionDetalleRepository
    {
        public SancionDetalleRepository(AppDbContext context) : base(context)
        {
        }
    }
}