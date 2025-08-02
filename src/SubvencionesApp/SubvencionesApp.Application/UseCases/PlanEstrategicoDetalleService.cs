using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class PlanEstrategicoDetalleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanEstrategicoDetalleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanEstrategicoDetalleDto>> GetAllAsync()
        {
            var planesEstrategicosDetalle = await _unitOfWork.PlanesEstrategicosDetalle.GetAllAsync();
            return planesEstrategicosDetalle.Select(ped => new PlanEstrategicoDetalleDto
            {
                Id = ped.Id,
                Nombre = ped.Nombre,
                Descripcion = ped.Descripcion,
                Estado = ped.Estado,
                FechaAprobacion = ped.FechaAprobacion
            });
        }
    }
}