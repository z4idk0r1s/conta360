using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class MunicipioRepository : GenericRepository<Municipio>, IMunicipioRepository
    {
        public MunicipioRepository(AppDbContext context) : base(context)
        {
        }
    }
}