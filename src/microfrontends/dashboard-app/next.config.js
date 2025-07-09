const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  experimental: {
    appDir: true,
  },

  webpack(config, options) {
    // MODIFICACIÓN CLAVE 1: Configuración robusta para @svgr/webpack
    // Esto asegura que los SVG se manejen correctamente como componentes React
    // y se alineen con tu `src/svg.d.ts` para evitar errores de tipado.
    config.module.rules.push({
      test: /\.svg$/i, // Añadido 'i' para case-insensitive
      issuer: { and: [/\.(ts|tsx|js|jsx|md|mdx)$/] }, // Restringe la aplicación del cargador a archivos JS/TSX
      use: [
        {
          loader: '@svgr/webpack',
          options: {
            svgo: false,      // Importante para no eliminar atributos que podrían ser necesarios
            titleProp: true,  // Permite pasar la prop 'title' para accesibilidad
            ref: true,        // Permite usar 'ref' en el componente SVG
          },
        },
      ],
    });

    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'dashboardApp',
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './DashboardModule': './src/app/(admin)/page.tsx', // dashboard principal
            './AdminLayout': './src/app/(admin)/layout.tsx',
            './FullWidthPagesLayout': './src/app/(full-width-pages)/layout.tsx',
            './SignIn': './src/app/(full-width-pages)/(auth)/signin/page.tsx',
            './SignUp': './src/app/(full-width-pages)/(auth)/signup/page.tsx',
            // Agrega cualquier otro módulo o página que tu 'root-config' necesite cargar directamente.
          },
          shared: {
            react: { singleton: true, eager: true, requiredVersion: '^19.0.0' },
            'react-dom': { singleton: true, eager: true, requiredVersion: '^19.0.0' },
            next: { singleton: true, eager: true, requiredVersion: '^15.2.3' },
            axios: { singleton: true, eager: true, requiredVersion: '^1.6.8' },
          },
        })
      );
    }

    return config;
  },
};

module.exports = nextConfig;