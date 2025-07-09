'use client';

// Esta ruta apunta a la ubicación de la página principal del dashboard
// Según la estructura que proporcionaste, está en 'src/app/(admin)/page.tsx'.
import TailAdminDashboardContent from './(admin)/page';

/**
 * Componente principal del microfrontend de Dashboard.
 * Este componente actúa como el punto de entrada para el microfrontend
 * y se encarga de renderizar la interfaz completa del dashboard de Tailadmin.
 */
export default function DashboardEntryPage() {
  // El sistema de Next.js App Router, junto con los layouts de Tailadmin
  // (como 'src/app/(admin)/layout.tsx'), se encargará de envolver este contenido
  // con el sidebar, el header y otros elementos de la estructura del dashboard.
  return (
    <TailAdminDashboardContent />
  );
}