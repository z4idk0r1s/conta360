using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class InstrumentoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InstrumentoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InstrumentoDto>> GetAllAsync()
        {
            var instrumentos = await _unitOfWork.Instrumentos.GetAllAsync();
            return instrumentos.Select(i => new InstrumentoDto
            {
                Id = i.Id,
                Nombre = i.Nombre,
                Descripcion = i.Descripcion
            });
        }
    }
}