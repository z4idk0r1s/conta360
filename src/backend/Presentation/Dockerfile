FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Conta360.Presentation.Api", "./"]
RUN dotnet restore "Conta360.Presentation.Api"
COPY . .
RUN dotnet build "Conta360.Presentation.Api" -c Release -o /app/build
RUN dotnet publish "Conta360.Presentation.Api" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1
ENTRYPOINT ["dotnet", "Conta360.Presentation.Api.dll"]