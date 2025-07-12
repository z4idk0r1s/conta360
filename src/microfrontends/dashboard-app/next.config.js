const { NextFederationPlugin } = require('@module-federation/nextjs-mf');

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,

  webpack(config, options) {
    // Configuración para @svgr/webpack (se mantiene como la tenías, asumiendo su correcto funcionamiento)
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
          filename: 'static/chunks/remoteEntry.js', // Ruta del remoteEntry del microfrontend
          exposes: {
            // Componente principal del dashboard (ejemplo de exposición)
            './DashboardComp': './components/Dashboard/E-commerce.tsx',
            // ¡ATENCIÓN!: Habilita estas exposiciones a medida que las necesites y las consumas dinámicamente.
            // Es buena práctica asignarles nombres más específicos para evitar colisiones globales
            // y para que la importación en el host sea más clara.
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
            // Dependencias React
            react: {
              singleton: true,
              eager: true,
              requiredVersion: '18.2.0', // **Versión exacta y fija** de tu package.json
              import: true, // ¡CRÍTICO! Permite que el host provea React
            },
            'react-dom': {
              singleton: true,
              eager: true,
              requiredVersion: '18.2.0', // **Versión exacta y fija**
              import: true, // ¡CRÍTICO! Permite que el host provea React-DOM
            },
            // Dependencia Next.js principal
            next: {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3', // **Versión exacta y fija**
            },
            // Submódulos críticos de Next.js (deben coincidir con los del host)
            'next/router': {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3',
            },
            'next/link': {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3',
            },
            'next/head': {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3',
            },
            'next/image': {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3',
            },
            // Otras dependencias compartidas
            axios: {
              singleton: true,
              eager: true,
              requiredVersion: '1.6.8', // **Versión exacta y fija**
            },
            'tailwind-merge': {
              singleton: true,
              requiredVersion: '2.6.0', // **Versión exacta y fija**
            },
            // Agrega aquí otras librerías grandes que uses y quieras compartir
            // para evitar duplicados en el bundle del microfrontend.
            // Por ejemplo, las de FullCalendar si se utilizan en el host o en otros MF.
            '@fullcalendar/core': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            '@fullcalendar/react': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            '@fullcalendar/daygrid': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            '@fullcalendar/interaction': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            '@fullcalendar/list': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            '@fullcalendar/timegrid': {
              singleton: true,
              requiredVersion: '6.1.15',
            },
            // Si `@tailwindcss/forms` se va a compartir
            '@tailwindcss/forms': {
              singleton: true,
              requiredVersion: '0.5.9',
            },
            // Si `postcss` o `autoprefixer` se usaran en runtime compartido, también se podrían añadir
            // "postcss": { singleton: true, requiredVersion: "8.4.35" },
            // "autoprefixer": { singleton: true, requiredVersion: "10.4.20" },
          },
        })
      );
    }

    return config;
  },
};

module.exports = nextConfig;