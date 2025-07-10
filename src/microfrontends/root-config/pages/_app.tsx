// root-config/pages/_app.tsx
import type { AppProps } from 'next/app';
import Head from 'next/head'; // Para gestionar <head>
import { Inter } from 'next/font/google'; // Para la fuente Inter

// Importación de los estilos globales.
// La ruta es relativa desde 'root-config/pages/_app.tsx' hasta 'root-config/pages/globals.css'.
import '../pages/globals.css';

// Inicializa la fuente Inter
const inter = Inter({ subsets: ['latin'] });

// Este componente _app.tsx es el "RootLayout" de la aplicación host.
// Contiene la estructura HTML, Body y Metadatos globales.
// No debe contener Header, Footer, o lógica específica de página.
function MyApp({ Component, pageProps }: AppProps) {
  return (
    <html lang="es">
      <Head>
        {/* Metadatos globales de la aplicación */}
        <title>Conta360 Desktop App</title>
        <meta name="description" content="Aplicación Contable de Escritorio con Tauri y Micro-frontends" />
        {/* Aquí puedes añadir otros meta tags, enlaces a favicons, etc. */}
      </Head>

      {/* La clase de la fuente se aplica al <body> */}
      <body className={inter.className}>
        {/* Aquí se renderiza el componente de la página actual (ej. pages/index.tsx).
            Esta página (index.tsx) será la responsable de su propio layout (header, main, footer)
            si la dashboardApp los incluye y la root-config actúa solo como orquestador. */}
        <Component {...pageProps} />
      </body>
    </html>
  );
}

export default MyApp;