using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SectorRepository : GenericRepository<Sector>, ISectorRepository
    {
        public SectorRepository(AppDbContext context) : base(context)
        {
        }
    }
}