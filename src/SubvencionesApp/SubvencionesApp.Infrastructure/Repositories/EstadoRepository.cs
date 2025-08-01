using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class EstadoRepository : GenericRepository<Estado>, IEstadoRepository
    {
        public EstadoRepository(AppDbContext context) : base(context)
        {
        }
    }
}