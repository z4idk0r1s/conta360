using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class TipoSubvencionRepository : GenericRepository<TipoSubvencion>, ITipoSubvencionRepository
    {
        public TipoSubvencionRepository(AppDbContext context) : base(context)
        {
        }
    }
}