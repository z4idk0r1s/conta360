// ConvocatoriaDetalleService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ConvocatoriaDetalleService : BaseService<ConvocatoriaDetalle, ConvocatoriaDetalleDto>
    {
        public ConvocatoriaDetalleService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<ConvocatoriaDetalle> GetRepository()
        {
            return _unitOfWork.ConvocatoriasDetalle;
        }
    }
}