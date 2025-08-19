// src/microfrontends/root-config/next.config.js
const { NextFederationPlugin } = require('@module-federation/nextjs-mf');
const { getRemotes } = require('./mf-remotes.config');

/** @type {import('next').NextConfig} */
const nextConfig = {
  typescript: {
    ignoreBuildErrors: false,
  },
  reactStrictMode: true,
  // Optimización para Docker Compose
  output: process.env.NODE_ENV === 'production' ? 'standalone' : undefined,
  
  // Configuración experimental para mejorar el rendimiento
  experimental: {
    // Reduce el tiempo de compilación en desarrollo
    turbotrace: {
      logLevel: 'error'
    },
    // Optimiza la carga de chunks
    optimizePackageImports: ['react', 'react-dom'],
  },

  // Configuración de headers para mejor caching en producción
  async headers() {
    if (process.env.NODE_ENV === 'production') {
      return [
        {
          source: '/_next/static/chunks/remoteEntry.js',
          headers: [
            {
              key: 'Cache-Control',
              value: 'public, max-age=31536000, immutable',
            },
          ],
        },
      ];
    }
    return [];
  },

  webpack(config, options) {
    const { isServer, dev } = options;
    
    console.log(`[next.config] webpack - isServer: ${isServer}, isDev: ${dev}, NODE_ENV: ${process.env.NODE_ENV}`);
    
    // Configuración del publicPath
    config.output.publicPath = 'auto';
    
    // Optimizaciones específicas para cada contexto
    if (isServer) {
      // Configuración del servidor
      config.externals = config.externals || [];
      // Evita bundlear las dependencias de módulos remotos en el servidor
      config.externals.push(/@module-federation/);
    } else {
      // Configuración del cliente
      config.optimization = config.optimization || {};
      config.optimization.splitChunks = {
        ...config.optimization.splitChunks,
        cacheGroups: {
          ...config.optimization.splitChunks?.cacheGroups,
          // Optimiza la carga de dependencias compartidas
          vendor: {
            test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
            name: 'vendor',
            chunks: 'all',
            priority: 10,
          },
        },
      };
    }

    // Obtener la configuración de remotos con cache
    const remotes = getRemotes(options);
    
    // En Docker Compose, tanto servidor como cliente usan la misma configuración
    // porque ambos se ejecutan dentro de la red Docker
    const remotesForPlugin = remotes;
    
    // Log de la configuración de remotos para debugging
    if (!isServer && dev) {
      console.log('[next.config] Remotos configurados:', remotesForPlugin);
    }

    config.plugins.push(
      new NextFederationPlugin({
        name: 'rootConfig',
        filename: 'static/chunks/remoteEntry.js',
        remotes: remotesForPlugin,
        exposes: {},
        shared: getSharedDependencies(),
        // Configuración adicional para optimizar el rendimiento
        runtimePlugins: isServer ? [] : [
          require.resolve('./federation-runtime-plugin.js')
        ],
        // Configuración específica para el servidor
        ...(isServer && {
          // En el servidor, configurar para evitar intentos de conexión real
          library: { type: 'commonjs-module' },
        }),
      })
    );
   
    return config;
  },
};

// Función para centralizar las dependencias compartidas
function getSharedDependencies() {
  const isDevelopment = process.env.NODE_ENV === 'development';
  
  return {
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
    // Next.js dependencies - lazy load para mejor performance
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
    // Dependencias externas
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
    // Agregar más dependencias compartidas según sea necesario
    ...(isDevelopment && {
      // Dependencias solo para desarrollo
      'react-refresh': {
        singleton: true,
        eager: false
      }
    })
  };
}

module.exports = nextConfig;