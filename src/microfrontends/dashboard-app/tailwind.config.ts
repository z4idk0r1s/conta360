import rootConfig from '../root-config/tailwind.config';

const dashboardConfig = {
  ...rootConfig,
  content: [
    './pages/**/*.{js,ts,jsx,tsx,mdx}',
    './components/**/*.{js,ts,jsx,tsx,mdx}',
    './node_modules/flowbite/**/*.js',
  ],
};

export default dashboardConfig;
