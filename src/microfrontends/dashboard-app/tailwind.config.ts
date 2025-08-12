import type { Config } from 'tailwindcss';
import sharedConfig from '@cont360/tailwind-config';

const config: Config = {
  ...sharedConfig,
  content: [
    './pages/**/*.{js,ts,jsx,tsx,mdx}',
    './components/**/*.{js,ts,jsx,tsx,mdx}',
    './node_modules/flowbite/**/*.js',
  ],
};

export default config;