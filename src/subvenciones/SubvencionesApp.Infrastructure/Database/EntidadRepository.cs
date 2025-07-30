using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class EntidadRepository : GenericRepository<Entidad>, IEntidadRepository
    {
        public EntidadRepository(AppDbContext context) : base(context)
        {
        }
    }
}