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

# INSTALACIÓN CORREGIDA: Instala las herramientas de Entity Framework Core en un directorio público y accesible.
RUN dotnet tool install dotnet-ef --tool-path /usr/local/bin

# Copia los archivos de configuración centralizados desde la raíz del proyecto.
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
RUN dotnet restore "./SubvencionesApp.Api/SubvencionesApp.Api.csproj"

# Copia el resto del código de la solución.
COPY ["./src/SubvencionesApp", "."]

# Publica la aplicación, optimizada para el entorno de producción.
WORKDIR /src/SubvencionesApp/SubvencionesApp.Api
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

################################################################################
# ETAPA DE DESARROLLO (DEVELOPMENT STAGE) <-- AÑADIDO
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src
COPY ["./src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj", "./SubvencionesApp/SubvencionesApp.Api/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Application/SubvencionesApp.Application.csproj", "./SubvencionesApp/SubvencionesApp.Application/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Domain/SubvencionesApp.Domain.csproj", "./SubvencionesApp/SubvencionesApp.Domain/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "./SubvencionesApp/SubvencionesApp.Infrastructure/"]
COPY ["./src/SubvencionesApp/Directory.Packages.props", "./SubvencionesApp/"]
COPY ["./src/SubvencionesApp/Directory.Build.props", "./SubvencionesApp/"]
COPY ["./src/SubvencionesApp/Directory.Build.targets", "./SubvencionesApp/"]
RUN dotnet restore "./SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj"
COPY ./src/SubvencionesApp ./SubvencionesApp
WORKDIR /src/SubvencionesApp/SubvencionesApp.Api
ENTRYPOINT ["dotnet", "watch", "--no-hot-reload", "run", "--project", "SubvencionesApp.Api.csproj"]


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

# Copia los archivos publicados desde la etapa 'build'.
COPY --from=build /app/publish .

# Esto es necesario para el servicio de migraciones, que usa la misma imagen.
COPY --from=build /usr/local/bin/dotnet-ef /usr/local/bin/

# Establecer el usuario final como vscode
RUN useradd -ms /bin/bash vscode
USER vscode

# El comando de arranque directo de la aplicación.
# Este es el nuevo punto de entrada de la aplicación.
CMD ["dotnet", "SubvencionesApp.Api.dll"]