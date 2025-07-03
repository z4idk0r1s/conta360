# src/backend/Presentation/Conta360.Presentation.Api/Dockerfile
# Dockerfile para la construcción de la imagen de la API de Conta360.

# --- Etapa base para la imagen de producción/ejecución final (base stage) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Expone el puerto 80, que es el puerto interno donde la aplicación escuchará en el contenedor.
EXPOSE 80
# Configura las URLs en las que la aplicación ASP.NET Core escuchará.
ENV ASPNETCORE_URLS=http://+:80

# --- Etapa de construcción general para la imagen de Release (build stage) ---
# Utiliza la imagen del SDK de .NET para compilar y publicar la aplicación.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Establece el directorio de trabajo dentro del contenedor. El contexto de build se copia aquí.
WORKDIR /src

# Copia solo los archivos .csproj de los proyectos dependientes primero.
# Esto optimiza el caché de Docker: esta capa solo se invalida si cambian los .csproj.
# Las rutas son relativas al CONTEXTO DE BUILD (que es ./src/backend/Presentation/Conta360.Presentation.Api).
# Los destinos son rutas relativas al WORKDIR /src dentro del contenedor.
COPY ["backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj", "backend/Presentation/Conta360.Presentation.Api/"]
# Copia de proyectos compartidos y de infraestructura (ajustar rutas según tu estructura real si es necesario).
COPY ["backend/Shared/Conta360.CrossCutting.IoC/Conta360.CrossCutting.IoC.csproj", "backend/Shared/Conta360.CrossCutting.IoC/"]
COPY ["backend/Core/Conta360.Application/Conta360.Application.csproj", "backend/Core/Conta360.Application/"]
COPY ["backend/Core/Conta360.Domain/Conta360.Domain.csproj", "backend/Core/Conta360.Domain/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Sqlite/Conta360.Infrastructure.Sqlite.csproj", "backend/Infrastructure/Conta360.Infrastructure.Sqlite/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Postgres/Conta360.Infrastructure.Postgres.csproj", "backend/Infrastructure/Conta360.Infrastructure.Postgres/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Excel/Conta360.Infrastructure.Excel.csproj", "backend/Infrastructure/Conta360.Infrastructure.Excel/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.PGC/Conta360.Infrastructure.PGC.csproj", "backend/Infrastructure/Conta360.Infrastructure.PGC/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Reporting/Conta360.Infrastructure.Reporting.csproj", "backend/Infrastructure/Conta360.Infrastructure.Reporting/"]
# Si hay más proyectos .csproj que tu API necesite para restaurar, añádelos aquí.

# Restaura las dependencias de NuGet.
RUN dotnet restore "backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"

# Copia el resto del código fuente de la aplicación.
COPY . .
# Establece el directorio de trabajo para la publicación del proyecto de la API.
WORKDIR "/src/backend/Presentation/Conta360.Presentation.Api" 
# Compila la aplicación en modo Release.
RUN dotnet build "Conta360.Presentation.Api.csproj" -c Release -o /app/build
# Publica la aplicación para crear los binarios de despliegue.
RUN dotnet publish "Conta360.Presentation.Api.csproj" -c Release -o /app/publish

# --- Etapa final para la imagen de producción (final stage) ---
# Utiliza la imagen base de ASP.NET, que es más ligera para la ejecución.
FROM base AS final
WORKDIR /app
# Copia los archivos publicados desde la etapa 'build' a la etapa final.
COPY --from=build /app/publish .
# Configura un healthcheck para verificar la disponibilidad y salud de la aplicación.
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1
# Define el punto de entrada para ejecutar la aplicación .NET.
ENTRYPOINT ["dotnet", "Conta360.Presentation.Api.dll"]

# --- Etapa de desarrollo para hot reload (development target) ---
# Utiliza la imagen del SDK de .NET para permitir el desarrollo con hot reload.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
# El WORKDIR en esta etapa será la raíz donde se montará el volumen del código fuente del host.
WORKDIR /app

# Copia solo los archivos .csproj para restaurar dependencias y construir proyectos inicialmente.
# Esto asegura que las referencias entre proyectos sean resueltas para 'dotnet watch'.
COPY ["backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj", "backend/Presentation/Conta360.Presentation.Api/"]
COPY ["backend/Shared/Conta360.CrossCutting.IoC/Conta360.CrossCutting.IoC.csproj", "backend/Shared/Conta360.CrossCutting.IoC/"]
COPY ["backend/Core/Conta360.Application/Conta360.Application.csproj", "backend/Core/Conta360.Application/"]
COPY ["backend/Core/Conta360.Domain/Conta360.Domain.csproj", "backend/Core/Conta360.Domain/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Sqlite/Conta360.Infrastructure.Sqlite.csproj", "backend/Infrastructure/Conta360.Infrastructure.Sqlite/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Postgres/Conta360.Infrastructure.Postgres.csproj", "backend/Infrastructure/Conta360.Infrastructure.Postgres/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Excel/Conta360.Infrastructure.Excel.csproj", "backend/Infrastructure/Conta360.Infrastructure.Excel/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.PGC/Conta360.Infrastructure.PGC.csproj", "backend/Infrastructure/Conta360.Infrastructure.PGC/"]
COPY ["backend/Infrastructure/Conta360.Infrastructure.Reporting/Conta360.Infrastructure.Reporting.csproj", "backend/Infrastructure/Conta360.Infrastructure.Reporting/"]

# Restaura las dependencias de NuGet para el contexto de desarrollo.
RUN dotnet restore "backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"

# Expone el puerto 80, que es el puerto interno donde la aplicación escuchará.
EXPOSE 80

# Variables de entorno específicas para el entorno de desarrollo.
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
# Habilita el 'file watcher' basado en polling, vital para que 'dotnet watch'
# detecte cambios en volúmenes montados desde el host (especialmente en WSL/macOS).
ENV DOTNET_USE_POLLING_FILE_WATCHER=1 

# No se define un ENTRYPOINT o CMD aquí, ya que 'docker-compose.yml' se encarga
# de ejecutar 'dotnet watch' para habilitar el hot reload y migraciones.