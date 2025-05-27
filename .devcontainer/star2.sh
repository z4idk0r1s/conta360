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

# 1. Crear o limpiar la solución
if [ ! -f "conta360.sln" ]; then
    echo "🧪 Generando conta360.sln..."
    dotnet new sln -n conta360
else
    echo "🧹 Limpiando estructura de carpetas virtuales en conta360.sln..."
    sed -i '/^Project("{2150E333-.*}") = .*$/,/^EndProject$/d' conta360.sln
    sed -i '/GlobalSection(NestedProjects)/,/EndGlobalSection/d' conta360.sln
fi

# 2. Buscar todos los archivos .csproj
echo "🔍 Buscando archivos .csproj..."
mapfile -t csproj_files < <(find src/backend -name "*.csproj" | sort)

# 3. Agregar proyectos a la solución
for proj in "${csproj_files[@]}"; do
    if dotnet sln conta360.sln list | grep -qF "$proj"; then
        echo "✔️ Ya agregado: $proj"
    else
        echo "➕ Agregando: $proj"
        dotnet sln conta360.sln add "$proj"
    fi
done

echo "✅ Proyectos .NET añadidos a conta360.sln"

# 4. Restaurar y compilar
echo "🔧 Restaurando dependencias .NET..."
dotnet restore conta360.sln

echo "🔨 Compilando proyectos .NET..."
dotnet build conta360.sln --configuration Debug

echo "✅ Compilación exitosa."

# 5. Instalar dependencias npm
MICROFRONTEND_DIRS=(
    "src/microfrontends/root-config"
    "src/microfrontends/dashboard-app"
    "src/microfrontends/analisis-app"
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

# 6. Ejecutar pruebas
echo "🧪 Ejecutando pruebas..."

if [ -f "src/backend/Conta360.Services.Api/Conta360.Services.Api.csproj" ]; then
    dotnet test src/backend/Conta360.Services.Api/Conta360.Services.Api.csproj \
        || echo "⚠️ Pruebas de backend fallaron."
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

# 7. Detener instancias previas
pkill -f "dotnet run --project .*Conta360.Services.Api.csproj" 2>/dev/null \
  && echo "🛑 Backend detenido" || echo "ℹ️ No había backend corriendo"

pkill -f "npm run dev.*root-config" 2>/dev/null \
  && echo "🛑 root-config detenido" || echo "ℹ️ No había root-config activo"

# 8. Iniciar servicios
echo "🚀 Iniciando servicios..."

nohup dotnet run --project src/backend/Conta360.Services.Api/Conta360.Services.Api.csproj \
  --no-build --no-restore --urls "http://localhost:5000" \
  > /tmp/backend_log.log 2>&1 &

cd src/microfrontends/root-config
nohup npm run dev > /tmp/frontend_root_config_log.log 2>&1 &
cd ../../..

# 9. Esperar servicios
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
