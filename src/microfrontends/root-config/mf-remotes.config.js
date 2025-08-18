// root-config/mf-remotes.config.js

const REMOTE_PORTS = {
  dashboardApp: 3001,
  // Para añadir un nuevo MF, añade una entrada aquí:
  // nuevoMfApp: 3005,
};

const getRemotes = (options) => {
  const isDevelopment = process.env.NODE_ENV === 'development';
  const isDockerComposeEnv = isDevelopment && process.env.IS_DOCKER_COMPOSE === 'true';

  const remotesConfig = {};

  for (const [mfName, mfPort] of Object.entries(REMOTE_PORTS)) {
    let remoteEntryPath;

    if (isDevelopment) {
      if (isDockerComposeEnv) {
        remoteEntryPath = `http://${mfName}:${mfPort}/_next/static/chunks/remoteEntry.js`;
        //remoteEntryPath = `http://localhost:${mfPort}/_next/static/chunks/remoteEntry.js`;
      } else {
        //remoteEntryPath = `http://localhost:${mfPort}/_next/static/chunks/remoteEntry.js`;
        remoteEntryPath = `http://${mfName}:${mfPort}/_next/static/chunks/remoteEntry.js`;
      }
    } else {
      remoteEntryPath = `/_next/static/chunks/${mfName}-remoteEntry.js`;
      // Para builds estáticas (Tauri), asegúrate de que el `remoteEntry.js` del MF se copie
      // a la ruta esperada dentro del directorio 'out' del host.
    }
    
    remotesConfig[mfName] = `${mfName}@${remoteEntryPath}`;
  }
  console.log('[mf-remotes.config] remotesConfig:', remotesConfig);

  return remotesConfig;
};

module.exports = {
  REMOTE_PORTS,
  getRemotes,
};