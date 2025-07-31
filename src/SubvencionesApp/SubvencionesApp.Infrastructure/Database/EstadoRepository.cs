using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class EstadoRepository : GenericRepository<Estado>, IEstadoRepository
    {
        public EstadoRepository(AppDbContext context) : base(context)
        {
        }
    }
}