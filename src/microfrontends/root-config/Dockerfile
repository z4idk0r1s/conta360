# builder
FROM node:20-alpine AS builder

# El contexto de construcción es ./src/microfrontends
# Copiamos todo el contenido al directorio /app del contenedor.
COPY . /app

# Nos movemos a la carpeta de Tailwind.
WORKDIR /app/tailwind-config
COPY ./tailwind-config/package.json ./

# Ahora nos movemos a la carpeta de root-config.
WORKDIR /app/root-config
COPY ./root-config/package.json ./

# Nos movemos a la carpeta de Tailwind.
WORKDIR /app/tailwind-config
RUN npm install

# Ahora nos movemos a la carpeta de root-config.
WORKDIR /app/root-config
RUN npm install

# Compilar la aplicación de Next.js.
ARG NEXT_PUBLIC_API_URL
ENV NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL
ARG NEXT_PUBLIC_SUBVENCIONES_API_URL
ENV NEXT_PUBLIC_SUBVENCIONES_API_URL=$NEXT_PUBLIC_SUBVENCIONES_API_URL
RUN npm run build

# Opcional: Elimina dependencias de desarrollo para reducir el tamaño de la imagen final
RUN npm prune --production

# runner
FROM node:20-alpine AS runner
WORKDIR /app/root-config
ENV NODE_ENV=production

# Copia solo los artefactos necesarios.
COPY --from=builder /app/root-config/.next ./.next
COPY --from=builder /app/root-config/public ./public
COPY --from=builder /app/root-config/package.json ./package.json
COPY --from=builder /app/root-config/node_modules ./node_modules
COPY --from=builder /app/tailwind-config /app/tailwind-config

EXPOSE 3000
CMD ["npm", "run", "start"]