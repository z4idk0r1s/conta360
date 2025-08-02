using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class GrandeBeneficiarioRepository : GenericRepository<GrandeBeneficiario>, IGrandeBeneficiarioRepository
    {
        public GrandeBeneficiarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}