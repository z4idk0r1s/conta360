using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.UseCases.Commons;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ConcesionService : BaseService<Concesion, ConcesionDto>
    {
        public ConcesionService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Concesion> GetRepository()
        {
            return _unitOfWork.Concesiones;
        }

        public async Task<ConcesionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Concesiones.GetByIdAsync(id);
            return _mapper.Map<ConcesionDto>(entity);
        }   
    }
}