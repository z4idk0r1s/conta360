using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ConvocatoriaDetalleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConvocatoriaDetalleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ConvocatoriaDetalleDto>> GetAllAsync()
        {
            var convocatoriasDetalle = await _unitOfWork.ConvocatoriasDetalle.GetAllAsync();
            return convocatoriasDetalle.Select(cd => new ConvocatoriaDetalleDto
            {
                Id = cd.Id,
                Nombre = cd.Nombre,
                Descripcion = cd.Descripcion,
                Detalles = cd.Detalles,
                Estado = cd.Estado,
                FechaInicio = cd.FechaInicio,
                FechaFin = cd.FechaFin,
                FechaPublicacion = cd.FechaPublicacion,
                OrganismoId = cd.OrganismoId,
                RegionId = cd.RegionId,
                TipoBeneficiarioId = cd.TipoBeneficiarioId,
                InstrumentoId = cd.InstrumentoId
            });
        }
    }
}