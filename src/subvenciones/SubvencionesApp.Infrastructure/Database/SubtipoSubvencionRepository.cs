using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class SubtipoSubvencionRepository : GenericRepository<SubtipoSubvencion>, ISubtipoSubvencionRepository
    {
        public SubtipoSubvencionRepository(AppDbContext context) : base(context)
        {
        }
    }
}