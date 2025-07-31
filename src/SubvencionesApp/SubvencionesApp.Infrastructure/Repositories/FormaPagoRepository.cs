using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;

namespace SubvencionesApp.Infrastructure.Repositories
{
    public class FormaPagoRepository : GenericRepository<FormaPago>, IFormaPagoRepository
    {
        public FormaPagoRepository(AppDbContext context) : base(context)
        {
        }
    }
}