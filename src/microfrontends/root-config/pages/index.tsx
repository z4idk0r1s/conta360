//src/microfrontends/root-config/pages/index.tsx
import dynamic from 'next/dynamic';
import { fetchBackendData } from '../src/lib/api';
import { useState } from 'react';

console.log('[root-config] Importing dashboardApp/Dashboard...');

const DashboardApp = dynamic(
  () => import('dashboardApp/Dashboard').catch(err => {
    console.error('Error loading Dashboard:', err);
    return () => <div>Error al cargar el Dashboard</div>;
  }),
  { 
    ssr: false,
    loading: () => <p>Cargando el Dashboard...</p>,
  }
);

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
    <main style={{ padding: 20 }}>
      <h1 style={{ marginBottom: 20, fontWeight: 'bold', fontSize: '1.5rem' }}>Root Config - Contenido Principal</h1>
      <button
        onClick={handleFetchData}
        disabled={loading}
        style={{ padding: '10px 20px', backgroundColor: '#2563eb', color: 'white', borderRadius: 5, cursor: loading ? 'not-allowed' : 'pointer' }}
      >
        {loading ? 'Cargando...' : 'Obtener Datos del Backend'}
      </button>

      {error && <p style={{ color: 'red', marginTop: 10 }}>{error}</p>}

      {backendData && (
        <pre style={{ marginTop: 10, backgroundColor: '#f90707ff', padding: 10, borderRadius: 5, overflowX: 'auto' }}>
          {backendData}
        </pre>
      )}

      <section style={{ marginTop: 30 }}>
        <DashboardApp />
      </section>
    </main>
  );
}
