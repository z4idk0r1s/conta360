using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class TramoRepository : GenericRepository<Tramo>, ITramoRepository
    {
        public TramoRepository(AppDbContext context) : base(context)
        {
        }
    }
}