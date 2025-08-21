// src/microfrontends/dashboard_app/pages/index.tsx
// index.tsx de pages
import ECommerce from "@/components/Dashboard/E-commerce";
import { Metadata } from "next";

export const metadata: Metadata = {
  title: "Conta360 | Next.js Dashboard Template",
  description: "This is Home Blog page",
  // other metadata
};

export default function Home() {
  return (
    <>
      <ECommerce />
    </>
  );
}
