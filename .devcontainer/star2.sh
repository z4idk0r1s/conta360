#!/bin/bash
set -e # Salir si un comando falla

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

# Rutas importantes
SOLUTION_FILE="Conta360.sln" # <-- ¡Variable definida para el archivo de solución!
API_PROJECT="src/backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"
API_PROJECT_DIR="src/backend/Presentation/Conta360.Presentation.Api"

# --- SECCIÓN MODIFICADA PARA USAR EL ARCHIVO .SLN ---
# 1. Restaurar dependencias de la solución
# Se elimina la búsqueda de .csproj individuales y el bucle asociado.
echo "🔧 Restaurando dependencias .NET de la solución..."
if [ -f "$SOLUTION_FILE" ]; then
  echo "🔄 dotnet restore \"$SOLUTION_FILE\""
  dotnet restore "$SOLUTION_FILE"
else
  echo "❌ Archivo de solución ($SOLUTION_FILE) no encontrado. No se pueden restaurar dependencias."
  exit 1
fi

# 2. Compilar la solución
# Se elimina el bucle de compilación de proyectos individuales.
echo "🔨 Compilando la solución .NET..."
if [ -f "$SOLUTION_FILE" ]; then
  echo "🏗️ dotnet build \"$SOLUTION_FILE\" --configuration Debug"
  dotnet build "$SOLUTION_FILE" --configuration Debug
else
  echo "❌ Archivo de solución ($SOLUTION_FILE) no encontrado. No se puede compilar."
  exit 1
fi
echo "✅ Compilación de proyectos .NET completada <<<<<<<<<<<<<<<<<"
# --- FIN DE LA SECCIÓN MODIFICADA ---


# 4. Aplicar migraciones de base de datos
echo "📋 Aplicando migraciones de base de datos..."
if [ -f "$API_PROJECT" ]; then
  (cd "$API_PROJECT_DIR" && dotnet ef database update)
  if [ $? -eq 0 ]; then
    echo "✅ Migraciones aplicadas correctamente."
  else
    echo "❌ Fallo al aplicar las migraciones. Revisa los logs."
    exit 1
  fi
else
  echo "⚠️ Proyecto API no encontrado en $API_PROJECT. Omitiendo migraciones."
fi

# 5. Instalar dependencias npm
MICROFRONTEND_DIRS=(
  "src/microfrontends/root-config"
  "src/microfrontends/dashboard-app"
)

echo "📦 Instalando dependencias npm..."

for dir in "${MICROFRONTEND_DIRS[@]}"; do
  if [ -f "$dir/package.json" ]; then
    echo "📁 $dir"
    (cd "$dir" && [ -f package.json ] && npm ci || npm install)
  else
    echo "⚠️ No encontrado: $dir/package.json. Omitiendo."
  fi
done

# --- SECCIÓN MODIFICADA PARA EJECUTAR PRUEBAS EN LA SOLUCIÓN ---
# 6. Ejecutar pruebas
echo "🧪 Ejecutando pruebas..."

# Aquí se ejecuta dotnet test sobre la solución, que gestionará las pruebas de todos los proyectos en ella.
if [ -f "$SOLUTION_FILE" ]; then
  echo "🧪 dotnet test \"$SOLUTION_FILE\" --no-build"
  dotnet test "$SOLUTION_FILE" --no-build || echo "⚠️ Pruebas de backend fallaron."
else
  echo "⚠️ Archivo de solución ($SOLUTION_FILE) no encontrado. Omitiendo pruebas backend."
fi

for dir in "${MICROFRONTEND_DIRS[@]}"; do
  if [ -f "$dir/package.json" ]; then
    if (cd "$dir" && grep -q '"test":' package.json); then
      echo "▶️ npm test en $dir"
      (cd "$dir" && npm test) || echo "⚠️ Pruebas fallaron en $dir"
    else
      echo "ℹ️ $dir no tiene script test"
    fi
  fi
done
# --- FIN DE LA SECCIÓN MODIFICADA ---

# 7. Detener servicios previos
pkill -f "dotnet run --project .*Conta360.Presentation.Api.csproj" 2>/dev/null \
  && echo "🛑 Backend detenido" || echo "ℹ️ No había backend corriendo"

pkill -f "npm run dev.*root-config" 2>/dev/null \
  && echo "🛑 root-config detenido" || echo "ℹ️ No había root-config activo"

# Detener servicio de dashboard-app si está corriendo
pkill -f "npm run dev.*dashboard-app" 2>/dev/null \
  && echo "🛑 dashboard-app detenido" || echo "ℹ️ No había dashboard-app activo"


# 8. Iniciar servicios
echo "🚀 Iniciando servicios..."

echo "🚀 Iniciando Backend API..."
nohup dotnet run --project "$API_PROJECT" \
  --no-build --no-restore --urls "http://localhost:5000" \
  > /tmp/backend_log.log 2>&1 &
echo "✅ Backend API iniciado. Logs en /tmp/backend_log.log"

echo "🚀 Iniciando Microfrontend Dashboard App..."
cd src/microfrontends/dashboard-app
nohup npm run dev > /tmp/frontend_dashboard_app_log.log 2>&1 &
cd ../../..
echo "✅ Microfrontend Dashboard App iniciado. Logs en /tmp/frontend_dashboard_app_log.log"

echo "🚀 Iniciando Microfrontend Root Config..."
cd src/microfrontends/root-config
nohup npm run dev > /tmp/frontend_root_config_log.log 2>&1 &
cd ../../..
echo "✅ Microfrontend Root Config iniciado. Logs en /tmp/frontend_root_config_log.log"


# 9. Esperar disponibilidad
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

wait_for_service "Backend API" "http://localhost:5000/health" || { echo "❌ Falló el inicio del Backend API."; exit 1; }
wait_for_service "Frontend Dashboard App" "http://localhost:3001" || { echo "❌ Falló el inicio del Frontend Dashboard App."; exit 1; }
wait_for_service "Frontend Root Config" "http://localhost:3000" || { echo "❌ Falló el inicio del Frontend Root Config."; exit 1; }

echo ""
echo "✅ Todos los PROCESOS FINALIZADOS - OK - Conta360."
echo "👉 Para ver logs:"
echo " tail o cat -f /tmp/backend_log.log"
echo " tail o cat -f /tmp/frontend_root_config_log.log"
echo " tail o cat -f /tmp/frontend_dashboard_app_log.log"