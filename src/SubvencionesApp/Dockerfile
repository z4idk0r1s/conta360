# src/SubvencionesApp/Dockerfile

################################################################################
# ETAPA BASE (IMAGEN DE PRODUCCIÓN)
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

################################################################################
# ETAPA DE CONSTRUCCIÓN (BUILD STAGE)
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/SubvencionesApp

# Instala las herramientas de Entity Framework Core en la etapa de construcción
RUN dotnet tool install --global dotnet-ef

# Copia los archivos de configuración centralizados desde la raíz del proyecto.
# Las rutas son relativas al contexto de compilación (la raíz de tu proyecto).
COPY ["./Directory.Packages.props", "."]
COPY ["./Directory.Build.props", "."]
COPY ["./Directory.Build.targets", "."]
COPY ["./global.json", "."]
COPY ["./Conta360.sln", "."]

# Copia los archivos de proyecto (.csproj) para optimizar la caché de Docker.
COPY ["./src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj", "./SubvencionesApp.Api/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Application/SubvencionesApp.Application.csproj", "./SubvencionesApp.Application/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Domain/SubvencionesApp.Domain.csproj", "./SubvencionesApp.Domain/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "./SubvencionesApp.Infrastructure/"]

# Restaura las dependencias de NuGet.
# La ruta es ahora relativa a la carpeta raíz del proyecto.
RUN dotnet restore "./SubvencionesApp.Api/SubvencionesApp.Api.csproj"

# Copia el resto del código de la solución.
COPY ["./src/SubvencionesApp", "."]

# Publica la aplicación, optimizada para el entorno de producción.
# Primero, se cambia al directorio del proyecto para que el comando 'publish' funcione.
WORKDIR "./SubvencionesApp.Api"
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

################################################################################
# ETAPA FINAL (IMAGEN DE PRODUCCIÓN)
################################################################################
# Usamos la imagen de ASP.NET para que la imagen final sea más ligera y segura.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar cliente PostgreSQL y herramientas de red.
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-openbsd dnsutils && \
    rm -rf /var/lib/apt/lists/*

# Copia los archivos publicados y el script de entrada desde la etapa 'build'.
COPY --from=build /app/publish .
COPY --from=build /src/SubvencionesApp/entrypointpostgre.sh .

# Copia el directorio de herramientas de dotnet que contiene dotnet-ef
COPY --from=build /root/.dotnet /root/.dotnet

RUN chmod +x ./entrypointpostgre.sh

# Establecer el usuario final como vscode
RUN useradd -ms /bin/bash vscode
USER vscode

# Asegurar que el PATH incluya las herramientas de EF Core
ENV PATH="/root/.dotnet/tools:${PATH}"

ENTRYPOINT ["./entrypointpostgre.sh"]
CMD ["dotnet", "SubvencionesApp.Api.dll"]