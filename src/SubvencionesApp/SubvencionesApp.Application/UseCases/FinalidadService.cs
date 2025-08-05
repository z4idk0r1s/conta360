// FinalidadService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class FinalidadService : BaseService<Finalidad, FinalidadDto>
    {
        public FinalidadService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Finalidad> GetRepository()
        {
            return _unitOfWork.Finalidades;
        }
    }
}