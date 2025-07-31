using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
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