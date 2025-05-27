FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Conta360Service.csproj", "./"]
RUN dotnet restore "Conta360Service.csproj"
COPY . .
RUN dotnet build "Conta360Service.csproj" -c Release -o /app/build
RUN dotnet publish "Conta360Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost/health || exit 1
ENTRYPOINT ["dotnet", "Conta360Service.dll"]