@echo off
echo "Iniciando limpieza de Docker..."

:: Detener todos los contenedores activos
docker stop $(docker ps -q)
echo "Contenedores activos detenidos."

:: Detener y eliminar contenedores de Docker Compose
docker-compose down -v
docker-compose down --volumes --remove-orphans

echo "Docker Compose down ejecutado."

:: Eliminar todos los contenedores
docker rm $(docker ps -a -q)
echo "Contenedores eliminados."

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
docker rmi $(docker images -q --filter "label=devcontainer.image=true")
echo "Imágenes con etiqueta 'devcontainer.image=true' eliminadas."

:: Limpiar volúmenes y redes no utilizados
docker network prune -f
echo "Redes no utilizadas eliminadas."

:: Limpiar sqlite3
docker volume rm sqlite_data
echo "sqlite3 eliminada."

:: Limpiar al finalizar
docker-compose down -v
echo "---LIMPIEZA FINALIZADA---"

pause
