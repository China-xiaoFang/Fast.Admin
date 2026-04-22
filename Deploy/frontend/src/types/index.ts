export enum AppType { DotNet = 0, Vue = 1 }
export enum DeployStrategy { Single = 0, Rolling = 1, BlueGreen = 2 }
export enum DeployStatus { Pending = 0, Running = 1, Success = 2, Failed = 3, RollingBack = 4, RolledBack = 5 }
export enum NodeStatus { Online = 0, Offline = 1 }
export enum OsType { Windows = 0, Linux = 1 }

export interface App {
  id: number
  name: string
  description: string
  appType: AppType
  createdAt: string
}

export interface AppVersion {
  id: number
  appId: number
  version: string
  notes: string
  isActive: boolean
  createdAt: string
}

export interface Node {
  id: number
  name: string
  ip: string
  port: number
  osType: OsType
  status: NodeStatus
  lastHeartbeat: string | null
  createdAt: string
}

export interface Deployment {
  id: number
  appId: number
  appName: string
  versionId: number
  versionName: string
  strategy: DeployStrategy
  status: DeployStatus
  operator: string
  startedAt: string | null
  finishedAt: string | null
  createdAt: string
}

export interface DeployLog {
  id: number
  deploymentId: number
  nodeId: number | null
  level: string
  message: string
  createdAt: string
}
