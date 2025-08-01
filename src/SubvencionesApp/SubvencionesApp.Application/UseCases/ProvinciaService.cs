using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
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