// microfrontends/dashboard-app/src/app/page.tsx (CONTENIDO FINAL Y COMPLETO)
'use client'; // ¡Es crucial mantener esto ya que Tailadmin usa componentes interactivos y Hooks de React!

// Importa la página principal del dashboard de Tailadmin.
// Esta ruta apunta a la ubicación donde copiaste la página principal del dashboard de Tailadmin.
// Según la estructura que proporcionaste, está en 'src/app/(admin)/page.tsx'.
import TailAdminDashboardContent from './(admin)/page';

/**
 * Componente principal del microfrontend de Dashboard.
 * Este componente actúa como el punto de entrada para el microfrontend
 * y se encarga de renderizar la interfaz completa del dashboard de Tailadmin.
 */
export default function DashboardEntryPage() {
  // Cuando este microfrontend se cargue a través de Module Federation,
  // renderizará el componente de la página principal del dashboard de Tailadmin.
  // El sistema de Next.js App Router, junto con los layouts de Tailadmin
  // (como 'src/app/(admin)/layout.tsx'), se encargará de envolver este contenido
  // con el sidebar, el header y otros elementos de la estructura del dashboard.
  return (
    <TailAdminDashboardContent />
  );
}