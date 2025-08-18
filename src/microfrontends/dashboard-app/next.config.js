// src/microfrontends/dashboard-app/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,

  webpack(config, options) {
    config.output.publicPath = 'auto';

    // ✅ SVGR config
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

    // ✅ Solo en cliente: el remoto expone módulos, no consume del host
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'dashboardApp',
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './E-commerce': './components/Dashboard/E-commerce.tsx',
            /*
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
            */
          },
          shared: {
            react: { singleton: true, eager: false, requiredVersion: '18.2.0' },
            'react-dom': { singleton: true, eager: false, requiredVersion: '18.2.0' },

            // ⚠️ No compartir "next" entero
            'next/router': { singleton: true, eager: true, requiredVersion: '14.1.4' },
            'next/link': { singleton: true, eager: true, requiredVersion: '14.1.4' },
            'next/head': { singleton: true, eager: true, requiredVersion: '14.1.4' },
            'next/image': { singleton: true, eager: true, requiredVersion: '14.1.4' },
            'next/dynamic': { singleton: true, eager: false, requiredVersion: '14.1.4' },

            axios: { singleton: true, eager: true, requiredVersion: '1.6.8' },
            'tailwind-merge': { singleton: true, requiredVersion: '2.6.0' },
            '@fullcalendar/core': { singleton: true, requiredVersion: '6.1.15' },
            '@fullcalendar/react': { singleton: true, requiredVersion: '6.1.15' },
            '@fullcalendar/daygrid': { singleton: true, requiredVersion: '6.1.15' },
            '@fullcalendar/interaction': { singleton: true, requiredVersion: '6.1.15' },
            '@fullcalendar/list': { singleton: true, requiredVersion: '6.1.15' },
            '@fullcalendar/timegrid': { singleton: true, requiredVersion: '6.1.15' },
            '@tailwindcss/forms': { singleton: true, requiredVersion: '0.5.9' },
            postcss: { singleton: true, requiredVersion: '8.4.35' },
            autoprefixer: { singleton: true, requiredVersion: '10.4.20' },
          },
        })
      );
    }

    return config;
  },
};

module.exports = nextConfig;