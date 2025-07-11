const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,

  webpack(config, options) {
    // Configuración robusta para @svgr/webpack (mantenemos la que ya tenías)
    config.module.rules.push({
      test: /\.svg$/i,
      issuer: { and: [/\.(ts|tsx|js|jsx|md|mdx)$/] },
      use: [
        {
          loader: '@svgr/webpack',
          options: {
            svgo: false,
            titleProp: true,
            ref: true,
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
            // **¡IMPORTANTE!** Estas rutas deben ser actualizadas
            './DashboardComp': './components/Dashboard/E-commerce.tsx', 
            /*'./pages/auth/signin/index.tsx': './pages/auth/signin/index.tsx', 
            './pages/auth/signup/index.tsx': './pages/auth/signup/index.tsx', 
            './pages/calendar/index.tsx': './pages/calendar/index.tsx', 
            './pages/chart/index.tsx': './pages/chart/index.tsx', 
            './pages/forms/form-elements/index.tsx': './pages/forms/form-elements/index.tsx', 
            './pages/forms/form-layout/index.tsx': './pages/forms/form-layout/index.tsx', 
            './pages/profile/index.tsx': './pages/profile/index.tsx', 
            './pages/settings/index.tsx': './pages/settings/index.tsx', 
            './pages/tables/index.tsx': './pages/tables/index.tsx', 
            './pages/ui/alerts/index.tsx': './pages/ui/alerts/index.tsx', 
            './pages/ui/buttons/index.tsx': './pages/ui/buttons/index.tsx', */
            // También puedes exponer componentes reutilizables si es necesario:
            // './layouts/DefaultLayout': './components/layouts/DefaultLayout.tsx', // Si existiera un layout principal compartible
          },
          shared: {
            react: { singleton: true, eager: true, requiredVersion: false, import: false, },
            'react-dom': { singleton: true, eager: true, requiredVersion: false, import: false, },
            next: { singleton: true, eager: true, requiredVersion: false },
            axios: { singleton: true, eager: true, requiredVersion: false },
          },
        })
      );
    }

    return config;
  },
};

module.exports = nextConfig;