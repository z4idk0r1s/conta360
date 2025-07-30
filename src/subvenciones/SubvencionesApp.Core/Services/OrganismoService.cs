using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class OrganismoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganismoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OrganismoDto>> GetAllAsync()
        {
            var organismos = await _unitOfWork.Organismos.GetAllAsync();
            return organismos.Select(o => new OrganismoDto { Id = o.Id, Descripcion = o.Descripcion });
        }
    }
}