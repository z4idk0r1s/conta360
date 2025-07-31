using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class LineaRepository : GenericRepository<Linea>, ILineaRepository
    {
        public LineaRepository(AppDbContext context) : base(context)
        {
        }
    }
}