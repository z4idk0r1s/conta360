// AgrupacionService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class AgrupacionService : BaseService<Agrupacion, AgrupacionDto>
    {
        public AgrupacionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Agrupacion> GetRepository()
        {
            return _unitOfWork.Agrupaciones;
        }
    }
}