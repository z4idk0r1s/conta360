import type { Config } from 'tailwindcss';

const config: Config = {
  content: [
    './src/**/*.{js,ts,jsx,tsx,mdx}',
    './public/**/*.svg', // opcional si usas SVG como componentes react
  ],
  theme: {
    extend: {
      backgroundImage: {
        'gradient-conic': 'conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))',
        'gradient-radial': 'radial-gradient(var(--tw-gradient-stops))',
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),       // Para estilos de inputs
    require('@tailwindcss/typography'),  // Por si usas prose
    require('@tailwindcss/aspect-ratio'),// Para responsividad de videos/images
  ],
  darkMode: 'class',
};
export default config;
