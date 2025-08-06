// SuscripcionService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class SuscripcionService : BaseService<Suscripcion, SuscripcionDto>
    {
        public SuscripcionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Suscripcion> GetRepository()
        {
            return _unitOfWork.Suscripciones;
        }
    }
}