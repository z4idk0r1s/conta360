// src/microfrontends/dashboard-app/src/app/page.tsx
'use client'; // Necesario si usas Hooks de React en un componente de Next.js App Router

import { useState, useEffect } from 'react';
// Si tu dashboard necesita su propia conexión a la API, puedes crearla aquí.
// Ejemplo: import { fetchDashboardData } from '../lib/api';

export default function Dashboard() {
  const [data, setData] = useState<string>('Datos iniciales del Dashboard');
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  // Puedes añadir lógica para cargar datos específicos del dashboard aquí
  useEffect(() => {
    // Ejemplo de carga de datos al montar el componente
    // const loadData = async () => {
    //   setLoading(true);
    //   try {
    //     const result = await fetchDashboardData(); // Si tienes una API específica para el dashboard
    //     setData(JSON.stringify(result, null, 2));
    //   } catch (err) {
    //     setError('Error al cargar datos del dashboard.');
    //     console.error(err);
    //   } finally {
    //     setLoading(false);
    //   }
    // };
    // loadData();
  }, []);

  return (
    <div className="p-4 border border-blue-300 rounded-lg bg-blue-50">
      <h3 className="text-xl font-semibold mb-2 text-blue-800">Contenido del Dashboard</h3>
      <p className="text-blue-700">¡Este es el microfrontend de Dashboard cargado dinámicamente!</p>
      <div className="mt-4 p-3 bg-blue-100 rounded-md text-sm">
        {loading ? (
          <p>Cargando datos del Dashboard...</p>
        ) : error ? (
          <p className="text-red-600">Error: {error}</p>
        ) : (
          <pre className="whitespace-pre-wrap">{data}</pre>
        )}
      </div>
      <button
        onClick={() => setData('¡Datos del Dashboard actualizados! ' + new Date().toLocaleTimeString())}
        className="mt-4 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors"
      >
        Actualizar Dashboard Interno
      </button>
    </div>
  );
}