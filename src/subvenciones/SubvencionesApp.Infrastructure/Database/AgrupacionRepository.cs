using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class AgrupacionRepository : GenericRepository<Agrupacion>, IAgrupacionRepository
    {
        public AgrupacionRepository(AppDbContext context) : base(context)
        {
        }
    }
}