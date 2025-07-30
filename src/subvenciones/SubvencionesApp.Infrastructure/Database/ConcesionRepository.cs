using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class ConcesionRepository : GenericRepository<Concesion>, IConcesionRepository
    {
        public ConcesionRepository(AppDbContext context) : base(context)
        {
        }
    }
}