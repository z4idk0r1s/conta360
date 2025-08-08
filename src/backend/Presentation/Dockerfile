# src/backend/Presentation/Dockerfile
# Dockerfile para la construcción de la imagen de la API de Conta360.

################################################################################
#                              ETAPA BASE (IMAGEN DE PRODUCCIÓN)               #
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Configura las URLs en las que la aplicación ASP.NET Core escuchará.
ENV ASPNETCORE_URLS=http://+:80

################################################################################
#                         ETAPA DE CONSTRUCCIÓN (BUILD STAGE)                  #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos de configuración centralizados y la solución
COPY ["Directory.Packages.props", "."]

# Copia solo los archivos .csproj para aprovechar la cache de Docker
COPY ["backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj", "backend/Presentation/Conta360.Presentation.Api/"]
COPY ["backend/Shared/Conta360.CrossCutting.IoC/Conta360.CrossCutting.IoC.csproj", "backend/Shared/Conta360.CrossCutting.IoC/"]
COPY ["backend/Core/Conta360.Application/Conta360.Application.csproj", "backend/Core/Conta360.Application/"]
COPY ["backend/Core/Conta360.Domain/Conta360.Domain.csproj", "backend/Core/Conta360.Domain/"]
COPY ["backend/Core/Conta360.Core/Conta360.Core.csproj", "backend/Core/Conta360.Core/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Sqlite/Conta360.Infrastructure.Sqlite.csproj", "backend/Infrastructure/Conta360.Infrastructure.Sqlite/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Postgres/Conta360.Infrastructure.Postgres.csproj", "backend/Infrastructure/Conta360.Infrastructure.Postgres/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Excel/Conta360.Infrastructure.Excel.csproj", "backend/Infrastructure/Conta360.Infrastructure.Excel/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.PGC/Conta360.Infrastructure.PGC.csproj", "backend/Infrastructure/Conta360.Infrastructure.PGC/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Reporting/Conta360.Infrastructure.Reporting.csproj", "backend/Infrastructure/Conta360.Infrastructure.Reporting/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.A3Cash/Conta360.Infrastructure.A3Cash.csproj", "backend/Infrastructure/Conta360.Infrastructure.A3Cash/"]

# Restaura dependencias
RUN dotnet restore "backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"

# Copia el resto del código
COPY . .

# Compila en modo Release
WORKDIR "/src/backend/Presentation/Conta360.Presentation.Api"
RUN dotnet build "Conta360.Presentation.Api.csproj" -c Release -o /app/build

# Publica la aplicación
RUN dotnet publish "Conta360.Presentation.Api.csproj" -c Release -o /app/publish

################################################################################
#                         ETAPA FINAL (IMAGEN DE PRODUCCIÓN)                   #
################################################################################
FROM base AS final
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /app/publish .

# Healthcheck
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1

# Copiar script de migraciones y dar permisos
COPY src/backend/Presentation/entrypointsqlite.sh /app/entrypointsqlite.sh
RUN chmod +x /app/entrypointsqlite.sh

# Ejecutar con el script de entrada
ENTRYPOINT ["/app/entrypointsqlite.sh"]

################################################################################
#                    ETAPA DE DESARROLLO PARA HOT RELOAD                       #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /app

# Instalar herramientas globales necesarias en desarrollo
RUN dotnet tool install --global dotnet-ef

# Variables de entorno de desarrollo
ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_USE_POLLING_FILE_WATCHER=1 \
    PATH="$PATH:/root/.dotnet/tools"

EXPOSE 80

# ⚠️ No copiamos código aquí: docker-compose monta el volumen
# El código se usará directamente desde el host para hot reload
