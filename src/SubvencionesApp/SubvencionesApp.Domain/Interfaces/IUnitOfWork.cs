using System;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccionRepository Acciones { get; }
        IAgrupacionRepository Agrupaciones { get; }
        IAreaRepository Areas { get; }
        IBeneficiarioRepository Beneficiarios { get; }
        IConcesionRepository Concesiones { get; }
        IConvocatoriaRepository Convocatorias { get; }
        IDatosEstadisticosRepository DatosEstadisticos { get; }
        IEntidadRepository Entidades { get; }
        IEstadoRepository Estados { get; }
        IFormaPagoRepository FormasPago { get; }
        ILineaRepository Lineas { get; }
        IMunicipioRepository Municipios { get; }
        IOrganismoRepository Organismos { get; }
        IProgramaRepository Programas { get; }
        IProvinciaRepository Provincias { get; }
        ISectorRepository Sectores { get; }
        ISituacionEntornoRepository SituacionesEntorno { get; }
        ISubtipoSubvencionRepository SubtiposSubvencion { get; }
        ITipoBeneficiarioRepository TiposBeneficiario { get; }
        ITipoConvocatoriaRepository TiposConvocatoria { get; }
        ITipoOrganismoRepository TiposOrganismo { get; }
        ITipoSubvencionRepository TiposSubvencion { get; }
        ITramoRepository Tramos { get; }
        IUnidadAdministrativaRepository UnidadesAdministrativas { get; }
       
        Task<int> CommitAsync();
        Task RollbackAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
    }
}