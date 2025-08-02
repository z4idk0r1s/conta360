using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class OrganosCodigoAdminRepository : GenericRepository<OrganosCodigoAdmin>, IOrganosCodigoAdminRepository
    {
        public OrganosCodigoAdminRepository(AppDbContext context) : base(context)
        {
        }
    }
}