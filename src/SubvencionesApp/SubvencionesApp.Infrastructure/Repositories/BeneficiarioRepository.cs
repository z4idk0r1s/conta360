using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class BeneficiarioRepository : GenericRepository<Beneficiario>, IBeneficiarioRepository
    {
        public BeneficiarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}