using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class UnidadAdministrativaRepository : GenericRepository<UnidadAdministrativa>, IUnidadAdministrativaRepository
    {
        public UnidadAdministrativaRepository(AppDbContext context) : base(context)
        {
        }
    }
}