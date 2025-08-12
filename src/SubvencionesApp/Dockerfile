# src/SubvencionesApp/Dockerfile

################################################################################
# ETAPA BASE (IMAGEN DE PRODUCCIÓN)
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ARG ASPNETCORE_URLS
ENV ASPNETCORE_URLS=$ASPNETCORE_URLS

################################################################################
# ETAPA DE CONSTRUCCIÓN (BUILD STAGE)
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN dotnet tool install dotnet-ef --tool-path /usr/local/bin

COPY ["./Directory.Packages.props", "."]
COPY ["./Directory.Build.props", "."]
COPY ["./Directory.Build.targets", "."]
COPY ["./global.json", "."]

COPY ["./src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj", "SubvencionesApp/SubvencionesApp.Api/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Application/SubvencionesApp.Application.csproj", "SubvencionesApp/SubvencionesApp.Application/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Domain/SubvencionesApp.Domain.csproj", "SubvencionesApp/SubvencionesApp.Domain/"]
COPY ["./src/SubvencionesApp/SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj", "SubvencionesApp/SubvencionesApp.Infrastructure/"]

RUN dotnet restore "SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj"
COPY ["./src/SubvencionesApp", "SubvencionesApp/"]
WORKDIR /src/SubvencionesApp/SubvencionesApp.Api
RUN dotnet publish "SubvencionesApp.Api.csproj" -c Release -o /app/publish

################################################################################
# ETAPA DE DESARROLLO (DEVELOPMENT STAGE)
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src/SubvencionesApp/SubvencionesApp.Api

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "watch", "--no-hot-reload", "run", "--project", "SubvencionesApp.Api.csproj"]

################################################################################
# ETAPA FINAL (IMAGEN DE PRODUCCIÓN)
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update && \
    apt-get install -y postgresql-client netcat-openbsd dnsutils && \
    rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .

RUN useradd -ms /bin/bash vscode
USER vscode
CMD ["dotnet", "SubvencionesApp.Api.dll"]