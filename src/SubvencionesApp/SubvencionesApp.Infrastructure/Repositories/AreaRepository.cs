using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class AreaRepository : GenericRepository<Area>, IAreaRepository
    {
        public AreaRepository(AppDbContext context) : base(context)
        {
        }
    }
}