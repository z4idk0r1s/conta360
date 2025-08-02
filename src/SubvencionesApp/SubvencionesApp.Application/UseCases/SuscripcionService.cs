using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class SuscripcionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SuscripcionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SuscripcionDto>> GetAllAsync()
        {
            var suscripciones = await _unitOfWork.Suscripciones.GetAllAsync();
            return suscripciones.Select(s => new SuscripcionDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Email = s.Email,
                FechaInicio = s.FechaInicio,
                Activa = s.Activa
            });
        }
    }
}