using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class GrandeBeneficiarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrandeBeneficiarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GrandeBeneficiarioDto>> GetAllAsync()
        {
            var grandesBeneficiarios = await _unitOfWork.GrandesBeneficiarios.GetAllAsync();
            return grandesBeneficiarios.Select(gb => new GrandeBeneficiarioDto
            {
                Id = gb.Id,
                Nombre = gb.Nombre,
                Tipo = gb.Tipo,
                Importe = gb.Importe
            });
        }
    }
}