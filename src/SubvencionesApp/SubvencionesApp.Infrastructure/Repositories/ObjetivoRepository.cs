using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ObjetivoRepository : GenericRepository<Objetivo>, IObjetivoRepository
    {
        public ObjetivoRepository(AppDbContext context) : base(context)
        {
        }
    }
}