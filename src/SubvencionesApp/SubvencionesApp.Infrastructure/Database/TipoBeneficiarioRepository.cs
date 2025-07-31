using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class TipoBeneficiarioRepository : GenericRepository<TipoBeneficiario>, ITipoBeneficiarioRepository
    {
        public TipoBeneficiarioRepository(AppDbContext context) : base(context)
        {
        }
    }
}