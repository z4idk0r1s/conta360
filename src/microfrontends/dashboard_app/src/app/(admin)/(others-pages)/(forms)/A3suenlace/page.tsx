import PageBreadcrumb from "@/components/common/PageBreadCrumb";
import SelectInputs from "@/components/form/form-elements/SelectInputs";
import DropzoneComponent from "@/components/form/form-elements/DropZone";
import InteractiveButton from "@/components/ui/button/InteractiveButton";
import { Metadata } from "next";
import React from "react";

export const metadata: Metadata = {
  title: "WSK 360 | Next.js Dashboard Template",
  description:
    "This is WSK 360 - Next.js Tailwind CSS Admin Dashboard Template",
};

export default function A3Elements() {
  return (
    <div>
      <PageBreadcrumb pageTitle="A3suenlace" />
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">
        <div className="space-y-6">
          <SelectInputs />
        </div>
        <div className="space-y-6">
          <DropzoneComponent />
        </div>
        <InteractiveButton />
      </div>
    </div>
  );
}
