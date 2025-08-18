// src/microfrontends/root-config/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');
console.log('NextFederationPlugin:', NextFederationPlugin);
const { getRemotes } = require('./mf-remotes.config');
/** @type {import('next').NextConfig} */
const nextConfig = {
  typescript: {
    ignoreBuildErrors: false, // Asegurar que los errores de TS se detecten
  },
  reactStrictMode: true,
  //output: 'export', // Necesario para builds estáticas (Tauri)
  //distDir: 'out', // Directorio de salida para la build estática
  webpack(config, options) {
    console.log('[next.config] webpack isServer:', options.isServer);
    config.output.publicPath = 'auto';

    // Este es el cambio crucial. La configuración de remotos se decide aquí.
    const remotes = options.isServer
      ? {
          // Para la compilación del servidor, se usa una URL fija.
          dashboardApp: 'dashboardApp@http://localhost:3001/_next/static/chunks/remoteEntry.js',
        }
      : getRemotes(options);

    console.log('[next.config] Adding NextFederationPlugin with remotes:', remotes);

    // El plugin se aplica ahora en ambos entornos de compilación.
    config.plugins.push(
      new NextFederationPlugin({
        name: 'rootConfig',
        filename: 'static/chunks/remoteEntry.js', // Ruta del remoteEntry del host
        remotes: remotes, // Configuración de remotos del host
        exposes: {},
        shared: {
          react: { singleton: true, eager: true, requiredVersion: '18.2.0' },
          'react-dom': { singleton: true, eager: true, requiredVersion: '18.2.0' },
          next: { singleton: true, eager: true, requiredVersion: '14.1.4' },
          'next/router': { singleton: true, eager: true, requiredVersion: '14.1.4' },
          'next/link': { singleton: true, eager: true, requiredVersion: '14.1.4' },
          'next/head': { singleton: true, eager: true, requiredVersion: '14.1.4' },
          'next/image': { singleton: true, eager: true, requiredVersion: '14.1.4' },
          'next/dynamic': { singleton: true, eager: false, requiredVersion: '14.1.4' },
          axios: { singleton: true, eager: true, requiredVersion: '1.6.8' },
          'tailwind-merge': { singleton: true, requiredVersion: '2.6.0' },
          postcss: { singleton: true, requiredVersion: '8.4.35' },
          autoprefixer: { singleton: true, requiredVersion: '10.4.20' },
        },
      })
    );
    return config;
  },
};
module.exports = nextConfig;