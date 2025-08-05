// ConcesionDetalleService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ConcesionDetalleService : BaseService<ConcesionDetalle, ConcesionDetalleDto>
    {
        public ConcesionDetalleService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<ConcesionDetalle> GetRepository()
        {
            return _unitOfWork.ConcesionesDetalle;
        }
    }
}