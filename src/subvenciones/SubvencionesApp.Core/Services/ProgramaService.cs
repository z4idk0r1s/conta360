using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class ProgramaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProgramaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProgramaDto>> GetAllAsync()
        {
            var programas = await _unitOfWork.Programas.GetAllAsync();
            return programas.Select(p => new ProgramaDto { Id = p.Id, Codigo = p.Codigo, Descripcion = p.Descripcion });
        }
    }
}