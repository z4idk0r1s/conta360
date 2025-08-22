// root_config/federation-runtime-plugin.js

/**
 * Plugin de runtime para manejar errores y optimizar la carga de módulos remotos.
 * Este plugin refuerza la resiliencia y facilita la depuración en una arquitectura de microfrontends.
 *
 * @returns {Readonly<object>} Objeto con los hooks del plugin de runtime.
 */
const FederationRuntimePlugin = () => Object.freeze({
  name: 'federation-runtime-plugin',

  /**
   * Hook ejecutado antes de iniciar una petición a un remoto.
   * Realiza validaciones críticas de la información del remoto.
   *
   * @param {object} args - Argumentos de ejecución del hook.
   * @param {string} args.origin - Origen de la petición.
   * @param {object} args.remoteInfo - Información del remoto que se está cargando.
   * @param {string} args.remoteInfo.name - Nombre del remoto.
   * @param {Request} args.request - Objeto de la petición.
   * @returns {object|Promise<Error>} Argumentos válidos o un error en caso de fallo.
   */
  beforeRequest(args) {
    const { origin, remoteInfo, request } = args;

    // Validación crítica
    if (!remoteInfo || !remoteInfo.name) {
      const error = new Error('RemoteInfo o su propiedad "name" no están definidos. Carga abortada.');
      console.error('[Federation] Error en beforeRequest:', error.message, {
        origin,
        request,
        remoteInfo,
      });
      return Promise.reject(error);
    }

    // Log solo en entorno de desarrollo
    if (process.env.NODE_ENV === 'development') {
      console.log(`[Federation] Petición preparada para remoto "${remoteInfo.name}" desde ${origin}`);
    }

    return args;
  },

  /**
   * Hook ejecutado cuando ocurre un error al cargar un remoto.
   *
   * @param {object} args - Argumentos del error de carga.
   * @param {string} args.id - Identificador del remoto.
   * @param {Error} args.error - Error ocurrido durante la carga.
   * @param {string} args.from - Contexto de la carga.
   * @returns {object} Argumentos originales para su propagación.
   */
  errorLoadRemote(args) {
    const { id, error, from } = args;

    if (process.env.NODE_ENV === 'development') {
      console.error(`[Federation] Fallo al cargar remoto "${id}":`, {
        from,
        message: error.message,
        stack: error.stack,
      });
    }

    return args;
  },

  /**
   * Hook ejecutado antes de cargar una dependencia compartida.
   *
   * @param {object} args - Argumentos de carga de la dependencia.
   * @param {string} args.shareName - Nombre de la dependencia.
   * @param {Function} args.shareProvider - Proveedor de la dependencia.
   * @returns {object} Argumentos originales.
   */
  beforeLoadShare(args) {
    const { shareName, shareProvider } = args;

    if (!shareProvider) {
      console.warn(`[Federation] Proveedor no definido para dependencia compartida "${shareName}".`);
    } else if (process.env.NODE_ENV === 'development') {
      console.log(`[Federation] Cargando dependencia compartida: "${shareName}"`);
    }

    return args;
  },

  /**
   * Hook ejecutado antes de inicializar el runtime de Module Federation.
   * Permite optimizar estrategias de carga en función del entorno.
   *
   * @param {object} args - Argumentos de inicialización del runtime.
   * @param {object} args.userOptions - Opciones configuradas por el usuario.
   * @returns {object} Argumentos originales.
   */
  beforeInit(args) {
    const { userOptions } = args;

    if (process.env.NODE_ENV === 'development') {
      // En desarrollo: priorizar la recarga rápida
      userOptions.shared = {
        ...userOptions.shared,
        react: { eager: true, singleton: true, ...userOptions.shared?.react },
        'react-dom': { eager: true, singleton: true, ...userOptions.shared?.['react-dom'] },
      };
    } else {
      // En producción: optimizar carga y precarga de remotos críticos
      userOptions.remotes = userOptions.remotes || [];

      if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
        window.requestIdleCallback(() => {
          userOptions.remotes
            .filter(remote => remote.includes('dashboard_app'))
            .forEach(remote =>
              import(remote).catch(() => {
                // Silenciar errores de precarga
              })
            );
        });
      }
    }

    return args;
  },

  /**
   * Hook ejecutado antes de resolver un módulo compartido.
   *
   * @param {object} args - Argumentos de resolución del módulo.
   * @param {object} args.shareScopeMap - Mapa de scopes compartidos.
   * @param {string} args.scope - Scope de la dependencia.
   * @param {string} args.pkgName - Nombre del paquete.
   * @returns {object} Argumentos originales.
   */
  beforeResolve(args) {
    const { pkgName } = args;

    if (process.env.NODE_ENV === 'development' && pkgName) {
      console.log(`[Federation] Resolviendo dependencia compartida: "${pkgName}"`);
    }

    return args;
  },

  /**
   * Hook ejecutado después de la inicialización del runtime.
   * Configura timeouts globales para las peticiones de los remotos.
   *
   * @param {object} args - Argumentos de inicialización.
   * @returns {object} Argumentos originales.
   */
  init(args) {
    if (typeof window !== 'undefined') {
      const isDockerCompose = process.env.IS_DOCKER_COMPOSE === 'true';
      window.__FEDERATION_TIMEOUT__ = isDockerCompose ? 10000 : 5000;
    }

    console.log('[Federation] Runtime plugin inicializado correctamente.');

    return args;
  },
});

module.exports = FederationRuntimePlugin;