using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class PartidoPoliticoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartidoPoliticoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PartidoPoliticoDto>> GetAllAsync()
        {
            var partidosPoliticos = await _unitOfWork.PartidosPoliticos.GetAllAsync();
            return partidosPoliticos.Select(pp => new PartidoPoliticoDto
            {
                Id = pp.Id,
                Nombre = pp.Nombre,
                Importe = pp.Importe,
                Fecha = pp.Fecha,
                OrganismoId = pp.OrganismoId
            });
        }
    }
}