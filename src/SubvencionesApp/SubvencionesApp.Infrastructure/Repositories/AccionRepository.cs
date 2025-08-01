using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class AccionRepository : GenericRepository<Accion>, IAccionRepository
    {
        public AccionRepository(AppDbContext context) : base(context)
        {
        }
    }
}