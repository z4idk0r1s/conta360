using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class LineaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LineaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LineaDto>> GetAllAsync()
        {
            var lineas = await _unitOfWork.Lineas.GetAllAsync();
            return lineas.Select(l => new LineaDto { Id = l.Id, Codigo = l.Codigo, Nombre = l.Nombre });
        }
    }
}