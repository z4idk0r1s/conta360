import React, { useState, useEffect } from "react";
import type { ReactNode } from "react";
import ECommerce from "@/components/Dashboard/E-commerce";
import Loader from "@/components/common/Loader";

// Contrato explícito con props estables
export interface FederatedDashboardV1Props {
  onError?: (err: Error) => void;
  theme?: "light" | "dark";
}

// Punto de entrada federado estable para el host
export default function FederatedDashboardV1({
  onError,
  theme = "light",
}: FederatedDashboardV1Props) {
  const [loading, setLoading] = useState(true);
  const [hasError, setHasError] = useState(false);

  useEffect(() => {
    const loadDashboard = async () => {
      try {
        await new Promise(resolve => setTimeout(resolve, 500));
        setLoading(false);
      } catch (err) {
        setHasError(true);
        onError?.(err as Error);
        setLoading(false);
      }
    };
    loadDashboard();
  }, [onError]);

  if (hasError) {
    return (
      <div>
        Error al cargar el Dashboard remoto!!!
      </div>
    );
  }

  return (
    <div>
      {loading ? <Loader /> : <ECommerce />}
    </div>
  );
}
