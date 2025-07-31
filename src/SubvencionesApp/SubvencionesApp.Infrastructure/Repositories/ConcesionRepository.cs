using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ConcesionRepository : GenericRepository<Concesion>, IConcesionRepository
    {
        public ConcesionRepository(AppDbContext context) : base(context)
        {
        }
    }
}