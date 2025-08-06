// MunicipioService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class MunicipioService : BaseService<Municipio, MunicipioDto>
    {
        public MunicipioService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Municipio> GetRepository()
        {
            return _unitOfWork.Municipios;
        }
    }
}