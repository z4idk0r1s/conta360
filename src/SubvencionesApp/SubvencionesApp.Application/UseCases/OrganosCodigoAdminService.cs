// OrganosCodigoAdminService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class OrganosCodigoAdminService : BaseService<OrganosCodigoAdmin, OrganosCodigoAdminDto>
    {
        public OrganosCodigoAdminService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<OrganosCodigoAdmin> GetRepository()
        {
            return _unitOfWork.OrganosCodigoAdmin;
        }
    }
}