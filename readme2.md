Conta360: Plataforma Integral de Gestión Financiera y Contable
Descripción General

Conta360 es una solución robusta y escalable diseñada para la gestión financiera y contable completa de empresas. Desarrollada con una arquitectura de microservicios y Clean Architecture en el backend (.NET) y una interfaz de usuario flexible basada en microfrontends (Next.js con Tauri para la aplicación de escritorio), esta plataforma ofrece herramientas para el registro contable, la gestión de facturas, la generación de informes financieros, el cálculo de KPIs, y la integración con el Plan General Contable (PGC).

Nuestro objetivo es proporcionar una herramienta eficiente, segura y adaptable que optimice los procesos contables y ofrezca información financiera valiosa para la toma de decisiones.
Características Principales

    Gestión Contable Centralizada: Registro, consulta y modificación de asientos contables.
    Gestión de Facturas: Emisión y recepción de facturas con detalle de líneas.
    Informes Financieros Avanzados: Generación de Balances, Cuentas de Pérdidas y Ganancias, Estados de Cambios en el Patrimonio Neto, y otros informes personalizados.
    Cálculo de KPIs Financieros: Monitorización de indicadores clave de rendimiento (Liquidez, Endeudamiento, Rentabilidad, etc.).
    Importación de Datos: Facilita la importación de asientos contables y facturas desde archivos Excel.
    Integración PGC: Gestión y clasificación de cuentas contables conforme al Plan General Contable español.
    Autenticación y Autorización: Sistema de usuarios y roles basado en JWT.
    Soporte Multi-Base de Datos: Flexibilidad para usar PostgreSQL (producción) o SQLite (desarrollo/local).
    Aplicación de Escritorio: Shell de escritorio basado en Tauri para una experiencia de usuario nativa y mejor rendimiento.
    Microfrontends: Arquitectura frontend modular para una fácil expansión y mantenimiento.



                          [HTTP Request]
                                |
                                V
              +--------------------------------------+
              |         Capa de Presentación         |
              |    (Conta360.Presentation.Api)       |
              |                                      |
              |             [Controller]             |
              +------------------+-------------------+
                                 |
         (Envía Comando/Query vía MediatR)
                                 |
                                 V
              +--------------------------------------+
              |         Capa de Aplicación           |
              |       (Conta360.Application)         |
              |         (Núcleo de Negocio)          |
              |                                      |
              |       [Pipeline Behaviors]           |
              | (Autorización, Logging, Validación,  |
              |      Excepciones, etc.)              |
              +------------------+-------------------+
                                 |
                                 V
              +--------------------------------------+
              |     [Command / Query Handler]        |
              |  (Orquesta la Lógica de Negocio)     |
              |                  |                   |
              |   +--------------+---------------+   |
              |   |                              |   |
              |   V                              V   |
              | [Interfaces / Abstracciones]     | [Publica Domain Events]
              | (Conta360.Persistence.Interfaces) |     (Conta360.Domain)
              | (Interfaces de Infraestructura)   |             ^
              +--------------+--------------------+             |
                             |                                  |
                             V                                  |
              +--------------------------------------+          |
              |            Capa de Dominio           |          |
              |          (Conta360.Domain)           |          |
              |    (Reglas de Negocio Centrales)     |          |
              |                                      |          |
              |   [Entidades de Dominio]             <----------+
              |   [Value Objects / Reglas de Dominio]|
              +------------------+-------------------+
                                 ^
                                 |
              +------------------+-------------------+
              |            Capa de Apoyo             |
              |          (Conta360.Core)             |
              |                                      |
              | [Utilidades Comunes:                 |
              |   Excepciones, Validaciones, etc.]   |
              +------------------+-------------------+
                                 |
                                 V
                   (Operación Exitosa / Fallida)
                                 |
                                 V
              +--------------------------------------+
              |       Capa de Infraestructura        |
              |   (Conta360.Infrastructure.*)        |
              |    (Implementaciones Concretas)      |
              |                                      |
              | [Repositorios] ------------------> [Base de Datos]
              |   (Postgres, Sqlite)            (PostgreSQL / SQLite)
              |                                      |
              | [Servicios Infraestructura] --> [Servicios Externos]
              | (Excel, PGC, Reporting, etc.)   (APIs, Archivos, etc.)
              +------------------+-------------------+
                                 ^
                                 |
              +--------------------------------------+
              |         Capa Cross-Cutting           |
              |     (Conta360.CrossCutting.IoC)      |
              |                                      |
              | [Inyección de Dependencias]          |
              | [Configuración de Servicios]         |
              +--------------------------------------+

   (Retorno de DTOs de Aplicación, Mapeo a DTOs de API, y Respuesta HTTP)


C:.
│ .dockerignore
│ .gitignore
│ Directory.Build.props
│ Directory.Build.targets
│ Directory.Packages.props
│ docker-cleanup.bat
│ docker-compose.yml
│ global.json
│ LICENSE
│ README.md
│ readme2.md
│
├───.devcontainer
│   │ .env
│   │ devcontainer.json
│   │ Dockerfile
│   │ star2.sh
│   │ startup.sh
│   │ startup1.sh
│   │
│   └───.vscode
│       settings.json
│
├───desktop-app
│   └───Conta360.DesktopShell
│       ├── .gitignore
│       ├── package.json
│       ├── scripts
│       │   └── copy-mf-assets.js
│       ├── src-tauri
│       │   └── src
│       │       └── main.rs
│       └── tauri.conf.json
│
├───microfrontends
│   └───root-config
│       ├── .gitignore
│       ├── next.config.js
│       ├── package.json
│       ├── postcss.config.js
│       ├── src
│       │   ├── app
│       │   │   ├── globals.css
│       │   │   ├── layout.tsx
│       │   │   └── page.tsx
│       │   └── lib
│       │       └── api.ts
│       ├── tailwind.config.ts
│       └── tsconfig.json
│
└───src
    └───backend
        │ GlobalUsings.cs (Para 'using' globales que apliquen a todo el backend)
        │
        ├───Core
        │   ├───Conta360.Domain
        │   │   │ Conta360.Domain.csproj
        │   │   │
        │   │   ├───Entities
        │   │   │       Account.cs
        │   │   │       AccountingEntry.cs
        │   │   │       AuditLogEntry.cs
        │   │   │       BaseEntity.cs
        │   │   │       EmittedInvoice.cs
        │   │   │       FinancialPeriod.cs
        │   │   │       Invoice.cs
        │   │   │       InvoiceLine.cs
        │   │   │       PGCStructure.cs
        │   │   │       ReceivedInvoice.cs
        │   │   │       User.cs
        │   │   │
        │   │   ├───Enums
        │   │   │       AccountGroup.cs
        │   │   │       AuditLogAction.cs
        │   │   │       EntryType.cs
        │   │   │       FinancialReportType.cs
        │   │   │       InvoiceType.cs
        │   │   │       KpiType.cs
        │   │   │
        │   │   ├───Events
        │   │   │       AccountingEntryAddedEvent.cs
        │   │   │       IDomainEvent.cs
        │   │   │       InvoiceCreatedEvent.cs
        │   │   │
        │   │   ├───Rules
        │   │   │       AccountingEntryValidationRule.cs
        │   │   │       BalanceStructureValidationRule.cs
        │   │   │       FinancialPeriodOverlapRule.cs
        │   │   │       IValidationRule.cs
        │   │   │       InvoiceDateRangeRule.cs
        │   │   │       PGCAccountClassificationRule.cs
        │   │   │       TotalAmountValidationRule.cs
        │   │   │
        │   │   └───ValueObjects
        │   │           Address.cs
        │   │           ChartOfAccountsCode.cs
        │   │           Money.cs
        │   │           TaxIdNumber.cs
        │   │
        │   ├───Conta360.Application
        │   │   │ Conta360.Application.csproj
        │   │   │
        │   │   ├───Common
        │   │   │       BaseCommand.cs
        │   │   │       BaseQuery.cs
        │   │   │       PaginatedList.cs
        │   │   │       ReportParameters.cs
        │   │   │       UserRoles.cs
        │   │   │       ValidationMessages.cs
        │   │   │
        │   │   ├───DTOs
        │   │   │       BalanceSheetDto.cs
        │   │   │       ChangesInEquityDto.cs
        │   │   │       FinancialReportDto.cs
        │   │   │       KpiDataDto.cs
        │   │   │       PGCAccountDetailDto.cs
        │   │   │       ProfitAndLossDto.cs
        │   │   │
        │   │   ├───Interfaces
        │   │   │       ICurrentUserService.cs
        │   │   │       IDateTimeProvider.cs
        │   │   │       IDomainEventDispatcher.cs
        │   │   │       IExcelProcessor.cs
        │   │   │       IFinancialReportingService.cs
        │   │   │       IKpiCalculationService.cs
        │   │   │       IPGCStructureService.cs
        │   │   │
        │   │   ├───Mappers
        │   │   │       ApplicationMappingProfile.cs
        │   │   │
        │   │   ├───PipelineBehaviors
        │   │   │       AuthorizationBehavior.cs
        │   │   │       LoggingBehavior.cs
        │   │   │       UnhandledExceptionBehavior.cs
        │   │   │       ValidationBehavior.cs
        │   │   │
        │   │   └───UseCases
        │   │       ├───AccountingEntries
        │   │       │       AccountingEntryDto.cs
        │   │       │       CreateAccountingEntryCommand.cs
        │   │       │       CreateAccountingEntryHandler.cs
        │   │       │       DeleteAccountingEntryCommand.cs
        │   │       │       DeleteAccountingEntryHandler.cs
        │   │       │       GetAccountingEntriesByPeriodHandler.cs
        │   │       │       GetAccountingEntriesByPeriodQuery.cs
        │   │       │       GetAccountingEntryByIdHandler.cs
        │   │       │       GetAccountingEntryByIdQuery.cs
        │   │       │       UpdateAccountingEntryCommand.cs
        │   │       │       UpdateAccountingEntryHandler.cs
        │   │       ├───FinancialReports
        │   │       │       FinancialReportResponse.cs
        │   │       │       GenerateFinancialReportHandler.cs
        │   │       │       GenerateFinancialReportQuery.cs
        │   │       │       GetFinancialReportTypesHandler.cs
        │   │       │       GetFinancialReportTypesQuery.cs
        │   │       ├───ImportData
        │   │       │       ImportAccountingEntriesCommand.cs
        │   │       │       ImportAccountingEntriesHandler.cs
        │   │       │       ImportInvoiceCommand.cs
        │   │       │       ImportInvoiceHandler.cs
        │   │       │       ImportResultDto.cs
        │   │       ├───Invoices
        │   │       │       CreateInvoiceCommand.cs
        │   │       │       CreateInvoiceHandler.cs
        │   │       │       GetInvoiceByIdHandler.cs
        │   │       │       GetInvoiceByIdQuery.cs
        │   │       │       GetInvoicesByPeriodHandler.cs
        │   │       │       GetInvoicesByPeriodQuery.cs
        │   │       │       InvoiceDto.cs
        │   │       │       InvoiceLineDto.cs
        │   │       │       UpdateInvoiceCommand.cs
        │   │       │       UpdateInvoiceHandler.cs
        │   │       ├───Kpis
        │   │       │       CalculateKpiHandler.cs
        │   │       │       CalculateKpiQuery.cs
        │   │       │       GetAvailableKpisHandler.cs
        │   │       │       GetAvailableKpisQuery.cs
        │   │       │       KpiResultResponse.cs
        │   │       ├───PGCManagement
        │   │       │       GetPGCStructureHandler.cs
        │   │       │       GetPGCStructureQuery.cs
        │   │       │       PGCAccountDto.cs
        │   │       │       UpdatePGCStructureCommand.cs
        │   │       │       UpdatePGCStructureHandler.cs
        │   │       └───UserManagement
        │   │               AuthResultDto.cs
        │   │               LoginUserHandler.cs
        │   │               LoginUserQuery.cs
        │   │               RegisterUserCommand.cs
        │   │               RegisterUserHandler.cs
        │   │               UserDto.cs
        │   │
        │   └───Conta360.Core (Renombrado de `Conta360.Common` para mayor claridad en Clean Architecture)
        │       │ Conta360.Core.csproj
        │       │
        │       ├───Common
        │       │       ApplicationException.cs
        │       │       DomainException.cs
        │       │       Error.cs
        │       │       Guard.cs
        │       │       OperationResult.cs
        │       │
        │       ├───Interfaces
        │       │       IResult.cs
        │       │       IValidator.cs
        │       │
        │       └───Validation
        │               ValidationContext.cs
        │               ValidationEngine.cs
        │               ValidationResult.cs
        │
        ├───Infrastructure
        │   ├───Conta360.Persistence
        │   │   │ Conta360.Persistence.csproj
        │   │   │
        │   │   ├───Interfaces (Repositorios genéricos y específicos para la capa de persistencia)
        │   │   │       IAccountRepository.cs
        │   │   │       IAccountingEntryRepository.cs
        │   │   │       IAuditLogRepository.cs
        │   │   │       IInvoiceRepository.cs
        │   │   │       IPGCStructureRepository.cs
        │   │   │       IRepository.cs
        │   │   │       IUnitOfWork.cs
        │   │   │       IUserRepository.cs
        │   │   │
        │   │   └───Specifications
        │   │           AccountingEntryByPeriodSpecification.cs
        │   │           BaseSpecification.cs
        │   │           InvoiceByDateRangeSpecification.cs
        │   │
        │   ├───Conta360.Infrastructure.Postgres
        │   │   │ Conta360.Infrastructure.Postgres.csproj
        │   │   │
        │   │   ├───Context
        │   │   │   ├── AppDbContext.cs
        │   │   │   └── Configurations
        │   │   │           AccountConfiguration.cs
        │   │   │           AccountingEntryConfiguration.cs
        │   │   │           AuditLogEntryConfiguration.cs
        │   │   │           FinancialPeriodConfiguration.cs
        │   │   │           InvoiceConfiguration.cs
        │   │   │           InvoiceLineConfiguration.cs
        │   │   │           PGCStructureConfiguration.cs
        │   │   │           UserConfiguration.cs
        │   │   │
        │   │   ├───Migrations
        │   │   │       20240529120000_InitialMigration.cs
        │   │   │       AppDbContextModelSnapshot.cs
        │   │   │
        │   │   ├───Repositories (Implementaciones concretas para Postgres)
        │   │   │       AccountRepository.cs
        │   │   │       AccountingEntryRepository.cs
        │   │   │       AuditLogRepository.cs
        │   │   │       BaseRepository.cs
        │   │   │       InvoiceRepository.cs
        │   │   │       PGCStructureRepository.cs
        │   │   │       UnitOfWork.cs
        │   │   │       UserRepository.cs
        │   │   │
        │   │   └───Seed
        │   │           AppDbContextSeed.cs
        │   │
        │   ├───Conta360.Infrastructure.Sqlite
        │   │   │ Conta360.Infrastructure.Sqlite.csproj
        │   │   │
        │   │   ├───Context
        │   │   │   ├── AppDbContext.cs
        │   │   │   └── Configurations
        │   │   │           AccountConfiguration.cs
        │   │   │
        │   │   └───Repositories (Implementaciones concretas para Sqlite)
        │   │           AccountRepository.cs
        │   │
        │   ├───Conta360.Infrastructure.Excel
        │   │   │ Conta360.Infrastructure.Excel.csproj
        │   │   │
        │   │   ├───DTOs
        │   │   │       RawAccountingEntryDto.cs
        │   │   │       RawInvoiceDto.cs
        │   │   │
        │   │   │ ExcelDataMapper.cs
        │   │   │ ExcelFileValidator.cs
        │   │   │ ExcelProcessor.cs (Implementa IExcelProcessor de Application)
        │   │
        │   ├───Conta360.Infrastructure.PGC
        │   │   │ Conta360.Infrastructure.PGC.csproj
        │   │   │
        │   │   ├───Internal (Proyectos internos que conforman el adaptador PGC)
        │   │   │   ├───PGCExtractor.Core
        │   │   │   │   │ PGCExtractor.Core.csproj
        │   │   │   │   │
        │   │   │   │   └───Models
        │   │   │   │           PgcXmlAccount.cs
        │   │   │   │           PgcXmlGroup.cs
        │   │   │   │           PgcXmlSubAccount.cs
        │   │   │   │           PgcXmlSubgroup.cs
        │   │   │   │
        │   │   │   ├───PGCExtractor.Data
        │   │   │   │   │ PGCExtractor.Data.csproj
        │   │   │   │   │
        │   │   │   │   └───Services
        │   │   │   │           PgcBoeXmlScraper.cs
        │   │   │   │           PgcJsonLoader.cs
        │   │   │   │           XmlValidator.cs
        │   │   │   │
        │   │   │   └───PGCExtractor.Logic
        │   │   │       │ PGCExtractor.Logic.csproj
        │   │   │       │
        │   │   │       └───Services
        │   │   │               AccountClassifier.cs
        │   │   │               PgcTaxonomyBuilder.cs
        │   │   │
        │   │   └───Services
        │   │           PGCStructureService.cs (Implementa IPGCStructureService de Application)
        │   │
        │   └───Conta360.Infrastructure.Reporting
        │       │ Conta360.Infrastructure.Reporting.csproj
        │       │
        │       ├───Calculators
        │       │       AcidTestCalculator.cs
        │       │       CurrentLiquidityCalculator.cs
        │       │       DebtRatioCalculator.cs
        │       │       FinancialRatioCalculator.cs
        │       │       RoACalculator.cs
        │       │       RoECalculator.cs
        │       │
        │       ├───Reports
        │       │       BalanceSheetGenerator.cs
        │       │       ChangesInEquityGenerator.cs
        │       │       ProfitAndLossGenerator.cs
        │       │       ReportDataAggregator.cs
        │       │
        │       └───Services
        │               FinancialReportingService.cs (Implementa IFinancialReportingService de Application)
        │               KpiCalculationService.cs (Implementa IKpiCalculationService de Application)
        │
        ├───Presentation
        │   └───Conta360.Presentation.Api
        │       │ Conta360.Presentation.Api.csproj
        │       │ appsettings.Development.json
        │       │ appsettings.json
        │       │ appsettings.Production.json
        │       │ Dockerfile
        │       │ Program.cs
        │       │
        │       ├───Controllers
        │       │       AccountingEntriesController.cs
        │       │       AuthController.cs
        │       │       FinancialReportController.cs
        │       │       ImportController.cs
        │       │       InvoicesController.cs
        │       │       KpiController.cs
        │       │       PGCController.cs
        │       │       UserController.cs
        │       │
        │       ├───DTOs (Modelos específicos de la API: Request/Response para la API)
        │       │       AccountingEntryApiResponse.cs
        │       │       ApiFinancialReportRequest.cs
        │       │       CreateAccountingEntryApiRequest.cs
        │       │       CreateInvoiceApiRequest.cs
        │       │       FinancialReportApiDto.cs
        │       │       ImportRequest.cs
        │       │       ImportResponse.cs
        │       │       InvoiceApiResponse.cs
        │       │       KpiRequest.cs
        │       │       KpiResponse.cs
        │       │       LoginRequest.cs
        │       │       LoginResponse.cs
        │       │       PGCStructureApiResponse.cs
        │       │       RegisterUserRequest.cs
        │       │
        │       ├───Filters (Filtros de acción, autorización, validación para la API)
        │       │       ApiExceptionFilter.cs
        │       │       AuthorizeAttribute.cs
        │       │       ValidationFilter.cs
        │       │
        │       ├───Mappers
        │       │       ApiMappingProfile.cs
        │       │
        │       └───Services (Servicios específicos de la capa de presentación/API)
        │               CurrentUserService.cs (Implementa ICurrentUserService de Application)
        │               TokenService.cs
        │
        └───Shared
            └───Conta360.CrossCutting.IoC (Contenedor de Inversión de Control/Inyección de Dependencias)
                │ Conta360.CrossCutting.IoC.csproj
                │
                └───DependencyInjection.cs (Métodos de extensión para configurar la DI en Startup/Program)

Arquitectura y Diseño

El proyecto Conta360 está construido sobre una sólida base de Clean Architecture y el patrón CQRS (Command Query Responsibility Segregation), con MediatR como orquestador de los flujos de comandos y consultas. Esta aproximación garantiza una clara separación de responsabilidades, alta mantenibilidad, escalabilidad y facilidad para realizar pruebas unitarias e integrales.
1. Capas Fundamentales (Core)

Son el corazón de la aplicación, independientes de cualquier tecnología o base de datos específica.

    Conta360.Domain: Define el dominio de la aplicación, encapsulando las entidades, las reglas de negocio, los eventos de dominio y los objetos de valor.
        Entities: Objetos de negocio con comportamiento y lógica de dominio intrínseca.
        Enums: Enumeraciones clave para el dominio.
        Events: Implementación del patrón de eventos de dominio para un acoplamiento bajo.
        Rules: Reglas de validación y negocio que garantizan la integridad del dominio.
        ValueObjects: Tipos inmutables que representan conceptos del dominio (ej. Money, Address).
    Conta360.Application: Contiene los casos de uso de la aplicación, orquestando las entidades de dominio y definiendo las interfaces para la infraestructura. Es la capa principal del CQRS.
        Common: Utilidades y DTOs genéricos compartidos en la capa de aplicación.
        DTOs: Objetos de transferencia de datos para la comunicación entre capas.
        Interfaces: Define los "puertos" que la capa de aplicación necesita de la infraestructura (ej. IExcelProcessor, IFinancialReportingService).
        Mappers: Configuración de mapeo (ej. AutoMapper) entre entidades de dominio y DTOs.
        PipelineBehaviors: Comportamientos de MediatR para un procesamiento transversal (autorización, logging, validación, manejo de excepciones).
        UseCases: Implementa el patrón CQRS, con subcarpetas por módulo (ej. AccountingEntries, FinancialReports). Cada caso de uso define sus Commands (modifican estado), Queries (leen estado) y sus respectivos Handlers (lógica de negocio).
    Conta360.Core (anteriormente Conta360.Common): Proporciona utilidades y abstracciones básicas que son compartidas por todas las capas, incluyendo el manejo de excepciones, errores y un framework de validación genérico.

2. Capa de Infraestructura

Contiene las implementaciones de los "puertos" definidos en la capa de Application y la lógica de integración con tecnologías externas.

    Conta360.Persistence: Abstracciones para el acceso a datos.
        Interfaces: Define las interfaces para los repositorios (IRepository, IUnitOfWork) que serán implementadas por las bases de datos específicas.
        Specifications: Patrón para encapsular la lógica de filtrado y consulta.
    Conta360.Infrastructure.Postgres: Implementación concreta de la persistencia usando Entity Framework Core y PostgreSQL.
    Conta360.Infrastructure.Sqlite: Implementación para SQLite, útil para desarrollo y pruebas locales.
    Conta360.Infrastructure.Excel: Adaptador para el procesamiento de archivos Excel, implementando IExcelProcessor.
    Conta360.Infrastructure.PGC: Adaptador para la integración con el Plan General Contable (PGC). Encapsula un módulo PGCExtractor interno para la extracción y clasificación de datos PGC.
    Conta360.Infrastructure.Reporting: Adaptador para la generación de informes financieros y el cálculo de KPIs, implementando IFinancialReportingService y IKpiCalculationService.

3. Capa de Presentación

La capa más externa, responsable de exponer la funcionalidad del backend a los clientes.

    Conta360.Presentation.Api: La API RESTful de ASP.NET Core que sirve como interfaz para la aplicación.
        Controllers: Puntos de entrada HTTP que delegan las operaciones a la capa de Application.
        DTOs: Modelos de petición y respuesta específicos de la API.
        Filters: Filtros de ASP.NET Core para manejo transversal (errores, autorización, validación).
        Mappers: Mapeo entre DTOs de API y DTOs de Application.
        Services: Servicios específicos de la capa de presentación (ej. CurrentUserService para el contexto del usuario).

4. Capa Compartida (Shared)

Contiene componentes transversales o que necesitan ser compartidos entre diferentes partes del monorepo.

    Conta360.CrossCutting.IoC: Proyecto dedicado a la configuración del Contenedor de Inversión de Control (IoC).
        DependencyInjection.cs: Métodos de extensión para registrar todas las dependencias en el contenedor DI, manteniendo Program.cs limpio y la configuración de DI desacoplada.

Flujo y Lógica de la Aplicación

La arquitectura sigue un flujo de datos y lógica bien definido:

    Capa de Presentación (API): Recibe una solicitud HTTP, la valida, y la mapea a un Command o Query de la capa de Application. Envía este comando/query a través de MediatR.
    Capa de Aplicación: MediatR intercepta el comando/query y lo pasa por una cadena de PipelineBehaviors (Autorización, Logging, Manejo de Excepciones, Validación). Luego, el comando/query es despachado a su Handler correspondiente. El Handler orquesta la lógica de negocio, interactuando con las interfaces de repositorio y los servicios de infraestructura definidos en Application. Puede despachar Domain Events cuando ocurren cambios significativos. Finalmente, mapea las entidades a DTOs de aplicación para la respuesta.
    Capa de Dominio: Las Entidades y Objetos de Valor aplican las reglas de negocio críticas y el comportamiento del dominio. Los Eventos de Dominio notifican a otras partes del sistema sobre cambios importantes. Las Reglas de Dominio validan el estado y las operaciones.
    Capa de Infraestructura: Las implementaciones de los Repositorios se encargan de la comunicación directa con la base de datos (PostgreSQL/SQLite) utilizando Entity Framework Core. Los Servicios de Infraestructura implementan las interfaces de Application y se encargan de las interacciones con servicios externos (Excel, PGC) o lógica compleja de informes.
    Capa Cross-Cutting (IoC): Durante el arranque de la aplicación, esta capa se encarga de registrar todas las dependencias en el contenedor DI, resolviendo las abstracciones con sus implementaciones concretas.

Principios Clave

    Separación de Preocupaciones: Cada componente tiene una responsabilidad única.
    Independencia del Dominio y la Aplicación: El núcleo de la lógica de negocio no depende de la UI, bases de datos o servicios externos.
    CQRS: Diferenciación clara entre operaciones de lectura y escritura para optimizar rendimiento y escalabilidad.
    Manejo de Transacciones: A través del UnitOfWork en la capa de persistencia.
    Validación Multinivel: Reglas de dominio, validación de comandos/queries y validación de DTOs de API.
    Manejo de Errores Centralizado: Con filtros y comportamientos de pipeline.
    Extensibilidad: Diseño modular que facilita la adición de nuevas funcionalidades.

Requisitos Previos

    .NET 8 SDK (o superior)
    Node.js (versión LTS recomendada)
    Yarn (o npm)
    Docker Desktop (opcional, para entornos de desarrollo/producción basados en contenedores)
    PostgreSQL (para entorno de producción o desarrollo completo)