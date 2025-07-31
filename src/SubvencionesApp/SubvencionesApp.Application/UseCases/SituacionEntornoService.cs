using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Services
{
    public class SituacionEntornoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SituacionEntornoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SituacionEntornoDto>> GetAllAsync()
        {
            var situacionesEntorno = await _unitOfWork.SituacionesEntorno.GetAllAsync();
            return situacionesEntorno.Select(s => new SituacionEntornoDto { Id = s.Id, Descripcion = s.Descripcion });
        }
    }
}