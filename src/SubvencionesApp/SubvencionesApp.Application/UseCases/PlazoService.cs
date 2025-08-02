using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class PlazoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlazoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlazoDto>> GetAllAsync()
        {
            var plazos = await _unitOfWork.Plazos.GetAllAsync();
            return plazos.Select(p => new PlazoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                ConvocatoriaId = p.ConvocatoriaId
            });
        }
    }
}