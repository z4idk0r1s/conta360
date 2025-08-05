// SectorProductoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class SectorProductoService : BaseService<SectorProducto, SectorProductoDto>
    {
        public SectorProductoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<SectorProducto> GetRepository()
        {
            return _unitOfWork.SectoresProductos;
        }
    }
}