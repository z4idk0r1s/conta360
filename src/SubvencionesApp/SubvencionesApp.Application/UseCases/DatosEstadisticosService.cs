// DatosEstadisticosService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class DatosEstadisticosService : BaseService<DatosEstadisticos, DatosEstadisticosDto>
    {
        public DatosEstadisticosService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<DatosEstadisticos> GetRepository()
        {
            return _unitOfWork.DatosEstadisticos;
        }
    }
}