// BaseService.cs
using AutoMapper;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases.Commons
{
    public abstract class BaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        protected abstract IGenericRepository<TEntity> GetRepository();

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await GetRepository().GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }
    }
}