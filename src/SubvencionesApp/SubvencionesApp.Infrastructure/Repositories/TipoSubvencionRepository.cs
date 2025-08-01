using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TipoSubvencionRepository : GenericRepository<TipoSubvencion>, ITipoSubvencionRepository
    {
        public TipoSubvencionRepository(AppDbContext context) : base(context)
        {
        }
    }
}