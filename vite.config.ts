import { resolve } from 'node:path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import glsl from 'vite-plugin-glsl'
import UnoCSS from 'unocss/vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), glsl(), UnoCSS()],
  base: '/learning-threejs/',
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src'),
      '@public': resolve(__dirname, 'public'),
    },
  },
  server: {
    host: 'localhost',
    port: 8080,
    open: true,
  },
  build: {
    outDir: './docs',
  },
})
