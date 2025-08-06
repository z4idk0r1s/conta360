using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class PlanEstrategicoDetalleRepository : GenericRepository<PlanEstrategicoDetalle>, IPlanEstrategicoDetalleRepository
    {
        public PlanEstrategicoDetalleRepository(AppDbContext context) : base(context)
        {
        }
    }
}