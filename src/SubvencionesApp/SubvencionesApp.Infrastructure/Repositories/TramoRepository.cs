using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TramoRepository : GenericRepository<Tramo>, ITramoRepository
    {
        public TramoRepository(AppDbContext context) : base(context)
        {
        }
    }
}