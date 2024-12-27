import type { Config } from "tailwindcss"

export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}"
  ],
  theme: {
    extend: {
      colors: {
        "background-color": "#1e1e1e",
        "text-color": "#c8c8c8",
        "primary-color": "#007bff",
      }
    },
  },
  plugins: [],
} satisfies Config
