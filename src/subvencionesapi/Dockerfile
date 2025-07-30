# src/SubvencionesApi/Dockerfile

# --- Etapa base para la imagen de producción/ejecución final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# --- Etapa de construcción para la imagen de Release ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos .csproj del microservicio primero para optimizar el caché de Docker.
COPY ["SubvencionesApi.csproj", "SubvencionesApi/"]
COPY ["SubvencionesApp.Core/SubvencionesApp.Core.csproj", "SubvencionesApp.Core/"]
COPY ["SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "SubvencionesApp.Infrastructure/"]

# Restaura las dependencias de NuGet.
RUN dotnet restore "SubvencionesApi/SubvencionesApi.csproj"

# Copia el resto del código fuente.
COPY . .
WORKDIR "/src/SubvencionesApi"
RUN dotnet publish "SubvencionesApi.csproj" -c Release -o /app/publish

# --- Etapa final para la imagen de producción ---
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define el punto de entrada para ejecutar la aplicación.
ENTRYPOINT ["dotnet", "SubvencionesApi.dll"]