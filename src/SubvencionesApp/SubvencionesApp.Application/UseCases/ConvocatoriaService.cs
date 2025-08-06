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
    public class ConvocatoriaService : BaseService<Convocatoria, ConvocatoriaDto>
    {
        public ConvocatoriaService(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Convocatoria> GetRepository()
        {
            return _unitOfWork.Convocatorias;
        }

        public async Task<ConvocatoriaDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Convocatorias.GetByIdAsync(id);
            return _mapper.Map<ConvocatoriaDto>(entity);
        }
    }
}