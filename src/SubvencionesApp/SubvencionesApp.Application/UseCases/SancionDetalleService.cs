// SancionDetalleService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class SancionDetalleService : BaseService<SancionDetalle, SancionDetalleDto>
    {
        public SancionDetalleService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<SancionDetalle> GetRepository()
        {
            return _unitOfWork.SancionesDetalle;
        }
    }
}