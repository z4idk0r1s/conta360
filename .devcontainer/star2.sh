#!/bin/bash
set -e # Salir si un comando falla

# --- Asegurar permisos del espacio de trabajo antes de cualquier operación ---
echo "✅ Asegurando permisos del espacio de trabajo para el usuario vscode..."
sudo chown -R vscode:vscode /workspaces
echo "✅ Permisos del espacio de trabajo configurados correctamente."

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
SOLUTION_FILE="Conta360.sln"
API_PROJECT="src/backend/Presentation/Conta360.Presentation.Api/Conta360.Presentation.Api.csproj"
API_PROJECT_DIR="src/backend/Presentation/Conta360.Presentation.Api"
SUBVENCIONES_API_PROJECT="src/SubvencionesApp/SubvencionesApp.Api/SubvencionesApp.Api.csproj"
SUBVENCIONES_API_PROJECT_DIR="src/SubvencionesApp/SubvencionesApp.Api"

# --- SECCIÓN DE BUILD Y RESTORE ---
echo "🔧 Restaurando dependencias .NET de la solución..."
if [ -f "$SOLUTION_FILE" ]; then
  dotnet restore "$SOLUTION_FILE"
else
  echo "❌ Archivo de solución ($SOLUTION_FILE) no encontrado. No se pueden restaurar dependencias."
  exit 1
fi

echo "🔨 Compilando la solución .NET..."
if [ -f "$SOLUTION_FILE" ]; then
  dotnet build "$SOLUTION_FILE" --configuration Debug
else
  echo "❌ Archivo de solución ($SOLUTION_FILE) no encontrado. No se puede compilar."
  exit 1
fi
echo "✅ Compilación de proyectos .NET completada <<<<<<<<<<<<<<<<<"


# --- DEPENDENCIAS DE MICROFRONTENDS ---
MICROFRONTEND_DIRS=(
  "src/microfrontends/root-config"
  "src/microfrontends/dashboard-app"
)

echo "📦 Instalando dependencias npm..."
for dir in "${MICROFRONTEND_DIRS[@]}"; do
  if [ -f "$dir/package.json" ]; then
    echo "📁 $dir"
    (cd "$dir" && npm install)
  else
    echo "⚠️ No encontrado: $dir/package.json. Omitiendo."
  fi
done

# --- PRUEBAS ---
echo "🧪 Ejecutando pruebas Conta360 (sin servicios SubvencionesApp)..."
if [ -f "$SOLUTION_FILE" ]; then
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

echo ""
echo "✅ Entorno de desarrollo Conta360 (backend, microfrontends) preparado correctamente."
echo "👉 Los servicios ya están gestionados por docker-compose."
