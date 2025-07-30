using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class TipoOrganismoRepository : GenericRepository<TipoOrganismo>, ITipoOrganismoRepository
    {
        public TipoOrganismoRepository(AppDbContext context) : base(context)
        {
        }
    }
}