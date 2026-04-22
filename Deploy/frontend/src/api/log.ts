import request from './index'
import type { DeployLog } from '@/types'

export const logApi = {
  list: (deploymentId: number): Promise<DeployLog[]> => request.get(`/deployments/${deploymentId}/logs`)
}
