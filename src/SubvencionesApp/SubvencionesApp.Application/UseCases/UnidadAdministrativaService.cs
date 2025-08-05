// UnidadAdministrativaService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class UnidadAdministrativaService : BaseService<UnidadAdministrativa, UnidadAdministrativaDto>
    {
        public UnidadAdministrativaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<UnidadAdministrativa> GetRepository()
        {
            return _unitOfWork.UnidadesAdministrativas;
        }
    }
}