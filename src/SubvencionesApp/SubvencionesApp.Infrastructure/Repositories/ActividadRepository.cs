using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ActividadRepository : GenericRepository<Actividad>, IActividadRepository
    {
        public ActividadRepository(AppDbContext context) : base(context)
        {
        }
    }
}