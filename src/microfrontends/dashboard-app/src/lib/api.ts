// src/microfrontends/dashboard-app/src/lib/api.ts
import axios from 'axios';

// La URL del backend para este MF, si la necesita.
// Es crucial que 'NEXT_PUBLIC_API_URL' se pase también a este contenedor.
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchDashboardData = async () => {
  try {
    // Este podría ser un endpoint específico del dashboard, por ejemplo:
    const response = await api.get('/api/dashboard-metrics');
    return response.data;
  } catch (error) {
    console.error('Error fetching dashboard data:', error);
    throw error;
  }
};