// src/microfrontends/root-config/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');
const { getRemotes } = require('./mf-remotes.config'); // Asegúrate de que esta ruta sea correcta

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  output: 'export', // Necesario para builds estáticas (Tauri)
  distDir: 'out', // Directorio de salida para la build estática

  webpack(config, options) {
    if (!options.isServer) {
      config.plugins.push(
        new NextFederationPlugin({
          name: 'hostApp',
          filename: 'static/chunks/remoteEntry.js', // Ruta del remoteEntry del host
          remotes: getRemotes(options), // Configuración de remotos del host
          shared: {
            // Dependencias React
            react: {
              singleton: true, // Asegura una única instancia
              eager: true,     // Carga la dependencia inmediatamente
              requiredVersion: '18.2.0', // **Versión exacta y fija** de tu package.json              
            },
            'react-dom': {
              singleton: true,
              eager: true,
              requiredVersion: '18.2.0', // **Versión exacta y fija**              
            },
            // Dependencia Next.js principal
            next: {
              singleton: true,
              eager: true,
              requiredVersion: '15.2.3', // **Versión exacta y fija**              
            },
            // Submódulos críticos de Next.js
            'next/router': {
              singleton: true,
              eager: true, //  ya que el router es fundamental
              requiredVersion: '15.2.3',              
            },
            'next/link': {
              singleton: true,
              eager: true, //  para funcionalidad de navegación
              requiredVersion: '15.2.3',              
            },
            'next/head': {
              singleton: true,
              eager: true, //  para manejo de metadatos
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
              eager: true, //  si se usa al inicio de la aplicación
              requiredVersion: '1.6.8', // **Versión exacta y fija**
            },
            // Si `tailwind-merge` se utiliza directamente en el root-config y también se quiere compartir:
            'tailwind-merge': {
              singleton: true,
              requiredVersion: '2.6.0',              
            },
            // Agrega aquí cualquier otra dependencia importante que deba ser compartida y singleton
            // Por ejemplo, si @fullcalendar/react o @tailwindcss/forms se usaran en ambos:
            // '@fullcalendar/react': {
            //   singleton: true,
            //   requiredVersion: '6.1.15',
            //    // Si el host la provee
            // },
            // '@tailwindcss/forms': {
            //   singleton: true,
            //   requiredVersion: '0.5.9',
            //   
            // },
          },
        })
      );
    }
    return config;
  },
};

module.exports = nextConfig;