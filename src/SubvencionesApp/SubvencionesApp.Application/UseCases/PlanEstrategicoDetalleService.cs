// PlanEstrategicoDetalleService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class PlanEstrategicoDetalleService : BaseService<PlanEstrategicoDetalle, PlanEstrategicoDetalleDto>
    {
        public PlanEstrategicoDetalleService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<PlanEstrategicoDetalle> GetRepository()
        {
            return _unitOfWork.PlanesEstrategicosDetalle;
        }
    }
}