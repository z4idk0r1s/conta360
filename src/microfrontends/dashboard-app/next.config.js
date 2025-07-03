const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  output: 'export', // Necesario para builds estáticas (ej. para Tauri o servidores estáticos)
  distDir: 'out', // Directorio de salida para la build estática

  webpack(config, options) {
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'dashboardApp', // ¡CRÍTICO! Este nombre DEBE coincidir con la clave en REMOTE_PORTS de root-config (mf-remotes.config.js)
          filename: 'static/chunks/remoteEntry.js', // Ruta donde este MF expone su "puerta de entrada"
          exposes: {
            // Define qué componentes o páginas de este MF se harán disponibles para otros.
            // La ruta es relativa al directorio raíz del proyecto de dashboardApp.
            // 'Dashboard' es el nombre que usará root-config para importar (ej. import('dashboardApp/Dashboard'))
            './Dashboard': './src/app/page.tsx', // Expone el componente Page como 'Dashboard'
          },
          shared: {
            // Módulos compartidos. Es CRÍTICO que estos coincidan con los del host (root-config)
            // para evitar duplicación y asegurar que se use una única instancia (singleton).
            react: { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            'react-dom': { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            next: { singleton: true, eager: true, requiredVersion: '^13.5.6' },
            axios: { singleton: true, eager: true, requiredVersion: '^1.6.8' },
          },
        })
      );
    }
    return config;
  },
};

module.exports = nextConfig;