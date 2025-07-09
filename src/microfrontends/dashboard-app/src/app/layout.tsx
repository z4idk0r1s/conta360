import { Inter } from 'next/font/google';
import './globals.css'; 

// Importa los proveedores de contexto de .
import { ThemeProvider } from '@/context/ThemeContext';
import { SidebarProvider } from '@/context/SidebarContext';

const inter = Inter({ subsets: ['latin'] });

// Metadatos actualizados para reflejar la integración de 
export const metadata = {
  title: 'Dashboard App', 
  description: 'Microfrontend de Dashboard Conta360',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <head>
        {/* Aquí puedes añadir otros elementos <head> si  los requiere y no están ya en globals.css.
            Por ejemplo, si usa un CDN para Font Awesome o fuentes específicas:
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" />
        */}
      </head>
      <body className={`loading ${inter.className}`}>
        {/* Los 'children' (el contenido de tus páginas de )
            se envolverán en los contextos globales de  */}
        <ThemeProvider>
          <SidebarProvider>
            {children}
          </SidebarProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}