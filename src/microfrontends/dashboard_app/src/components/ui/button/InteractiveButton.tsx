// dashboard_app/src/components/forms/InteractiveButton.tsx
"use client";

import Button from "@/components/ui/button/Button";

export default function InteractiveButton() {
    return (
        <Button onClick={() => console.log("Formulario enviado!")}>
            Crear SUENLACE.DAT
        </Button>
    );
}