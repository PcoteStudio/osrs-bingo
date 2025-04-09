import tailwindcss_primeui from 'tailwindcss-primeui';

/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,ts}'],
  purge: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  darkMode: true,
  theme: {
    extend: {},
  },
  plugins: [tailwindcss_primeui]
};

