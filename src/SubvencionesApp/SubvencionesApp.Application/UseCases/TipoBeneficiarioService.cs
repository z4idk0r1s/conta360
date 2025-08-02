using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class TipoBeneficiarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TipoBeneficiarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TipoBeneficiarioDto>> GetAllAsync()
        {
            var tiposBeneficiario = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            return tiposBeneficiario.Select(tb => new TipoBeneficiarioDto
            {
                Id = tb.Id,
                Nombre = tb.Nombre,
                Descripcion = tb.Descripcion
            });
        }
    }
}