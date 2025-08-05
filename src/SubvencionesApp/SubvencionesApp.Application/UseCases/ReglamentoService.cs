// ReglamentoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ReglamentoService : BaseService<Reglamento, ReglamentoDto>
    {
        public ReglamentoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Reglamento> GetRepository()
        {
            return _unitOfWork.Reglamentos;
        }
    }
}