using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class OrganismoRepository : GenericRepository<Organismo>, IOrganismoRepository
    {
        public OrganismoRepository(AppDbContext context) : base(context)
        {
        }
    }
}