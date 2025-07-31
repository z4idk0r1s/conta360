using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Services
{
    public class TipoSubvencionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoSubvencionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TipoSubvencionDto>> GetAllAsync()
        {
            var tipos = await _unitOfWork.TiposSubvencion.GetAllAsync();
            return tipos.Select(t => new TipoSubvencionDto { Id = t.Id, Descripcion = t.Descripcion });
        }
    }
}