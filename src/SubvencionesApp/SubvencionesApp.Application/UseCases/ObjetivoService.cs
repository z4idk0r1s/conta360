using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ObjetivoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObjetivoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ObjetivoDto>> GetAllAsync()
        {
            var objetivos = await _unitOfWork.Objetivos.GetAllAsync();
            return objetivos.Select(o => new ObjetivoDto
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Descripcion = o.Descripcion
            });
        }
    }
}