using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
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