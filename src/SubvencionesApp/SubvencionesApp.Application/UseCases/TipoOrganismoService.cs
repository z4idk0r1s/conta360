using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoOrganismoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoOrganismoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TipoOrganismoDto>> GetAllAsync()
        {
            var tipos = await _unitOfWork.TiposOrganismo.GetAllAsync();
            return tipos.Select(t => new TipoOrganismoDto { Id = t.Id, Descripcion = t.Descripcion });
        }
    }
}