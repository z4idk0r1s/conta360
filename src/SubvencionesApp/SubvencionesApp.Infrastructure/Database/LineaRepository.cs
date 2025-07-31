using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class LineaRepository : GenericRepository<Linea>, ILineaRepository
    {
        public LineaRepository(AppDbContext context) : base(context)
        {
        }
    }
}