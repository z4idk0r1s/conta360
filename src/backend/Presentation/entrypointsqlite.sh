#!/bin/bash
set -e

echo "📋 Aplicando migraciones de base de datos..."

if [ -f "$API_PROJECT_DIR/$API_PROJECT" ]; then
  (cd "$API_PROJECT_DIR" && dotnet ef database update)
  if [ $? -eq 0 ]; then
    echo "✅ Migraciones (NO POSTGRES) aplicadas correctamente."
  else
    echo "❌ Fallo al aplicar las migraciones. Revisa los logs."
    exit 1
  fi
else
  echo "⚠️ Proyecto API no encontrado en $API_PROJECT_DIR/$API_PROJECT. Omitiendo migraciones."
fi

# Luego se arranca la aplicación normalmente
exec dotnet "$API_PROJECT_DIR/Conta360.Presentation.Api.dll"
