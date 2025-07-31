using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class AgrupacionRepository : GenericRepository<Agrupacion>, IAgrupacionRepository
    {
        public AgrupacionRepository(AppDbContext context) : base(context)
        {
        }
    }
}