import ComponentCard from "@/components/common/ComponentCard";
import PageBreadcrumb from "@/components/common/PageBreadCrumb";
import A3bankTable from "@/components/tables/A3bankTable";
import DropzoneComponent from "@/components/form/form-elements/DropZone";
import InteractiveButton from "@/components/ui/button/InteractiveButton";
import { Metadata } from "next";
import React from "react";

export const metadata: Metadata = {
  title: "WSK 360 | Next.js Dashboard Template",
  description:
    "This is WSK 360 - Next.js Tailwind CSS Admin Dashboard Template",
};

export default function BankTables() {
  return (
    <div>
      <PageBreadcrumb pageTitle="A3 Bank" />

      {/* Fila superior: botón a la izquierda y Dropzone a la derecha */}
      <div className="grid grid-cols-1 xl:grid-cols-2 gap-6 mb-6">
        {/* Contenedor para alinear botón y dropzone */}
        <div className="flex flex-col xl:flex-row w-full gap-6 xl:gap-4 items-start xl:items-center">
          {/* Columna izquierda: botón */}
          <div className="flex justify-start">
            <InteractiveButton />
          </div>

          {/* Columna derecha: dropzone ajustada a la altura del botón */}
          <div className="flex justify-end w-full">
            <div className="flex-1 h-full max-h-[48px] overflow-x-auto">
              <DropzoneComponent />
            </div>
          </div>
        </div>
      </div>

      {/* Tabla bancos */}
      <ComponentCard title="Relacion de Asientos">
        <A3bankTable />
      </ComponentCard>
    </div>
  );
}
