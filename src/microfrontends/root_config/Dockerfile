# src/microfrontends/root_config/Dockerfile
# Dockerfile para construir la imagen del microfrontend 'root_config'.

# --- Etapa de construcción (builder stage) ---
# Utiliza una imagen base de Node.js ligera para la construcción.
FROM node:20-alpine AS builder
# Establece el directorio de trabajo dentro del contenedor.
WORKDIR /app
# Copia los archivos de manifiesto de dependencias para aprovechar el cache de Docker.
# Esto asegura que 'npm install' solo se ejecute si cambian estas dependencias.
COPY package.json .

# Instala las dependencias del proyecto.
RUN npm install
# Copia el resto del código fuente al directorio de trabajo.
COPY . .

# La instrucción `RUN npm run build` está comentada intencionalmente para desarrollo.
# Si se quisiera una build para producción que se sirva estáticamente (ej. para Tauri)
# y no mediante 'npm run dev', se descomentaría esta línea.
# RUN npm run build

# La variable de entorno `ENV NEXT_PUBLIC_API_URL` se gestiona en docker-compose.yml
# para que sea dinámica según el entorno. No se define aquí para evitar hardcoding.
# ENV NEXT_PUBLIC_API_URL=http://validation-core:80

# --- Etapa de ejecución (runner stage) ---
# Utiliza una imagen base de Node.js ligera para la ejecución final.
FROM node:20-alpine AS runner
# Establece el directorio de trabajo.
WORKDIR /app
# Copia los artefactos de la etapa 'builder' a la etapa 'runner'.
COPY --from=builder /app .

EXPOSE 3000
# Comando por defecto para ejecutar al iniciar el contenedor.
# En desarrollo, se usa 'npm run dev' para habilitar el hot reload del servidor Next.js.
CMD ["npm", "run", "dev"]

# Para entornos de producción donde se sirve una build estática (generada por 'npm run build'),
# se usaría 'npm run start' si hay un servidor Next.js, o un servidor web estático.
# CMD ["npm", "run", "start"]