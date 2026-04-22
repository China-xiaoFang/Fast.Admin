import request from './index'
import type { Node } from '@/types'

export const nodeApi = {
  list: (): Promise<Node[]> => request.get('/nodes'),
  register: (data: { name: string; ip: string; port: number; token: string; osType: number }): Promise<Node> =>
    request.post('/nodes', data),
  remove: (id: number) => request.delete(`/nodes/${id}`)
}
