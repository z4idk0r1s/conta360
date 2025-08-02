using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SuscripcionRepository : GenericRepository<Suscripcion>, ISuscripcionRepository
    {
        public SuscripcionRepository(AppDbContext context) : base(context)
        {
        }
    }
}