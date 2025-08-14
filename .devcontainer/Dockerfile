# .devcontainer/Dockerfile
ARG VARIANT="8.0"
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# Configuración de entorno
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

# === ELIMINAR .NET DE LA IMAGEN BASE E INSTALAR 8.0.413 ESPECÍFICO ===
USER root
RUN DOTNET_VERSION=8.0.413 && \
    echo "🗑️ Eliminando .NET de la imagen base y extensiones de VSCode..." && \
    # ELIMINAR COMPLETAMENTE todas las instalaciones de .NET (incluidas las de VSCode)
    rm -rf /usr/share/dotnet && \
    rm -rf /usr/local/share/dotnet && \
    rm -rf /opt/dotnet && \
    rm -rf /home/vscode/.vscode-server/data/User/globalStorage/ms-dotnettools.vscode-dotnet-runtime && \
    rm -f /usr/local/bin/dotnet && \
    rm -f /usr/bin/dotnet && \
    # Limpiar variables de entorno anteriores
    unset DOTNET_ROOT && \
    echo "✅ Todas las instalaciones de .NET eliminadas" && \
    # Crear directorio temporal para descarga
    TEMP_DIR=$(mktemp -d) && \
    cd $TEMP_DIR && \
    echo "⬇️ Descargando .NET SDK ${DOTNET_VERSION}..." && \
    # Descargar SDK desde CDN oficial de Microsoft
    curl -fsSL "https://dotnetcli.azureedge.net/dotnet/Sdk/${DOTNET_VERSION}/dotnet-sdk-${DOTNET_VERSION}-linux-x64.tar.gz" \
         -o dotnet-sdk.tar.gz && \
    # Verificar descarga exitosa
    if [ ! -f dotnet-sdk.tar.gz ] || [ ! -s dotnet-sdk.tar.gz ]; then \
        echo "❌ ERROR: No se pudo descargar .NET SDK ${DOTNET_VERSION}"; \
        exit 1; \
    fi && \
    echo "📦 Instalando .NET SDK ${DOTNET_VERSION}..." && \
    # Crear directorio limpio para la nueva instalación
    mkdir -p /usr/share/dotnet && \
    # Extraer e instalar ÚNICAMENTE la versión 8.0.413
    tar -zxf dotnet-sdk.tar.gz -C /usr/share/dotnet && \
    # Crear enlace simbólico en el PATH
    ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet && \
    # Establecer permisos correctos
    chmod +x /usr/share/dotnet/dotnet && \
    # Configurar DOTNET_ROOT para evitar conflictos
    echo 'export DOTNET_ROOT=/usr/share/dotnet' >> /etc/environment && \
    # RESTAURAR PERMISOS DEL USUARIO VSCODE
    chown -R vscode:vscode /home/vscode && \
    # Limpieza completa
    cd / && rm -rf $TEMP_DIR && \
    echo "✅ .NET SDK ${DOTNET_VERSION} instalado como ÚNICA VERSIÓN"

# Configurar PATH y DOTNET_ROOT para evitar conflictos
ENV PATH=/usr/share/dotnet:$PATH \
    DOTNET_ROOT=/usr/share/dotnet \
    DOTNET_MULTILEVEL_LOOKUP=0

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

# === VERIFICACIÓN FINAL DE ÚNICA VERSIÓN ===
RUN echo "=== VERIFICACIÓN DE VERSIONES INSTALADAS ===" && \
    echo "🔍 Verificando que SOLO existe .NET 8.0.413:" && \
    dotnet --version && \
    dotnet --list-sdks && \
    echo "📦 Verificando Node.js:" && \
    node --version && \
    npm --version && \
    echo "🔧 Verificando Entity Framework:" && \
    dotnet ef --version && \
    echo "🌍 Variables de entorno:" && \
    echo "DOTNET_ROOT=$DOTNET_ROOT" && \
    echo "DOTNET_MULTILEVEL_LOOKUP=$DOTNET_MULTILEVEL_LOOKUP" && \
    echo "PATH (dotnet)=$(which dotnet)" && \
    echo "✅ VERIFICACIÓN COMPLETADA - CONFIGURACIÓN OPTIMIZADA" && \
    echo "🎯 .NET SDK 8.0.413 es la ÚNICA versión - MSBuild funcionará correctamente"

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
EXPOSE 8081

# No ejecutar nada, entorno de desarrollo puro
CMD ["sleep", "infinity"]