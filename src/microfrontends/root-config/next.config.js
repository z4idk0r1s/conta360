// src/microfrontends/root-config/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');
// Importa la configuración centralizada de remotes.
const { getRemotes } = require('./mf-remotes.config');

/** @type {import('next').NextConfig} */
const nextConfig = {
  // Habilita el modo estricto de React para mejorar la detección de problemas.
  reactStrictMode: true,
  // Configura Next.js para generar una build estática exportable (útil para Tauri o servidores estáticos).
  output: 'export',
  // Define el directorio de salida para la build estática.
  distDir: 'out',

  /**
   * Configuración de Webpack para inyectar el plugin de Module Federation.
   * @param {import('webpack').Configuration} config - La configuración actual de Webpack.
   * @param {object} options - Opciones proporcionadas por Next.js para la construcción.
   * @returns {import('webpack').Configuration} La configuración de Webpack modificada.
   */
  webpack(config, options) {
    // Aplica el plugin solo en el cliente (no en el servidor de Next.js si existiera).
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'hostApp', // Nombre único del host (root-config) en la federación.
          // Ruta donde el host expone su propio 'remoteEntry.js' (si otros MFs lo consumieran).
          filename: 'static/chunks/remoteEntry.js',
          // Define los microfrontends remotos que este host consumirá.
          // Las URLs se generan dinámicamente a través de mf-remotes.config.js.
          remotes: getRemotes(options),
          // Módulos compartidos para evitar duplicación de dependencias y asegurar singletons.
          shared: {
            react: { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            'react-dom': { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            next: { singleton: true, eager: true, requiredVersion: '^13.5.6' },
            axios: { singleton: true, eager: true, requiredVersion: '^1.6.8' }
          },
        })
      );
    }
    return config;
  },
};

module.exports = nextConfig;