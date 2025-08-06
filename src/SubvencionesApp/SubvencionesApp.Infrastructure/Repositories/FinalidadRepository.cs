using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class FinalidadRepository : GenericRepository<Finalidad>, IFinalidadRepository
    {
        public FinalidadRepository(AppDbContext context) : base(context)
        {
        }
    }
}