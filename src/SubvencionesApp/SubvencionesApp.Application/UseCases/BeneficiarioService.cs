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
        internal async Task<BeneficiarioDto?> GetByIdAsync(Guid id)
        {
            // 1. Llama de forma asíncrona al método del repositorio.
            var entity = await _unitOfWork.Beneficiarios.GetByIdAsync(id);

            // 2. Mapea la entidad al DTO y retorna el resultado.
            return _mapper.Map<BeneficiarioDto>(entity);
        }
    }
}