import dynamic from 'next/dynamic';
import { fetchBackendData } from '../src/lib/api';
import { useState } from 'react';

const DashboardApp = dynamic(() => import('dashboardApp/DashboardModule'), { ssr: false });

export default function Home() {
  const [backendData, setBackendData] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const handleFetchData = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchBackendData();
      setBackendData(JSON.stringify(data, null, 2));
    } catch (err) {
      setError('Error al conectar con el backend o al obtener datos. Asegúrate que tu backend C# esté corriendo.');
      console.error('Error fetching data:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 font-sans antialiased">
      <header className="bg-gradient-to-r from-blue-700 to-blue-500 text-white p-4 shadow-md">
        <nav className="container mx-auto flex justify-between items-center">
          <h1 className="text-2xl font-extrabold tracking-tight">Conta360</h1>
          <ul className="flex space-x-6">
            <li><a href="#" className="hover:text-blue-200 transition-colors">Inicio</a></li>
            <li><a href="#" className="hover:text-blue-200 transition-colors">Facturas</a></li>
            <li><a href="#" className="hover:text-blue-200 transition-colors">Informes</a></li>
            <li><a href="#" className="hover:text-blue-200 transition-colors">Configuración</a></li>
          </ul>
        </nav>
      </header>
      <main className="container mx-auto py-8">
        <div className="p-4 bg-gray-50">
          <h1 className="text-3xl font-bold mb-6 text-gray-800">Conta360 - Dashboard Principal - ROOT-CONFIG</h1>

          <div className="mb-8 p-6 bg-white rounded-lg shadow-md">
            <h2 className="text-2xl font-semibold mb-4 text-gray-700">Conexión con Backend C#</h2>
            <button
              onClick={handleFetchData}
              disabled={loading}
              className="px-6 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 transition-colors"
            >
              {loading ? 'Cargando...' : 'Obtener Datos del Backend'}
            </button>
            {error && <p className="mt-2 text-red-600">{error}</p>}
            {backendData && (
              <div className="mt-4 p-4 bg-gray-100 rounded-md overflow-x-auto text-sm text-gray-800">
                <pre>{backendData}</pre>
              </div>
            )}
            <p className="mt-4 text-gray-600 text-sm">
              Este botón simula una llamada a tu API C# (`/api/data`). Asegúrate de que tu backend C# esté corriendo.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div className="p-6 bg-white rounded-lg shadow-md">
              <h2 className="text-2xl font-semibold mb-4 text-gray-700">Dashboard</h2>
              <DashboardApp />
            </div>
          </div>

          <footer className="mt-12 text-center text-gray-500 text-sm">
            Conta360 - Desarrollado con Tauri, Next.js y Micro-frontends.
          </footer>
        </div>
      </main>
    </div>
  );
}
