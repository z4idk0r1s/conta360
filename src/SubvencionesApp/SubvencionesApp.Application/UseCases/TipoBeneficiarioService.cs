using SubvencionesApp.Domain.Dtos;
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
            var tipos = await _unitOfWork.TiposBeneficiario.GetAllAsync();
            return tipos.Select(t => new TipoBeneficiarioDto { Id = t.Id, Descripcion = t.Descripcion });
        }
    }
}