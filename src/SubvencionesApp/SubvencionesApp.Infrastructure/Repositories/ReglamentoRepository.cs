using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ReglamentoRepository : GenericRepository<Reglamento>, IReglamentoRepository
    {
        public ReglamentoRepository(AppDbContext context) : base(context)
        {
        }
    }
}