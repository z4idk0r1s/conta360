using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TipoOrganismoRepository : GenericRepository<TipoOrganismo>, ITipoOrganismoRepository
    {
        public TipoOrganismoRepository(AppDbContext context) : base(context)
        {
        }
    }
}