@echo off
setlocal enableDelayedExpansion

echo "Iniciando limpieza de Docker..."

:: Detener todos los contenedores activos
FOR /f "tokens=*" %%i IN ('docker ps -q') DO (
    docker stop %%i
)
IF %ERRORLEVEL% NEQ 0 (
    echo "No hay contenedores activos para detener."
) ELSE (
    echo "Contenedores activos detenidos."
)

:: Detener y eliminar contenedores de Docker Compose
docker-compose down -v
docker-compose down --volumes --remove-orphans

echo "Docker Compose down ejecutado."

:: Eliminar todos los contenedores
FOR /f "tokens=*" %%i IN ('docker ps -a -q') DO (
    docker rm %%i
)
IF %ERRORLEVEL% NEQ 0 (
    echo "No hay contenedores para eliminar."
) ELSE (
    echo "Contenedores eliminados."
)

:: Eliminar todas las imágenes, contenedores, redes y caché de construcción no utilizados
docker system prune -a -f
echo "Docker system prune ejecutado."

:: Si también quieres eliminar todos los volúmenes
docker system prune -a --volumes -f
echo "Docker system prune con volúmenes ejecutado."

:: Eliminar imágenes
docker image prune -a -f
echo "Docker image prune ejecutado."

:: Eliminar todos los cachés de la construcción
docker builder prune -a -f
echo "Docker builder prune ejecutado."

:: Eliminar volúmenes no utilizados
docker volume prune -f
echo "Docker volume prune ejecutado."

:: Eliminar imágenes etiquetadas como 'devcontainer.image=true'
FOR /f "tokens=*" %%i IN ('docker images -q --filter "label=devcontainer.image=true"') DO (
    docker rmi %%i
)
IF %ERRORLEVEL% NEQ 0 (
    echo "No hay imágenes con etiqueta 'devcontainer.image=true' para eliminar."
) ELSE (
    echo "Imágenes con etiqueta 'devcontainer.image=true' eliminadas."
)

:: Limpiar volúmenes y redes no utilizados
docker network prune -f
echo "Redes no utilizadas eliminadas."

:: Limpiar sqlite3
docker volume rm sqlite_data 2>nul
IF %ERRORLEVEL% EQU 0 (
    echo "Volumen 'sqlite_data' eliminado."
) ELSE (
    echo "Volumen 'sqlite_data' no encontrado o ya eliminado."
)

---

### **Limpieza de Carpetas bin y obj de Proyectos .NET**

REM Inicia la sección de limpieza de .NET.

echo "Iniciando limpieza de carpetas bin y obj de proyectos .NET..."

REM Ejecuta dotnet clean en el directorio actual (la raíz del proyecto) para limpiar todas las carpetas bin y obj.
dotnet clean
IF %ERRORLEVEL% NEQ 0 (
    echo "Advertencia: dotnet clean encontró problemas."
) ELSE (
    echo "dotnet clean completado."
)

---

:: Limpiar al finalizar
docker-compose down -v
echo "---LIMPIEZA FINALIZADA---"

pause