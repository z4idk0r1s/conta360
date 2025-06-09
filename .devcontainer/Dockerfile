ARG VARIANT="8.0"
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# Configuración de entorno
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5000

# === PERMISOS DE USUARIO ===
# Asegura que la caché de NPM pertenezca al usuario 'vscode' con los permisos correctos.
# Esto previene el error EACCES.
USER root
RUN mkdir -p /home/vscode/.npm && chown -R vscode:vscode /home/vscode/.npm
USER vscode
ENV npm_config_cache=/home/vscode/.npm

# ===============================

# Variables dinámicas
ARG WORKSPACE_NAME
WORKDIR /workspaces/${WORKSPACE_NAME}

# Exponer puertos del microfrontend y servicios backend
EXPOSE 3000
EXPOSE 3004
EXPOSE 5000
EXPOSE 5001

# Modo desarrollo (permite usar VSCode sin ejecutar nada directamente)
CMD ["sleep", "infinity"]
