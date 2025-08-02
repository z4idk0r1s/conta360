using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class AyudaEstadoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AyudaEstadoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AyudaEstadoDto>> GetAllAsync()
        {
            var ayudasEstados = await _unitOfWork.AyudasEstados.GetAllAsync();
            return ayudasEstados.Select(ae => new AyudaEstadoDto
            {
                Id = ae.Id,
                Nombre = ae.Nombre,
                Descripcion = ae.Descripcion,
                InstrumentoId = ae.InstrumentoId,
                TipoBeneficiarioId = ae.TipoBeneficiarioId,
                Estado = ae.Estado
            });
        }
    }
}