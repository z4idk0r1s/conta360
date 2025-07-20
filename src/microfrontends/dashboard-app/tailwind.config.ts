import rootConfig from '../../root-config/tailwind.config';

export default {
  ...rootConfig,
  content: [
    './pages/**/*.{ts,tsx,js,jsx}',
    './components/**/*.{ts,tsx,js,jsx}',
  ],
};
