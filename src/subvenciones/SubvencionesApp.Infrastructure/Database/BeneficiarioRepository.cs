using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class BeneficiarioRepository : GenericRepository<Beneficiario>, IBeneficiarioRepository
    {
        public BeneficiarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}