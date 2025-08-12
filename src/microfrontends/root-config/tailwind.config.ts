import type { Config } from 'tailwindcss';
import sharedConfig from '@cont360/tailwind-config';

const config: Config = {
  ...sharedConfig,
  content: [
    './pages/**/*.{js,ts,jsx,tsx,mdx,css}',
    './node_modules/flowbite/**/*.js',
  ],
};

export default config;