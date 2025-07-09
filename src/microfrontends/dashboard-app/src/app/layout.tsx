import { Inter } from 'next/font/google';
import './globals.css'; // Debe ser el globals.css de Tailadmin que copiaste

// Importa los proveedores de contexto de Tailadmin.
// Asegúrate de que las rutas sean correctas, asumiendo que copiaste 'src/context' a tu proyecto.
import { ThemeProvider } from '@/context/ThemeContext';
import { SidebarProvider } from '@/context/SidebarContext';

const inter = Inter({ subsets: ['latin'] });

// Metadatos actualizados para reflejar la integración de Tailadmin
export const metadata = {
  title: 'Dashboard App - Tailadmin Integrado', // Título actualizado
  description: 'Microfrontend de Dashboard integrado con Tailadmin para Conta360',
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="es">
      <head>
        {/* Aquí puedes añadir otros elementos <head> si Tailadmin los requiere y no están ya en globals.css.
            Por ejemplo, si usa un CDN para Font Awesome o fuentes específicas:
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" />
        */}
      </head>
      <body className={`loading ${inter.className}`}>
        {/* Los 'children' (el contenido de tus páginas de Tailadmin)
            se envolverán en los contextos globales de Tailadmin */}
        <ThemeProvider>
          <SidebarProvider>
            {children}
          </SidebarProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}