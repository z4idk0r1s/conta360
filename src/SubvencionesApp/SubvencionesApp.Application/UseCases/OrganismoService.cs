// OrganismoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class OrganismoService : BaseService<Organismo, OrganismoDto>
    {
        public OrganismoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Organismo> GetRepository()
        {
            return _unitOfWork.Organismos;
        }
    }
}