// ObjetivoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class ObjetivoService : BaseService<Objetivo, ObjetivoDto>
    {
        public ObjetivoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Objetivo> GetRepository()
        {
            return _unitOfWork.Objetivos;
        }
    }
}