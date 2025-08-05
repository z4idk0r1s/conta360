// SituacionEntornoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class SituacionEntornoService : BaseService<SituacionEntorno, SituacionEntornoDto>
    {
        public SituacionEntornoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<SituacionEntorno> GetRepository()
        {
            return _unitOfWork.SituacionesEntorno;
        }
    }
}