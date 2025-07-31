using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Database
{
    public class FormaPagoRepository : GenericRepository<FormaPago>, IFormaPagoRepository
    {
        public FormaPagoRepository(AppDbContext context) : base(context)
        {
        }
    }
}