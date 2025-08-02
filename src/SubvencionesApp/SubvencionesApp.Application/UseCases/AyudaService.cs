using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class AyudaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AyudaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AyudaDto>> GetAllAsync()
        {
            var ayudas = await _unitOfWork.Ayudas.GetAllAsync();
            return ayudas.Select(a => new AyudaDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                OrganismoId = a.OrganismoId,
                RegionId = a.RegionId,
                TipoBeneficiarioId = a.TipoBeneficiarioId,
                InstrumentoId = a.InstrumentoId
            });
        }
    }
}