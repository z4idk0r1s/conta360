using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Services
{
    public class UnidadAdministrativaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnidadAdministrativaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UnidadAdministrativaDto>> GetAllAsync()
        {
            var unidades = await _unitOfWork.UnidadesAdministrativas.GetAllAsync();
            return unidades.Select(u => new UnidadAdministrativaDto { Id = u.Id, Descripcion = u.Descripcion });
        }
    }
}