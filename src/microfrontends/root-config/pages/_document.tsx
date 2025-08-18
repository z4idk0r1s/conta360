// root-config/pages/_document.tsx
import { Html, Head, Main, NextScript } from 'next/document';

export default function Document() {
  return (
    <Html lang="es">
      <Head>
        {/* Aquí puedes añadir enlaces a iconos de favicons, etc. */}
      </Head>
      <body>
        <Main />
        <NextScript />
      </body>
    </Html>
  );
}