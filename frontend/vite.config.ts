import { defineConfig } from "vite"
import react from "@vitejs/plugin-react"
import hljsLangImportMapGen from "./src/plugins/hljs-lang-import-map-gen";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    hljsLangImportMapGen()
  ],
  build: {
    outDir: "../web",
    assetsDir: "static",
    emptyOutDir: true
  }
})
