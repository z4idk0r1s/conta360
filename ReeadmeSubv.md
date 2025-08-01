Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1

Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Core", "src\backend\Core\Conta360.Core\Conta360.Core.csproj", "{4D8A9F1B-C3E7-4F6A-9B0E-5C1F0A2D3E4F}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Domain", "src\backend\Core\Conta360.Domain\Conta360.Domain.csproj", "{1B2C3D4E-5F6A-7B8C-9D0E-1F2A3B4C5D6E}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Application", "src\backend\Core\Conta360.Application\Conta360.Application.csproj", "{A1B2C3D4-E5F6-7A8B-9C0D-1E2F3A4B5C6D}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.Postgres", "src\backend\Infrastructure\Conta360.Infrastructure.Postgres\Conta360.Infrastructure.Postgres.csproj", "{F1E2D3C4-B5A6-9E8D-7C6B-5A4D3E2C1B0A}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.Sqlite", "src\backend\Infrastructure\Conta360.Infrastructure.Sqlite\Conta360.Infrastructure.Sqlite.csproj", "{12345678-90AB-CDEF-1234-567890ABCDEF}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.Excel", "src\backend\Infrastructure\Conta360.Infrastructure.Excel\Conta360.Infrastructure.Excel.csproj", "{ABCDEF01-2345-6789-ABCD-EF0123456789}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.PGC", "src\backend\Infrastructure\Conta360.Infrastructure.PGC\Conta360.Infrastructure.PGC.csproj", "{09876543-210F-EDCB-A987-6543210FEDCB}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.Reporting", "src\backend\Infrastructure\Conta360.Infrastructure.Reporting\Conta360.Infrastructure.Reporting.csproj", "{FEDCBA98-7654-3210-FEDC-BA9876543210}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Infrastructure.A3Cash", "src\backend\Infrastructure\Conta360.Infrastructure.A3Cash\Conta360.Infrastructure.A3Cash.csproj", "{7E3A9C8D-B1F2-4E5C-A6B7-9D0E1F2A3B4C}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.CrossCutting.IoC", "src\backend\Shared\Conta360.CrossCutting.IoC\Conta360.CrossCutting.IoC.csproj", "{87654321-FEDC-BA98-7654-3210FEDCBA98}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Conta360.Presentation.Api", "src\backend\Presentation\Conta360.Presentation.Api\Conta360.Presentation.Api.csproj", "{210FEDCB-A987-6543-210F-EDCBA9876543}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{4D8A9F1B-C3E7-4F6A-9B0E-5C1F0A2D3E4F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4D8A9F1B-C3E7-4F6A-9B0E-5C1F0A2D3E4F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4D8A9F1B-C3E7-4F6A-9B0E-5C1F0A2D3E4F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4D8A9F1B-C3E7-4F6A-9B0E-5C1F0A2D3E4F}.Release|Any CPU.Build.0 = Release|Any CPU

		{1B2C3D4E-5F6A-7B8C-9D0E-1F2A3B4C5D6E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1B2C3D4E-5F6A-7B8C-9D0E-1F2A3B4C5D6E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1B2C3D4E-5F6A-7B8C-9D0E-1F2F3A4B5C6D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1B2C3D4E-5F6A-7B8C-9D0E-1F2F3A4B5C6D}.Release|Any CPU.Build.0 = Release|Any CPU

		{A1B2C3D4-E5F6-7A8B-9C0D-1E2F3A4B5C6D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1B2C3D4-E5F6-7A8B-9C0D-1E2F3A4B5C6D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1B2C3D4-E5F6-7A8B-9C0D-1E2F3A4B5C6D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1B2C3D4-E5F6-7A8B-9C0D-1E2F3A4B5C6D}.Release|Any CPU.Build.0 = Release|Any CPU

		{F1E2D3C4-B5A6-9E8D-7C6B-5A4D3E2C1B0A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F1E2D3C4-B5A6-9E8D-7C6B-5A4D3E2C1B0A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F1E2D3C4-B5A6-9E8D-7C6B-5A4D3E2C1B0A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F1E2D3C4-B5A6-9E8D-7C6B-5A4D3E2C1B0A}.Release|Any CPU.Build.0 = Release|Any CPU

		{12345678-90AB-CDEF-1234-567890ABCDEF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{12345678-90AB-CDEF-1234-567890ABCDEF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{12345678-90AB-CDEF-1234-567890ABCDEF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{12345678-90AB-CDEF-1234-567890ABCDEF}.Release|Any CPU.Build.0 = Release|Any CPU

		{ABCDEF01-2345-6789-ABCD-EF0123456789}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{ABCDEF01-2345-6789-ABCD-EF0123456789}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{ABCDEF01-2345-6789-ABCD-EF0123456789}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{ABCDEF01-2345-6789-ABCD-EF0123456789}.Release|Any CPU.Build.0 = Release|Any CPU

		{09876543-210F-EDCB-A987-6543210FEDCB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{09876543-210F-EDCB-A987-6543210FEDCB}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{09876543-210F-EDCB-A987-6543210FEDCB}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{09876543-210F-EDCB-A987-6543210FEDCB}.Release|Any CPU.Build.0 = Release|Any CPU

		{FEDCBA98-7654-3210-FEDC-BA9876543210}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FEDCBA98-7654-3210-FEDC-BA9876543210}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FEDCBA98-7654-3210-FEDC-BA9876543210}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FEDCBA98-7654-3210-FEDC-BA9876543210}.Release|Any CPU.Build.0 = Release|Any CPU

        {7E3A9C8D-B1F2-4E5C-A6B7-9D0E1F2A3B4C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7E3A9C8D-B1F2-4E5C-A6B7-9D0E1F2A3B4C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7E3A9C8D-B1F2-4E5C-A6B7-9D0E1F2A3B4C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7E3A9C8D-B1F2-4E5C-A6B7-9D0E1F2A3B4C}.Release|Any CPU.Build.0 = Release|Any CPU

		{87654321-FEDC-BA98-7654-3210FEDCBA98}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{87654321-FEDC-BA98-7654-3210FEDCBA98}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{87654321-FEDC-BA98-7654-3210FEDCBA98}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{87654321-FEDC-BA98-7654-3210FEDCBA98}.Release|Any CPU.Build.0 = Release|Any CPU

		{210FEDCB-A987-6543-210F-EDCBA9876543}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{210FEDCB-A987-6543-210F-EDCBA9876543}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{210FEDCB-A987-6543-210F-EDCBA9876543}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{210FEDCB-A987-6543-210F-EDCBA9876543}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {YOUR-SOLUTION-GUID-HERE}
	EndGlobalSection
EndGlobal




--------------


conta360\src\SubvencionesApp>tree /F
Listado de rutas de carpetas
El número de serie del volumen es 6EE0-B276
C:.
│   appsettings.Development.json
│   appsettings.json
│   Dockerfile
│   Program.cs
│   SubvencionesApp.csproj
│
├───SubvencionesApp.Api
│   ├───Client
│   │       InfoSubvencionesApiClient.cs
│   │
│   └───Controllers
│           SubvencionesController.cs
│
├───SubvencionesApp.Application
│   ├───Dtos
│   │       AccionDto.cs
│   │       AgrupacionDto.cs
│   │       AreaDto.cs
│   │       BeneficiarioDto.cs
│   │       ConcesionDto.cs
│   │       ConvocatoriaDto.cs
│   │       DatosEstadisticosDto.cs
│   │       EntidadDto.cs
│   │       EstadoDto.cs
│   │       FormaPagoDto.cs
│   │       LineaDto.cs
│   │       MunicipioDto.cs
│   │       OrganismoDto.cs
│   │       ProgramaDto.cs
│   │       ProvinciaDto.cs
│   │       SectorDto.cs
│   │       SituacionEntornoDto.cs
│   │       SubtipoSubvencionDto.cs
│   │       TipoBeneficiarioDto.cs
│   │       TipoConvocatoriaDto.cs
│   │       TipoOrganismoDto.cs
│   │       TipoSubvencionDto.cs
│   │       TramoDto.cs
│   │       UnidadAdministrativaDto.cs
│   │
│   ├───Interfaces
│   │       IExternalSubvencionesService.cs
│   │       ISubvencionQueryService.cs
│   │       ISubvencionSyncService.cs
│   │
│   └───Services
│           SubvencionSyncService.cs
│
├───SubvencionesApp.Core
│   │   SubvencionesApp.Core.csproj
│   │
│   ├───Entities
│   │       Accion.cs
│   │       Agrupacion.cs
│   │       Area.cs
│   │       Beneficiario.cs
│   │       Concesion.cs
│   │       Convocatoria.cs
│   │       DatosEstadisticos.cs
│   │       Entidad.cs
│   │       Estado.cs
│   │       FormaPago.cs
│   │       Linea.cs
│   │       Municipio.cs
│   │       Organismo.cs
│   │       Programa.cs
│   │       Provincia.cs
│   │       Sector.cs
│   │       SituacionEntorno.cs
│   │       SubtipoSubvencion.cs
│   │       TipoBeneficiario.cs
│   │       TipoConvocatoria.cs
│   │       TipoOrganismo.cs
│   │       TipoSubvencion.cs
│   │       Tramo.cs
│   │       UnidadAdministrativa.cs
│   │
│   └───Interfaces
│           IAccionRepository.cs
│           IAgrupacionRepository.cs
│           IAreaRepository.cs
│           IBeneficiarioRepository.cs
│           IConcesionRepository.cs
│           IConvocatoriaRepository.cs
│           IDatosEstadisticosRepository.cs
│           IEntidadRepository.cs
│           IEstadoRepository.cs
│           IFormaPagoRepository.cs
│           ILineaRepository.cs
│           IMunicipioRepository.cs
│           IOrganismoRepository.cs
│           IProgramaRepository.cs
│           IProvinciaRepository.cs
│           ISectorRepository.cs
│           ISituacionEntornoRepository.cs
│           ISubtipoSubvencionRepository.cs
│           ISubvencionesService.cs
│           ITipoBeneficiarioRepository.cs
│           ITipoConvocatoriaRepository.cs
│           ITipoOrganismoRepository.cs
│           ITipoSubvencionRepository.cs
│           ITramoRepository.cs
│           IUnidadAdministrativaRepository.cs
│
├───SubvencionesApp.Domain
│   ├───Repositories
│   │       IGenericRepository.cs
│   │       IUnitOfWork.cs
│   │
│   └───Services
│           AccionService.cs
│           AgrupacionService.cs
│           AreaService.cs
│           BeneficiarioService.cs
│           ConcesionService.cs
│           ConvocatoriaService.cs
│           DatosEstadisticosService.cs
│           EntidadService.cs
│           EstadoService.cs
│           FormaPagoService.cs
│           LineaService.cs
│           MunicipioService.cs
│           OrganismoService.cs
│           ProgramaService.cs
│           ProvinciaService.cs
│           SectorService.cs
│           SituacionEntornoService.cs
│           SubtipoSubvencionService.cs
│           SubvencionesService.cs
│           TipoBeneficiarioService.cs
│           TipoConvocatoriaService.cs
│           TipoOrganismoService.cs
│           TipoSubvencionService.cs
│           TramoService.cs
│           UnidadAdministrativaService.cs
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
    │   │
    │   └───Models
    │           AccionApiModel.cs
    │           AgrupacionApiModel.cs
    │           AreaApiModel.cs
    │           BeneficiarioApiModel.cs
    │           ConcesionApiModel.cs
    │           ConvocatoriaApiModel.cs
    │           DatosEstadisticosApiModel.cs
    │           EntidadApiModel.cs
    │           EstadoApiModel.cs
    │           FormaPagoApiModel.cs
    │           LineaApiModel.cs
    │           MunicipioApiModel.cs
    │           OrganismoApiModel.cs
    │           ProgramaApiModel.cs
    │           ProvinciaApiModel.cs
    │           SectorApiModel.cs
    │           SituacionEntornoApiModel.cs
    │           SubtipoSubvencionApiModel.cs
    │           TipoBeneficiarioApiModel.cs
    │           TipoConvocatoriaApiModel.cs
    │           TipoOrganismoApiModel.cs
    │           TipoSubvencionApiModel.cs
    │           TramoApiModel.cs
    │           UnidadAdministrativaApiModel.cs
    │
    └───Repositories
            AccionRepository.cs
            AgrupacionRepository.cs
            AreaRepository.cs
            BeneficiarioRepository.cs
            ConcesionRepository.cs
            ConvocatoriaRepository.cs
            DatosEstadisticosRepository.cs
            EntidadRepository.cs
            EstadoRepository.cs
            FormaPagoRepository.cs
            GenericRepository.cs
            LineaRepository.cs
            MunicipioRepository.cs
            OrganismoRepository.cs
            ProgramaRepository.cs
            ProvinciaRepository.cs
            SectorRepository.cs
            SituacionEntornoRepository.cs
            SubtipoSubvencionRepository.cs
            TipoBeneficiarioRepository.cs
            TipoConvocatoriaRepository.cs
            TipoOrganismoRepository.cs
            TipoSubvencionRepository.cs
            TramoRepository.cs
            UnidadAdministrativaRepository.cs