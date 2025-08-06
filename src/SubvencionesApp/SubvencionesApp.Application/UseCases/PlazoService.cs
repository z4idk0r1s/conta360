// PlazoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class PlazoService : BaseService<Plazo, PlazoDto>
    {
        public PlazoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Plazo> GetRepository()
        {
            return _unitOfWork.Plazos;
        }
    }
}