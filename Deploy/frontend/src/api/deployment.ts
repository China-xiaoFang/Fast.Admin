import request from './index'
import type { Deployment } from '@/types'

export const deploymentApi = {
  list: (): Promise<Deployment[]> => request.get('/deployments'),
  get: (id: number): Promise<Deployment> => request.get(`/deployments/${id}`),
  start: (data: { appId: number; versionId: number; strategy: number; nodeIds: number[]; healthCheckUrl: string }): Promise<Deployment> =>
    request.post('/deployments', data),
  rollback: (id: number): Promise<Deployment> => request.post(`/deployments/${id}/rollback`)
}
