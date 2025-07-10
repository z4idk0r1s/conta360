const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  experimental: {
    appDir: false,
  },

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
            // Tendremos que revisar y ajustar esto en el siguiente paso.
            './DashboardModule': './pages/index.tsx', 
            './SignIn': './pages/auth/signin/index.tsx', 
            './SignUp': './pages/auth/signup/index.tsx', 
            './Calendar': './pages/calendar/index.tsx', 
            './Chart': './pages/chart/index.tsx', 
            './FormsElements': './pages/forms/form-elements/index.tsx', 
            './FormsLayout': './pages/forms/form-layout/index.tsx', 
            './Profile': './pages/profile/index.tsx', 
            './Settings': './pages/settings/index.tsx', 
            './Tables': './pages/tables/index.tsx', 
            './Alerts': './pages/ui/alerts/index.tsx', 
            './Buttons': './pages/ui/buttons/index.tsx', 
            // También puedes exponer componentes reutilizables si es necesario:
            // './layouts/DefaultLayout': './components/layouts/DefaultLayout.tsx', // Si existiera un layout principal compartible
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