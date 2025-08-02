using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ConvocatoriaDetalleRepository : GenericRepository<ConvocatoriaDetalle>, IConvocatoriaDetalleRepository
    {
        public ConvocatoriaDetalleRepository(AppDbContext context) : base(context)
        {
        }
    }
}