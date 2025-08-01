using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TipoBeneficiarioRepository : GenericRepository<TipoBeneficiario>, ITipoBeneficiarioRepository
    {
        public TipoBeneficiarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}