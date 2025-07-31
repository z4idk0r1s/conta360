using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class DatosEstadisticosRepository : GenericRepository<DatosEstadisticos>, IDatosEstadisticosRepository
    {
        public DatosEstadisticosRepository(AppDbContext context) : base(context)
        {
        }
    }
}