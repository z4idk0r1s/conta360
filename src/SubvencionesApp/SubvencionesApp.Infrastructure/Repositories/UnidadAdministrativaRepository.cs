using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class UnidadAdministrativaRepository : GenericRepository<UnidadAdministrativa>, IUnidadAdministrativaRepository
    {
        public UnidadAdministrativaRepository(AppDbContext context) : base(context)
        {
        }
    }
}