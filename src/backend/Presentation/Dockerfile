# src/backend/Presentation/Dockerfile
# Dockerfile para la construcción de la imagen de la API de Conta360.

################################################################################
#                                ETAPA BASE (IMAGEN DE PRODUCCIÓN)             #
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Configura las URLs en las que la aplicación ASP.NET Core escuchará.
ENV ASPNETCORE_URLS=http://+:80

################################################################################
#                            ETAPA DE CONSTRUCCIÓN (BUILD STAGE)               #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Se establece el directorio de trabajo para todas las operaciones de construcción.
WORKDIR /src

# Copia los archivos de configuración centralizados de la raíz del proyecto
COPY ["./Directory.Packages.props", "."]
COPY ["./Directory.Build.props", "."]
COPY ["./Directory.Build.targets", "."]
COPY ["./global.json", "."]

# Copia solo los archivos .csproj para aprovechar la cache de Docker
COPY ["./src/backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj", "backend/Presentation/Conta360.Presentation.Api/"]
COPY ["./src/backend/Shared/Conta360.CrossCutting.IoC/Conta360.CrossCutting.IoC.csproj", "backend/Shared/Conta360.CrossCutting.IoC/"]
COPY ["./src/backend/Core/Conta360.Application/Conta360.Application.csproj", "backend/Core/Conta360.Application/"]
COPY ["./src/backend/Core/Conta360.Domain/Conta360.Domain.csproj", "backend/Core/Conta360.Domain/"]
COPY ["./src/backend/Core/Conta360.Core/Conta360.Core.csproj", "backend/Core/Conta360.Core/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.Sqlite/Conta360.Infrastructure.Sqlite.csproj", "backend/Infrastructure/Conta360.Infrastructure.Sqlite/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.Postgres/Conta360.Infrastructure.Postgres.csproj", "backend/Infrastructure/Conta360.Infrastructure.Postgres/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.Excel/Conta360.Infrastructure.Excel.csproj", "backend/Infrastructure/Conta360.Infrastructure.Excel/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.PGC/Conta360.Infrastructure.PGC.csproj", "backend/Infrastructure/Conta360.Infrastructure.PGC/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.Reporting/Conta360.Infrastructure.Reporting.csproj", "backend/Infrastructure/Conta360.Infrastructure.Reporting/"]
COPY ["./src/backend/Infrastructure/Conta360.Infrastructure.A3Cash/Conta360.Infrastructure.A3Cash.csproj", "backend/Infrastructure/Conta360.Infrastructure.A3Cash/"]

# Restaura dependencias
# La ruta es coherente con el WORKDIR '/src'.
RUN dotnet restore "backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"

# Copia el resto del código
COPY . .

# Copia el script de migraciones para usarlo en la etapa final
# La ruta de origen es relativa al contexto de compilación.
# **CORRECCIÓN:** Esta línea estaba duplicada y mal ubicada en tu borrador. La copia se debe hacer aquí.
COPY ["./src/backend/Presentation/entrypointsqlite.sh", "backend/Presentation/entrypointsqlite.sh"]

# Compila en modo Release
# El WORKDIR se ajusta a la carpeta del proyecto a compilar.
WORKDIR src/backend/Presentation/Conta360.Presentation.Api
RUN dotnet build "Conta360.Presentation.Api.csproj" -c Release -o /app/build

# Publica la aplicación
RUN dotnet publish "Conta360.Presentation.Api.csproj" -c Release -o /app/publish

################################################################################
#                              ETAPA FINAL (IMAGEN DE PRODUCCIÓN)              #
################################################################################
FROM base AS final
WORKDIR /app

# Copia los archivos publicados desde la etapa de compilación.
COPY --from=build /app/publish .

# Healthcheck
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1

# Copia el script desde la etapa 'build' y da permisos.
# **CORRECCIÓN:** Se especifica la ruta completa dentro de la etapa 'build' para garantizar que se encuentre.
COPY --from=build /src/backend/Presentation/entrypointsqlite.sh /app/entrypointsqlite.sh
RUN chmod +x /app/entrypointsqlite.sh

# **CORRECCIÓN:** Instala las herramientas de EF Core de forma global en la etapa de construcción y luego cópialas aquí.
COPY --from=build /root/.dotnet/tools/dotnet-ef /usr/local/bin/

# Ejecuta con el script de entrada.
ENTRYPOINT ["/app/entrypointsqlite.sh"]

################################################################################
#                              ETAPA DE DESARROLLO PARA HOT RELOAD             #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
# Usa WORKDIR /src para alinearse con los volúmenes de docker-compose.
WORKDIR /src

# Instala herramientas globales necesarias en desarrollo
RUN dotnet tool install --global dotnet-ef

# Variables de entorno de desarrollo
ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_USE_POLLING_FILE_WATCHER=1 \
    PATH="$PATH:/root/.dotnet/tools"

EXPOSE 80

# ⚠️ No copiamos código aquí: docker-compose monta el volumen
# El código se usará directamente desde el host para hot reload
