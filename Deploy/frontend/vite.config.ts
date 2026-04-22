import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: { '@': resolve(__dirname, 'src') }
  },
  server: {
    port: 3200,
    proxy: {
      '/api': { target: 'http://localhost:5200', changeOrigin: true },
      '/hubs': { target: 'http://localhost:5200', changeOrigin: true, ws: true }
    }
  }
})
