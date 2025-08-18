# src/microfrontends/dashboard-app/Dockerfile
# Dockerfile para construir la imagen del microfrontend 'dashboardApp'.

# --- Etapa de construcción (builder stage) ---
FROM node:20-alpine AS builder
WORKDIR /app
# Copia los archivos de manifiesto de dependencias para aprovechar el cache de Docker.
COPY package.json .

# Instala las dependencias. '--frozen-lockfile' asegura versiones exactas.
RUN npm install --legacy-peer-deps
# Copia el resto del código fuente al directorio de trabajo.
COPY . .

# Si fuera para una build estática de producción (ej. para Tauri), se descomentaría:
# RUN npm run build

# --- Etapa de ejecución (runner stage) ---
FROM node:20-alpine AS runner
WORKDIR /app
# Copia los artefactos (incluyendo node_modules si no se usó --omit=dev) desde la etapa 'builder'.
COPY --from=builder /app .

# Expone el puerto 3001, que es el puerto interno donde la aplicación Next.js escuchará.
# Debe coincidir con REMOTE_PORTS.dashboardApp en mf-remotes.config.js
EXPOSE 3001

# Comando por defecto para ejecutar al iniciar el contenedor.
# En desarrollo, se usa 'npm run dev' para habilitar el hot reload del servidor Next.js.
CMD ["npm", "run", "dev"]

# Para entornos de producción donde se sirve una build estática (generada por 'npm run build'),
# se usaría un servidor web estático o 'npm run start' si hay un servidor Next.js.
# CMD ["npm", "run", "start"]