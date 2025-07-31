using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class AccionRepository : GenericRepository<Accion>, IAccionRepository
    {
        public AccionRepository(AppDbContext context) : base(context)
        {
        }
    }
}