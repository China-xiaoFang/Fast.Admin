import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { App, Node, Deployment } from '@/types'
import { appApi } from '@/api/app'
import { nodeApi } from '@/api/node'
import { deploymentApi } from '@/api/deployment'

export const useDeployStore = defineStore('deploy', () => {
  const apps = ref<App[]>([])
  const nodes = ref<Node[]>([])
  const deployments = ref<Deployment[]>([])

  const fetchApps = async () => { apps.value = await appApi.list() }
  const fetchNodes = async () => { nodes.value = await nodeApi.list() }
  const fetchDeployments = async () => { deployments.value = await deploymentApi.list() }

  return { apps, nodes, deployments, fetchApps, fetchNodes, fetchDeployments }
})
