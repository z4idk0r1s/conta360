// src/microfrontends/root-config/types/next-dynamic.d.ts
declare module 'next/dynamic' {
  import { ComponentType } from 'react';
  function dynamic<P = {}>(
    importFunc: () => Promise<{ default: ComponentType<P> }>,
    options?: { ssr?: boolean }
  ): ComponentType<P>;
  export default dynamic;
}


// Declaraciones para módulos de Next.js que TypeScript no encuentra
declare module 'next/font/google' {
  type FontOptions = { subsets?: string[]; weight?: string | string[]; style?: string | string[]; display?: string };
  export function Inter(options?: FontOptions): { className: string };
  // Aquí puedes añadir otras fuentes si las usas, ej. Roboto, etc.
}

declare module 'next/head' {
  import { ComponentType, ReactNode } from 'react';

  interface HeadProps {
    children?: ReactNode;
  }

  const Head: ComponentType<HeadProps>;
  export default Head;
}

declare module 'next/app' {
  import { ComponentType } from 'react';

  // AppProps con cualquier props de página
  export interface AppProps<P = {}> {
    Component: ComponentType<P>;
    pageProps: P;
  }

  const App: ComponentType<AppProps>;
  export default App;
}

