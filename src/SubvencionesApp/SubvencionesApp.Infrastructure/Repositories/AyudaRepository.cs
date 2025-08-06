using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class AyudaRepository : GenericRepository<Ayuda>, IAyudaRepository
    {
        public AyudaRepository(AppDbContext context) : base(context)
        {
        }
    }
}