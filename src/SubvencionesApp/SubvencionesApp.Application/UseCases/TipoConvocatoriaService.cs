// TipoConvocatoriaService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoConvocatoriaService : BaseService<TipoConvocatoria, TipoConvocatoriaDto>
    {
        public TipoConvocatoriaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<TipoConvocatoria> GetRepository()
        {
            return _unitOfWork.TiposConvocatoria;
        }
    }
}