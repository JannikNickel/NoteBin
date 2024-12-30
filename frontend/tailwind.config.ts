import type { Config } from "tailwindcss"

export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}"
  ],
  theme: {
    extend: {
      colors: {
        "background": "#1e1e1e",
        "text": "#c8c8c8",
        "primary": {
          default: "#1b6cc2",
          hover: "#16569b",
          active: "#104174",
          text: "#ebebeb"
        },
        "secondary": {
          default: "#171a1c",
          hover: "#151719",
          active: "#121516",
          text: "#c8c8c8"
        }
      }
    },
  },
  plugins: [],
} satisfies Config
