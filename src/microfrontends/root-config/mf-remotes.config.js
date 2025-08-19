// root-config/mf-remotes.config.js
const REMOTE_PORTS = {
  'dashboard-app': 3000, // Puerto INTERNO del contenedor (no el externo 3001)
  // Para añadir un nuevo MF, añade una entrada aquí:
  // 'user-management': 3000,
  // 'analytics-app': 3000,
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
  
  for (const [mfName, mfPort] of Object.entries(REMOTE_PORTS)) {
    let remoteEntryPath;
    
    if (isDevelopment) {
      if (isDockerComposeEnv) {
        // En Docker Compose, SIEMPRE usar nombre de servicio (tanto servidor como cliente)
        // porque ambos ejecutan dentro de la red Docker
        remoteEntryPath = `${protocol}://${mfName}:${mfPort}/_next/static/chunks/remoteEntry.js`;
      } else {
        // En desarrollo local, usar localhost
        remoteEntryPath = `${protocol}://localhost:${mfPort}/_next/static/chunks/remoteEntry.js`;
      }
    } else {
      // En producción, asumir que están servidos desde el mismo dominio
      const basePath = process.env.NEXT_PUBLIC_BASE_PATH || '';
      remoteEntryPath = `${basePath}/_next/static/chunks/${mfName}-remoteEntry.js`;
    }
   
    // Formato correcto para Module Federation
    remotesConfig[mfName] = `${mfName}@${remoteEntryPath}`;
  }
  
  // Cachear solo para el cliente y no en el servidor
  if (!isServer) {
    cachedRemotes = remotesConfig;
    lastEnvironment = currentEnv;
  }
  
  console.log(`[mf-remotes] Configuración generada para ${currentEnv}:`, remotesConfig);
  
  return remotesConfig;
};

// Función para validar la disponibilidad de remotos en desarrollo
const validateRemoteAvailability = async (remoteName, remoteUrl) => {
  if (process.env.NODE_ENV !== 'development') return true;
  
  try {
    const response = await fetch(remoteUrl.split('@')[1], { 
      method: 'HEAD',
      timeout: 5000 
    });
    return response.ok;
  } catch (error) {
    console.warn(`[mf-remotes] Remoto '${remoteName}' no disponible: ${error.message}`);
    return false;
  }
};

// Función para obtener remotos disponibles (útil para desarrollo)
const getAvailableRemotes = async (options = {}) => {
  const allRemotes = getRemotes(options);
  const availableRemotes = {};
  
  for (const [name, url] of Object.entries(allRemotes)) {
    const isAvailable = await validateRemoteAvailability(name, url);
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
  
  for (const [mfName, mfPort] of Object.entries(REMOTE_PORTS)) {
    let baseUrl;
    
    if (isDevelopment) {
      if (isDockerComposeEnv) {
        baseUrl = `http://${mfName}:${mfPort}`;
      } else {
        baseUrl = `http://localhost:${mfPort}`;
      }
    } else {
      baseUrl = `https://${mfName}.${process.env.DOMAIN || 'localhost'}`;
    }
    
    healthCheckUrls[mfName] = `${baseUrl}/api/health`;
  }
  
  return healthCheckUrls;
};

module.exports = {
  REMOTE_PORTS,
  getRemotes,
  getAvailableRemotes,
  validateRemoteAvailability,
  clearCache,
  getHealthCheckUrls,
};