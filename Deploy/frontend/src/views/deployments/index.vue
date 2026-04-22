<template>
  <div>
    <div style="margin-bottom:16px;display:flex;justify-content:space-between;align-items:center;">
      <span style="font-size:16px;font-weight:600;">发布管理</span>
      <el-button type="primary" :icon="Promotion" @click="openDeploy">发起部署</el-button>
    </div>

    <el-card>
      <el-table :data="deployments" v-loading="loading" stripe>
        <el-table-column prop="id" label="ID" width="70" />
        <el-table-column prop="appName" label="应用" />
        <el-table-column prop="versionName" label="版本" width="120" />
        <el-table-column label="策略" width="100">
          <template #default="{ row }">{{ strategyLabel(row.strategy) }}</template>
        </el-table-column>
        <el-table-column label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="statusType(row.status)" size="small">{{ statusLabel(row.status) }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="operator" label="操作人" width="100" />
        <el-table-column label="开始时间" width="160">
          <template #default="{ row }">{{ row.startedAt ? formatDate(row.startedAt) : '—' }}</template>
        </el-table-column>
        <el-table-column label="结束时间" width="160">
          <template #default="{ row }">{{ row.finishedAt ? formatDate(row.finishedAt) : '—' }}</template>
        </el-table-column>
        <el-table-column label="操作" width="160">
          <template #default="{ row }">
            <el-button size="small" plain @click="$router.push(`/logs/${row.id}`)">日志</el-button>
            <el-popconfirm title="确定回滚到上一版本？" @confirm="handleRollback(row.id)">
              <template #reference>
                <el-button size="small" plain type="warning">回滚</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- Deploy Dialog -->
    <el-dialog v-model="deployDialog" title="发起部署" width="560px">
      <el-form :model="deployForm" label-width="90px">
        <el-form-item label="应用" required>
          <el-select v-model="deployForm.appId" style="width:100%;" @change="onAppChange">
            <el-option v-for="a in apps" :key="a.id" :label="a.name" :value="a.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="版本" required>
          <el-select v-model="deployForm.versionId" style="width:100%;" :disabled="!deployForm.appId">
            <el-option v-for="v in appVersions" :key="v.id" :label="v.version + (v.isActive ? ' (激活)' : '')" :value="v.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="部署策略">
          <el-radio-group v-model="deployForm.strategy">
            <el-radio :value="DeployStrategy.Single">单机部署</el-radio>
            <el-radio :value="DeployStrategy.Rolling">滚动发布</el-radio>
            <el-radio :value="DeployStrategy.BlueGreen">蓝绿发布</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="目标节点" required>
          <el-select v-model="deployForm.nodeIds" multiple style="width:100%;">
            <el-option v-for="n in nodes" :key="n.id" :label="`${n.name} (${n.ip}:${n.port})`" :value="n.id" />
          </el-select>
        </el-form-item>
        <el-form-item label="健康检查">
          <el-input v-model="deployForm.healthCheckUrl" placeholder="http://... (可选)" />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="deployDialog = false">取消</el-button>
        <el-button type="primary" :loading="deploying" @click="handleDeploy">开始部署</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Promotion } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { appApi } from '@/api/app'
import { versionApi } from '@/api/version'
import { nodeApi } from '@/api/node'
import { deploymentApi } from '@/api/deployment'
import { DeployStrategy, DeployStatus, type App, type AppVersion, type Node, type Deployment } from '@/types'

defineOptions({ name: 'DeployManagement' })

const deployments = ref<Deployment[]>([])
const apps = ref<App[]>([])
const nodes = ref<Node[]>([])
const appVersions = ref<AppVersion[]>([])
const loading = ref(false)
const deploying = ref(false)
const deployDialog = ref(false)
const deployForm = ref({ appId: 0, versionId: 0, strategy: DeployStrategy.Single, nodeIds: [] as number[], healthCheckUrl: '' })

const formatDate = (d: string) => new Date(d).toLocaleString('zh-CN')
const strategyLabel = (s: DeployStrategy) => (['单机', '滚动', '蓝绿'])[s]
const statusLabel = (s: DeployStatus) => (['待执行', '执行中', '成功', '失败', '回滚中', '已回滚'])[s]
const statusType = (s: DeployStatus) => (['info', 'primary', 'success', 'danger', 'warning', 'info'])[s] as any

const load = async () => {
  loading.value = true
  try { deployments.value = await deploymentApi.list() } finally { loading.value = false }
}

const openDeploy = async () => {
  apps.value = await appApi.list()
  nodes.value = await nodeApi.list()
  deployForm.value = { appId: 0, versionId: 0, strategy: DeployStrategy.Single, nodeIds: [], healthCheckUrl: '' }
  deployDialog.value = true
}

const onAppChange = async (appId: number) => {
  deployForm.value.versionId = 0
  appVersions.value = await versionApi.list(appId)
}

const handleDeploy = async () => {
  if (!deployForm.value.appId || !deployForm.value.versionId) { ElMessage.warning('请选择应用和版本'); return }
  if (!deployForm.value.nodeIds.length) { ElMessage.warning('请选择目标节点'); return }
  deploying.value = true
  try {
    await deploymentApi.start(deployForm.value)
    ElMessage.success('部署已启动')
    deployDialog.value = false
    await load()
  } finally { deploying.value = false }
}

const handleRollback = async (id: number) => {
  try {
    await deploymentApi.rollback(id)
    ElMessage.success('回滚指令已发送')
    await load()
  } catch { /* handled by interceptor */ }
}

onMounted(load)
</script>
