import axios from 'axios';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';
const SUBVENCIONES_API_BASE_URL = process.env.NEXT_PUBLIC_SUBVENCIONES_API_URL || 'http://localhost:5001';

const mainApi = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

const subvencionesApi = axios.create({
  baseURL: SUBVENCIONES_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchBackendData = async () => {
  try {
    const response = await mainApi.get('/health');
    return response.data;
  } catch (error) {
    console.error('Error fetching data from backend:', error);
    throw error;
  }
};

export const fetchSubvencionesData = async () => {
  try {
    const response = await subvencionesApi.get('/health');
    return response.data;
  } catch (error) {
    console.error('Error fetching data from subvenciones backend:', error);
    throw error;
  }
};