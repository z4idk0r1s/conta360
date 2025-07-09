const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,

  // Requerido para builds estáticas (por ejemplo, para Tauri o servidores estáticos)
  output: 'export',
  distDir: 'out',

  experimental: {
    appDir: true // Requerido por TailAdmin (usa la nueva estructura App Router)
  },

  webpack(config, options) {
    // Añadimos soporte para importar SVGs como componentes (necesario para TailAdmin)
    config.module.rules.push({
      test: /\.svg$/,
      use: ['@svgr/webpack']
    });

    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'dashboardApp', // ¡CRÍTICO! Coincide con REMOTE_PORTS de root-config
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './Dashboard': './src/app/page.tsx' // Puedes mantener esta ruta si usas 'src/app'
          },
          shared: {
            react: { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            'react-dom': { singleton: true, eager: true, requiredVersion: '^18.2.0' },
            next: { singleton: true, eager: true, requiredVersion: '^13.5.6' },
            axios: { singleton: true, eager: true, requiredVersion: '^1.6.8' }
          }
        })
      );
    }

    return config;
  }
};

module.exports = nextConfig;
