📊 Conta360


![.NET 8](https://img.shields.io/badge/.NET-8-blue?logo=dotnet&logoColor=white)
![Next.js](https://img.shields.io/badge/Next.js-13-black?logo=nextdotjs)
![Tauri](https://img.shields.io/badge/Tauri-Desktop-orange?logo=tauri)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-SQL-blue?logo=postgresql)
![Arquitectura Limpa](https://img.shields.io/badge/Arquitectura-Clean-green?logo=vercel)
![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-yellow)


🧭 Descripción General

Conta360 es una solución moderna, modular y escalable para la gestión financiera y contable empresarial. Desarrollada con una arquitectura de microservicios y Clean Architecture en el backend (.NET), y una interfaz modular basada en microfrontends con Next.js y Tauri para escritorio.

Proporciona herramientas para el registro contable, la gestión de facturas, informes financieros, KPIs, e integración con el Plan General Contable (PGC) español.

    ✅ Eficiencia | 🔐 Seguridad | ⚙️ Extensibilidad | 📈 Decisiones Informadas

🚀 Características Principales

    🧾 Gestión Contable: Registro, modificación y consulta de asientos contables.

    📑 Facturación: Emisión y recepción de facturas detalladas.

    📊 Informes Financieros: Balances, PyG, Estados de Patrimonio y más.

    📉 Cálculo de KPIs: Liquidez, Endeudamiento, Rentabilidad y más.

    📥 Importación desde Excel: Carga masiva de datos contables y facturas.

    📚 Integración con PGC: Clasificación contable conforme al Plan General Contable.

    👥 Autenticación y Roles (JWT): Control de acceso seguro por usuario y perfil.

    🛠 Soporte Multibase de Datos: PostgreSQL (producción) y SQLite (dev).

    🖥 Aplicación de Escritorio (Tauri): Experiencia nativa multiplataforma.

    🧩 Microfrontends (Next.js): Modularidad total del frontend.

🧱 Arquitectura General (Clean Architecture + CQRS)

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

🧬 Capas del Proyecto
1. Núcleo (Independiente de Tecnología)

    Conta360.Domain: Entidades, reglas, eventos y objetos de valor.

    Conta360.Application: Casos de uso, interfaces, DTOs, comportamientos CQRS.

    Conta360.Core: Excepciones, validaciones, helpers.

2. Infraestructura

    Conta360.Persistence: Interfaces, UnitOfWork, Specifications.

    Infraestructuras concretas: Postgres, SQLite, Excel, PGC, Reporting.

3. Presentación

    Conta360.Presentation.Api: ASP.NET Core API REST con controllers, DTOs y servicios.

4. Cross-Cutting

    Conta360.CrossCutting.IoC: Configuración global de dependencias vía DI.

🔄 Flujo de Solicitudes (CQRS + MediatR)

    Request llega a API.

    Se mapea a un Command o Query.

    MediatR enruta la solicitud por comportamientos transversales (pipeline behaviors).

    El Handler ejecuta lógica de negocio.

    Interactúa con el dominio y/o infraestructura.

    Eventos de Dominio pueden ser disparados.

    Persistencia vía EF Core.

    Se responde con DTOs mapeados.

⚙️ Principios Clave

    ✅ Separación de Preocupaciones

    🧠 Dominio Independiente

    🔄 CQRS + MediatR

    🔒 Seguridad JWT

    🧪 Testabilidad

    🔁 Extensible y modular

📁 Estructura Simplificada del Proyecto

/Conta360
├── Domain/
├── Application/
├── Core/
├── Persistence/
├── Infrastructure/
├── Presentation/
├── CrossCutting/
├── apps/
│   └── desktop/  (Tauri + Next.js)
└── tests/

🛠 Requisitos Técnicos
Tecnología	Versión Requerida
.NET SDK	8.0 o superior
Node.js	LTS
Yarn / npm	Última estable
Docker Desktop	Opcional
PostgreSQL	Última estable
🧪 Comandos Útiles

# Restaurar dependencias
dotnet restore

# Ejecutar tests
dotnet test

# Levantar el backend
dotnet run --project Conta360.Presentation.Api

# Ejecutar escritorio (Tauri)
cd apps/desktop
yarn tauri dev

🤝 Contribuciones

¿Quieres aportar? ¡Estás más que bienvenido!

    Forkea este repo

    Crea tu rama: feature/nueva-funcionalidad

    Commit + Push

    Abre un Pull Request ✨

🛡 Licencia

MIT © 2025 - Conta360 Team