// src/microfrontends/root-config/mf-remotes.config.js

/**
 * @file Archivo de configuración centralizado para los microfrontends remotos.
 * Este archivo define los puertos de desarrollo y la lógica para generar URLs de remotos
 * dinámicamente, adaptándose tanto a entornos de desarrollo local como a Docker Compose.
 *
 * Objetivo: Facilitar la adición de nuevos microfrontends remotos y asegurar la
 * comunicación correcta en distintos entornos de desarrollo.
 */

/**
 * Objeto que mapea el nombre lógico de cada microfrontend remoto a su puerto de desarrollo local.
 * Este nombre también se utilizará como nombre de servicio en Docker Compose.
 * @type {Object.<string, number>}
 */
const REMOTE_PORTS = {
  // Ejemplo: Nombre lógico del MF: Puerto de desarrollo
  dashboardApp: 3001,
  analisisApp: 3002,
  pgcApp: 3003,
  sharedComponents: 3004,
  // Para añadir un nuevo MF, simplemente añade una nueva entrada aquí:
  // nuevoMfApp: 3005,
};

/**
 * Genera la configuración 'remotes' para Next.js Module Federation de forma dinámica.
 * Las URLs de los remotos se adaptan según el entorno de ejecución (local o Docker Compose).
 *
 * @param {object} options - Opciones de Webpack proporcionadas por Next.js, incluyendo 'isWebpack5'.
 * @returns {Object.<string, string>} Un objeto con la configuración 'remotes' para NextFederationPlugin.
 */
const getRemotes = (options) => {
  // Determina si la aplicación se está ejecutando dentro de Docker Compose en modo desarrollo.
  // La variable IS_DOCKER_COMPOSE debe ser inyectada en el contenedor por docker-compose.yml.
  const isDockerComposeEnv = process.env.NODE_ENV === 'development' && process.env.IS_DOCKER_COMPOSE === 'true';

  const remotesConfig = {};

  for (const [mfName, mfPort] of Object.entries(REMOTE_PORTS)) {
    let remoteUrl;

    if (isDockerComposeEnv) {
      // En entorno Docker Compose, la comunicación es a través del nombre del servicio Docker
      // y su puerto interno (que coincide con el puerto de desarrollo del MF).
      // Ejemplo: http://dashboardApp:3001/_next/static/chunks/remoteEntry.js
      remoteUrl = `http://${mfName}:${mfPort}/_next/static/chunks/remoteEntry.js`;
    } else {
      // En desarrollo local (fuera de Docker), la comunicación es a través de localhost.
      // Ejemplo: http://localhost:3001/_next/static/chunks/remoteEntry.js
      remoteUrl = `http://localhost:${mfPort}/_next/static/chunks/remoteEntry.js`;
    }

    // La lógica de 'options.isWebpack5' es para compatibilidad con versiones de Webpack
    // y para especificar la ruta del 'remoteEntry.js' en la build de producción (cuando output: 'export').
    // Para producción (ej. con Tauri), los MFs se esperan servir desde una ruta relativa,
    // asumiendo que están empaquetados junto al host.
    const productionRelativePath = `/_next/static/chunks/${mfName}-remoteEntry.js`;

    remotesConfig[mfName] = `${mfName}@${options.isWebpack5 ? remoteUrl : productionRelativePath}`;
  }
  return remotesConfig;
};

module.exports = {
  REMOTE_PORTS,
  getRemotes,
};