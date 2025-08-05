// BeneficiarioService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class BeneficiarioService : BaseService<Beneficiario, BeneficiarioDto>
    {
        public BeneficiarioService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Beneficiario> GetRepository()
        {
            return _unitOfWork.Beneficiarios;
        }
    }
}