// src/microfrontends/root_config/types/remote-declarations.d.ts

declare module 'dashboard_app/*' {
  import { ComponentType } from 'react';
  const Component: ComponentType<any>;
  export default Component;
}

// Extender tipos globales para Module Federation
declare global {
  const __webpack_init_sharing__: (scope: string) => Promise<void>;
  const __webpack_share_scopes__: Record<string, any>;
  
  interface Window {
    __FEDERATION_TIMEOUT__?: number;
  }
}
/*
declare module 'dashboard_app/E-commerce' {
  import { ComponentType } from 'react';
  const ECommerce: ComponentType;
  export default ECommerce;
}

declare module 'dashboard_app/AuthSignInPage' {
  import { ComponentType } from 'react';
  const AuthSignInPage: ComponentType;
  export default AuthSignInPage;
}

declare module 'dashboard_app/AuthSignUpPage' {
  import { ComponentType } from 'react';
  const AuthSignUpPage: ComponentType;
  export default AuthSignUpPage;
}

declare module 'dashboard_app/CalendarPage' {
  import { ComponentType } from 'react';
  const CalendarPage: ComponentType;
  export default CalendarPage;
}

declare module 'dashboard_app/ChartPage' {
  import { ComponentType } from 'react';
  const ChartPage: ComponentType;
  export default ChartPage;
}

declare module 'dashboard_app/FormElementsPage' {
  import { ComponentType } from 'react';
  const FormElementsPage: ComponentType;
  export default FormElementsPage;
}

declare module 'dashboard_app/FormLayoutPage' {
  import { ComponentType } from 'react';
  const FormLayoutPage: ComponentType;
  export default FormLayoutPage;
}

declare module 'dashboard_app/ProfilePage' {
  import { ComponentType } from 'react';
  const ProfilePage: ComponentType;
  export default ProfilePage;
}

declare module 'dashboard_app/SettingsPage' {
  import { ComponentType } from 'react';
  const SettingsPage: ComponentType;
  export default SettingsPage;
}

declare module 'dashboard_app/TablesPage' {
  import { ComponentType } from 'react';
  const TablesPage: ComponentType;
  export default TablesPage;
}

declare module 'dashboard_app/AlertsPage' {
  import { ComponentType } from 'react';
  const AlertsPage: ComponentType;
  export default AlertsPage;
}

declare module 'dashboard_app/ButtonsPage' {
  import { ComponentType } from 'react';
  const ButtonsPage: ComponentType;
  export default ButtonsPage;
}
  */