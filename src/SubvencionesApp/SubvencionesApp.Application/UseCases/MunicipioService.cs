using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class MunicipioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MunicipioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MunicipioDto>> GetAllAsync()
        {
            var municipios = await _unitOfWork.Municipios.GetAllAsync();
            return municipios.Select(m => new MunicipioDto { Id = m.Id, Descripcion = m.Descripcion });
        }
    }
}