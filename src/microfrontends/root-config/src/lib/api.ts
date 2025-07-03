import axios from 'axios';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000'; // Ajusta este puerto si tu backend C# usa uno diferente

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const fetchBackendData = async () => {
  try {
    // Llama al endpoint de salud, ya que backend expone y confirma su funcionamiento.
    const response = await api.get('/health');
    return response.data;
  } catch (error) {
    console.error('Error fetching data from backend:', error);
    throw error;
  }
};