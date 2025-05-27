import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000'; // Ajusta este puerto si tu backend C# usa uno diferente

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchBackendData = async () => {
  try {
    const response = await api.get('/api/data');
    return response.data;
  } catch (error) {
    console.error('Error fetching data from backend:', error);
    throw error;
  }
};