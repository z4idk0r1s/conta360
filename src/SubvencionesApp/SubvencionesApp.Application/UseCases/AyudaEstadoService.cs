// AyudaEstadoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class AyudaEstadoService : BaseService<AyudaEstado, AyudaEstadoDto>
    {
        public AyudaEstadoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<AyudaEstado> GetRepository()
        {
            return _unitOfWork.AyudasEstados;
        }
    }
}