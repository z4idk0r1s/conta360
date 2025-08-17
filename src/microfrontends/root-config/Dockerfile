# src/microfrontends/root-config/Dockerfile
# Dockerfile para construir la imagen del microfrontend 'root-config'.

# --- Etapa de construcción (builder stage) ---
FROM node:20-alpine AS builder
# Crea el usuario 'vscode' y su grupo.
RUN addgroup -S vscode && adduser -S vscode -G vscode
# Establece el directorio de trabajo.
WORKDIR /app
# Cambia al usuario 'vscode' antes de copiar y hacer 'npm install'.
USER vscode
# Copia los archivos y asigna los permisos en el mismo paso.
COPY --chown=vscode:vscode package.json .
# Instala las dependencias.
RUN npm install
# Copia el resto del código fuente, asignando los permisos en el mismo paso.
COPY --chown=vscode:vscode . .

# --- Etapa de ejecución (runner stage) ---
FROM node:20-alpine AS runner
# Crea el usuario 'vscode' y su grupo.
RUN addgroup -S vscode && adduser -S vscode -G vscode
# Establece el directorio de trabajo.
WORKDIR /app
# Copia los artefactos de la etapa 'builder', manteniendo los permisos.
COPY --from=builder --chown=vscode:vscode /app .
# Cambia al usuario 'vscode' para la ejecución del servidor.
USER vscode
# Comando por defecto para ejecutar al iniciar el contenedor.
CMD ["npm", "run", "dev"]