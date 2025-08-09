# src\backend\Presentation\entrypointsqlite.sh
#!/bin/bash
set -e

echo "Iniciando y aplicando migraciones de base de datos SQLITE..."

# Ejecutamos las migraciones. La cadena de conexión se obtiene del entorno del contenedor.
# Como el WORKDIR en la etapa final es /app, y los archivos publicados están allí,
# el comando funciona sin necesidad de rutas de proyecto.
dotnet ef database update

# Capturamos el código de salida de la migración
if [ $? -ne 0 ]; then
    echo "!!!! ERROR al aplicar las migraciones. PROCESO DETENIDO POR COMPLETO (punto de salida, entrypointsqlite.sh!!!!"
    exit 1
fi

echo " - MIGRACIONES SQLITE OK - (PORCESO EN entrypointsqlite.sh finalizado)"

# Se arranca la aplicación de forma segura
echo "Iniciando la aplicación..."
exec dotnet "Conta360.Presentation.Api.dll"
