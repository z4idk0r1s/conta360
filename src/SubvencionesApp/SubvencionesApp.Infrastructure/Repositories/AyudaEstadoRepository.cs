using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class AyudaEstadoRepository : GenericRepository<AyudaEstado>, IAyudaEstadoRepository
    {
        public AyudaEstadoRepository(AppDbContext context) : base(context)
        {
        }
    }
}