using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class ProvinciaRepository : GenericRepository<Provincia>, IProvinciaRepository
    {
        public ProvinciaRepository(AppDbContext context) : base(context)
        {
        }
    }
}