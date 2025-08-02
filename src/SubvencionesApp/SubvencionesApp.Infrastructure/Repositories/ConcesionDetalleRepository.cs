using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ConcesionDetalleRepository : GenericRepository<ConcesionDetalle>, IConcesionDetalleRepository
    {
        public ConcesionDetalleRepository(AppDbContext context) : base(context)
        {
        }
    }
}