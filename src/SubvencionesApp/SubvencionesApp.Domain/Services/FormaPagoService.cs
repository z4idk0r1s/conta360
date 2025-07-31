using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class FormaPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FormaPagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FormaPagoDto>> GetAllAsync()
        {
            var formasPago = await _unitOfWork.FormasPago.GetAllAsync();
            return formasPago.Select(f => new FormaPagoDto { Id = f.Id, Descripcion = f.Descripcion });
        }
    }
}