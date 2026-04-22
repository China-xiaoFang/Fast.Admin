<template>
  <div>
    <div style="margin-bottom:16px;display:flex;gap:12px;align-items:center;flex-wrap:wrap;">
      <span style="font-size:16px;font-weight:600;">部署日志</span>
      <el-select v-model="selectedId" placeholder="选择部署记录" style="width:280px;" clearable @change="onDeploymentChange">
        <el-option v-for="d in deployments" :key="d.id" :label="`#${d.id} - ${d.appName} ${d.versionName}`" :value="d.id" />
      </el-select>
      <el-button v-if="!connected" type="primary" :loading="connecting" :disabled="!selectedId" @click="connectSignalR">
        连接实时日志
      </el-button>
      <el-button v-else type="danger" @click="disconnectSignalR">断开连接</el-button>
      <el-button @click="logs = []">清空</el-button>
      <el-tag v-if="connected" type="success" size="small">实时连接中</el-tag>
      <el-tag v-else-if="connecting" type="warning" size="small">连接中...</el-tag>
    </div>

    <!-- Terminal log output -->
    <div ref="terminalRef" class="terminal">
      <div v-if="!logs.length" style="color:#666;">暂无日志，请选择部署记录...</div>
      <div v-for="log in logs" :key="log.id" class="log-line">
        <span style="color:#888;">{{ formatTime(log.createdAt) }}</span>
        <span :class="levelClass(log.level)" style="margin:0 8px;">[{{ log.level }}]</span>
        <span>{{ log.message }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, nextTick, watch } from 'vue'
import { useRoute } from 'vue-router'
import * as signalR from '@microsoft/signalr'
import { deploymentApi } from '@/api/deployment'
import { logApi } from '@/api/log'
import type { Deployment, DeployLog } from '@/types'

defineOptions({ name: 'DeployLogs' })

const route = useRoute()
const deployments = ref<Deployment[]>([])
const selectedId = ref<number | null>(null)
const logs = ref<DeployLog[]>([])
const connected = ref(false)
const connecting = ref(false)
const terminalRef = ref<HTMLElement>()

let connection: signalR.HubConnection | null = null

const formatTime = (d: string) => new Date(d).toLocaleTimeString('zh-CN', { hour12: false })
const levelClass = (level: string) => {
  if (level === 'Error') return 'log-error'
  if (level === 'Warn') return 'log-warn'
  return 'log-info'
}

const scrollToBottom = () => {
  nextTick(() => {
    if (terminalRef.value) terminalRef.value.scrollTop = terminalRef.value.scrollHeight
  })
}

const onDeploymentChange = async (id: number) => {
  logs.value = []
  if (!id) return
  try { logs.value = await logApi.list(id) } catch { /* ignore */ }
  scrollToBottom()
  // Auto-connect SignalR
  if (!connected.value) await connectSignalR()
}

const connectSignalR = async () => {
  if (!selectedId.value) return
  if (connection) await connection.stop()
  connecting.value = true
  connection = new signalR.HubConnectionBuilder()
    .withUrl('/hubs/deploy')
    .withAutomaticReconnect()
    .build()

  connection.on('DeployLog', (log: DeployLog) => {
    logs.value.push(log)
    scrollToBottom()
  })

  connection.onclose(() => { connected.value = false })
  connection.onreconnected(() => { connected.value = true })

  try {
    await connection.start()
    await connection.invoke('JoinDeployment', String(selectedId.value))
    connected.value = true
  } catch (e) {
    connected.value = false
  } finally {
    connecting.value = false
  }
}

const disconnectSignalR = async () => {
  if (connection) { await connection.stop(); connection = null }
  connected.value = false
}

watch(logs, scrollToBottom)

onMounted(async () => {
  deployments.value = await deploymentApi.list()
  const paramId = route.params.deploymentId
  if (paramId) {
    selectedId.value = Number(paramId)
    await onDeploymentChange(selectedId.value)
  }
})

onUnmounted(() => { connection?.stop() })
</script>

<style scoped>
.terminal {
  background: #1e1e1e;
  color: #d4d4d4;
  font-family: 'Courier New', Consolas, monospace;
  font-size: 13px;
  line-height: 1.6;
  padding: 16px;
  border-radius: 6px;
  min-height: 500px;
  max-height: 700px;
  overflow-y: auto;
}
.log-line { white-space: pre-wrap; word-break: break-all; }
.log-info { color: #9cdcfe; }
.log-warn { color: #f0d68a; }
.log-error { color: #f48771; }
</style>
