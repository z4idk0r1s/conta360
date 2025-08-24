// root_config/federation-runtime-plugin.js
/**
 * federation-runtime-plugin.js
 *
 * Plugin de runtime para Module Federation (Next.js)
 * - Seguro para SSR: evita rechazos/throws en servidor.
 * - Validaciones activas en cliente: mantiene comportamiento estricto en runtime.
 * - Logs útiles en dev.
 */

const FederationRuntimePlugin = () => Object.freeze({
  name: 'federation-runtime-plugin',

  // beforeRequest: VALIDACIÓN — solo activa en cliente
  beforeRequest(args) {
    try {
      // SSR: NO validar / NO rechazar, devolver tal cual para evitar 500 durante SSR.
      if (typeof window === 'undefined') return args;

      const { origin, remoteInfo, request } = args;

      if (!remoteInfo || !remoteInfo.name) {
        const error = new Error('RemoteInfo o su propiedad "name" no están definidos. Carga abortada.');
        console.error('[Federation] Error en beforeRequest:', error.message, { origin, request, remoteInfo });
        // En cliente sí rechazamos para que la lógica de error carga remotos la gestione.
        return Promise.reject(error);
      }

      if (process.env.NODE_ENV === 'development') {
        console.log(`[Federation] Petición preparada para remoto "${remoteInfo.name}" desde ${origin}`);
      }

      return args;
    } catch (err) {
      // Si ocurre algo inesperado, en SSR devolvemos args; en cliente, propagamos.
      if (typeof window === 'undefined') {
        console.error('[Federation] beforeRequest fallo en SSR, ignorando:', err);
        return args;
      } else {
        console.error('[Federation] beforeRequest fallo en cliente:', err);
        return Promise.reject(err);
      }
    }
  },

  // errorLoadRemote: logging defensivo (sirve en cliente y servidor)
  errorLoadRemote(args) {
    try {
      const { id, error, from } = args;
      if (process.env.NODE_ENV === 'development') {
        console.error(`[Federation] Fallo al cargar remoto "${id}":`, { from, message: error?.message, stack: error?.stack });
      } else {
        // En producción, log mínimo
        console.warn(`[Federation] Fallo al cargar remoto "${id}": ${error?.message || 'unknown'}`);
      }
      return args;
    } catch (err) {
      // Nunca lanzar aquí: solo devolver args para no romper flujo.
      console.error('[Federation] errorLoadRemote fallo interno:', err);
      return args;
    }
  },

  // beforeLoadShare: distinguir SSR/cliente para logs
  beforeLoadShare(args) {
    try {
      if (typeof window === 'undefined') return args;
      const { shareName, shareProvider } = args;
      if (!shareProvider) {
        console.warn(`[Federation] Proveedor no definido para dependencia compartida "${shareName}".`);
      } else if (process.env.NODE_ENV === 'development') {
        console.log(`[Federation] Cargando dependencia compartida: "${shareName}"`);
      }
      return args;
    } catch (err) {
      if (typeof window === 'undefined') {
        return args;
      } else {
        return args;
      }
    }
  },

  // beforeInit: ajustar shared en desarrollo (solo cliente)
  beforeInit(args) {
    try {
      // Modificaciones seguras: si estamos en SSR no hacemos cambios peligrosos
      if (typeof window === 'undefined') return args;

      const { userOptions } = args;
      if (process.env.NODE_ENV === 'development') {
        userOptions.shared = {
          ...userOptions.shared,
          react: { eager: true, singleton: true, ...userOptions.shared?.react },
          'react-dom': { eager: true, singleton: true, ...userOptions.shared?.['react-dom'] },
        };
      } else {
        // producción: intentamos prefetch en idle, pero solo en navegador
        userOptions.remotes = userOptions.remotes || [];
        if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
          window.requestIdleCallback(() => {
            userOptions.remotes
              .filter(remote => typeof remote === 'string' && remote.includes('dashboard'))
              .forEach(remote => import(remote).catch(() => {}));
          });
        }
      }
      return args;
    } catch (err) {
      console.error('[Federation] beforeInit fallo:', err);
      return args;
    }
  },

  // beforeResolve: logs seguros
  beforeResolve(args) {
    try {
      if (typeof window === 'undefined') return args;
      const { pkgName } = args;
      if (process.env.NODE_ENV === 'development' && pkgName) {
        console.log(`[Federation] Resolviendo dependencia compartida: "${pkgName}"`);
      }
      return args;
    } catch (err) {
      return args;
    }
  },

  // init: configurar timeouts y variable global — solo en cliente
  init(args) {
    try {
      if (typeof window !== 'undefined') {
        const isDockerCompose = process.env.IS_DOCKER_COMPOSE === 'true';
        // tiempo más generoso cuando usamos Docker Compose
        window.__FEDERATION_TIMEOUT__ = isDockerCompose ? 10000 : 5000;
      }
      if (process.env.NODE_ENV === 'development') {
        console.log('[Federation] Runtime plugin inicializado correctamente.');
      }
      return args;
    } catch (err) {
      // No romper inicialización
      console.error('[Federation] init fallo:', err);
      return args;
    }
  },

  // Opción: hook adicional para manejar precarga segura (no obligatorio)
  beforePreloadRemote(args) {
    try {
      if (typeof window === 'undefined') return args;
      // Puedes añadir lógica de control de concurrencia aquí si quieres.
      return args;
    } catch (err) {
      return args;
    }
  }
});

module.exports = FederationRuntimePlugin;
