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