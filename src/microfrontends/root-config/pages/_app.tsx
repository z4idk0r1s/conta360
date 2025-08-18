// root-config/pages/_app.tsx
import type { AppProps } from 'next/app';
import Head from 'next/head';
import './globals.css';

/**
 * Componente _app.tsx de la aplicación host (root-config)
 *
 * Función principal:
 * - Inicializar páginas de Next.js
 * - Importar estilos globales
 * - Mantener el estado entre cambios de página
 */
function MyApp({ Component, pageProps }: AppProps) {
  return (
    <>
      <Head>
        {/* Metadatos generales de la aplicación */}
        <title>Conta360 Desktop App</title>
        <meta
          name="description"
          content="Aplicación Contable de Escritorio con Tauri y Micro-frontends"
        />
      </Head>

      {/* Renderiza la página actual */}
      <Component {...pageProps} />
    </>
  );
}

export default MyApp;
