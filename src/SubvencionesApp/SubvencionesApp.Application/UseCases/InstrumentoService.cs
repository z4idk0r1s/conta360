// InstrumentoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class InstrumentoService : BaseService<Instrumento, InstrumentoDto>
    {
        public InstrumentoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Instrumento> GetRepository()
        {
            return _unitOfWork.Instrumentos;
        }
    }
}