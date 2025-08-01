using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class OrganismoRepository : GenericRepository<Organismo>, IOrganismoRepository
    {
        public OrganismoRepository(AppDbContext context) : base(context)
        {
        }
    }
}