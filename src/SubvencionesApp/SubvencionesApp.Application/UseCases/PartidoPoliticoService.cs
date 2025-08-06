// PartidoPoliticoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class PartidoPoliticoService : BaseService<PartidoPolitico, PartidoPoliticoDto>
    {
        public PartidoPoliticoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<PartidoPolitico> GetRepository()
        {
            return _unitOfWork.PartidosPoliticos;
        }
    }
}