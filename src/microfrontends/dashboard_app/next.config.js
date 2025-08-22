// src/microfrontends/dashboard_app/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  webpack(config, options) {
    config.output.publicPath = 'auto';
    
    // SVGR config
    config.module.rules.push({
      test: /\.svg$/i,
      issuer: { and: [/\.(ts|tsx|js|jsx|md|mdx)$/] },
      use: [
        {
          loader: '@svgr/webpack',
          options: { svgo: false, titleProp: true, ref: true },
        },
      ],
    });

    // ✅ Solo en cliente - el remoto expone módulos
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'dashboard_app',
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './dashboard_v1': './pages/FederatedDashboardV1.tsx', 
            './AuthSignInPage': './pages/auth/signin/index.tsx',
            './AuthSignUpPage': './pages/auth/signup/index.tsx',
            './CalendarPage': './pages/calendar/index.tsx',
            './ChartPage': './pages/chart/index.tsx',
            './FormElementsPage': './pages/forms/form-elements/index.tsx',
            './FormLayoutPage': './pages/forms/form-layout/index.tsx',
            './ProfilePage': './pages/profile/index.tsx',
            './SettingsPage': './pages/settings/index.tsx',
            './TablesPage': './pages/tables/index.tsx',
            './AlertsPage': './pages/ui/alerts/index.tsx',
            './ButtonsPage': './pages/ui/buttons/index.tsx',
            // Puedes añadir más componentes reutilizables o layouts específicos si es necesario exponerlos:
            // './CommonLoader': './components/common/Loader/index.tsx',
          },
          shared: {
            react: { 
              singleton: true, 
              eager: true,
              requiredVersion: '18.2.0',
              strictVersion: false 
            },
            'react-dom': { 
              singleton: true, 
              eager: true,
              requiredVersion: '18.2.0',
              strictVersion: false 
            },

            // ✅ Next.js components - MISMA configuración que HOST
            'next/router': { 
              singleton: true, 
              eager: true, 
              requiredVersion: '14.1.4',
              strictVersion: false 
            },
            'next/link': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '14.1.4',
              strictVersion: false 
            },
            'next/head': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '14.1.4',
              strictVersion: false 
            },
            'next/image': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '14.1.4',
              strictVersion: false 
            },
            'next/dynamic': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '14.1.4',
              strictVersion: false 
            },

            // ✅ Otras dependencias - MISMA configuración que HOST
            axios: { 
              singleton: true, 
              eager: false, 
              requiredVersion: '1.6.8',
              strictVersion: false 
            },
            'tailwind-merge': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '2.6.0',
              strictVersion: false 
            },

            // ✅ Solo las dependencias específicas del remoto (no en HOST)
            '@fullcalendar/core': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '6.1.15',
              strictVersion: false 
            },
            '@fullcalendar/react': { 
              singleton: true, 
              eager: false, 
              requiredVersion: '6.1.15',
              strictVersion: false 
            },
          },
        })
      );
    }
    
    return config;
  },
};

module.exports = nextConfig;