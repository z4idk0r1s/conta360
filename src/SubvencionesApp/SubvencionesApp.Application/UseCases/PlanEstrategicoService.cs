using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class PlanEstrategicoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanEstrategicoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanEstrategicoDto>> GetAllAsync()
        {
            var planesEstrategicos = await _unitOfWork.PlanesEstrategicos.GetAllAsync();
            return planesEstrategicos.Select(pe => new PlanEstrategicoDto
            {
                Id = pe.Id,
                Nombre = pe.Nombre,
                Descripcion = pe.Descripcion,
                Estado = pe.Estado,
                FechaAprobacion = pe.FechaAprobacion
            });
        }
    }
}