using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class MunicipioRepository : GenericRepository<Municipio>, IMunicipioRepository
    {
        public MunicipioRepository(AppDbContext context) : base(context)
        {
        }
    }
}