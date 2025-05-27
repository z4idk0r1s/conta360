Análisis del Ciclo:

    Conta360.Application referencia a Conta360.Infrastructure.
    Conta360.Infrastructure referencia a Conta360.Application (esto es correcto, ya que Infrastructure implementa interfaces definidas en Application).

Esto crea un ciclo directo: Application -> Infrastructure -> Application.

Lo mismo ocurre con Persistence:

    Conta360.Application referencia a Conta360.Persistence.
    Conta360.Persistence referencia a Conta360.Application (esto es correcto, ya que Persistence implementa interfaces definidas en Application).

Esto también crea un ciclo directo: Application -> Persistence -> Application.
Por Qué Esto es un Problema (y Cómo Resolverlo)

En una arquitectura limpia (o de cebolla), las capas "interiores" (como Application y Domain) nunca deben tener una referencia directa a las capas "exteriores" (como Infrastructure o Persistence). Las capas exteriores son las que implementan los contratos (interfaces) definidos en las capas interiores.

¿Pero cómo Application usa Infrastructure o Persistence entonces?

Aquí es donde entra en juego la Inyección de Dependencias (DI).

    Conta360.Application debe definir interfaces (contratos) para cualquier servicio o repositorio que necesite de las capas externas. Por ejemplo, IUserRepository en Application.
    Conta360.Persistence (o Infrastructure) implementará esa interfaz (ej. UserRepository : IUserRepository).
    Tu proyecto de inicio (Conta360.Services.Api) será el encargado de "conectar" estas implementaciones con las interfaces, registrándolas en el contenedor de Inyección de Dependencias.
    De esta forma, Application solo conoce la interfaz (IUserRepository), no la implementación concreta que reside en Persistence o Infrastructure.


    Estado Actual de las Dependencias Circulares

Con las referencias de proyectos que me has proporcionado y el entendimiento de la ubicación de Conta360.Shared.Models, la situación de las dependencias circulares es la siguiente:

    Conta360.Application:
        Referencias: Conta360.Domain.csproj, Conta360.Shared.Models.csproj.
        Estado: Correcto. No hay ciclos. Application depende de Domain (lo cual es lógico) y de Shared.Models (que contiene DTOs utilizados por Application).

    Conta360.Infrastructure:
        Referencias: Conta360.Domain.csproj, Conta360.Application.csproj, y varios adaptadores.
        Estado: Correcto. Infrastructure implementa cosas definidas en Domain y Application, por lo que depende de ellas.

    Conta360.Persistence:
        Referencias: Conta360.Domain.csproj, Conta360.Application.csproj, Conta360.Shared.Models.csproj.
        Estado: Correcto. Persistence implementa cosas definidas en Domain y Application, y puede necesitar Shared.Models.

    Conta360.Services.Api:
        Referencias: Conta360.Application.csproj, Conta360.Infrastructure.csproj, Conta360.Persistence.Postgres.csproj, Conta360.SDK.csproj.
        Estado: Correcto. La API es el orquestador y necesita referenciar a todas las capas que va a inicializar o con las que va a interactuar.

# arquitectura limpia o de cebolla.

    Conta360.Domain: Esta es la capa que contiene tus entidades de negocio, reglas de negocio, agregados, etc. Application (que contiene los casos de uso y la lógica de aplicación) absolutamente necesita referenciar a Domain para interactuar con estas entidades y aplicar las reglas de negocio. Esta es una dependencia fundamental y correcta.

    Conta360.Shared.Models: Como hemos discutido, este proyecto contiene DTOs (Data Transfer Objects) y posiblemente enums o clases de valor simples que son utilizadas para la comunicación entre capas.
        Application a menudo define los DTOs de entrada para sus comandos y DTOs de salida para sus queries.
        Si estos DTOs están centralizados en Conta360.Shared.Models para ser reutilizados por la API (que envía/recibe estos DTOs) y otras capas, entonces Application debe referenciar Conta360.Shared.Models para conocer la estructura de estos DTOs.
        Importante: La clave para que Shared.Models no cause un problema es que no debe tener ninguna dependencia de Application, Domain, Infrastructure, o Persistence. Su contenido debe ser lo más agnóstico posible a la lógica de negocio o la implementación de infraestructura. Por tu tree de archivos, parece que Conta360.Shared.Models contiene DTOs y Enums, lo cual es exactamente lo que esperaríamos de un proyecto de este tipo.