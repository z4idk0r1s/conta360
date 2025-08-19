// src/microfrontends/root-config/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');
const { getRemotes } = require('./mf-remotes.config');

/** @type {import('next').NextConfig} */
const nextConfig = {
  typescript: {
    ignoreBuildErrors: false,
  },
  reactStrictMode: true,
  webpack(config, options) {
    console.log('[next.config] webpack isServer:', options.isServer);
    config.output.publicPath = 'auto';

    // ✅ Solo aplicar Module Federation en el CLIENTE
    if (!options.isServer) {
      const remotes = getRemotes(options);
      console.log('[next.config] Adding NextFederationPlugin with remotes:', remotes);
      
      config.plugins.push(
        new NextFederationPlugin({
          name: 'rootConfig',
          filename: 'static/chunks/remoteEntry.js',
          remotes: remotes,
          exposes: {},
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

            // ✅ Next.js components - MISMA configuración en ambos
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

            // ✅ Otras dependencias - MISMA configuración en ambos
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
          },
        })
      );
    }
    
    return config;
  },
};

module.exports = nextConfig;