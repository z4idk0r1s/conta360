using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class OrganosCodigoAdminService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganosCodigoAdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrganosCodigoAdminDto>> GetAllAsync()
        {
            var organosCodigoAdmin = await _unitOfWork.OrganosCodigoAdmin.GetAllAsync();
            return organosCodigoAdmin.Select(oca => new OrganosCodigoAdminDto
            {
                Id = oca.Id,
                CodigoAdmin = oca.CodigoAdmin,
                Nombre = oca.Nombre
            });
        }
    }
}