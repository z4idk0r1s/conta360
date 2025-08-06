// SubtipoSubvencionService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class SubtipoSubvencionService : BaseService<SubtipoSubvencion, SubtipoSubvencionDto>
    {
        public SubtipoSubvencionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<SubtipoSubvencion> GetRepository()
        {
            return _unitOfWork.SubtiposSubvencion;
        }
    }
}