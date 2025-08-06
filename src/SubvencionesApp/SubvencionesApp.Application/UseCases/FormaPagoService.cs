// FormaPagoService.cs
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;

namespace SubvencionesApp.Application.UseCases
{
    public class FormaPagoService : BaseService<FormaPago, FormaPagoDto>
    {
        public FormaPagoService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<FormaPago> GetRepository()
        {
            return _unitOfWork.FormasPago;
        }
    }
}