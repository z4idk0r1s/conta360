using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class PlazoRepository : GenericRepository<Plazo>, IPlazoRepository
    {
        public PlazoRepository(AppDbContext context) : base(context)
        {
        }
    }
}