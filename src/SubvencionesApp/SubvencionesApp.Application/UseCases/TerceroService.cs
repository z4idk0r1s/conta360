using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class TerceroService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TerceroService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TerceroDto>> GetAllAsync()
        {
            var terceros = await _unitOfWork.Terceros.GetAllAsync();
            return terceros.Select(t => new TerceroDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Nif = t.Nif,
                Tipo = t.Tipo
            });
        }
    }
}