// GrandeBeneficiarioService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class GrandeBeneficiarioService : BaseService<GrandeBeneficiario, GrandeBeneficiarioDto>
    {
        public GrandeBeneficiarioService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<GrandeBeneficiario> GetRepository()
        {
            return _unitOfWork.GrandesBeneficiarios;
        }
    }
}