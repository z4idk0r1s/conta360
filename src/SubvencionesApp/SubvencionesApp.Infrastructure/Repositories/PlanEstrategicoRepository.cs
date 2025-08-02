using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class PlanEstrategicoRepository : GenericRepository<PlanEstrategico>, IPlanEstrategicoRepository
    {
        public PlanEstrategicoRepository(AppDbContext context) : base(context)
        {
        }
    }
}