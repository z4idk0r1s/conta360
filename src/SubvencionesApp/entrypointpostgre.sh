#!/bin/bash
set -e

echo "=== Iniciando script de entrada (entrypointpostgre.sh) ==="

# Validar variables de entorno
: "${POSTGRES_HOST_SUBVENCIONES:?Variable POSTGRES_HOST_SUBVENCIONES no definida}"
: "${POSTGRES_PORT_SUBVENCIONES:?Variable POSTGRES_PORT_SUBVENCIONES no definida}"
: "${POSTGRES_USER_SUBVENCIONES:?Variable POSTGRES_USER_SUBVENCIONES no definida}"
: "${POSTGRES_DB_SUBVENCIONES:?Variable POSTGRES_DB_SUBVENCIONES no definida}"

# Verificar que dotnet ef esté disponible
if ! command -v dotnet-ef &> /dev/null; then
    echo "❌ dotnet-ef no está disponible. Intentando con dotnet ef..."
    if ! dotnet ef --version &> /dev/null; then
        echo "❌ Entity Framework Core tools no disponibles."
        exit 1
    fi
    EF_COMMAND="dotnet ef"
else
    EF_COMMAND="dotnet-ef"
fi

echo "✅ Usando comando EF: $EF_COMMAND"

# Función para esperar a PostgreSQL
wait_for_postgres() {
    local host="$POSTGRES_HOST_SUBVENCIONES"
    local port="$POSTGRES_PORT_SUBVENCIONES"
    local user="$POSTGRES_USER_SUBVENCIONES"
    local db="$POSTGRES_DB_SUBVENCIONES"
    local max_tries=30
    local count=0

    echo "🔍 Esperando a que PostgreSQL esté disponible en $host:$port..."
    
    while [ $count -lt $max_tries ]; do
        echo "📡 Intento $((count+1))/$max_tries..."
        
        # Verificar conectividad de red
        if nc -z "$host" "$port" 2>/dev/null; then
            echo "✅ Puerto $port abierto en $host"
            
            # Verificar que PostgreSQL esté listo
            if PGPASSWORD="$POSTGRES_PASSWORD_SUBVENCIONES" pg_isready -h "$host" -p "$port" -U "$user" -d "$db" -q; then
                echo "✅ PostgreSQL está listo para conexiones"
                return 0
            else
                echo "⏳ PostgreSQL detectado pero no está listo..."
            fi
        else
            echo "❌ No se puede conectar a $host:$port"
            
            # Diagnóstico DNS
            if nslookup "$host" >/dev/null 2>&1; then
                echo "✅ DNS resuelve $host correctamente"
            else
                echo "❌ No se puede resolver el host $host"
            fi
        fi
        
        sleep 3
        count=$((count+1))
    done
    
    echo "⏰ Timeout esperando PostgreSQL después de $((max_tries * 3)) segundos"
    return 1
}

# Esperar a PostgreSQL
if ! wait_for_postgres; then
    echo "❌ No se pudo conectar a PostgreSQL. Abortando."
    exit 1
fi

# Aplicar migraciones
echo "🚀 Aplicando migraciones EF Core..."
if $EF_COMMAND database update --verbose; then
    echo "✅ Migraciones aplicadas exitosamente"
else
    echo "❌ Error aplicando migraciones"
    exit 1
fi

echo "🟢 Iniciando la aplicación SubvencionesApp.Api..."
exec "$@"