// ProvinciaService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ProvinciaService : BaseService<Provincia, ProvinciaDto>
    {
        public ProvinciaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Provincia> GetRepository()
        {
            return _unitOfWork.Provincias;
        }
    }
}