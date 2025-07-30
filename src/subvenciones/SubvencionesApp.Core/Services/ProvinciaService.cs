using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class ProvinciaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProvinciaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProvinciaDto>> GetAllAsync()
        {
            var provincias = await _unitOfWork.Provincias.GetAllAsync();
            return provincias.Select(p => new ProvinciaDto { Id = p.Id, Descripcion = p.Descripcion });
        }
    }
}