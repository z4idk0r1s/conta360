# src/SubvencionesApp/Dockerfile

################################################################################
#                              ETAPA BASE (IMAGEN DE PRODUCCIÓN)               #
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

################################################################################
#                         ETAPA DE CONSTRUCCIÓN (BUILD STAGE)                 #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos de configuración centralizados y la solución
COPY ["Directory.Packages.props", "."]

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

# Publica la aplicación, optimizada para el entorno de producción.
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

################################################################################
#                   ETAPA FINAL (IMAGEN DE PRODUCCIÓN)                         #
################################################################################
# CAMBIO: Usamos la imagen del SDK para que 'dotnet-ef' funcione.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app

# Instalar cliente PostgreSQL y herramientas de red.
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-openbsd dnsutils && \
    rm -rf /var/lib/apt/lists/*

# Instalar dotnet-ef globalmente.
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=build /app/publish .

# Define el punto de entrada para ejecutar la aplicación.
COPY --from=build /src/SubvencionesApp/entrypointpostgre.sh .
RUN chmod +x ./entrypointpostgre.sh

# Establecer el usuario final como vscode
# NOTA: Asegúrate de que el usuario 'vscode' exista en la imagen,
# y de que el PATH esté configurado correctamente para ese usuario,
# si no, 'dotnet-ef' podría no encontrarse.
USER vscode

ENTRYPOINT ["./entrypointpostgre.sh"]
CMD ["dotnet", "SubvencionesApp.Api.dll"]
