// src/microfrontends/dashboard-app/next.config.js
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
          name: 'dashboardApp',
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './E-commerce': './components/Dashboard/E-commerce.tsx',
          },
          shared: {
            // ✅ CONFIGURACIÓN IDÉNTICA a la del HOST
            react: { 
              singleton: true, 
              eager: false,
              requiredVersion: '18.2.0',
              strictVersion: false 
            },
            'react-dom': { 
              singleton: true, 
              eager: false,
              requiredVersion: '18.2.0',
              strictVersion: false 
            },

            // ✅ Next.js components - MISMA configuración que HOST
            'next/router': { 
              singleton: true, 
              eager: false, 
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