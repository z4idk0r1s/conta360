# .devcontainer/Dockerfile
ARG VARIANT="8.0"
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# Configuración de entorno
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5000

# === Instalar Node.js 20.11.1 en el HOME del usuario vscode ===
USER vscode
WORKDIR /home/vscode

# Directorio de instalación local
ENV NODE_VERSION=20.11.1
ENV NODE_DIR=/home/vscode/.local/node-v$NODE_VERSION
ENV PATH=$NODE_DIR/bin:$PATH

# Descargar y extraer Node.js
RUN mkdir -p $NODE_DIR && \
    curl -fsSL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.xz | \
    tar -xJf - --strip-components=1 -C $NODE_DIR

# Configurar la caché de npm para evitar errores de permisos
ENV npm_config_cache=/home/vscode/.npm
RUN mkdir -p /home/vscode/.npm

# === EF === Instalar dotnet-ef como herramienta global en el contenedor
ENV PATH="$PATH:/home/vscode/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef

# Variables dinámicas
ARG WORKSPACE_NAME
WORKDIR /workspaces/${WORKSPACE_NAME}

# Exponer puertos
EXPOSE 3000
EXPOSE 3001
EXPOSE 3004
EXPOSE 5000
EXPOSE 5001
EXPOSE 5432
EXPOSE 8080

# No ejecutar nada, entorno de desarrollo puro
CMD ["sleep", "infinity"]
