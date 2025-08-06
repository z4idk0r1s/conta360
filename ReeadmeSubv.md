conta360\src\SubvencionesApp>tree /F
Listado de rutas de carpetas
El número de serie del volumen es 6EE0-B276
C:.
│   Dockerfile
│
├───SubvencionesApp.Api
│   │   appsettings.Development.json
│   │   appsettings.json
│   │   Program.cs
│   │   SubvencionesApp.Api.csproj
│   │
│   ├───Clients
│   │       InfoSubvencionesApiClient.cs
│   │       SubvencionesService.cs
│   │
│   ├───Configurations
│   │       ApiConfiguration.cs
│   │       ApplicationServicesConfiguration.cs
│   │       DatabaseConfiguration.cs
│   │       DomainConfiguration.cs
│   │       InfrastructureConfiguration.cs
│   │       PipelineConfiguration.cs
│   │
│   └───Controllers
│           SubvencionesController.cs
│
├───SubvencionesApp.Application
│   │   SubvencionesApp.Application.csproj
│   │
│   ├───Dtos
│   │       AccionDto.cs
│   │       ActividadDto.cs
│   │       AgrupacionDto.cs
│   │       AreaDto.cs
│   │       AyudaDto.cs
│   │       AyudaEstadoDto.cs
│   │       BeneficiarioDto.cs
│   │       ConcesionDetalleDto.cs
│   │       ConcesionDto.cs
│   │       ConfiguracionMicroportalDto.cs
│   │       ConvocatoriaDetalleDto.cs
│   │       ConvocatoriaDto.cs
│   │       DatosEstadisticosDto.cs
│   │       EnlaceMicroVentanaDto.cs
│   │       EntidadDto.cs
│   │       EstadoDto.cs
│   │       FinalidadDto.cs
│   │       FormaPagoDto.cs
│   │       GrandeBeneficiarioDto.cs
│   │       InstrumentoDto.cs
│   │       LineaDto.cs
│   │       MinimisDto.cs
│   │       MunicipioDto.cs
│   │       ObjetivoDto.cs
│   │       OrganismoDto.cs
│   │       OrganosCodigoAdminDto.cs
│   │       PartidoPoliticoDto.cs
│   │       PlanEstrategicoDetalleDto.cs
│   │       PlanEstrategicoDto.cs
│   │       PlazoDto.cs
│   │       ProgramaDto.cs
│   │       ProvinciaDto.cs
│   │       RegionDto.cs
│   │       ReglamentoDto.cs
│   │       SancionDetalleDto.cs
│   │       SancionDto.cs
│   │       SectorDto.cs
│   │       SectorProductoDto.cs
│   │       SituacionEntornoDto.cs
│   │       SubtipoSubvencionDto.cs
│   │       SuscripcionDto.cs
│   │       TerceroDto.cs
│   │       TipoBeneficiarioDto.cs
│   │       TipoConvocatoriaDto.cs
│   │       TipoOrganismoDto.cs
│   │       TipoSubvencionDto.cs
│   │       TramoDto.cs
│   │       UnidadAdministrativaDto.cs
│   │
│   ├───Interfaces
│   │       IExternalSubvencionesService.cs
│   │       ISubvencionesService.cs
│   │       ISubvencionQueryService.cs
│   │       ISubvencionSyncService.cs
│   │
│   ├───Services
│   │       SubvencionQueryService.cs
│   │       SubvencionSyncService.cs
│   │       SubvencionSyncServiceOld.cs
│   │
│   └───UseCases
│           AccionService.cs
│           AgrupacionService.cs
│           AreaService.cs
│           AyudaEstadoService.cs
│           AyudaService.cs
│           BeneficiarioService.cs
│           ConcesionDetalleService.cs
│           ConcesionService.cs
│           ConvocatoriaDetalleService.cs
│           ConvocatoriaService.cs
│           DatosEstadisticosService.cs
│           EntidadService.cs
│           EstadoService.cs
│           FinalidadService.cs
│           FormaPagoService.cs
│           GrandeBeneficiarioService.cs
│           InstrumentoService.cs
│           LineaService.cs
│           MinimisService.cs
│           MunicipioService.cs
│           ObjetivoService.cs
│           OrganismoService.cs
│           OrganosCodigoAdminService.cs
│           PartidoPoliticoService.cs
│           PlanEstrategicoDetalleService.cs
│           PlanEstrategicoService.cs
│           PlazoService.cs
│           ProgramaService.cs
│           ProvinciaService.cs
│           RegionService.cs
│           ReglamentoService.cs
│           SancionDetalleService.cs
│           SancionService.cs
│           SectorProductoService.cs
│           SectorService.cs
│           SituacionEntornoService.cs
│           SubtipoSubvencionService.cs
│           SuscripcionService.cs
│           TerceroService.cs
│           TipoBeneficiarioService.cs
│           TipoConvocatoriaService.cs
│           TipoOrganismoService.cs
│           TipoSubvencionService.cs
│           TramoService.cs
│           UnidadAdministrativaService.cs
│
├───SubvencionesApp.Domain
│   │   SubvencionesApp.Domain.csproj
│   │
│   ├───Entities
│   │       Accion.cs
│   │       Actividad.cs
│   │       Agrupacion.cs
│   │       Area.cs
│   │       Ayuda.cs
│   │       AyudaEstado.cs
│   │       Beneficiario.cs
│   │       Concesion.cs
│   │       ConcesionDetalle.cs
│   │       ConfiguracionMicroportal.cs
│   │       Convocatoria.cs
│   │       ConvocatoriaDetalle.cs
│   │       DatosEstadisticos.cs
│   │       EnlaceMicroVentana.cs
│   │       Entidad.cs
│   │       Estado.cs
│   │       Finalidad.cs
│   │       FormaPago.cs
│   │       GrandeBeneficiario.cs
│   │       Instrumento.cs
│   │       Linea.cs
│   │       Minimis.cs
│   │       Municipio.cs
│   │       Objetivo.cs
│   │       Organismo.cs
│   │       OrganosCodigoAdmin.cs
│   │       PartidoPolitico.cs
│   │       PlanEstrategico.cs
│   │       PlanEstrategicoDetalle.cs
│   │       Plazo.cs
│   │       Programa.cs
│   │       Provincia.cs
│   │       Region.cs
│   │       Reglamento.cs
│   │       Sancion.cs
│   │       SancionDetalle.cs
│   │       Sector.cs
│   │       SectorProducto.cs
│   │       SituacionEntorno.cs
│   │       SubtipoSubvencion.cs
│   │       Suscripcion.cs
│   │       Tercero.cs
│   │       TipoBeneficiario.cs
│   │       TipoConvocatoria.cs
│   │       TipoOrganismo.cs
│   │       TipoSubvencion.cs
│   │       Tramo.cs
│   │       UnidadAdministrativa.cs
│   │
│   └───Interfaces
│           IAccionRepository.cs
│           IActividadRepository.cs
│           IAgrupacionRepository.cs
│           IAreaRepository.cs
│           IAyudaEstadoRepository.cs
│           IAyudaRepository.cs
│           IBeneficiarioRepository.cs
│           IConcesionDetalleRepository.cs
│           IConcesionRepository.cs
│           IConvocatoriaDetalleRepository.cs
│           IConvocatoriaRepository.cs
│           IDatosEstadisticosRepository.cs
│           IEntidadRepository.cs
│           IEstadoRepository.cs
│           IFinalidadRepository.cs
│           IFormaPagoRepository.cs
│           IGenericRepository.cs
│           IGrandeBeneficiarioRepository.cs
│           IInstrumentoRepository.cs
│           ILineaRepository.cs
│           IMinimisRepository.cs
│           IMunicipioRepository.cs
│           IObjetivoRepository.cs
│           IOrganismoRepository.cs
│           IOrganosCodigoAdminRepository.cs
│           IPartidoPoliticoRepository.cs
│           IPlanEstrategicoDetalleRepository.cs
│           IPlanEstrategicoRepository.cs
│           IPlazoRepository.cs
│           IProgramaRepository.cs
│           IProvinciaRepository.cs
│           IRegionRepository.cs
│           IReglamentoRepository.cs
│           ISancionDetalleRepository.cs
│           ISancionRepository.cs
│           ISectorProductoRepository.cs
│           ISectorRepository.cs
│           ISituacionEntornoRepository.cs
│           ISubtipoSubvencionRepository.cs
│           ISuscripcionRepository.cs
│           ITerceroRepository.cs
│           ITipoBeneficiarioRepository.cs
│           ITipoConvocatoriaRepository.cs
│           ITipoOrganismoRepository.cs
│           ITipoSubvencionRepository.cs
│           ITramoRepository.cs
│           IUnidadAdministrativaRepository.cs
│           IUnitOfWork.cs
│
└───SubvencionesApp.Infrastructure
    │   SubvencionesApp.Infrastructure.csproj
    │
    ├───Database
    │       AppDbContext.cs
    │       UnitOfWork.cs
    │
    ├───ExternalServices
    │   │   ExternalSubvencionesService.cs
    │   │   MappingHelpers.cs
    │   │   MappingProfile.cs
    │   │
    │   └───Models
    │           AccionApiModel.cs
    │           ActividadApiModel.cs
    │           AgrupacionApiModel.cs
    │           AreaApiModel.cs
    │           AyudaApiModel.cs
    │           AyudaEstadoApiModel.cs
    │           BeneficiarioApiModel.cs
    │           ConcesionApiModel.cs
    │           ConcesionDetalleApiModel.cs
    │           ConfiguracionMicroportalApiModel.cs
    │           ConvocatoriaApiModel.cs
    │           ConvocatoriaDetalleApiModel.cs
    │           DatosEstadisticosApiModel.cs
    │           EnlaceMicroVentanaApiModel.cs
    │           EntidadApiModel.cs
    │           EstadoApiModel.cs
    │           FinalidadApiModel.cs
    │           FormaPagoApiModel.cs
    │           GrandeBeneficiarioApiModel.cs
    │           InstrumentoApiModel.cs
    │           LineaApiModel.cs
    │           MinimisApiModel.cs
    │           MunicipioApiModel.cs
    │           ObjetivoApiModel.cs
    │           OrganismoApiModel.cs
    │           OrganosCodigoAdminApiModel.cs
    │           PartidoPoliticoApiModel.cs
    │           PlanEstrategicoApiModel.cs
    │           PlanEstrategicoDetalleApiModel.cs
    │           PlazoApiModel.cs
    │           ProgramaApiModel.cs
    │           ProvinciaApiModel.cs
    │           RegionApiModel.cs
    │           ReglamentoApiModel.cs
    │           SancionApiModel.cs
    │           SancionDetalleApiModel.cs
    │           SectorApiModel.cs
    │           SectorProductoApiModel.cs
    │           SituacionEntornoApiModel.cs
    │           SubtipoSubvencionApiModel.cs
    │           SuscripcionApiModel.cs
    │           TerceroApiModel.cs
    │           TipoBeneficiarioApiModel.cs
    │           TipoConvocatoriaApiModel.cs
    │           TipoOrganismoApiModel.cs
    │           TipoSubvencionApiModel.cs
    │           TramoApiModel.cs
    │           UnidadAdministrativaApiModel.cs
    │
    └───Repositories
            AccionRepository.cs
            ActividadRepository.cs
            AgrupacionRepository.cs
            AreaRepository.cs
            AyudaEstadoRepository.cs
            AyudaRepository.cs
            BeneficiarioRepository.cs
            ConcesionDetalleRepository.cs
            ConcesionRepository.cs
            ConvocatoriaDetalleRepository.cs
            ConvocatoriaRepository.cs
            DatosEstadisticosRepository.cs
            EntidadRepository.cs
            EstadoRepository.cs
            FinalidadRepository.cs
            FormaPagoRepository.cs
            GenericRepository.cs
            GrandeBeneficiarioRepository.cs
            InstrumentoRepository.cs
            LineaRepository.cs
            MinimisRepository.cs
            MunicipioRepository.cs
            ObjetivoRepository.cs
            OrganismoRepository.cs
            OrganosCodigoAdminRepository.cs
            PartidoPoliticoRepository.cs
            PlanEstrategicoDetalleRepository.cs
            PlanEstrategicoRepository.cs
            PlazoRepository.cs
            ProgramaRepository.cs
            ProvinciaRepository.cs
            RegionRepository.cs
            ReglamentoRepository.cs
            SancionDetalleRepository.cs
            SancionRepository.cs
            SectorProductoRepository.cs
            SectorRepository.cs
            SituacionEntornoRepository.cs
            SubtipoSubvencionRepository.cs
            SuscripcionRepository.cs
            TerceroRepository.cs
            TipoBeneficiarioRepository.cs
            TipoConvocatoriaRepository.cs
            TipoOrganismoRepository.cs
            TipoSubvencionRepository.cs
            TramoRepository.cs
            UnidadAdministrativaRepository.cs


    Integridad de los datos: Las claves foráneas (OrganismoId, BeneficiarioId, etc.) aseguran que las relaciones entre tablas sean siempre válidas. Por ejemplo, no puedes tener una Convocatoria asociada a un OrganismoId que no exista en la tabla Organismo.

    Reducción de la redundancia: La información se almacena en una sola ubicación (por ejemplo, los detalles del Beneficiario están en una tabla, y otras tablas solo guardan su Id), lo que ahorra espacio y evita inconsistencias.

    Flexibilidad para consultas complejas: La estructura te permite combinar los datos de diferentes tablas (JOIN) para responder a preguntas muy detalladas.

El principal "coste" de este diseño es que, para obtener toda la información de una sola Convocatoria o Concesion, necesitas realizar múltiples uniones (joins), lo que puede ser más lento que un esquema desnormalizado si no se usan índices adecuados. Sin embargo, tu AppDbContext ya define índices en campos importantes, lo que mitiga este problema.

Tipos de Consultas que puedes hacer

La clave para obtener toda la información es usar consultas con uniones (joins). Con las relaciones definidas, puedes navegar entre las entidades como si fueran una red de información interconectada. Puedes hacer consultas para:

    Recuperar datos completos: Obtener una vista completa de una entidad, incluyendo los detalles de todas las entidades relacionadas.

    Filtrar y buscar: Encontrar registros que cumplan con criterios específicos en diferentes tablas. Por ejemplo, buscar Convocatorias de un Organismo en particular.

    Realizar análisis y agregaciones: Contar el número de concesiones, sumar importes, o calcular estadísticas por Ejercicio, Organismo, Beneficiario, etc.

Consultas para Obtener Toda la Información Posible

A continuación, te muestro ejemplos de consultas en un formato similar a LINQ, que es la forma en que Entity Framework Core trabaja con tu modelo. Estas consultas te permitirán verificar que puedes obtener toda la información de tu base de datos.

1. Obtener una Concesión con todos sus detalles relacionados

Esta consulta te permite obtener los datos de una Concesion y, de un solo golpe, recuperar los datos del Beneficiario y de la Convocatoria asociada, incluyendo los detalles de catálogo de esta última.
C#

var concesionCompleta = await _dbContext.Concesiones
    .Where(c => c.IdConcesion == concesionId)
    .Include(c => c.Beneficiario)
    .Include(c => c.Convocatoria)
        .ThenInclude(conv => conv.Organismo)
    .Include(c => c.Convocatoria)
        .ThenInclude(conv => conv.TipoConvocatoria)
    .Include(c => c.Convocatoria)
        .ThenInclude(conv => conv.SituacionEntorno)
    .FirstOrDefaultAsync();

2. Obtener todas las Convocatorias de un Organismo específico para un ejercicio determinado

Esta consulta demuestra cómo filtrar por propiedades de entidades relacionadas, lo que es esencial para búsquedas de información.
C#

var convocatoriasFiltradas = await _dbContext.Convocatorias
    .Where(c => c.Organismo.Descripcion == "Nombre del Organismo" && c.Ejercicio == 2025)
    .Include(c => c.Organismo)
    .Include(c => c.TipoConvocatoria)
    .ToListAsync();

3. Calcular el importe total de las concesiones para un Beneficiario

Con esta consulta, puedes realizar agregaciones sobre colecciones para obtener datos estadísticos o de resumen.
C#

var importeTotalBeneficiario = await _dbContext.Concesiones
    .Where(c => c.Beneficiario.Nombre == "Nombre del Beneficiario")
    .SumAsync(c => c.Importe);

4. Obtener todos los detalles de una Convocatoria completa, incluyendo sus Plazos

Esta consulta te permite verificar la relación uno-a-muchos con los plazos de la convocatoria.
C#

var convocatoriaConPlazos = await _dbContext.Convocatorias
    .Where(c => c.Id == convocatoriaId)
    .Include(c => c.Plazos)
    .FirstOrDefaultAsync();

Estos ejemplos demuestran que tu esquema permite recuperar toda la información disponible al combinar las tablas de manera lógica. Si tienes la clave principal de una entidad, puedes "navegar" a través de las propiedades para obtener los datos de todas las entidades relacionadas.