using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class ProgramaRepository : GenericRepository<Programa>, IProgramaRepository
    {
        public ProgramaRepository(AppDbContext context) : base(context)
        {
        }
    }
}