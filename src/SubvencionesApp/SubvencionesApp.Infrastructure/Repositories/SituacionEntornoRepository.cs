using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class SituacionEntornoRepository : GenericRepository<SituacionEntorno>, ISituacionEntornoRepository
    {
        public SituacionEntornoRepository(AppDbContext context) : base(context)
        {
        }
    }
}