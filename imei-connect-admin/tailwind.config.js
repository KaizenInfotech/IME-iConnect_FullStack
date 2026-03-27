/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#1a297d',
          dark: '#092c5e',
          light: '#5997ff',
          hover: '#1477c7',
        },
        accent: {
          DEFAULT: '#ffc107',
          orange: '#ffa200',
          gold: '#fcba15',
          dark: '#e3a712',
        },
        success: {
          DEFAULT: '#6b9300',
          light: '#bbe679',
        },
        danger: {
          DEFAULT: '#d9534f',
          orange: '#e87e04',
          bright: '#ff5722',
        },
        teal: {
          DEFAULT: '#0ead9a',
          light: '#10d2bb',
        },
        info: {
          DEFAULT: '#00bcd4',
          blue: '#00a1e3',
        },
        stats: {
          orange: '#fa5833',
          lorange: '#fabb3d',
          blue: '#38ade8',
          green: '#bbe679',
          yellow: '#e8e57a',
          pink: '#e42b75',
        },
        gray: {
          50: '#f7f7f7',
          100: '#f2f2f2',
          150: '#efefef',
          200: '#e5e5e5',
          300: '#d0d0d0',
          400: '#999',
          500: '#7e7e7e',
          600: '#7a7a7a',
          700: '#646464',
          800: '#333',
          900: '#222',
        },
      },
      fontFamily: {
        sans: ['Roboto', 'Heebo', 'sans-serif'],
      },
      borderRadius: {
        'card': '10px',
        'btn': '8px',
        'input': '8px',
        'module': '16px',
      },
      boxShadow: {
        'card': '0 3px 5px 0 rgba(0, 0, 0, 0.1)',
        'card-hover': '0 0.4rem 1rem rgba(0, 0, 0, 0.1)',
        'table': '0 3px 5px 0px rgba(0, 0, 0, 0.06)',
        'input-focus': 'inset 0 0 0px rgba(0, 0, 0, 0.08), 0 0 13px rgba(89, 151, 255, 0.24)',
        'module': '0px 4px 8px 0 rgba(0, 0, 0, 0.05)',
        'module-hover': '0px 10px 20px 0 rgba(0, 0, 0, 0.05)',
      },
    },
  },
  plugins: [],
}
