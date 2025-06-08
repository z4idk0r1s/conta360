#!/bin/bash
set -e

echo "🔍 Verificando prerrequisitos..."

# Verificación de herramientas necesarias
for cmd in dotnet node npm git curl; do
    command -v $cmd >/dev/null 2>&1 || {
        echo "❌ Herramienta requerida no encontrada: $cmd"
        exit 1
    }
done

echo "🛠️ Iniciando configuración del entorno Conta360..."

trap 'echo "❌ Error inesperado en línea $LINENO. Saliendo."; exit 1' ERR

# 1. Buscar todos los archivos .csproj
echo "🔍 Buscando archivos .csproj..."
mapfile -t csproj_files < <(find src/backend -name "*.csproj" | sort)

# 2. Restaurar dependencias
echo "🔧 Restaurando dependencias .NET..."
for proj in "${csproj_files[@]}"; do
    echo "🔄 dotnet restore: $proj"
    dotnet restore "$proj"
done

# 3. Compilar proyectos
echo "🔨 Compilando proyectos .NET..."
for proj in "${csproj_files[@]}"; do
    echo "🏗️ dotnet build: $proj"
    dotnet build "$proj" --configuration Debug
done

echo "✅ Compilación de proyectos .NET completada <<<<<<<<<<<<<<<<<"

# 4. Instalar dependencias npm
MICROFRONTEND_DIRS=(
    "src/microfrontends/root-config"
)

echo "📦 Instalando dependencias npm..."

for dir in "${MICROFRONTEND_DIRS[@]}"; do
    if [ -f "$dir/package.json" ]; then
        echo "📁 $dir"
        cd "$dir"
        [ -f package-lock.json ] && npm ci || npm install
        cd - >/dev/null
    else
        echo "⚠️ No encontrado: $dir/package.json. Omitiendo."
    fi
done

# 5. Ejecutar pruebas
echo "🧪 Ejecutando pruebas..."

API_PROJECT="src/backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"
if [ -f "$API_PROJECT" ]; then
    echo "🧪 dotnet test: $API_PROJECT"
    dotnet test "$API_PROJECT" || echo "⚠️ Pruebas de backend fallaron."
else
    echo "⚠️ Proyecto API no encontrado. Omitiendo pruebas backend."
fi

for dir in "${MICROFRONTEND_DIRS[@]}"; do
    if [ -f "$dir/package.json" ]; then
        cd "$dir"
        if grep -q '"test":' package.json; then
            echo "▶️ npm test en $dir"
            npm test || echo "⚠️ Pruebas fallaron en $dir"
        else
            echo "ℹ️ $dir no tiene script test"
        fi
        cd - >/dev/null
    fi
done

# 6. Detener instancias previas
pkill -f "dotnet run --project .*Conta360.Presentation.Api.csproj" 2>/dev/null \
  && echo "🛑 Backend detenido" || echo "ℹ️ No había backend corriendo"

pkill -f "npm run dev.*root-config" 2>/dev/null \
  && echo "🛑 root-config detenido" || echo "ℹ️ No había root-config activo"

# 7. Iniciar servicios
echo "🚀 Iniciando servicios..."

nohup dotnet run --project "$API_PROJECT" \
  --no-build --no-restore --urls "http://localhost:5000" \
  > /tmp/backend_log.log 2>&1 &

cd src/microfrontends/root-config
nohup npm run dev > /tmp/frontend_root_config_log.log 2>&1 &
cd ../../..

# 8. Esperar servicios
wait_for_service() {
    local name="$1"; local url="$2"
    echo "⏳ Esperando $name en $url"
    local timeout=180; local start_time=$(date +%s)
    while true; do
        if curl --silent --insecure "$url" >/dev/null 2>&1; then
            echo "✅ $name disponible"
            return 0
        fi
        if (( $(date +%s) - start_time > timeout )); then
            echo "❌ Timeout esperando $name. Verifica logs."
            return 1
        fi
        printf "."; sleep 2
    done
}

wait_for_service "Backend API" "http://localhost:5000/health"
wait_for_service "Frontend Root Config" "http://localhost:3000"

echo ""
echo "✅ Todo listo. Puedes trabajar con Conta360."
echo "👉 Abre VSCode o tus editores preferidos. Inicia microfrontends restantes si es necesario."
