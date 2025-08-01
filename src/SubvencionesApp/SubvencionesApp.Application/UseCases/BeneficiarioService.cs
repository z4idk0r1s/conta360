using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class BeneficiarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BeneficiarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BeneficiarioDto>> GetAllAsync()
        {
            var beneficiarios = await _unitOfWork.Beneficiarios.GetAllAsync();
            return beneficiarios.Select(b => new BeneficiarioDto { Id = b.Id, Nombre = b.Nombre, Identificacion = b.Identificacion });
        }
    }
}