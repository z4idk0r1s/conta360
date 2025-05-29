import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import './globals.css'; // Importa los estilos globales (Tailwind CSS)

const inter = Inter({ subsets: ['latin'] });

export const metadata: Metadata = {
  title: 'Conta360 Desktop App',
  description: 'Aplicación Contable de Escritorio con Tauri y Micro-frontends',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <body className={inter.className}>
        {children}
      </body>
    </html>
  );
}