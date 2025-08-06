# src/SubvencionesApp/Dockerfile
# Etapa base para la imagen final de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos .csproj para optimizar la caché de Docker.
# Esto asegura que la restauración de dependencias solo se ejecute si los archivos de proyecto han cambiado.
COPY ["SubvencionesApp.Api/SubvencionesApp.Api.csproj", "SubvencionesApp.Api/"]
COPY ["SubvencionesApp.Application/SubvencionesApp.Application.csproj", "SubvencionesApp.Application/"]
COPY ["SubvencionesApp.Domain/SubvencionesApp.Domain.csproj", "SubvencionesApp.Domain/"]
COPY ["SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "SubvencionesApp.Infrastructure/"]

# Restaura las dependencias de NuGet.
RUN dotnet restore "SubvencionesApp.Api/SubvencionesApp.Api.csproj"

# Copia el resto del código de la solución.
COPY . .
WORKDIR "/src/SubvencionesApp.Api"

# Copia el script de entrada y le da permisos de ejecución.
# NOTA: Este script DEBE estar en la misma carpeta que este Dockerfile.
COPY ./entrypointpostgre.sh .
RUN chmod +x entrypointpostgre.sh

# Publica la aplicación, optimizada para el entorno de producción.
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

# Etapa final: Crea la imagen de producción ligera
FROM base AS final
WORKDIR /app
# Instalar cliente PostgreSQL
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Define el punto de entrada para ejecutar la aplicación.
# script de espera.
COPY --from=build /src/SubvencionesApp/entrypointpostgre.sh .
RUN chmod +x ./entrypointpostgre.sh
ENTRYPOINT ["./entrypointpostgre.sh"]

# El comando por defecto a ejecutar, que será llamado por el script.
CMD ["dotnet", "SubvencionesApp.Api.dll"]