// root-config/pages/_document.tsx
import { Html, Head, Main, NextScript } from 'next/document';
import { Inter } from 'next/font/google';

// Inicializa la fuente Inter para su uso aquí
const inter = Inter({ subsets: ['latin'] });

export default function Document() {
  return (
    <Html lang="es">
      <Head>
        {/* Aquí puedes añadir enlaces a iconos de favicons, etc. */}
      </Head>
      <body className={inter.className}>
        <Main />
        <NextScript />
      </body>
    </Html>
  );
}