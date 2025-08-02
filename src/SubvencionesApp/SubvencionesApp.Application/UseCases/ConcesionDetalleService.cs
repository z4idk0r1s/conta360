using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ConcesionDetalleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConcesionDetalleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ConcesionDetalleDto>> GetAllAsync()
        {
            var concesionesDetalle = await _unitOfWork.ConcesionesDetalle.GetAllAsync();
            return concesionesDetalle.Select(cd => new ConcesionDetalleDto
            {
                Id = cd.Id,
                Nombre = cd.Nombre,
                Descripcion = cd.Descripcion,
                Detalles = cd.Detalles,
                Importe = cd.Importe,
                BeneficiarioId = cd.BeneficiarioId,
                OrganismoId = cd.OrganismoId,
                FechaResolucion = cd.FechaResolucion
            });
        }
    }
}