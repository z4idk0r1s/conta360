using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class ProvinciaRepository : GenericRepository<Provincia>, IProvinciaRepository
    {
        public ProvinciaRepository(AppDbContext context) : base(context)
        {
        }
    }
}