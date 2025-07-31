using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class DatosEstadisticosRepository : GenericRepository<DatosEstadisticos>, IDatosEstadisticosRepository
    {
        public DatosEstadisticosRepository(AppDbContext context) : base(context)
        {
        }
    }
}