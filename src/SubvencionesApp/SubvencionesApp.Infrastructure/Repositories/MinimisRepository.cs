using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class MinimisRepository : GenericRepository<Minimis>, IMinimisRepository
    {
        public MinimisRepository(AppDbContext context) : base(context)
        {
        }
    }
}