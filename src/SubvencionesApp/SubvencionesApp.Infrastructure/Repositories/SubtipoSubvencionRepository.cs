using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SubtipoSubvencionRepository : GenericRepository<SubtipoSubvencion>, ISubtipoSubvencionRepository
    {
        public SubtipoSubvencionRepository(AppDbContext context) : base(context)
        {
        }
    }
}