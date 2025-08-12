# src\backend\Presentation\Dockerfile

################################################################################
#                                ETAPA BASE (IMAGEN DE PRODUCCIÓN)             #
################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ARG ASPNETCORE_URLS
ENV ASPNETCORE_URLS=$ASPNETCORE_URLS

################################################################################
#                            ETAPA DE CONSTRUCCIÓN (BUILD STAGE)               #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./Directory.Packages.props", "."]
COPY ["./Directory.Build.props", "."]
COPY ["./Directory.Build.targets", "."]
COPY ["./global.json", "."]
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

RUN dotnet restore "backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"
COPY . .
COPY ["./src/backend/Presentation/entrypointsqlite.sh", "backend/Presentation/entrypointsqlite.sh"]
WORKDIR src/backend/Presentation/Conta360.Presentation.Api
RUN dotnet build "Conta360.Presentation.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Conta360.Presentation.Api.csproj" -c Release -o /app/publish

################################################################################
#                              ETAPA FINAL (IMAGEN DE PRODUCCIÓN)              #
################################################################################
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1
COPY --from=build /src/backend/Presentation/entrypointsqlite.sh /app/entrypointsqlite.sh
RUN chmod +x /app/entrypointsqlite.sh

ENTRYPOINT ["/app/entrypointsqlite.sh"]

################################################################################
#                              ETAPA DE DESARROLLO PARA HOT RELOAD             #
################################################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /src
RUN dotnet tool install --global dotnet-ef
ARG ASPNETCORE_URLS
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_URLS=$ASPNETCORE_URLS \
    ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT \
    DOTNET_USE_POLLING_FILE_WATCHER=1 \
    PATH="$PATH:/root/.dotnet/tools"
EXPOSE 80
# Se elimina el COPY del código para que el volumen montado sea el origen del código.
# El código se usará directamente desde el host para hot reload