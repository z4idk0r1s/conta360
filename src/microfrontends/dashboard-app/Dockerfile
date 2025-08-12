# builder
FROM node:20-alpine AS builder

# El contexto de construcción es ./src/microfrontends, así que las rutas
# de origen deben ser relativas a él.
WORKDIR /app/tailwind-config
COPY ./tailwind-config/package.json ./

WORKDIR /app/dashboard-app
COPY ./dashboard-app/package.json ./package.json

WORKDIR /app/tailwind-config
RUN npm install

WORKDIR /app/dashboard-app
RUN npm install

# copiar el resto del código (ahora sí estará en /app)
COPY ./dashboard-app/. .

ARG NEXT_PUBLIC_API_URL
ENV NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL

RUN npm run build

# opcional: eliminar dev deps para dejar sólo producción (reduce tamaño)
RUN npm prune --production

# runner
FROM node:20-alpine AS runner
WORKDIR /app/dashboard-app
ENV NODE_ENV=production

# copiar artefactos de build y node_modules
COPY --from=builder /app/dashboard-app/.next ./.next
COPY --from=builder /app/dashboard-app/public ./public
COPY --from=builder /app/dashboard-app/package.json ./package.json
COPY --from=builder /app/dashboard-app/node_modules ./node_modules
# También necesita el paquete de tailwind compilado
COPY --from=builder /app/tailwind-config /app/tailwind-config

EXPOSE 3001
CMD ["npm", "run", "start"]