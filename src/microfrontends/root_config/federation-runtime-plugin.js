// root_config/federation-runtime-plugin.js
// Plugin de runtime para manejar errores y optimizar la carga de módulos remotos

const FederationRuntimePlugin = () => ({
  name: 'federation-runtime-plugin',
  
  // Interceptar la carga de remotos para manejar errores
  beforeRequest(args) {
    const { origin, remoteInfo } = args;
    
    // Log en desarrollo para debugging
    if (process.env.NODE_ENV === 'development') {
      console.log(`[Federation] Cargando remoto: ${remoteInfo.name} desde ${origin}`);
    }
    
    return args;
  },
  
  // Manejar errores de carga de módulos remotos
  errorLoadRemote(args) {
    const { id, error, from } = args;
    
    console.error(`[Federation] Error cargando remoto ${id}:`, error);
    
    // En desarrollo, mostrar información más detallada
    if (process.env.NODE_ENV === 'development') {
      console.error(`[Federation] Detalles del error:`, {
        id,
        from,
        error: error.message,
        stack: error.stack
      });
    }
    
    // Retornar un componente de fallback en lugar de fallar completamente
    return {
      default: () => {
        if (typeof window !== 'undefined') {
          return React.createElement('div', {
            style: {
              padding: '20px',
              border: '2px dashed #ff6b6b',
              borderRadius: '8px',
              textAlign: 'center',
              color: '#ff6b6b',
              backgroundColor: '#ffe0e0'
            }
          }, [
            React.createElement('h3', { key: 'title' }, 'Microfrontend no disponible'),
            React.createElement('p', { key: 'message' }, `El módulo "${id}" no pudo cargarse.`),
            process.env.NODE_ENV === 'development' && 
              React.createElement('p', { key: 'error' }, `Error: ${error.message}`)
          ].filter(Boolean));
        }
        return null;
      }
    };
  },
  
  // Optimizar la resolución de módulos compartidos
  beforeInit(args) {
    const { userOptions } = args;
    
    // Configurar estrategias de carga según el entorno
    if (process.env.NODE_ENV === 'development') {
      // En desarrollo, priorizar la velocidad de recarga
      userOptions.shared = {
        ...userOptions.shared,
        react: {
          ...userOptions.shared?.react,
          eager: true,
          singleton: true
        },
        'react-dom': {
          ...userOptions.shared?.['react-dom'],
          eager: true,
          singleton: true
        }
      };
    } else {
      // En producción, priorizar la optimización de carga
      userOptions.remotes = userOptions.remotes || [];
      
      // Precargar remotos críticos
      if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
        window.requestIdleCallback(() => {
          userOptions.remotes.forEach(remote => {
            if (remote.includes('dashboard_app')) {
              // Precargar el dashboard que es crítico
              import(remote).catch(() => {
                // Ignorar errores de precarga
              });
            }
          });
        });
      }
    }
    
    return args;
  },
  
  // Manejar la resolución de módulos compartidos
  beforeResolve(args) {
    const { shareScopeMap, scope, pkgName } = args;
    
    // Log en desarrollo para debugging de dependencias compartidas
    if (process.env.NODE_ENV === 'development' && pkgName) {
      console.log(`[Federation] Resolviendo dependencia compartida: ${pkgName}`);
    }
    
    return args;
  },
  
  // Hook para después de la inicialización
  init(args) {
    const { options } = args;
    
    // Configurar timeouts específicos para diferentes entornos
    if (typeof window !== 'undefined') {
      const isDockerCompose = process.env.IS_DOCKER_COMPOSE === 'true';
      const timeout = isDockerCompose ? 10000 : 5000; // Más tiempo para Docker Compose
      
      // Configurar timeout global para requests de módulos remotos
      window.__FEDERATION_TIMEOUT__ = timeout;
    }
    
    console.log('[Federation] Plugin de runtime inicializado');
    
    return args;
  }
});

module.exports = FederationRuntimePlugin;