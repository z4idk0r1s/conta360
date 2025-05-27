#!/bin/bash
set -e # Salir inmediatamente si un comando falla

echo "🔍 Verificando prerrequisitos..."

# Verificar herramientas necesarias
command -v dotnet >/dev/null 2>&1 || { echo "❌ .NET SDK no encontrado"; exit 1; }
command -v node >/dev/null 2>&1 || { echo "❌ Node.js no encontrado"; exit 1; }
command -v npm >/dev/null 2>&1 || { echo "❌ npm no encontrado"; exit 1; }
command -v git >/dev/null 2>&1 || { echo "❌ git no encontrado"; exit 1; }

echo "🛠️ Iniciando configuración del entorno de desarrollo conta360..."

# Función para manejar errores (se activa si 'set -e' no captura un error de tubería, etc.)
handle_error() {
    echo "❌ Error inesperado en línea $1. Saliendo."
    exit 1
}
trap 'handle_error $LINENO' ERR

# Verificar estructura del proyecto
REQUIRED_DIRS=(
    "./backend/Conta360Service"
    "./microfrontends/mf-validation"
    "./shared-models"
    "./backend/Adapters"
    "./backend/Conta360SDK"
    "./PGCExtractor/PGCExtractor.Core"
    "./PGCExtractor/PGCExtractor.Data"
    "./PGCExtractor/PGCExtractor.Logic"
    "./PGCExtractor/PGCExtractor.Tracker"
)

for dir in "${REQUIRED_DIRS[@]}"; do
    if [ ! -d "$dir" ]; then
        echo "❌ Directorio requerido no encontrado: $dir"
        exit 1
    fi
done

# Crear conta360.sln si no existe
if [ ! -f "conta360.sln" ]; then
    echo "🧪 Generando conta360.sln automáticamente..."
    dotnet new sln -n conta360
    dotnet sln conta360.sln add \
        ./shared-models/shared-models.csproj \
        ./backend/Adapters/Adapters.csproj \
        ./backend/Conta360SDK/Conta360SDK.csproj \
        ./backend/Conta360Service/Conta360Service.csproj \
        ./PGCExtractor/PGCExtractor.Core/PGCExtractor.Core.csproj \
        ./PGCExtractor/PGCExtractor.Data/PGCExtractor.Data.csproj \
        ./PGCExtractor/PGCExtractor.Logic/PGCExtractor.Logic.csproj \
        ./PGCExtractor/PGCExtractor.Tracker/PGCExtractor.Tracker.csproj
    echo "✅ conta360.sln creado con éxito."
else
    echo "✅ conta360.sln ya existe. Omitido."
fi

echo "🔧 Restaurando dependencias .NET..."
dotnet restore conta360.sln


echo "🔨 COMPILANDO proyectos .NET..."
echo "📦 (1/8)..."
dotnet build ./shared-models/shared-models.csproj --configuration Debug
echo "📦 (2/8)..."
dotnet build ./backend/Adapters/Adapters.csproj --configuration Debug
echo "📦 (3/8)..."
dotnet build ./backend/Conta360SDK/Conta360SDK.csproj --configuration Debug
echo "📦 (4/8)..."
dotnet build ./backend/Conta360Service/Conta360Service.csproj --configuration Debug
echo "📦 (5/8)..."
dotnet build ./PGCExtractor/PGCExtractor.Core/PGCExtractor.Core.csproj --configuration Debug
echo "📦 (6/8)..."
dotnet build ./PGCExtractor/PGCExtractor.Data/PGCExtractor.Data.csproj --configuration Debug
echo "📦 (7/8)..."
dotnet build ./PGCExtractor/PGCExtractor.Logic/PGCExtractor.Logic.csproj --configuration Debug
echo "📦 (8/8)..."
dotnet build ./PGCExtractor/PGCExtractor.Tracker/PGCExtractor.Tracker.csproj --configuration Debug


echo "📦 Instalando dependencias npm para microfrontend..."
if [ -f "./microfrontends/mf-validation/package.json" ]; then
    cd ./microfrontends/mf-validation
    echo "Limpiando la caché de NPM para evitar errores de permisos..."
    # Esta línea asume que el 'chown' en el Dockerfile ya estableció los permisos correctamente.
    # Si sigue habiendo problemas, el problema podría estar en el Dockerfile o la imagen base.
    sudo rm -rf /home/vscode/.npm/_cacache

    if [ -f "package-lock.json" ]; then
        echo "✅ package-lock.json encontrado. Ejecutando 'npm ci'..."
        npm ci
    else
        echo "⚠️ package-lock.json no encontrado. Ejecutando 'npm install' para generar..."
        npm install
    fi
    cd ../..
else
    echo "❌ package.json no encontrado en mf-validation. No se instalarán dependencias npm."
    exit 1 # Considera si quieres que esto sea un fallo fatal o solo una advertencia.
fi

## Ejecución de Pruebas de Código

echo "🧪 Ejecutando pruebas de código..."

# Ejecutar pruebas del backend (C#)
if [ -f "./backend/Conta360Service/Conta360Service.csproj" ]; then
    echo "  Ejecutando pruebas unitarias/integración de Conta360Service..."
    dotnet test ./backend/Conta360Service/Conta360Service.csproj || echo "⚠️ Las pruebas de Conta360Service fallaron, pero el setup del entorno continuará."
else
    echo "  ❌ No se encontró el proyecto de pruebas para Conta360Service. Omitiendo pruebas de backend."
fi

# Ejecutar pruebas del frontend (mf-validation)
if [ -f "./microfrontends/mf-validation/package.json" ]; then
    cd ./microfrontends/mf-validation
    echo "  Ejecutando pruebas unitarias/integración de mf-validation (npm test)..."
    # Asegúrate de que el script "test" esté definido en package.json de mf-validation.
    npm test || echo "⚠️ Las pruebas de mf-validation fallaron o el script 'test' no está definido en package.json. El setup del entorno continuará."
    cd ../..
else
    echo "  ❌ No se encontró package.json para mf-validation. Omitiendo pruebas de frontend."
fi

## Iniciando y Verificando Servicios

echo "🚀 Iniciando servicios en segundo plano..."

# --- 1. Iniciar el Backend (Conta360Service) ---
echo "  Iniciando backend (Conta360Service) en http://localhost:5000..."
nohup dotnet run --project ./backend/Conta360Service/Conta360Service.csproj --no-build --no-restore > /tmp/backend_log.log 2>&1 &
BACKEND_PID=$!

# --- 2. Iniciar el Frontend (mf-validation) ---
echo "  Iniciando frontend (mf-validation) en http://localhost:3000..."
cd ./microfrontends/mf-validation
nohup npm run serve > /tmp/frontend_log.log 2>&1 &
FRONTEND_PID=$!
cd ../..

# --- 3. Comprobar la Salud de los Servicios ---
echo "🔍 Realizando comprobaciones de salud de los servicios (esperando hasta 60 segundos por cada uno)..."

# Función para esperar a que el servicio esté listo
wait_for_service() {
    local host="$1"
    local port="$2"
    local name="$3"
    local url="$4"
    local timeout=180 # Segundos
    local start_time=$(date +%s)

    echo "    Esperando a que $name (${host}:${port}) esté listo..."
    while ! curl -s --fail $url > /dev/null; do
        current_time=$(date +%s)
        elapsed=$((current_time - start_time))
        if [ "$elapsed" -ge "$timeout" ]; then
            echo ""
            echo "❌ ERROR: $name no se levantó en ${timeout} segundos. Verifique los logs en /tmp/${name,,}_log.log"
            return 1
        fi
        printf "."
        sleep 2
    done
    echo ""
    echo "    ✅ $name está listo."
    return 0
}

# Comprobar Backend
wait_for_service localhost 5000 "backend" "http://localhost:5000/health"
if [ $? -ne 0 ]; then
    echo "⚠️ El backend no está funcionando correctamente. El entorno puede no ser completamente funcional."
    # Para un entorno de desarrollo, mejor advertir y continuar que fallar y detenerse.
    # Si prefieres que el contenedor falle si el backend no arranca: exit 1
fi

# Comprobar Frontend
wait_for_service localhost 3000 "frontend" "http://localhost:3000"
if [ $? -ne 0 ]; then
    echo "⚠️ El frontend no está funcionando correctamente. El entorno puede no ser completamente funcional."
    # Si prefieres que el contenedor falle si el frontend no arranca: exit 1
fi

echo "✅ Todos los servicios (backend y frontend) están levantados y parecen funcionar."
echo "✅ Entorno de desarrollo de Conta360 listo para usar en VSCode."