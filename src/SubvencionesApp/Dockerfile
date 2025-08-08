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
WORKDIR /src

# Copia los archivos de configuración centralizados y la solución
COPY ["./Directory.Packages.props", "."]
COPY ["./Directory.Build.props", "."]
COPY ["./Directory.Build.targets", "."]
COPY ["./global.json", "."]
# Es una buena práctica copiar el archivo de la solución
# COPY ["./Conta360.sln", "."]

# Copia los archivos .csproj para optimizar la caché de Docker.
# Las rutas de origen ahora son correctas respecto al contexto de compilación.
COPY ["./src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj", "src/SubvencionesApp/SubvencionesApp.Api/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Application/SubvencionesApp.Application.csproj", "src/SubvencionesApp/SubvencionesApp.Application/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Domain/SubvencionesApp.Domain.csproj", "src/SubvencionesApp/SubvencionesApp.Domain/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "src/SubvencionesApp/SubvencionesApp.Infrastructure/"]

# Restaura las dependencias de NuGet.
RUN dotnet restore "src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj"

# Copia el resto del código de la solución.
COPY . .
WORKDIR "/src/SubvencionesApp/SubvencionesApp.Api"

# Publica la aplicación, optimizada para el entorno de producción.
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

# Copiar el script de entrada para usarlo en la etapa final
COPY ["./src/SubvencionesApp/entrypointpostgre.sh", "/app/entrypointpostgre.sh"]
RUN chmod +x /app/entrypointpostgre.sh

################################################################################
# ETAPA FINAL (IMAGEN DE PRODUCCIÓN)
################################################################################
# Usamos la imagen del SDK para que 'dotnet-ef' funcione.
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
# Se corrige la ruta de origen para copiar desde la etapa de 'build'.
COPY --from=build /app/entrypointpostgre.sh .
RUN chmod +x ./entrypointpostgre.sh

# Establecer el usuario final como vscode
USER vscode

ENTRYPOINT ["./entrypointpostgre.sh"]
CMD ["dotnet", "SubvencionesApp.Api.dll"]