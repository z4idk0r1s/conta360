#!/bin/bash
set -e # Salir si un comando falla

# --- Configurar variables de entorno para la base de datos ---
export POSTGRES_HOST="${POSTGRES_HOST_SUBVENCIONES}"
export POSTGRES_DB="${POSTGRES_DB_SUBVENCIONES}"
export POSTGRES_USER="${POSTGRES_USER_SUBVENCIONES}"
export POSTGRES_PASSWORD="${POSTGRES_PASSWORD_SUBVENCIONES}"
export POSTGRES_PORT="${POSTGRES_PORT_SUBVENCIONES}"

# EF Core usa esta variable para la cadena de conexión
export ConnectionStrings__POSTGRESQLConnection="Host=${POSTGRES_HOST};Port=${POSTGRES_PORT};Database=${POSTGRES_DB};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Pooling=true;MinPoolSize=0;MaxPoolSize=100;CommandTimeout=60"

# Ruta al directorio de migraciones
MIGRATIONS_DIR="src/SubvencionesApp/SubvencionesApp.Infrastructure/Migrations"

# Verifica si el directorio de migraciones ya existe
if [ -d "$MIGRATIONS_DIR" ]; then
    echo "El directorio de migraciones ya existe. Saltando la creación de la migración inicial."
else
    echo "El directorio de migraciones no existe. Creando la migración inicial..."

    # Navega al directorio del proyecto de la API (startup project)
    cd src/SubvencionesApp/SubvencionesApp.Api

    # Ejecuta el comando para crear la migración inicial
    dotnet ef migrations add InitialCreate \
        --project ../SubvencionesApp.Infrastructure/SubvencionesApp.Infrastructure.csproj \
        --startup-project SubvencionesApp.Api.csproj \
        --output-dir ../SubvencionesApp.Infrastructure/Migrations

    # Volver al directorio raíz del workspace si es necesario
    cd ../../../..

    echo "Migración inicial creada con éxito."
fi
