/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,jsx,ts,tsx}'],
  theme: {
    extend: {
      // ── Brand colors ───────────────────────────────────────────
      colors: {
        brand: {
          DEFAULT: '#E8441A',
          dark:    '#CC3A15',
          light:   '#FF6B35',
        },
        surface: '#F8F5F0',
      },

      // ── Custom font families ───────────────────────────────────
      fontFamily: {
        sans:    ['DM Sans', 'ui-sans-serif', 'system-ui', 'sans-serif'],
        display: ['Syne', 'ui-serif', 'serif'],
      },

      // ── Float animation for hero emoji ─────────────────────────
      keyframes: {
        float: {
          '0%, 100%': { transform: 'translateY(0)' },
          '50%':      { transform: 'translateY(-12px)' },
        },
      },
      animation: {
        float: 'float 3s ease-in-out infinite',
      },
    },
  },
  plugins: [],
};