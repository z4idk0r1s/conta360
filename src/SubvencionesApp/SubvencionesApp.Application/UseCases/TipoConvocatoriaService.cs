using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoConvocatoriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoConvocatoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TipoConvocatoriaDto>> GetAllAsync()
        {
            var tipos = await _unitOfWork.TiposConvocatoria.GetAllAsync();
            return tipos.Select(t => new TipoConvocatoriaDto { Id = t.Id, Descripcion = t.Descripcion });
        }
    }
}