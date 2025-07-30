using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class SituacionEntornoRepository : GenericRepository<SituacionEntorno>, ISituacionEntornoRepository
    {
        public SituacionEntornoRepository(AppDbContext context) : base(context)
        {
        }
    }
}