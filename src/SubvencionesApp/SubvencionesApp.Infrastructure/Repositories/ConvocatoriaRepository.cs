using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ConvocatoriaRepository : GenericRepository<Convocatoria>, IConvocatoriaRepository
    {
        public ConvocatoriaRepository(AppDbContext context) : base(context)
        {
        }
    }
}