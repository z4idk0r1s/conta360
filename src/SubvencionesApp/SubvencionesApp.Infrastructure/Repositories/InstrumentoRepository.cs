using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class InstrumentoRepository : GenericRepository<Instrumento>, IInstrumentoRepository
    {
        public InstrumentoRepository(AppDbContext context) : base(context)
        {
        }
    }
}