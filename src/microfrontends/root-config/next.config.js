const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  output: 'export', // Para que Next.js compile a estáticos para Tauri
  distDir: 'out', // Directorio de salida de la build

  webpack(config, options) {
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'hostApp',
          filename: 'static/chunks/remoteEntry.js', // Donde el host expone sus propios módulos (si los hubiera)
          remotes: {
            // En producción (Tauri), los remotes se sirven desde el mismo directorio '_next/static/chunks'
            // En desarrollo, se sirven desde sus propios servidores locales (puertos diferentes)
            dashboardApp: `dashboardApp@${options.isWebpack5 ? 'http://localhost:3001/_next/static/chunks/remoteEntry.js' : '/_next/static/chunks/dashboard-app-remoteEntry.js'}`,
            analisisApp: `analisisApp@${options.isWebpack5 ? 'http://localhost:3002/_next/static/chunks/remoteEntry.js' : '/_next/static/chunks/analisis-app-remoteEntry.js'}`,
            pgcApp: `pgcApp@${options.isWebpack5 ? 'http://localhost:3003/_next/static/chunks/remoteEntry.js' : '/_next/static/chunks/pgc-app-remoteEntry.js'}`,
            sharedComponents: `sharedComponents@${options.isWebpack5 ? 'http://localhost:3004/_next/static/chunks/remoteEntry.js' : '/_next/static/chunks/shared-components-remoteEntry.js'}`,
          },
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
