using System;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccionRepository Acciones { get; }
        IAgrupacionRepository Agrupaciones { get; }
        IAreaRepository Areas { get; }
        IAyudaRepository Ayudas { get; }
        IAyudaEstadoRepository AyudasEstados { get; }
        IBeneficiarioRepository Beneficiarios { get; }
        IConcesionRepository Concesiones { get; }
        IConcesionDetalleRepository ConcesionesDetalle { get; }
        IConvocatoriaRepository Convocatorias { get; }
        IConvocatoriaDetalleRepository ConvocatoriasDetalle { get; }
        IDatosEstadisticosRepository DatosEstadisticos { get; }
        IEntidadRepository Entidades { get; }
        IEstadoRepository Estados { get; }
        IFinalidadRepository Finalidades { get; }
        IFormaPagoRepository FormasPago { get; }
        IGrandeBeneficiarioRepository GrandesBeneficiarios { get; }
        IInstrumentoRepository Instrumentos { get; }
        ILineaRepository Lineas { get; }
        IMinimisRepository Minimis { get; }
        IMunicipioRepository Municipios { get; }
        IObjetivoRepository Objetivos { get; }
        IOrganismoRepository Organismos { get; }
        IOrganosCodigoAdminRepository OrganosCodigoAdmin { get; }
        IPartidoPoliticoRepository PartidosPoliticos { get; }
        IPlanEstrategicoRepository PlanesEstrategicos { get; }
        IPlanEstrategicoDetalleRepository PlanesEstrategicosDetalle { get; }
        IPlazoRepository Plazos { get; }
        IProgramaRepository Programas { get; }
        IProvinciaRepository Provincias { get; }
        IRegionRepository Regiones { get; }
        IReglamentoRepository Reglamentos { get; }
        ISancionRepository Sanciones { get; }
        ISancionDetalleRepository SancionesDetalle { get; }
        ISectorRepository Sectores { get; }
        ISectorProductoRepository SectoresProductos { get; }
        ISituacionEntornoRepository SituacionesEntorno { get; }
        ISubtipoSubvencionRepository SubtiposSubvencion { get; }
        ISuscripcionRepository Suscripciones { get; }
        ITerceroRepository Terceros { get; }
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