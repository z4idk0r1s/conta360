using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class TipoConvocatoriaRepository : GenericRepository<TipoConvocatoria>, ITipoConvocatoriaRepository
    {
        public TipoConvocatoriaRepository(AppDbContext context) : base(context)
        {
        }
    }
}