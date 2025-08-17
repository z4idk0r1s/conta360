# src/microfrontends/dashboard-app/Dockerfile
# Dockerfile para construir la imagen del microfrontend 'dashboardApp'.

# --- Etapa de construcción (builder stage) ---
FROM node:20-alpine AS builder
# Crea el usuario 'vscode' y su grupo.
RUN addgroup -S vscode && adduser -S vscode -G vscode
# Establece el directorio de trabajo y cambia al usuario.
WORKDIR /app
USER vscode
# Copia los archivos de manifiesto de dependencias y les asigna la propiedad correcta.
COPY --chown=vscode:vscode package.json .
# Instala las dependencias. '--frozen-lockfile' asegura versiones exactas.
RUN npm install
# Copia el resto del código fuente y les asigna la propiedad correcta.
COPY --chown=vscode:vscode . .

# Si fuera para una build estática de producción (ej. para Tauri), se descomentaría:
# RUN npm run build

# --- Etapa de ejecución (runner stage) ---
FROM node:20-alpine AS runner
# Crea el usuario 'vscode' y su grupo.
RUN addgroup -S vscode && adduser -S vscode -G vscode
# Establece el directorio de trabajo.
WORKDIR /app
# Copia los artefactos de la etapa 'builder', manteniendo los permisos.
COPY --from=builder --chown=vscode:vscode /app .

# Expone el puerto 3001, que es el puerto interno donde la aplicación Next.js escuchará.
EXPOSE 3001
# Cambia al usuario 'vscode' para la ejecución del servidor.
USER vscode
# Comando por defecto para ejecutar al iniciar el contenedor.
# En desarrollo, se usa 'npm run dev' para habilitar el hot reload del servidor Next.js.
CMD ["npm", "run", "dev"]

# Para entornos de producción donde se sirve una build estática (generada por 'npm run build'),
# se usaría un servidor web estático o 'npm run start' si hay un servidor Next.js.
# CMD ["npm", "run", "start"]