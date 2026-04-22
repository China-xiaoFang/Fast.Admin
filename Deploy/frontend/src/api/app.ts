import request from './index'
import type { App } from '@/types'

export const appApi = {
  list: (): Promise<App[]> => request.get('/apps'),
  create: (data: { name: string; description: string; appType: number }): Promise<App> => request.post('/apps', data),
  update: (id: number, data: { name: string; description: string }) => request.put(`/apps/${id}`, data),
  remove: (id: number) => request.delete(`/apps/${id}`)
}
