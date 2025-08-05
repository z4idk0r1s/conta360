// ConvocatoriaService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ConvocatoriaService : BaseService<Convocatoria, ConvocatoriaDto>
    {
        public ConvocatoriaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Convocatoria> GetRepository()
        {
            return _unitOfWork.Convocatorias;
        }
    }
}