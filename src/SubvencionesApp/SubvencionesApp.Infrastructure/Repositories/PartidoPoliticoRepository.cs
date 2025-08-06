using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class PartidoPoliticoRepository : GenericRepository<PartidoPolitico>, IPartidoPoliticoRepository
    {
        public PartidoPoliticoRepository(AppDbContext context) : base(context)
        {
        }
    }
}