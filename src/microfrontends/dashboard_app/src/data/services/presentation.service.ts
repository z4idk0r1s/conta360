// src/data/services/presentation.service.ts
import { presentationApiClient } from '../api/presentation-api';

export const presentationService = {
  getDashboardSummary: async () => {
    try {
      const data = await presentationApiClient.getGeneralData();
      return {
        totalVentas: data.sales,
        usuariosActivos: data.activeUsers,
        // ...
      };
    } catch (error) {
      console.error('Error in Presentation Service:', error);
      throw error;
    }
  },
};