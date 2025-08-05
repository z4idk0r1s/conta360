// PlanEstrategicoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class PlanEstrategicoService : BaseService<PlanEstrategico, PlanEstrategicoDto>
    {
        public PlanEstrategicoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<PlanEstrategico> GetRepository()
        {
            return _unitOfWork.PlanesEstrategicos;
        }
    }
}