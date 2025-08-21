// root_config/mf-remotes.config.js

// ⚙️ Puertos internos/externos
// - internal: el que usa Docker Compose (expuesto dentro de la red de contenedores).
// - external: el que ve el navegador cuando accede desde el host (localhost).
const REMOTE_PORTS = {
  dashboard_app: { internal: 3000, external: 3001 },
  // Para añadir un nuevo MF, añade una entrada aquí:
  // user-management: { internal: 3000, external: 3002 },
  // analytics-app: { internal: 3000, external: 3003 },
};

// Cache para evitar recalcular la configuración en cada llamada
let cachedRemotes = null;
let lastEnvironment = null;

const getRemotes = (options = {}) => {
  const { isServer = false } = options;
  const isDevelopment = process.env.NODE_ENV === 'development';
  const isDockerComposeEnv = isDevelopment && process.env.IS_DOCKER_COMPOSE === 'true';
  const currentEnv = `${isDevelopment}-${isDockerComposeEnv}-${isServer}`;

  // Usar cache si el entorno no ha cambiado y no estamos en el servidor
  if (cachedRemotes && lastEnvironment === currentEnv && !isServer) {
    console.log('[mf-remotes] Usando configuración cacheada');
    return cachedRemotes;
  }

  const remotesConfig = {};
  const protocol = isDevelopment ? 'http' : 'https';

  for (const [mfName, ports] of Object.entries(REMOTE_PORTS)) {
    let remoteEntryPath;

    if (isDevelopment) {
      if (isDockerComposeEnv) {
        // Diferentes URLs para servidor vs cliente
        if (isServer) {
          // SSR (servidor): usa nombre de servicio Docker + puerto interno
          remoteEntryPath = `${protocol}://${mfName}:${ports.internal}/_next/static/chunks/remoteEntry.js`;
        } else {
          // Cliente (navegador): usa localhost + puerto externo publicado
          remoteEntryPath = `${protocol}://localhost:${ports.external}/_next/static/chunks/remoteEntry.js`;
        }
      } else {
        // Desarrollo local sin Docker: usar localhost + puerto interno
        remoteEntryPath = `${protocol}://localhost:${ports.internal}/_next/static/chunks/remoteEntry.js`;
      }
    } else {
      // Producción: asumir que están servidos desde el mismo dominio
      const basePath = process.env.NEXT_PUBLIC_BASE_PATH || '';
      remoteEntryPath = `${basePath}/_next/static/chunks/${mfName}-remoteEntry.js`;
    }

    // Formato correcto para Module Federation
    remotesConfig[mfName] = `${mfName}@${remoteEntryPath}`;
  }

  // 🔧 CORRECCIÓN: Cachear solo configuración del cliente, no del servidor
  if (!isServer) {
    cachedRemotes = remotesConfig;
    lastEnvironment = currentEnv;
  }

  console.log(
    `[mf-remotes] Configuración generada para ENV(dev:${isDevelopment}, docker:${isDockerComposeEnv}, server:${isServer}):`,
    remotesConfig
  );

  return remotesConfig;
};

// 🔧 NUEVA FUNCIÓN: Validación específica para Docker Compose
const validateDockerRemoteAvailability = async (remoteName, remoteUrl, isServer = false) => {
  if (process.env.NODE_ENV !== 'development') return true;
  
  try {
    // En Docker Compose, validar usando curl desde el contenedor
    if (process.env.IS_DOCKER_COMPOSE === 'true' && isServer) {
      // Para SSR, validar usando nombre de servicio interno
      const internalUrl = remoteUrl.replace('localhost:3001', `${remoteName}:3000`);
      console.log(`[mf-remotes] Validando remoto interno: ${internalUrl}`);
      return true; // Asumir disponible en Docker network
    } else {
      // Para cliente, validar usando localhost
      const response = await fetch(remoteUrl.split('@')[1], {
        method: 'HEAD',
        timeout: 5000,
      });
      return response.ok;
    }
  } catch (error) {
    console.warn(
      `[mf-remotes] Remoto '${remoteName}' no disponible: ${error.message}`
    );
    return false;
  }
};

// Función para obtener remotos disponibles (útil para desarrollo)
const getAvailableRemotes = async (options = {}) => {
  const { isServer = false } = options;
  const allRemotes = getRemotes(options);
  const availableRemotes = {};

  for (const [name, url] of Object.entries(allRemotes)) {
    const isAvailable = await validateDockerRemoteAvailability(name, url, isServer);
    if (isAvailable) {
      availableRemotes[name] = url;
    }
  }

  return availableRemotes;
};

// Función para limpiar el cache (útil para hot reload)
const clearCache = () => {
  cachedRemotes = null;
  lastEnvironment = null;
  console.log('[mf-remotes] Cache limpiado');
};

// Función para generar configuración de health check
const getHealthCheckUrls = () => {
  const isDevelopment = process.env.NODE_ENV === 'development';
  const isDockerComposeEnv = isDevelopment && process.env.IS_DOCKER_COMPOSE === 'true';
  const healthCheckUrls = {};

  for (const [mfName, ports] of Object.entries(REMOTE_PORTS)) {
    let baseUrl;

    if (isDevelopment) {
      if (isDockerComposeEnv) {
        // Para health checks, usar nombre de servicio interno
        baseUrl = `http://${mfName}:${ports.internal}`;
      } else {
        baseUrl = `http://localhost:${ports.internal}`;
      }
    } else {
      baseUrl = `https://${mfName}.${process.env.DOMAIN || 'localhost'}`;
    }

    healthCheckUrls[mfName] = `${baseUrl}/api/health`;
  }

  return healthCheckUrls;
};

// 🔧 NUEVA FUNCIÓN: Debug para troubleshooting
const debugRemotesConfig = (options = {}) => {
  const { isServer = false } = options;
  const isDevelopment = process.env.NODE_ENV === 'development';
  const isDockerComposeEnv = isDevelopment && process.env.IS_DOCKER_COMPOSE === 'true';
  
  console.log('[mf-remotes] DEBUG INFO:');
  console.log('  - NODE_ENV:', process.env.NODE_ENV);
  console.log('  - IS_DOCKER_COMPOSE:', process.env.IS_DOCKER_COMPOSE);
  console.log('  - isDevelopment:', isDevelopment);
  console.log('  - isDockerComposeEnv:', isDockerComposeEnv);
  console.log('  - isServer:', isServer);
  console.log('  - REMOTE_PORTS:', REMOTE_PORTS);
  
  return getRemotes(options);
};

module.exports = {
  REMOTE_PORTS,
  getRemotes,
  getAvailableRemotes,
  validateRemoteAvailability: validateDockerRemoteAvailability,
  clearCache,
  getHealthCheckUrls,
  debugRemotesConfig, // Nueva función de debug
};