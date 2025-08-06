#!/bin/bash
set -e

# Espera a que el servicio de PostgreSQL esté listo
echo "Esperando a que el servicio de PostgreSQL esté listo..."
until pg_isready -h postgres-db -p 5432 -U conta360; do
  echo "PostgreSQL no está disponible, esperando..."
  sleep 2
done

echo "PostgreSQL está listo. Aplicando migraciones..."
dotnet ef database update --project SubvencionesApp.Api.csproj

echo "Migraciones aplicadas. Iniciando la aplicación..."
exec "$@"