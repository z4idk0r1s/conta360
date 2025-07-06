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
│         settings.json
│
├───desktop-app
│   └───Conta360.DesktopShell
│         ├── .gitignore
│         ├── package.json
│         ├── scripts
│         │   └── copy-mf-assets.js
│         ├── src-tauri
│         │   └── src
│         │         └── main.rs
│         └── tauri.conf.json
│
├───microfrontends
│   └───root-config
│         ├── .gitignore
│         ├── next.config.js
│         ├── package.json
│         ├── postcss.config.js
│         ├── src
│         │   ├── app
│         │   │   ├── globals.css
│         │   │   ├── layout.tsx
│         │   │   └── page.tsx
│         │   └── lib
│         │         └── api.ts
│         ├── tailwind.config.ts
│         └── tsconfig.json
│
└───src
    └───backend
        │ GlobalUsings.cs (Para 'using' globales que apliquen a todo el backend)
        │
        ├───Core
        │   ├───Conta360.Domain (Capas centrales: **Interfaces**, **Entidades**, **Reglas de Negocio**, **Eventos de Dominio**, **Objetos de Valor**)
        │   │   │ Conta360.Domain.csproj
        │   │   │
        │   │   ├───Entities (Define las **entidades** principales del negocio contable como Cuenta, Asiento Contable, Factura, Usuario, etc.)
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
        │   │   ├───Enums (Enumeraciones que representan estados o tipos específicos del dominio)
        │   │   │       AccountGroup.cs
        │   │   │       AuditLogAction.cs
        │   │   │       EntryType.cs
        │   │   │       FinancialReportType.cs
        │   │   │       InvoiceType.cs
        │   │   │       KpiType.cs
        │   │   │
        │   │   ├───Events (Define **eventos de dominio** que ocurren en el sistema y a los que otros componentes pueden reaccionar)
        │   │   │       AccountingEntryAddedEvent.cs
        │   │   │       IDomainEvent.cs (**Interfaz** base para todos los eventos de dominio)
        │   │   │       InvoiceCreatedEvent.cs
        │   │   │
        │   │   ├───Rules (Contiene las **reglas de validación y lógica de negocio** core del dominio, implementando la **interfaz** IValidationRule)
        │   │   │       AccountingEntryValidationRule.cs
        │   │   │       BalanceStructureValidationRule.cs
        │   │   │       FinancialPeriodOverlapRule.cs
        │   │   │       IValidationRule.cs (**Interfaz** para las reglas de validación)
        │   │   │       InvoiceDateRangeRule.cs
        │   │   │       PGCAccountClassificationRule.cs
        │   │   │       TotalAmountValidationRule.cs
        │   │   │
        │   │   └───ValueObjects (Define **objetos de valor** que representan conceptos del dominio sin identidad propia)
        │   │           Address.cs
        │   │           ChartOfAccountsCode.cs
        │   │           Money.cs
        │   │           TaxIdNumber.cs
        │   │
        │   ├───Conta360.Application (Capa de Aplicación: **Interfaces de servicios**, **Casos de Uso (comandos y queries)**, **DTOs**, **Comportamientos de pipeline**)
        │   │   │ Conta360.Application.csproj
        │   │   │
        │   │   ├───Common (Clases comunes o base para la capa de aplicación)
        │   │   │       BaseCommand.cs
        │   │   │       BaseQuery.cs
        │   │   │       PaginatedList.cs
        │   │   │       ReportParameters.cs
        │   │   │       UserRoles.cs
        │   │   │       ValidationMessages.cs
        │   │   │
        │   │   ├───DTOs (Define los **objetos de transferencia de datos** utilizados entre la capa de aplicación y la presentación/infraestructura)
        │   │   │       BalanceSheetDto.cs
        │   │   │       ChangesInEquityDto.cs
        │   │   │       FinancialReportDto.cs
        │   │   │       KpiDataDto.cs
        │   │   │       PGCAccountDetailDto.cs
        │   │   │       ProfitAndLossDto.cs
        │   │   │
        │   │   ├───Interfaces (Define **contratos (interfaces)** para servicios que serán implementados en la capa de infraestructura)
        │   │   │       ICurrentUserService.cs
        │   │   │       IDateTimeProvider.cs
        │   │   │       IDomainEventDispatcher.cs
        │   │   │       IExcelProcessor.cs
        │   │   │       IFinancialReportingService.cs
        │   │   │       IKpiCalculationService.cs
        │   │   │       IPGCStructureService.cs
        │   │   │
        │   │   ├───Mappers (Configuraciones de mapeo entre entidades de dominio y DTOs)
        │   │   │       ApplicationMappingProfile.cs
        │   │   │
        │   │   ├───PipelineBehaviors (Implementa comportamientos transversales como autenticación, logging, manejo de errores y validación, que se aplican a los comandos y queries)
        │   │   │       AuthorizationBehavior.cs
        │   │   │       LoggingBehavior.cs
        │   │   │       UnhandledExceptionBehavior.cs
        │   │   │       ValidationBehavior.cs
        │   │   │
        │   │   └───UseCases (Contiene la **lógica de negocio específica para cada caso de uso**, implementando comandos y queries junto con sus respectivos manejadores)
        │   │           ├───AccountingEntries (Casos de uso para Asientos Contables: creación, actualización, eliminación, consulta)
        │   │           │       AccountingEntryDto.cs
        │   │           │       CreateAccountingEntryCommand.cs
        │   │           │       CreateAccountingEntryHandler.cs
        │   │           │       DeleteAccountingEntryCommand.cs
        │   │           │       DeleteAccountingEntryHandler.cs
        │   │           │       GetAccountingEntriesByPeriodHandler.cs
        │   │           │       GetAccountingEntriesByPeriodQuery.cs
        │   │           │       GetAccountingEntryByIdHandler.cs
        │   │           │       GetAccountingEntryByIdQuery.cs
        │   │           │       UpdateAccountingEntryCommand.cs
        │   │           │       UpdateAccountingEntryHandler.cs
        │   │           ├───FinancialReports (Casos de uso para la generación y consulta de informes financieros)
        │   │           │       FinancialReportResponse.cs
        │   │           │       GenerateFinancialReportHandler.cs
        │   │           │       GenerateFinancialReportQuery.cs
        │   │           │       GetFinancialReportTypesHandler.cs
        │   │           │       GetFinancialReportTypesQuery.cs
        │   │           ├───ImportData (Casos de uso para la importación de datos)
        │   │           │       ImportAccountingEntriesCommand.cs
        │   │           │       ImportAccountingEntriesHandler.cs
        │   │           │       ImportInvoiceCommand.cs
        │   │           │       ImportInvoiceHandler.cs
        │   │           │       ImportResultDto.cs
        │   │           ├───Invoices (Casos de uso para Facturas: creación, actualización, consulta)
        │   │           │       CreateInvoiceCommand.cs
        │   │           │       CreateInvoiceHandler.cs
        │   │           │       GetInvoiceByIdHandler.cs
        │   │           │       GetInvoiceByIdQuery.cs
        │   │           │       GetInvoicesByPeriodHandler.cs
        │   │           │       GetInvoicesByPeriodQuery.cs
        │   │           │       InvoiceDto.cs
        │   │           │       InvoiceLineDto.cs
        │   │           │       UpdateInvoiceCommand.cs
        │   │           │       UpdateInvoiceHandler.cs
        │   │           ├───Kpis (Casos de uso para el cálculo y consulta de KPIs)
        │   │           │       CalculateKpiHandler.cs
        │   │           │       CalculateKpiQuery.cs
        │   │           │       GetAvailableKpisHandler.cs
        │   │           │       GetAvailableKpisQuery.cs
        │   │           │       KpiResultResponse.cs
        │   │           ├───PGCManagement (Casos de uso para la gestión del Plan General Contable)
        │   │           │       GetPGCStructureHandler.cs
        │   │           │       GetPGCStructureQuery.cs
        │   │           │       PGCAccountDto.cs
        │   │           │       UpdatePGCStructureCommand.cs
        │   │           │       UpdatePGCStructureHandler.cs
        │   │           └───UserManagement (Casos de uso para la autenticación y registro de usuarios)
        │   │                   AuthResultDto.cs
        │   │                   LoginUserHandler.cs
        │   │                   LoginUserQuery.cs
        │   │                   RegisterUserCommand.cs
        │   │                   RegisterUserHandler.cs
        │   │                   UserDto.cs
        │   │
        │   └───Conta360.Core (Renombrado de `Conta360.Common` para mayor claridad en Clean Architecture)
        │       │ Conta360.Core.csproj
        │       │
        │       ├───Common (Clases comunes transversales a todo el sistema, como manejo de errores y resultados)
        │       │       ApplicationException.cs
        │       │       DomainException.cs
        │       │       Error.cs
        │       │       Guard.cs
        │       │       OperationResult.cs
        │       │
        │       ├───Interfaces (Interfaces genéricas para resultados de operaciones y validadores)
        │       │       IResult.cs
        │       │       IValidator.cs
        │       │
        │       └───Validation (Componentes para el motor de validación general)
        │               ValidationContext.cs
        │               ValidationEngine.cs
        │               ValidationResult.cs
        │
        ├───Infrastructure
        │   ├───Conta360.Persistence (Capa de Persistencia: **Interfaces de Repositorios**, **Especificaciones**)
        │   │   │ Conta360.Persistence.csproj
        │   │   │
        │   │   ├───Interfaces (Define los **contratos (interfaces) para los repositorios** de datos, utilizados por la capa de aplicación)
        │   │   │       IAccountRepository.cs
        │   │   │       IAccountingEntryRepository.cs
        │   │   │       IAuditLogRepository.cs
        │   │   │       IInvoiceRepository.cs
        │   │   │       IPGCStructureRepository.cs
        │   │   │       IRepository.cs (**Interfaz** base para repositorios genéricos)
        │   │   │       IUnitOfWork.cs (**Interfaz** para la unidad de trabajo, orquesta transacciones)
        │   │   │       IUserRepository.cs
        │   │   │
        │   │   └───Specifications (Clases que encapsulan la lógica de consulta para diferentes entidades)
        │   │           AccountingEntryByPeriodSpecification.cs
        │   │           BaseSpecification.cs
        │   │           InvoiceByDateRangeSpecification.cs
        │   │
        │   ├───Conta360.Infrastructure.Postgres (Implementación concreta para PostgreSQL: **Contexto DB**, **Configuraciones de Entidades**, **Migraciones**, **Repositorios**)
        │   │   │ Conta360.Infrastructure.Postgres.csproj
        │   │   │
        │   │   ├───Context (Contexto de base de datos para Entity Framework Core)
        │   │   │   ├── AppDbContext.cs
        │   │   │   └── Configurations (Clases para la configuración de mapeo de entidades a tablas en la base de datos)
        │   │   │           AccountConfiguration.cs
        │   │   │           AccountingEntryConfiguration.cs
        │   │   │           AuditLogEntryConfiguration.cs
        │   │   │           FinancialPeriodConfiguration.cs
        │   │   │           InvoiceConfiguration.cs
        │   │   │           InvoiceLineConfiguration.cs
        │   │   │           PGCStructureConfiguration.cs
        │   │   │           UserConfiguration.cs
        │   │   │
        │   │   ├───Migrations (Archivos generados por Entity Framework Core para la gestión de esquemas de base de datos)
        │   │   │       20240529120000_InitialMigration.cs
        │   │   │       AppDbContextModelSnapshot.cs
        │   │   │
        │   │   ├───Repositories (Implementaciones concretas de las **interfaces de repositorios** definidas en Conta360.Persistence)
        │   │   │       AccountRepository.cs
        │   │   │       AccountingEntryRepository.cs
        │   │   │       AuditLogRepository.cs
        │   │   │       BaseRepository.cs
        │   │   │       InvoiceRepository.cs
        │   │   │       PGCStructureRepository.cs
        │   │   │       UnitOfWork.cs
        │   │   │       UserRepository.cs
        │   │   │
        │   │   └───Seed (Clase para la inicialización de datos en la base de datos)
        │   │           AppDbContextSeed.cs
        │   │
        │   ├───Conta360.Infrastructure.Sqlite (Implementación concreta para SQLite, similar a Postgres pero para otra base de datos)
        │   │   │ Conta360.Infrastructure.Sqlite.csproj
        │   │   │
        │   │   ├───Context
        │   │   │   ├── AppDbContext.cs
        │   │   │   └── Configurations
        │   │   │           AccountConfiguration.cs
        │   │   │
        │   │   └───Repositories (Implementaciones concretas para SQLite)
        │   │           AccountRepository.cs
        │   │
        │   ├───Conta360.Infrastructure.Excel (Implementación de servicios relacionados con la importación y procesamiento de archivos Excel)
        │   │   │ Conta360.Infrastructure.Excel.csproj
        │   │   │
        │   │   ├───DTOs (DTOs específicos para la importación desde Excel)
        │   │   │       RawAccountingEntryDto.cs
        │   │   │       RawInvoiceDto.cs
        │   │   │
        │   │   │ ExcelDataMapper.cs
        │   │   │ ExcelFileValidator.cs
        │   │   │ ExcelProcessor.cs (**Implementación** de IExcelProcessor de Application)
        │   │
        │   ├───Conta360.Infrastructure.PGC (Adaptador para la interacción con fuentes externas del Plan General Contable (PGC))
        │   │   │ Conta360.Infrastructure.PGC.csproj
        │   │   │
        │   │   ├───Internal (Proyectos internos que conforman el adaptador PGC, posiblemente para extracción y procesamiento de datos del PGC)
        │   │   │   ├───PGCExtractor.Core (Modelos de datos internos para el extractor PGC)
        │   │   │   │   │ PGCExtractor.Core.csproj
        │   │   │   │   │
        │   │   │   │   └───Models
        │   │   │   │           PgcXmlAccount.cs
        │   │   │   │           PgcXmlGroup.cs
        │   │   │   │           PgcXmlSubAccount.cs
        │   │   │   │           PgcXmlSubgroup.cs
        │   │   │   │
        │   │   │   ├───PGCExtractor.Data (Servicios de extracción de datos del PGC, como lectura de XML o JSON)
        │   │   │   │   │ PGCExtractor.Data.csproj
        │   │   │   │   │
        │   │   │   │   └───Services
        │   │   │   │           PgcBoeXmlScraper.cs
        │   │   │   │           PgcJsonLoader.cs
        │   │   │   │           XmlValidator.cs
        │   │   │   │
        │   │   │   └───PGCExtractor.Logic (Lógica de negocio específica para el procesamiento y clasificación de datos del PGC)
        │   │   │       │ PGCExtractor.Logic.csproj
        │   │   │       │
        │   │   │       └───Services
        │   │   │               AccountClassifier.cs
        │   │   │               PgcTaxonomyBuilder.cs
        │   │   │
        │   │   └───Services
        │   │           PGCStructureService.cs (**Implementación** de IPGCStructureService de Application)
        │   │
        │   └───Conta360.Infrastructure.Reporting (Implementación de servicios para la generación de informes financieros y cálculo de KPIs)
        │       │ Conta360.Infrastructure.Reporting.csproj
        │       │
        │       ├───Calculators (Clases para el cálculo de ratios financieros)
        │       │       AcidTestCalculator.cs
        │       │       CurrentLiquidityCalculator.cs
        │       │       DebtRatioCalculator.cs
        │       │       FinancialRatioCalculator.cs
        │       │       RoACalculator.cs
        │       │       RoECalculator.cs
        │       │
        │       ├───Reports (Generadores específicos para diferentes tipos de informes financieros)
        │       │       BalanceSheetGenerator.cs
        │       │       ChangesInEquityGenerator.cs
        │       │       ProfitAndLossGenerator.cs
        │       │       ReportDataAggregator.cs
        │       │
        │       └───Services
        │               FinancialReportingService.cs (**Implementación** de IFinancialReportingService de Application)
        │               KpiCalculationService.cs (**Implementación** de IKpiCalculationService de Application)
        │
        ├───Presentation
        │   └───Conta360.Presentation.Api (Capa de Presentación: **Controladores API**, **DTOs de API**, **Filtros**, **Mappers**, **Servicios de autenticación**)
        │       │ Conta360.Presentation.Api.csproj
        │       │ appsettings.Development.json
        │       │ appsettings.json
        │       │ appsettings.Production.json
        │       │ Dockerfile
        │       │ Program.cs
        │       │
        │       ├───Controllers (Controladores ASP.NET Core que exponen los **casos de uso** de la capa de aplicación a través de endpoints HTTP)
        │       │       AccountingEntriesController.cs
        │       │       AuthController.cs
        │       │       FinancialReportController.cs
        │       │       ImportController.cs
        │       │       InvoicesController.cs
        │       │       KpiController.cs
        │       │       PGCController.cs
        │       │       UserController.cs
        │       │
        │       ├───DTOs (Modelos específicos para las solicitudes y respuestas de la API)
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
        │       ├───Filters (Filtros de acción, autorización y validación que se aplican a las peticiones API)
        │       │       ApiExceptionFilter.cs
        │       │       AuthorizeAttribute.cs
        │       │       ValidationFilter.cs
        │       │
        │       ├───Mappers (Mapeo entre DTOs de aplicación y DTOs de API)
        │       │       ApiMappingProfile.cs
        │       │
        │       └───Services (Servicios específicos de la capa de presentación, como el manejo del usuario actual y la generación de tokens)
        │               CurrentUserService.cs (**Implementación** de ICurrentUserService de Application)
        │               TokenService.cs
        │
        └───Shared
            └───Conta360.CrossCutting.IoC (Contenedor de Inversión de Control/Inyección de Dependencias)
                │ Conta360.CrossCutting.IoC.csproj
                │
                └───DependencyInjection.cs (Métodos de extensión para configurar la **inyección de dependencias** en el sistema, conectando interfaces con sus implementaciones)



C:.
├── .devcontainer
│   └── .vscode
├── desktop-app
│   └── Conta360.DesktopShell
├── microfrontends
│   └── root-config
└── src
    └── backend
        ├── Core
        │   ├── Conta360.Domain
        │   │   └── Conta360.Domain.csproj
        │   ├── Conta360.Application
        │   │   └── Conta360.Application.csproj
        │   └── Conta360.Core
        │       └── Conta360.Core.csproj
        ├── Infrastructure
        │   ├── Conta360.Persistence
        │   │   └── Conta360.Persistence.csproj
        │   ├── Conta360.Infrastructure.Postgres
        │   │   └── Conta360.Infrastructure.Postgres.csproj
        │   ├── Conta360.Infrastructure.Sqlite
        │   │   └── Conta360.Infrastructure.Sqlite.csproj
        │   ├── Conta360.Infrastructure.Excel
        │   │   └── Conta360.Infrastructure.Excel.csproj
        │   ├── Conta360.Infrastructure.PGC
        │   │   ├── Internal
        │   │   │   ├── PGCExtractor.Core
        │   │   │   │   └── PGCExtractor.Core.csproj
        │   │   │   ├── PGCExtractor.Data
        │   │   │   │   └── PGCExtractor.Data.csproj
        │   │   │   └── PGCExtractor.Logic
        │   │   │       └── PGCExtractor.Logic.csproj
        │   │   └── Services
        │   └── Conta360.Infrastructure.Reporting
        │       ├── Conta360.Infrastructure.Reporting.csproj
        │       ├── Calculators
        │       ├── Reports
        │       └── Services
        ├── Presentation
        │   └── Conta360.Presentation.Api
        │       ├── Conta360.Presentation.Api.csproj
        │       ├── Controllers
        │       ├── DTOs
        │       ├── Filters
        │       ├── Mappers
        │       └── Services
        └── Shared
            └── Conta360.CrossCutting.IoC
                └── Conta360.CrossCutting.IoC.csproj


1. Core

    Conta360.Domain.csproj:
        No debería referenciar a ningún otro .csproj. Es el núcleo del dominio y debe ser independiente de otras capas.

    Conta360.Application.csproj:
        Referencia a: Conta360.Domain.csproj (Necesita las entidades y lógica de negocio del dominio).
        Referencia a: Conta360.Core.csproj (Para utilizar las clases comunes transversales a todo el sistema, como manejo de errores y resultados).

    Conta360.Core.csproj:
        No debería referenciar a ningún otro .csproj. Contiene componentes comunes y debe ser independiente.

2. Infrastructure

    Conta360.Persistence.csproj:
        Referencia a: Conta360.Domain.csproj (Necesita las entidades del dominio para definir los repositorios).

    Conta360.Infrastructure.Postgres.csproj:
        Referencia a: Conta360.Persistence.csproj (Implementa las interfaces de los repositorios).
        Referencia a: Conta360.Domain.csproj (Para poder mapear las entidades a la base de datos).

    Conta360.Infrastructure.Sqlite.csproj:
        Referencia a: Conta360.Persistence.csproj (Implementa las interfaces de los repositorios).
        Referencia a: Conta360.Domain.csproj (Para poder mapear las entidades a la base de datos).

    Conta360.Infrastructure.Excel.csproj:
        Referencia a: Conta360.Application.csproj (Implementa la interfaz IExcelProcessor).
        Referencia a: Conta360.Domain.csproj (Puede necesitar las entidades para mapear los datos del Excel).

    Conta360.Infrastructure.PGC.csproj:
        Referencia a: Conta360.Application.csproj (Implementa la interfaz IPGCStructureService).
        Referencia a: PGCExtractor.Core.csproj, PGCExtractor.Data.csproj, PGCExtractor.Logic.csproj (Estos son proyectos internos que conforman el adaptador PGC).

    Conta360.Infrastructure.Reporting.csproj:
        Referencia a: Conta360.Application.csproj (Implementa las interfaces IFinancialReportingService y IKpiCalculationService).
        Referencia a: Conta360.Domain.csproj (Necesita las entidades y sus datos para poder generar los informes).

3. Presentation

    Conta360.Presentation.Api.csproj:
        Referencia a: Conta360.Application.csproj (Utiliza los casos de uso definidos en la capa de aplicación).
        Referencia a: Conta360.Domain.csproj (Aunque idealmente debería interactuar solo con la capa de Application, puede necesitar las entidades para los DTOs de la API).

4. Shared

    Conta360.CrossCutting.IoC.csproj:
        Referencia a: Todos los demás proyectos .csproj (Necesita conocer todos los servicios e implementaciones para poder configurar la inyección de dependencias).

Resumen:

    Las capas Core (Domain, Application, Core) son las más internas y menos dependientes.
    La capa Infrastructure implementa las interfaces definidas en Application y depende de Domain para las entidades.
    La capa Presentation depende principalmente de Application.
    La capa Shared (IoC) es la más dependiente, ya que necesita conocer todas las demás capas.






cat /tmp/backend_log.log
    --------------------------------------------------

C:.
│   .dockerignore
│   .gitignore
│   Directory.Build.props
│   Directory.Build.targets
│   Directory.Packages.props
│   docker-cleanup.bat
│   docker-compose.yml
│   global.json
│   LICENSE
│   README.md
│   README.Migraciones.md
│   readme2.md
│
├───.devcontainer
│   │   .env
│   │   devcontainer.json
│   │   Dockerfile
│   │   star2.sh
│   │   startup.sh
│   │   startup1.sh
│   │
│   └───.vscode
│           settings.json
│
├───desktop-app
│   └───Conta360.DesktopShell
│       │   .gitignore
│       │   package.json
│       │   tauri.conf.json
│       │
│       ├───scripts
│       │       copy-mf-assets.js
│       │
│       └───src-tauri
│           │   Cargo.toml
│           │
│           └───src
│                   main.rs
│
└───src
    ├───backend
    │   ├───Core
    │   │   ├───Conta360.Application
    │   │   │   │   Conta360.Application.csproj
    │   │   │   │
    │   │   │   ├───Behaviours
    │   │   │   │       LoggingBehavior.cs
    │   │   │   │       ValidationBehavior.cs
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Application.deps.json
    │   │   │   │               Conta360.Application.dll
    │   │   │   │               Conta360.Application.pdb
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │               Conta360.Domain.dll
    │   │   │   │               Conta360.Domain.pdb
    │   │   │   │
    │   │   │   ├───DTOs
    │   │   │   │       AccountDto.cs
    │   │   │   │       PgcAccountDto.cs
    │   │   │   │       PgcAccountTreeDto.cs
    │   │   │   │
    │   │   │   ├───Features
    │   │   │   │   └───Accounts
    │   │   │   │       └───Commands
    │   │   │   │           └───CreateAccount
    │   │   │   │               │   CreateAccountCommand.cs
    │   │   │   │               │   CreateAccountCommandHandler.cs
    │   │   │   │               │   CreateAccountCommandValidator.cs
    │   │   │   │               │
    │   │   │   │               └───Queries
    │   │   │   │                       GetAccountByIdQuery.cs
    │   │   │   │                       GetAccountByIdQueryHandler.cs
    │   │   │   │                       GetAllPgcAccountsQuery.cs
    │   │   │   │                       GetAllPgcAccountsQueryHandler.cs
    │   │   │   │                       GetPgcAccountTreeQuery.cs
    │   │   │   │                       GetPgcAccountTreeQueryHandler.cs
    │   │   │   │
    │   │   │   ├───Interfaces
    │   │   │   │       IApplicationDbContext.cs
    │   │   │   │       IExcelProcessor.cs
    │   │   │   │       IFinancialReportingService.cs
    │   │   │   │       IKpiCalculationService.cs
    │   │   │   │       IPGCStructureService.cs
    │   │   │   │       IPgcTaxonomyService.cs
    │   │   │   │       IUnitOfWork.cs
    │   │   │   │
    │   │   │   ├───Mappings
    │   │   │   │       MappingProfile.cs
    │   │   │   │
    │   │   │   ├───obj
    │   │   │   │   │   Conta360.Application.csproj.nuget.dgspec.json
    │   │   │   │   │   Conta360.Application.csproj.nuget.g.props
    │   │   │   │   │   Conta360.Application.csproj.nuget.g.targets
    │   │   │   │   │   project.assets.json
    │   │   │   │   │   project.nuget.cache
    │   │   │   │   │
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │   │           │   Conta360.0E22A500.Up2Date
    │   │   │   │           │   Conta360.Application.assets.cache
    │   │   │   │           │   Conta360.Application.csproj.AssemblyReference.cache
    │   │   │   │           │   Conta360.Application.csproj.CoreCompileInputs.cache
    │   │   │   │           │   Conta360.Application.csproj.FileListAbsolute.txt
    │   │   │   │           │   Conta360.Application.dll
    │   │   │   │           │   Conta360.Application.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │   │           │   Conta360.Application.GlobalUsings.g.cs
    │   │   │   │           │   Conta360.Application.pdb
    │   │   │   │           │   Conta360.Application.sourcelink.json
    │   │   │   │           │
    │   │   │   │           ├───ref
    │   │   │   │           │       Conta360.Application.dll
    │   │   │   │           │
    │   │   │   │           └───refint
    │   │   │   │                   Conta360.Application.dll
    │   │   │   │
    │   │   │   └───Services
    │   │   │           PgcAccountTreeBuilder.cs
    │   │   │           PgcExtractorOptions.cs
    │   │   │
    │   │   ├───Conta360.Core
    │   │   │   │   Conta360.Core.csproj
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Core.deps.json
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │
    │   │   │   ├───Common
    │   │   │   │       Error.cs
    │   │   │   │       OperationResult.cs
    │   │   │   │       PgcModelType.cs
    │   │   │   │       ValidationResult.cs
    │   │   │   │
    │   │   │   ├───Interfaces
    │   │   │   │       ICurrentUserService.cs
    │   │   │   │       IDateTimeProvider.cs
    │   │   │   │       IPgcTaxonomyDownloader.cs
    │   │   │   │       IPgcTaxonomyValidator.cs
    │   │   │   │       IValidator.cs
    │   │   │   │
    │   │   │   └───obj
    │   │   │       │   Conta360.Core.csproj.nuget.dgspec.json
    │   │   │       │   Conta360.Core.csproj.nuget.g.props
    │   │   │       │   Conta360.Core.csproj.nuget.g.targets
    │   │   │       │   project.assets.json
    │   │   │       │   project.nuget.cache
    │   │   │       │
    │   │   │       └───Debug
    │   │   │           └───net8.0
    │   │   │               │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │               │   Conta360.Core.assets.cache
    │   │   │               │   Conta360.Core.csproj.CoreCompileInputs.cache
    │   │   │               │   Conta360.Core.csproj.FileListAbsolute.txt
    │   │   │               │   Conta360.Core.dll
    │   │   │               │   Conta360.Core.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │               │   Conta360.Core.GlobalUsings.g.cs
    │   │   │               │   Conta360.Core.pdb
    │   │   │               │   Conta360.Core.sourcelink.json
    │   │   │               │
    │   │   │               ├───ref
    │   │   │               │       Conta360.Core.dll
    │   │   │               │
    │   │   │               └───refint
    │   │   │                       Conta360.Core.dll
    │   │   │
    │   │   └───Conta360.Domain
    │   │       │   Conta360.Domain.csproj
    │   │       │
    │   │       ├───bin
    │   │       │   └───Debug
    │   │       │       └───net8.0
    │   │       │               Conta360.Domain.deps.json
    │   │       │               Conta360.Domain.dll
    │   │       │               Conta360.Domain.pdb
    │   │       │
    │   │       ├───Entities
    │   │       │       Account.cs
    │   │       │       BaseEntity.cs
    │   │       │       PgcAccount.cs
    │   │       │       Transact.cs
    │   │       │
    │   │       ├───Interfaces
    │   │       │       IPgcAccountRepository.cs
    │   │       │       IRepository.cs
    │   │       │
    │   │       ├───obj
    │   │       │   │   Conta360.Domain.csproj.nuget.dgspec.json
    │   │       │   │   Conta360.Domain.csproj.nuget.g.props
    │   │       │   │   Conta360.Domain.csproj.nuget.g.targets
    │   │       │   │   project.assets.json
    │   │       │   │   project.nuget.cache
    │   │       │   │
    │   │       │   └───Debug
    │   │       │       └───net8.0
    │   │       │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │       │           │   Conta360.Domain.assets.cache
    │   │       │           │   Conta360.Domain.csproj.AssemblyReference.cache
    │   │       │           │   Conta360.Domain.csproj.CoreCompileInputs.cache
    │   │       │           │   Conta360.Domain.csproj.FileListAbsolute.txt
    │   │       │           │   Conta360.Domain.dll
    │   │       │           │   Conta360.Domain.GeneratedMSBuildEditorConfig.editorconfig
    │   │       │           │   Conta360.Domain.GlobalUsings.g.cs
    │   │       │           │   Conta360.Domain.pdb
    │   │       │           │   Conta360.Domain.sourcelink.json
    │   │       │           │
    │   │       │           ├───ref
    │   │       │           │       Conta360.Domain.dll
    │   │       │           │
    │   │       │           └───refint
    │   │       │                   Conta360.Domain.dll
    │   │       │
    │   │       └───ValueObjects
    │   │               Address.cs
    │   │
    │   ├───Infrastructure
    │   │   ├───Conta360.Infrastructure.Excel
    │   │   │   │   Conta360.Infrastructure.Excel.csproj
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Application.dll
    │   │   │   │               Conta360.Application.pdb
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │               Conta360.Domain.dll
    │   │   │   │               Conta360.Domain.pdb
    │   │   │   │               Conta360.Infrastructure.Excel.deps.json
    │   │   │   │               Conta360.Infrastructure.Excel.dll
    │   │   │   │               Conta360.Infrastructure.Excel.pdb
    │   │   │   │
    │   │   │   ├───Configuration
    │   │   │   │       ExcelSettings.cs
    │   │   │   │
    │   │   │   ├───Models
    │   │   │   │       DetalleDiario.cs
    │   │   │   │       ResumenFiscalResponse.cs
    │   │   │   │       TotalesGenerales.cs
    │   │   │   │
    │   │   │   ├───obj
    │   │   │   │   │   Conta360.Infrastructure.Excel.csproj.nuget.dgspec.json
    │   │   │   │   │   Conta360.Infrastructure.Excel.csproj.nuget.g.props
    │   │   │   │   │   Conta360.Infrastructure.Excel.csproj.nuget.g.targets
    │   │   │   │   │   project.assets.json
    │   │   │   │   │   project.nuget.cache
    │   │   │   │   │
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │   │           │   Conta360.B422326C.Up2Date
    │   │   │   │           │   Conta360.Infrastructure.Excel.assets.cache
    │   │   │   │           │   Conta360.Infrastructure.Excel.csproj.AssemblyReference.cache
    │   │   │   │           │   Conta360.Infrastructure.Excel.csproj.CoreCompileInputs.cache
    │   │   │   │           │   Conta360.Infrastructure.Excel.csproj.FileListAbsolute.txt
    │   │   │   │           │   Conta360.Infrastructure.Excel.dll
    │   │   │   │           │   Conta360.Infrastructure.Excel.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │   │           │   Conta360.Infrastructure.Excel.GlobalUsings.g.cs
    │   │   │   │           │   Conta360.Infrastructure.Excel.pdb
    │   │   │   │           │   Conta360.Infrastructure.Excel.sourcelink.json
    │   │   │   │           │
    │   │   │   │           ├───ref
    │   │   │   │           │       Conta360.Infrastructure.Excel.dll
    │   │   │   │           │
    │   │   │   │           └───refint
    │   │   │   │                   Conta360.Infrastructure.Excel.dll
    │   │   │   │
    │   │   │   └───Services
    │   │   │       │   ExcelProcessor.cs
    │   │   │       │   ServiceRegistrationExcel.cs
    │   │   │       │
    │   │   │       ├───Implementation
    │   │   │       │       ExcelFiscalProcessor.cs
    │   │   │       │
    │   │   │       └───Interaces
    │   │   │               IExcelFiscalProcessor.cs
    │   │   │
    │   │   ├───Conta360.Infrastructure.PGC
    │   │   │   │   Conta360.Infrastructure.PGC.csproj
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Application.dll
    │   │   │   │               Conta360.Application.pdb
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │               Conta360.Domain.dll
    │   │   │   │               Conta360.Domain.pdb
    │   │   │   │               Conta360.Infrastructure.PGC.deps.json
    │   │   │   │               Conta360.Infrastructure.PGC.dll
    │   │   │   │               Conta360.Infrastructure.PGC.pdb
    │   │   │   │
    │   │   │   ├───Domain
    │   │   │   │   │   XmlTaxonomySerializer.cs
    │   │   │   │   │
    │   │   │   │   └───Models
    │   │   │   │           PGCEntity.cs
    │   │   │   │
    │   │   │   ├───Extraction
    │   │   │   │       PGCDataExtractor.cs
    │   │   │   │
    │   │   │   ├───obj
    │   │   │   │   │   Conta360.Infrastructure.PGC.csproj.nuget.dgspec.json
    │   │   │   │   │   Conta360.Infrastructure.PGC.csproj.nuget.g.props
    │   │   │   │   │   Conta360.Infrastructure.PGC.csproj.nuget.g.targets
    │   │   │   │   │   project.assets.json
    │   │   │   │   │   project.nuget.cache
    │   │   │   │   │
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │   │           │   Conta360.62902E05.Up2Date
    │   │   │   │           │   Conta360.Infrastructure.PGC.assets.cache
    │   │   │   │           │   Conta360.Infrastructure.PGC.csproj.AssemblyReference.cache
    │   │   │   │           │   Conta360.Infrastructure.PGC.csproj.CoreCompileInputs.cache
    │   │   │   │           │   Conta360.Infrastructure.PGC.csproj.FileListAbsolute.txt
    │   │   │   │           │   Conta360.Infrastructure.PGC.dll
    │   │   │   │           │   Conta360.Infrastructure.PGC.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │   │           │   Conta360.Infrastructure.PGC.GlobalUsings.g.cs
    │   │   │   │           │   Conta360.Infrastructure.PGC.pdb
    │   │   │   │           │   Conta360.Infrastructure.PGC.sourcelink.json
    │   │   │   │           │
    │   │   │   │           ├───ref
    │   │   │   │           │       Conta360.Infrastructure.PGC.dll
    │   │   │   │           │
    │   │   │   │           └───refint
    │   │   │   │                   Conta360.Infrastructure.PGC.dll
    │   │   │   │
    │   │   │   ├───Processing
    │   │   │   │       PgcTaxonomyBuilder.cs
    │   │   │   │       PgcTaxonomyDownloader.cs
    │   │   │   │       PgcTaxonomyValidator.cs
    │   │   │   │
    │   │   │   └───Services
    │   │   │           PcgTaxonomyService.cs
    │   │   │           ServiceRegistrationPcg.cs
    │   │   │
    │   │   ├───Conta360.Infrastructure.Postgres
    │   │   │   │   Conta360.Infrastructure.Postgres.csproj
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Application.dll
    │   │   │   │               Conta360.Application.pdb
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │               Conta360.Domain.dll
    │   │   │   │               Conta360.Domain.pdb
    │   │   │   │               Conta360.Infrastructure.Postgres.deps.json
    │   │   │   │               Conta360.Infrastructure.Postgres.dll
    │   │   │   │               Conta360.Infrastructure.Postgres.pdb
    │   │   │   │               Conta360.Infrastructure.Postgres.runtimeconfig.json
    │   │   │   │
    │   │   │   ├───Contexts
    │   │   │   │       PostgresDbContext.cs
    │   │   │   │
    │   │   │   ├───obj
    │   │   │   │   │   Conta360.Infrastructure.Postgres.csproj.nuget.dgspec.json
    │   │   │   │   │   Conta360.Infrastructure.Postgres.csproj.nuget.g.props
    │   │   │   │   │   Conta360.Infrastructure.Postgres.csproj.nuget.g.targets
    │   │   │   │   │   project.assets.json
    │   │   │   │   │   project.nuget.cache
    │   │   │   │   │
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │   │           │   Conta360.DC00351B.Up2Date
    │   │   │   │           │   Conta360.Infrastructure.Postgres.assets.cache
    │   │   │   │           │   Conta360.Infrastructure.Postgres.csproj.AssemblyReference.cache
    │   │   │   │           │   Conta360.Infrastructure.Postgres.csproj.CoreCompileInputs.cache
    │   │   │   │           │   Conta360.Infrastructure.Postgres.csproj.FileListAbsolute.txt
    │   │   │   │           │   Conta360.Infrastructure.Postgres.dll
    │   │   │   │           │   Conta360.Infrastructure.Postgres.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │   │           │   Conta360.Infrastructure.Postgres.genruntimeconfig.cache
    │   │   │   │           │   Conta360.Infrastructure.Postgres.GlobalUsings.g.cs
    │   │   │   │           │   Conta360.Infrastructure.Postgres.pdb
    │   │   │   │           │   Conta360.Infrastructure.Postgres.sourcelink.json
    │   │   │   │           │
    │   │   │   │           ├───ref
    │   │   │   │           │       Conta360.Infrastructure.Postgres.dll
    │   │   │   │           │
    │   │   │   │           └───refint
    │   │   │   │                   Conta360.Infrastructure.Postgres.dll
    │   │   │   │
    │   │   │   ├───Repositories
    │   │   │   │       AccountRepositoryPostgres.cs
    │   │   │   │       UnitOfWorkPostgres.cs
    │   │   │   │
    │   │   │   └───Services
    │   │   │           ServiceRegistrationPostgres.cs
    │   │   │
    │   │   ├───Conta360.Infrastructure.Reporting
    │   │   │   │   Conta360.Infrastructure.Reporting.csproj
    │   │   │   │
    │   │   │   ├───bin
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │               Conta360.Application.dll
    │   │   │   │               Conta360.Application.pdb
    │   │   │   │               Conta360.Core.dll
    │   │   │   │               Conta360.Core.pdb
    │   │   │   │               Conta360.Domain.dll
    │   │   │   │               Conta360.Domain.pdb
    │   │   │   │               Conta360.Infrastructure.Reporting.deps.json
    │   │   │   │               Conta360.Infrastructure.Reporting.dll
    │   │   │   │               Conta360.Infrastructure.Reporting.pdb
    │   │   │   │
    │   │   │   ├───obj
    │   │   │   │   │   Conta360.Infrastructure.Reporting.csproj.nuget.dgspec.json
    │   │   │   │   │   Conta360.Infrastructure.Reporting.csproj.nuget.g.props
    │   │   │   │   │   Conta360.Infrastructure.Reporting.csproj.nuget.g.targets
    │   │   │   │   │   project.assets.json
    │   │   │   │   │   project.nuget.cache
    │   │   │   │   │
    │   │   │   │   └───Debug
    │   │   │   │       └───net8.0
    │   │   │   │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │   │   │           │   Conta360.7CEB0AE5.Up2Date
    │   │   │   │           │   Conta360.Infrastructure.Reporting.assets.cache
    │   │   │   │           │   Conta360.Infrastructure.Reporting.csproj.AssemblyReference.cache
    │   │   │   │           │   Conta360.Infrastructure.Reporting.csproj.CoreCompileInputs.cache
    │   │   │   │           │   Conta360.Infrastructure.Reporting.csproj.FileListAbsolute.txt
    │   │   │   │           │   Conta360.Infrastructure.Reporting.dll
    │   │   │   │           │   Conta360.Infrastructure.Reporting.GeneratedMSBuildEditorConfig.editorconfig
    │   │   │   │           │   Conta360.Infrastructure.Reporting.GlobalUsings.g.cs
    │   │   │   │           │   Conta360.Infrastructure.Reporting.pdb
    │   │   │   │           │   Conta360.Infrastructure.Reporting.sourcelink.json
    │   │   │   │           │
    │   │   │   │           ├───ref
    │   │   │   │           │       Conta360.Infrastructure.Reporting.dll
    │   │   │   │           │
    │   │   │   │           └───refint
    │   │   │   │                   Conta360.Infrastructure.Reporting.dll
    │   │   │   │
    │   │   │   └───Services
    │   │   │           KpiCalculationService.cs
    │   │   │
    │   │   └───Conta360.Infrastructure.Sqlite
    │   │       │   Conta360.Infrastructure.Sqlite.csproj
    │   │       │
    │   │       ├───bin
    │   │       │   └───Debug
    │   │       │       └───net8.0
    │   │       │               Conta360.Application.dll
    │   │       │               Conta360.Application.pdb
    │   │       │               Conta360.Core.dll
    │   │       │               Conta360.Core.pdb
    │   │       │               Conta360.Domain.dll
    │   │       │               Conta360.Domain.pdb
    │   │       │               Conta360.Infrastructure.Sqlite.deps.json
    │   │       │               Conta360.Infrastructure.Sqlite.dll
    │   │       │               Conta360.Infrastructure.Sqlite.pdb
    │   │       │               Conta360.Infrastructure.Sqlite.runtimeconfig.json
    │   │       │
    │   │       ├───Contexts
    │   │       │       SqliteDbContext.cs
    │   │       │
    │   │       ├───obj
    │   │       │   │   Conta360.Infrastructure.Sqlite.csproj.nuget.dgspec.json
    │   │       │   │   Conta360.Infrastructure.Sqlite.csproj.nuget.g.props
    │   │       │   │   Conta360.Infrastructure.Sqlite.csproj.nuget.g.targets
    │   │       │   │   project.assets.json
    │   │       │   │   project.nuget.cache
    │   │       │   │
    │   │       │   └───Debug
    │   │       │       └───net8.0
    │   │       │           │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │       │           │   Conta360.9B2B662B.Up2Date
    │   │       │           │   Conta360.Infrastructure.Sqlite.assets.cache
    │   │       │           │   Conta360.Infrastructure.Sqlite.csproj.AssemblyReference.cache
    │   │       │           │   Conta360.Infrastructure.Sqlite.csproj.CoreCompileInputs.cache
    │   │       │           │   Conta360.Infrastructure.Sqlite.csproj.FileListAbsolute.txt
    │   │       │           │   Conta360.Infrastructure.Sqlite.dll
    │   │       │           │   Conta360.Infrastructure.Sqlite.GeneratedMSBuildEditorConfig.editorconfig
    │   │       │           │   Conta360.Infrastructure.Sqlite.genruntimeconfig.cache
    │   │       │           │   Conta360.Infrastructure.Sqlite.GlobalUsings.g.cs
    │   │       │           │   Conta360.Infrastructure.Sqlite.pdb
    │   │       │           │   Conta360.Infrastructure.Sqlite.sourcelink.json
    │   │       │           │
    │   │       │           ├───ref
    │   │       │           │       Conta360.Infrastructure.Sqlite.dll
    │   │       │           │
    │   │       │           └───refint
    │   │       │                   Conta360.Infrastructure.Sqlite.dll
    │   │       │
    │   │       ├───Repositories
    │   │       │       AccountRepositorySqlite.cs
    │   │       │       UnitOfWorkSqlite.cs
    │   │       │
    │   │       └───Services
    │   │               ServiceRegistrationSqlite.cs
    │   │
    │   ├───Presentation
    │   │   │   Dockerfile
    │   │   │
    │   │   └───Conta360.Presentation.Api
    │   │       │   appsettings.Development.json
    │   │       │   appsettings.json
    │   │       │   conta360.db
    │   │       │   Conta360.Presentation.Api.csproj
    │   │       │   Program.cs
    │   │       │
    │   │       ├───bin
    │   │       │   └───Debug
    │   │       │       └───net8.0
    │   │       │           │   appsettings.Development.json
    │   │       │           │   appsettings.json
    │   │       │           │   AutoMapper.dll
    │   │       │           │   AutoMapper.Extensions.Microsoft.DependencyInjection.dll
    │   │       │           │   Azure.Core.dll
    │   │       │           │   Azure.Identity.dll
    │   │       │           │   ClosedXML.dll
    │   │       │           │   ClosedXML.Parser.dll
    │   │       │           │   Conta360.Application.dll
    │   │       │           │   Conta360.Application.pdb
    │   │       │           │   Conta360.Core.dll
    │   │       │           │   Conta360.Core.pdb
    │   │       │           │   Conta360.CrossCutting.IoC.dll
    │   │       │           │   Conta360.CrossCutting.IoC.pdb
    │   │       │           │   Conta360.Domain.dll
    │   │       │           │   Conta360.Domain.pdb
    │   │       │           │   Conta360.Infrastructure.Excel.dll
    │   │       │           │   Conta360.Infrastructure.Excel.pdb
    │   │       │           │   Conta360.Infrastructure.PGC.dll
    │   │       │           │   Conta360.Infrastructure.PGC.pdb
    │   │       │           │   Conta360.Infrastructure.Postgres.dll
    │   │       │           │   Conta360.Infrastructure.Postgres.pdb
    │   │       │           │   Conta360.Infrastructure.Reporting.dll
    │   │       │           │   Conta360.Infrastructure.Reporting.pdb
    │   │       │           │   Conta360.Infrastructure.Sqlite.dll
    │   │       │           │   Conta360.Infrastructure.Sqlite.pdb
    │   │       │           │   Conta360.Presentation.Api
    │   │       │           │   Conta360.Presentation.Api.deps.json
    │   │       │           │   Conta360.Presentation.Api.dll
    │   │       │           │   Conta360.Presentation.Api.pdb
    │   │       │           │   Conta360.Presentation.Api.runtimeconfig.json
    │   │       │           │   DocumentFormat.OpenXml.dll
    │   │       │           │   DocumentFormat.OpenXml.Framework.dll
    │   │       │           │   EFCore.BulkExtensions.v8.dll
    │   │       │           │   EPPlus.dll
    │   │       │           │   EPPlus.Interfaces.dll
    │   │       │           │   EPPlus.System.Drawing.dll
    │   │       │           │   ExcelNumberFormat.dll
    │   │       │           │   FluentValidation.DependencyInjectionExtensions.dll
    │   │       │           │   FluentValidation.dll
    │   │       │           │   Humanizer.dll
    │   │       │           │   JeffFerguson.Gepsio.dll
    │   │       │           │   MedallionTopologicalSort.dll
    │   │       │           │   MediatR.Contracts.dll
    │   │       │           │   MediatR.dll
    │   │       │           │   Microsoft.AspNetCore.Authentication.JwtBearer.dll
    │   │       │           │   Microsoft.AspNetCore.OpenApi.dll
    │   │       │           │   Microsoft.Bcl.AsyncInterfaces.dll
    │   │       │           │   Microsoft.CodeAnalysis.CSharp.dll
    │   │       │           │   Microsoft.CodeAnalysis.CSharp.Workspaces.dll
    │   │       │           │   Microsoft.CodeAnalysis.dll
    │   │       │           │   Microsoft.CodeAnalysis.Workspaces.dll
    │   │       │           │   Microsoft.Data.SqlClient.dll
    │   │       │           │   Microsoft.Data.Sqlite.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.Abstractions.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.Design.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.Relational.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.Sqlite.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.SqlServer.Abstractions.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.SqlServer.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.SqlServer.HierarchyId.dll
    │   │       │           │   Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite.dll
    │   │       │           │   Microsoft.Extensions.Caching.Memory.dll
    │   │       │           │   Microsoft.Extensions.Configuration.Abstractions.dll
    │   │       │           │   Microsoft.Extensions.Configuration.Binder.dll
    │   │       │           │   Microsoft.Extensions.Configuration.dll
    │   │       │           │   Microsoft.Extensions.DependencyInjection.Abstractions.dll
    │   │       │           │   Microsoft.Extensions.DependencyInjection.dll
    │   │       │           │   Microsoft.Extensions.DependencyModel.dll
    │   │       │           │   Microsoft.Extensions.Diagnostics.Abstractions.dll
    │   │       │           │   Microsoft.Extensions.Diagnostics.dll
    │   │       │           │   Microsoft.Extensions.Http.dll
    │   │       │           │   Microsoft.Extensions.Logging.Abstractions.dll
    │   │       │           │   Microsoft.Extensions.Logging.dll
    │   │       │           │   Microsoft.Extensions.Options.ConfigurationExtensions.dll
    │   │       │           │   Microsoft.Extensions.Options.dll
    │   │       │           │   Microsoft.Extensions.Primitives.dll
    │   │       │           │   Microsoft.Identity.Client.dll
    │   │       │           │   Microsoft.Identity.Client.Extensions.Msal.dll
    │   │       │           │   Microsoft.IdentityModel.Abstractions.dll
    │   │       │           │   Microsoft.IdentityModel.JsonWebTokens.dll
    │   │       │           │   Microsoft.IdentityModel.Logging.dll
    │   │       │           │   Microsoft.IdentityModel.Protocols.dll
    │   │       │           │   Microsoft.IdentityModel.Protocols.OpenIdConnect.dll
    │   │       │           │   Microsoft.IdentityModel.Tokens.dll
    │   │       │           │   Microsoft.IO.RecyclableMemoryStream.dll
    │   │       │           │   Microsoft.OpenApi.dll
    │   │       │           │   Microsoft.SqlServer.Server.dll
    │   │       │           │   Microsoft.SqlServer.Types.dll
    │   │       │           │   Microsoft.Win32.SystemEvents.dll
    │   │       │           │   Mono.TextTemplating.dll
    │   │       │           │   MySqlConnector.dll
    │   │       │           │   NetTopologySuite.dll
    │   │       │           │   NetTopologySuite.IO.SpatiaLite.dll
    │   │       │           │   NetTopologySuite.IO.SqlServerBytes.dll
    │   │       │           │   Npgsql.dll
    │   │       │           │   Npgsql.EntityFrameworkCore.PostgreSQL.dll
    │   │       │           │   Pomelo.EntityFrameworkCore.MySql.dll
    │   │       │           │   RBush.dll
    │   │       │           │   Serilog.AspNetCore.dll
    │   │       │           │   Serilog.dll
    │   │       │           │   Serilog.Extensions.Hosting.dll
    │   │       │           │   Serilog.Extensions.Logging.dll
    │   │       │           │   Serilog.Formatting.Compact.dll
    │   │       │           │   Serilog.Settings.Configuration.dll
    │   │       │           │   Serilog.Sinks.Console.dll
    │   │       │           │   Serilog.Sinks.Debug.dll
    │   │       │           │   Serilog.Sinks.File.dll
    │   │       │           │   SixLabors.Fonts.dll
    │   │       │           │   SQLitePCLRaw.batteries_v2.dll
    │   │       │           │   SQLitePCLRaw.core.dll
    │   │       │           │   SQLitePCLRaw.provider.e_sqlite3.dll
    │   │       │           │   Swashbuckle.AspNetCore.Swagger.dll
    │   │       │           │   Swashbuckle.AspNetCore.SwaggerGen.dll
    │   │       │           │   Swashbuckle.AspNetCore.SwaggerUI.dll
    │   │       │           │   System.ClientModel.dll
    │   │       │           │   System.CodeDom.dll
    │   │       │           │   System.Composition.AttributedModel.dll
    │   │       │           │   System.Composition.Convention.dll
    │   │       │           │   System.Composition.Hosting.dll
    │   │       │           │   System.Composition.Runtime.dll
    │   │       │           │   System.Composition.TypedParts.dll
    │   │       │           │   System.Configuration.ConfigurationManager.dll
    │   │       │           │   System.Diagnostics.DiagnosticSource.dll
    │   │       │           │   System.Drawing.Common.dll
    │   │       │           │   System.IdentityModel.Tokens.Jwt.dll
    │   │       │           │   System.IO.Packaging.dll
    │   │       │           │   System.Memory.Data.dll
    │   │       │           │   System.Runtime.Caching.dll
    │   │       │           │   System.Security.Cryptography.ProtectedData.dll
    │   │       │           │
    │   │       │           ├───cs
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │
    │   │       │           ├───de
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───es
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───fr
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───it
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───ja
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───ko
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───pl
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │
    │   │       │           ├───pt-BR
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───ru
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           ├───runtimes
    │   │       │           │   ├───browser-wasm
    │   │       │           │   │   └───nativeassets
    │   │       │           │   │       └───net8.0
    │   │       │           │   │               e_sqlite3.a
    │   │       │           │   │
    │   │       │           │   ├───linux-arm
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-arm64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-armel
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-mips64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-musl-arm
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-musl-arm64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-musl-x64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-ppc64le
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-s390x
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-x64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───linux-x86
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.so
    │   │       │           │   │
    │   │       │           │   ├───maccatalyst-arm64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.dylib
    │   │       │           │   │
    │   │       │           │   ├───maccatalyst-x64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.dylib
    │   │       │           │   │
    │   │       │           │   ├───osx-arm64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.dylib
    │   │       │           │   │
    │   │       │           │   ├───osx-x64
    │   │       │           │   │   └───native
    │   │       │           │   │           libe_sqlite3.dylib
    │   │       │           │   │
    │   │       │           │   ├───unix
    │   │       │           │   │   └───lib
    │   │       │           │   │       └───net8.0
    │   │       │           │   │               Microsoft.Data.SqlClient.dll
    │   │       │           │   │
    │   │       │           │   ├───win
    │   │       │           │   │   └───lib
    │   │       │           │   │       ├───net7.0
    │   │       │           │   │       │       Microsoft.Win32.SystemEvents.dll
    │   │       │           │   │       │       System.Drawing.Common.dll
    │   │       │           │   │       │
    │   │       │           │   │       └───net8.0
    │   │       │           │   │               Microsoft.Data.SqlClient.dll
    │   │       │           │   │               System.Runtime.Caching.dll
    │   │       │           │   │
    │   │       │           │   ├───win-arm
    │   │       │           │   │   └───native
    │   │       │           │   │           e_sqlite3.dll
    │   │       │           │   │           Microsoft.Data.SqlClient.SNI.dll
    │   │       │           │   │
    │   │       │           │   ├───win-arm64
    │   │       │           │   │   └───native
    │   │       │           │   │           e_sqlite3.dll
    │   │       │           │   │           Microsoft.Data.SqlClient.SNI.dll
    │   │       │           │   │
    │   │       │           │   ├───win-x64
    │   │       │           │   │   └───native
    │   │       │           │   │           e_sqlite3.dll
    │   │       │           │   │           Microsoft.Data.SqlClient.SNI.dll
    │   │       │           │   │           SqlServerSpatial160.dll
    │   │       │           │   │
    │   │       │           │   └───win-x86
    │   │       │           │       └───native
    │   │       │           │               e_sqlite3.dll
    │   │       │           │               Microsoft.Data.SqlClient.SNI.dll
    │   │       │           │               SqlServerSpatial160.dll
    │   │       │           │
    │   │       │           ├───tr
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │
    │   │       │           ├───zh-Hans
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.resources.dll
    │   │       │           │       Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │           │       Microsoft.Data.SqlClient.resources.dll
    │   │       │           │       Microsoft.SqlServer.Types.resources.dll
    │   │       │           │
    │   │       │           └───zh-Hant
    │   │       │                   Microsoft.CodeAnalysis.CSharp.resources.dll
    │   │       │                   Microsoft.CodeAnalysis.CSharp.Workspaces.resources.dll
    │   │       │                   Microsoft.CodeAnalysis.resources.dll
    │   │       │                   Microsoft.CodeAnalysis.Workspaces.resources.dll
    │   │       │                   Microsoft.Data.SqlClient.resources.dll
    │   │       │                   Microsoft.SqlServer.Types.resources.dll
    │   │       │
    │   │       ├───Controllers
    │   │       │       AccountsController.cs
    │   │       │       PgcAccountsController.cs
    │   │       │
    │   │       ├───Data
    │   │       │   └───PGC
    │   │       ├───logs
    │   │       │       log-20250705.txt
    │   │       │
    │   │       ├───Models
    │   │       │       CreateAccountRequest.cs
    │   │       │
    │   │       └───obj
    │   │           │   Conta360.Presentation.Api.csproj.EntityFrameworkCore.targets
    │   │           │   Conta360.Presentation.Api.csproj.nuget.dgspec.json
    │   │           │   Conta360.Presentation.Api.csproj.nuget.g.props
    │   │           │   Conta360.Presentation.Api.csproj.nuget.g.targets
    │   │           │   project.assets.json
    │   │           │   project.nuget.cache
    │   │           │
    │   │           └───Debug
    │   │               └───net8.0
    │   │                   │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │   │                   │   apphost
    │   │                   │   Conta360.3523BC59.Up2Date
    │   │                   │   Conta360.Presentation.Api.assets.cache
    │   │                   │   Conta360.Presentation.Api.csproj.AssemblyReference.cache
    │   │                   │   Conta360.Presentation.Api.csproj.CoreCompileInputs.cache
    │   │                   │   Conta360.Presentation.Api.csproj.FileListAbsolute.txt
    │   │                   │   Conta360.Presentation.Api.dll
    │   │                   │   Conta360.Presentation.Api.GeneratedMSBuildEditorConfig.editorconfig
    │   │                   │   Conta360.Presentation.Api.genruntimeconfig.cache
    │   │                   │   Conta360.Presentation.Api.GlobalUsings.g.cs
    │   │                   │   Conta360.Presentation.Api.MvcApplicationPartsAssemblyInfo.cache
    │   │                   │   Conta360.Presentation.Api.MvcApplicationPartsAssemblyInfo.cs
    │   │                   │   Conta360.Presentation.Api.pdb
    │   │                   │   Conta360.Presentation.Api.sourcelink.json
    │   │                   │   staticwebassets.build.json
    │   │                   │
    │   │                   ├───ref
    │   │                   │       Conta360.Presentation.Api.dll
    │   │                   │
    │   │                   ├───refint
    │   │                   │       Conta360.Presentation.Api.dll
    │   │                   │
    │   │                   └───staticwebassets
    │   │                           msbuild.build.Conta360.Presentation.Api.props
    │   │                           msbuild.buildMultiTargeting.Conta360.Presentation.Api.props
    │   │                           msbuild.buildTransitive.Conta360.Presentation.Api.props
    │   │
    │   └───Shared
    │       └───Conta360.CrossCutting.IoC
    │           │   Conta360.CrossCutting.IoC.csproj
    │           │   DInjection.cs
    │           │
    │           ├───bin
    │           │   └───Debug
    │           │       └───net8.0
    │           │               Conta360.Application.dll
    │           │               Conta360.Application.pdb
    │           │               Conta360.Core.dll
    │           │               Conta360.Core.pdb
    │           │               Conta360.CrossCutting.IoC.deps.json
    │           │               Conta360.CrossCutting.IoC.dll
    │           │               Conta360.CrossCutting.IoC.pdb
    │           │               Conta360.Domain.dll
    │           │               Conta360.Domain.pdb
    │           │               Conta360.Infrastructure.Excel.dll
    │           │               Conta360.Infrastructure.Excel.pdb
    │           │               Conta360.Infrastructure.PGC.dll
    │           │               Conta360.Infrastructure.PGC.pdb
    │           │               Conta360.Infrastructure.Postgres.dll
    │           │               Conta360.Infrastructure.Postgres.pdb
    │           │               Conta360.Infrastructure.Reporting.dll
    │           │               Conta360.Infrastructure.Reporting.pdb
    │           │               Conta360.Infrastructure.Sqlite.dll
    │           │               Conta360.Infrastructure.Sqlite.pdb
    │           │
    │           └───obj
    │               │   Conta360.CrossCutting.IoC.csproj.nuget.dgspec.json
    │               │   Conta360.CrossCutting.IoC.csproj.nuget.g.props
    │               │   Conta360.CrossCutting.IoC.csproj.nuget.g.targets
    │               │   project.assets.json
    │               │   project.nuget.cache
    │               │
    │               └───Debug
    │                   └───net8.0
    │                       │   .NETCoreApp,Version=v8.0.AssemblyAttributes.cs
    │                       │   Conta360.9DDA1D2B.Up2Date
    │                       │   Conta360.CrossCutting.IoC.assets.cache
    │                       │   Conta360.CrossCutting.IoC.csproj.AssemblyReference.cache
    │                       │   Conta360.CrossCutting.IoC.csproj.CoreCompileInputs.cache
    │                       │   Conta360.CrossCutting.IoC.csproj.FileListAbsolute.txt
    │                       │   Conta360.CrossCutting.IoC.dll
    │                       │   Conta360.CrossCutting.IoC.GeneratedMSBuildEditorConfig.editorconfig
    │                       │   Conta360.CrossCutting.IoC.GlobalUsings.g.cs
    │                       │   Conta360.CrossCutting.IoC.pdb
    │                       │   Conta360.CrossCutting.IoC.sourcelink.json
    │                       │
    │                       ├───ref
    │                       │       Conta360.CrossCutting.IoC.dll
    │                       │
    │                       └───refint
    │                               Conta360.CrossCutting.IoC.dll
    │
    └───microfrontends
        ├───dashboard-app
        │   │   .dockerignore
        │   │   .gitignore
        │   │   Dockerfile
        │   │   next-env.d.ts
        │   │   next.config.js
        │   │   package.json
        │   │   postcss.config.js
        │   │   tailwind.config.ts
        │   │   tsconfig.json
        │   │
        │   └───src
        │       ├───app
        │       │       globals.css
        │       │       layout.tsx
        │       │       page.tsx
        │       │
        │       └───lib
        │               api.ts
        │
        └───root-config
            │   .dockerignore
            │   .gitignore
            │   Dockerfile
            │   mf-remotes.config.js
            │   next-env.d.ts
            │   next.config.js
            │   package-lock.json
            │   package.json
            │   postcss.config.js
            │   tailwind.config.ts
            │   tsconfig.json
            │
            └───src
                ├───app
                │       globals.css
                │       layout.tsx
                │       page.tsx
                │
                └───lib
                        api.ts

