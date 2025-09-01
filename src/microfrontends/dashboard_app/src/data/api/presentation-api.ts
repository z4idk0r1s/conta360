// src/data/api/presentation-api.ts
const BASE_URL = process.env.NEXT_PUBLIC_API_URL;

export const presentationApiClient = {
  getGeneralData: async () => {
    const response = await fetch(`${BASE_URL}/api/general`);
    if (!response.ok) throw new Error('API Error');
    return response.json();
  },
  // Más funciones para peticiones POST, PUT, DELETE, etc.
};