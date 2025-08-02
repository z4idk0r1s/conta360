using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SectorProductoRepository : GenericRepository<SectorProducto>, ISectorProductoRepository
    {
        public SectorProductoRepository(AppDbContext context) : base(context)
        {
        }
    }
}