// ConcesionService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ConcesionService : BaseService<Concesion, ConcesionDto>
    {
        public ConcesionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Concesion> GetRepository()
        {
            return _unitOfWork.Concesiones;
        }
    }
}