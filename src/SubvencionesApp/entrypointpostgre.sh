#!/bin/bash
set -e

echo "=== Iniciando script de entrada (entrypointpostgre.sh)==="

# Validar que las variables estén definidas
: "${POSTGRES_HOST_SUBVENCIONES:?Variable no definida}"
: "${POSTGRES_PORT_SUBVENCIONES:?Variable no definida}"
: "${POSTGRES_USER_SUBVENCIONES:?Variable no definida}"

# Verificar que dotnet ef esté disponible
if ! command -v dotnet-ef &> /dev/null; then
    echo "❌ dotnet-ef no está disponible. PROCESO DE CONSTRUCCION CANCELADO."
    exit 1
fi

# Función para esperar a PostgreSQL con timeout y mejores logs
wait_for_postgres() {
    local host="$POSTGRES_HOST_SUBVENCIONES"
    local port="$POSTGRES_PORT_SUBVENCIONES"
    local user="$POSTGRES_USER_SUBVENCIONES"
    local max_tries=30
    local count=0

    echo "Esperando a que PostgreSQL esté disponible en $host:$port..."

    while [ $count -lt $max_tries ]; do
        echo "Intento $((count+1))/$max_tries..."

        if nc -z "$host" "$port" 2>/dev/null; then
            echo "Puerto $port abierto en $host"

            if pg_isready -h "$host" -p "$port" -U "$user" -q; then
                echo "PostgreSQL está listo"
                return 0
            else
                echo "PostgreSQL está arriba pero no listo..."
            fi
        else
            echo "No se puede conectar a $host:$port..."
            if nslookup "$host" >/dev/null 2>&1; then
                echo "DNS resuelve $host correctamente"
            else
                echo "No se puede resolver $host"
            fi
        fi

        sleep 2
        count=$((count+1))
    done

    echo "⏰ Timeout esperando PostgreSQL después de $max_tries intentos"
    exit 1
}

# Esperar a PostgreSQL
wait_for_postgres

# Aplicar migraciones
echo "🚀 Aplicando migraciones EF (SubvencionesApp)..."
if dotnet ef database update --project SubvencionesApp.Api.csproj --verbose; then
    echo "✅ Migraciones aplicadas exitosamente"
else
    echo "❌ Error aplicando migraciones"
    exit 1
fi

# Iniciar la app
echo "🟢 PROCESO entrypointpostgresql OK ::: Iniciando la aplicación tras completar las migraciones POSTGRESQL ..."
exec "$@"
