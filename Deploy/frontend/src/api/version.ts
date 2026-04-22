import request from './index'
import type { AppVersion } from '@/types'

export const versionApi = {
  list: (appId: number): Promise<AppVersion[]> => request.get(`/apps/${appId}/versions`),
  upload: (appId: number, formData: FormData): Promise<AppVersion> =>
    request.post(`/apps/${appId}/versions/upload`, formData, { headers: { 'Content-Type': 'multipart/form-data' } }),
  activate: (appId: number, versionId: number) => request.post(`/apps/${appId}/versions/${versionId}/activate`),
  remove: (versionId: number) => request.delete(`/versions/${versionId}`)
}
