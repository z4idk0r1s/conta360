// TipoSubvencionService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoSubvencionService : BaseService<TipoSubvencion, TipoSubvencionDto>
    {
        public TipoSubvencionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<TipoSubvencion> GetRepository()
        {
            return _unitOfWork.TiposSubvencion;
        }
    }
}