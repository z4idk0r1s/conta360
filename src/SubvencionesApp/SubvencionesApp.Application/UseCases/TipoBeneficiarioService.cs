// TipoBeneficiarioService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoBeneficiarioService : BaseService<TipoBeneficiario, TipoBeneficiarioDto>
    {
        public TipoBeneficiarioService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<TipoBeneficiario> GetRepository()
        {
            return _unitOfWork.TiposBeneficiario;
        }
    }
}