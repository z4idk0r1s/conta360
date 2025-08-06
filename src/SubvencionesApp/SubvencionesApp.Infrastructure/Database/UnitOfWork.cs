using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace SubvencionesApp.Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Lazy initialization de repositorios
        private IAccionRepository? _acciones;
        private IAgrupacionRepository? _agrupaciones;
        private IAreaRepository? _areas;
        private IAyudaRepository? _ayudas;
        private IAyudaEstadoRepository? _ayudasEstados;
        private IBeneficiarioRepository? _beneficiarios;
        private IConcesionRepository? _concesiones;
        private IConcesionDetalleRepository? _concesionesDetalle;
        private IConvocatoriaRepository? _convocatorias;
        private IConvocatoriaDetalleRepository? _convocatoriasDetalle;
        private IDatosEstadisticosRepository? _datosEstadisticos;
        private IEntidadRepository? _entidades;
        private IEstadoRepository? _estados;
        private IFinalidadRepository? _finalidades;
        private IFormaPagoRepository? _formasPago;
        private IGrandeBeneficiarioRepository? _grandesBeneficiarios;
        private IInstrumentoRepository? _instrumentos;
        private ILineaRepository? _lineas;
        private IMinimisRepository? _minimis;
        private IMunicipioRepository? _municipios;
        private IObjetivoRepository? _objetivos;
        private IOrganismoRepository? _organismos;
        private IOrganosCodigoAdminRepository? _organosCodigoAdmin;
        private IPartidoPoliticoRepository? _partidosPoliticos;
        private IPlanEstrategicoRepository? _planesEstrategicos;
        private IPlanEstrategicoDetalleRepository? _planesEstrategicosDetalle;
        private IPlazoRepository? _plazos;
        private IProgramaRepository? _programas;
        private IProvinciaRepository? _provincias;
        private IRegionRepository? _regiones;
        private IReglamentoRepository? _reglamentos;
        private ISancionRepository? _sanciones;
        private ISancionDetalleRepository? _sancionesDetalle;
        private ISectorRepository? _sectores;
        private ISectorProductoRepository? _sectoresProductos;
        private ISituacionEntornoRepository? _situacionesEntorno;
        private ISubtipoSubvencionRepository? _subtiposSubvencion;
        private ISuscripcionRepository? _suscripciones;
        private ITerceroRepository? _terceros;
        private ITipoBeneficiarioRepository? _tiposBeneficiario;
        private ITipoConvocatoriaRepository? _tiposConvocatoria;
        private ITipoOrganismoRepository? _tiposOrganismo;
        private ITipoSubvencionRepository? _tiposSubvencion;
        private ITramoRepository? _tramos;
        private IUnidadAdministrativaRepository? _unidadesAdministrativas;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Propiedades con lazy loading para optimizar memoria
        public IAccionRepository Acciones => 
            _acciones ??= new AccionRepository(_context);

        public IAgrupacionRepository Agrupaciones => 
            _agrupaciones ??= new AgrupacionRepository(_context);

        public IAreaRepository Areas => 
            _areas ??= new AreaRepository(_context);

        public IAyudaRepository Ayudas => 
            _ayudas ??= new AyudaRepository(_context);

        public IAyudaEstadoRepository AyudasEstados => 
            _ayudasEstados ??= new AyudaEstadoRepository(_context);

        public IBeneficiarioRepository Beneficiarios => 
            _beneficiarios ??= new BeneficiarioRepository(_context);

        public IConcesionRepository Concesiones => 
            _concesiones ??= new ConcesionRepository(_context);

        public IConcesionDetalleRepository ConcesionesDetalle => 
            _concesionesDetalle ??= new ConcesionDetalleRepository(_context);

        public IConvocatoriaRepository Convocatorias => 
            _convocatorias ??= new ConvocatoriaRepository(_context);
        
        public IConvocatoriaDetalleRepository ConvocatoriasDetalle => 
            _convocatoriasDetalle ??= new ConvocatoriaDetalleRepository(_context);
        
        public IDatosEstadisticosRepository DatosEstadisticos => 
            _datosEstadisticos ??= new DatosEstadisticosRepository(_context);

        public IEntidadRepository Entidades => 
            _entidades ??= new EntidadRepository(_context);

        public IEstadoRepository Estados => 
            _estados ??= new EstadoRepository(_context);
            
        public IFinalidadRepository Finalidades => 
            _finalidades ??= new FinalidadRepository(_context);
            
        public IFormaPagoRepository FormasPago => 
            _formasPago ??= new FormaPagoRepository(_context);

        public IGrandeBeneficiarioRepository GrandesBeneficiarios => 
            _grandesBeneficiarios ??= new GrandeBeneficiarioRepository(_context);

        public IInstrumentoRepository Instrumentos => 
            _instrumentos ??= new InstrumentoRepository(_context);
            
        public ILineaRepository Lineas => 
            _lineas ??= new LineaRepository(_context);

        public IMinimisRepository Minimis => 
            _minimis ??= new MinimisRepository(_context);

        public IMunicipioRepository Municipios => 
            _municipios ??= new MunicipioRepository(_context);
            
        public IObjetivoRepository Objetivos => 
            _objetivos ??= new ObjetivoRepository(_context);

        public IOrganismoRepository Organismos => 
            _organismos ??= new OrganismoRepository(_context);
            
        public IOrganosCodigoAdminRepository OrganosCodigoAdmin => 
            _organosCodigoAdmin ??= new OrganosCodigoAdminRepository(_context);
        
        public IPartidoPoliticoRepository PartidosPoliticos =>
            _partidosPoliticos ??= new PartidoPoliticoRepository(_context);
        
        public IPlanEstrategicoRepository PlanesEstrategicos =>
            _planesEstrategicos ??= new PlanEstrategicoRepository(_context);
            
        public IPlanEstrategicoDetalleRepository PlanesEstrategicosDetalle =>
            _planesEstrategicosDetalle ??= new PlanEstrategicoDetalleRepository(_context);
        
        public IPlazoRepository Plazos =>
            _plazos ??= new PlazoRepository(_context);
            
        public IProgramaRepository Programas => 
            _programas ??= new ProgramaRepository(_context);

        public IProvinciaRepository Provincias => 
            _provincias ??= new ProvinciaRepository(_context);
            
        public IRegionRepository Regiones =>
            _regiones ??= new RegionRepository(_context);
            
        public IReglamentoRepository Reglamentos =>
            _reglamentos ??= new ReglamentoRepository(_context);

        public ISancionRepository Sanciones =>
            _sanciones ??= new SancionRepository(_context);
        
        public ISancionDetalleRepository SancionesDetalle =>
            _sancionesDetalle ??= new SancionDetalleRepository(_context);
        
        public ISectorRepository Sectores => 
            _sectores ??= new SectorRepository(_context);

        public ISectorProductoRepository SectoresProductos =>
            _sectoresProductos ??= new SectorProductoRepository(_context);
        
        public ISituacionEntornoRepository SituacionesEntorno => 
            _situacionesEntorno ??= new SituacionEntornoRepository(_context);

        public ISubtipoSubvencionRepository SubtiposSubvencion => 
            _subtiposSubvencion ??= new SubtipoSubvencionRepository(_context);
            
        public ISuscripcionRepository Suscripciones =>
            _suscripciones ??= new SuscripcionRepository(_context);
            
        public ITerceroRepository Terceros =>
            _terceros ??= new TerceroRepository(_context);

        public ITipoBeneficiarioRepository TiposBeneficiario => 
            _tiposBeneficiario ??= new TipoBeneficiarioRepository(_context);

        public ITipoConvocatoriaRepository TiposConvocatoria => 
            _tiposConvocatoria ??= new TipoConvocatoriaRepository(_context);

        public ITipoOrganismoRepository TiposOrganismo => 
            _tiposOrganismo ??= new TipoOrganismoRepository(_context);

        public ITipoSubvencionRepository TiposSubvencion => 
            _tiposSubvencion ??= new TipoSubvencionRepository(_context);

        public ITramoRepository Tramos => 
            _tramos ??= new TramoRepository(_context);

        public IUnidadAdministrativaRepository UnidadesAdministrativas => 
            _unidadesAdministrativas ??= new UnidadAdministrativaRepository(_context);

        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Error de concurrencia: Los datos fueron modificados por otro usuario.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error al guardar los cambios en la base de datos.", ex);
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Ya existe una transacción activa.");
            }

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No hay ninguna transacción activa para confirmar.");
            }

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            await RollbackTransactionAsync();
        }

        private async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context?.Dispose();
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}