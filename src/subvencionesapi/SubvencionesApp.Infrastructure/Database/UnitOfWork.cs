using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Infrastructure.Database;
using System;
using System.Threading.Tasks;

namespace SubvencionesApp.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IAccionRepository Acciones { get; }
        public IAgrupacionRepository Agrupaciones { get; }
        public IAreaRepository Areas { get; }
        public IBeneficiarioRepository Beneficiarios { get; }
        public IConcesionRepository Concesiones { get; }
        public IConvocatoriaRepository Convocatorias { get; }
        public IEntidadRepository Entidades { get; }
        public IEstadoRepository Estados { get; }
        public IFormaPagoRepository FormasPago { get; }
        public ILineaRepository Lineas { get; }
        public IMunicipioRepository Municipios { get; }
        public IOrganismoRepository Organismos { get; }
        public IProgramaRepository Programas { get; }
        public IProvinciaRepository Provincias { get; }
        public ISectorRepository Sectores { get; }
        public ISituacionEntornoRepository SituacionesEntorno { get; }
        public ISubtipoSubvencionRepository SubtiposSubvencion { get; }
        public ITipoBeneficiarioRepository TiposBeneficiario { get; }
        public ITipoConvocatoriaRepository TiposConvocatoria { get; }
        public ITipoOrganismoRepository TiposOrganismo { get; }
        public ITipoSubvencionRepository TiposSubvencion { get; }
        public ITramoRepository Tramos { get; }
        public IUnidadAdministrativaRepository UnidadesAdministrativas { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Acciones = new AccionRepository(context);
            Agrupaciones = new AgrupacionRepository(context);
            Areas = new AreaRepository(context);
            Beneficiarios = new BeneficiarioRepository(context);
            Concesiones = new ConcesionRepository(context);
            Convocatorias = new ConvocatoriaRepository(context);
            Entidades = new EntidadRepository(context);
            Estados = new EstadoRepository(context);
            FormasPago = new FormaPagoRepository(context);
            Lineas = new LineaRepository(context);
            Municipios = new MunicipioRepository(context);
            Organismos = new OrganismoRepository(context);
            Programas = new ProgramaRepository(context);
            Provincias = new ProvinciaRepository(context);
            Sectores = new SectorRepository(context);
            SituacionesEntorno = new SituacionEntornoRepository(context);
            SubtiposSubvencion = new SubtipoSubvencionRepository(context);
            TiposBeneficiario = new TipoBeneficiarioRepository(context);
            TiposConvocatoria = new TipoConvocatoriaRepository(context);
            TiposOrganismo = new TipoOrganismoRepository(context);
            TiposSubvencion = new TipoSubvencionRepository(context);
            Tramos = new TramoRepository(context);
            UnidadesAdministrativas = new UnidadAdministrativaRepository(context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}