using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TerceroRepository : GenericRepository<Tercero>, ITerceroRepository
    {
        public TerceroRepository(AppDbContext context) : base(context)
        {
        }
    }
}