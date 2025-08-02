using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ReglamentoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReglamentoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReglamentoDto>> GetAllAsync()
        {
            var reglamentos = await _unitOfWork.Reglamentos.GetAllAsync();
            return reglamentos.Select(r => new ReglamentoDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Tipo = r.Tipo
            });
        }
    }
}