using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class TipoConvocatoriaRepository : GenericRepository<TipoConvocatoria>, ITipoConvocatoriaRepository
    {
        public TipoConvocatoriaRepository(AppDbContext context) : base(context)
        {
        }
    }
}