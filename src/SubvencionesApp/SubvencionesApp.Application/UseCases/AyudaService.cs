// AyudaService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class AyudaService : BaseService<Ayuda, AyudaDto>
    {
        public AyudaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Ayuda> GetRepository()
        {
            return _unitOfWork.Ayudas;
        }
    }
}