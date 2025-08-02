using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class FinalidadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FinalidadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FinalidadDto>> GetAllAsync()
        {
            var finalidades = await _unitOfWork.Finalidades.GetAllAsync();
            return finalidades.Select(f => new FinalidadDto
            {
                Id = f.Id,
                Nombre = f.Nombre,
                Descripcion = f.Descripcion
            });
        }
    }
}