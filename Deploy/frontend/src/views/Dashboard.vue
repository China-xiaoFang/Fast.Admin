<template>
  <div>
    <el-row :gutter="16" style="margin-bottom:20px;">
      <el-col :span="6">
        <el-card shadow="hover">
          <el-statistic title="应用总数" :value="stats.apps" />
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <el-statistic title="在线节点" :value="stats.onlineNodes" suffix=" / {{ stats.totalNodes }}" />
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <el-statistic title="今日发布" :value="stats.todayDeploys" />
        </el-card>
      </el-col>
      <el-col :span="6">
        <el-card shadow="hover">
          <el-statistic title="发布成功率" :value="stats.successRate" suffix="%" />
        </el-card>
      </el-col>
    </el-row>

    <el-card shadow="hover">
      <template #header>
        <span>最近发布记录</span>
        <el-button style="float:right;" size="small" type="primary" @click="$router.push('/deployments')">发起部署</el-button>
      </template>
      <el-table :data="recentDeployments" stripe size="small">
        <el-table-column prop="id" label="ID" width="60" />
        <el-table-column prop="appName" label="应用" />
        <el-table-column prop="versionName" label="版本" />
        <el-table-column label="策略">
          <template #default="{ row }">{{ strategyLabel(row.strategy) }}</template>
        </el-table-column>
        <el-table-column label="状态">
          <template #default="{ row }">
            <el-tag :type="statusType(row.status)" size="small">{{ statusLabel(row.status) }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="operator" label="操作人" />
        <el-table-column label="操作" width="100">
          <template #default="{ row }">
            <el-button size="small" plain @click="$router.push(`/logs/${row.id}`)">查看日志</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useDeployStore } from '@/stores/deploy'
import { DeployStatus, DeployStrategy, NodeStatus } from '@/types'

defineOptions({ name: 'Dashboard' })

const store = useDeployStore()

const recentDeployments = computed(() => store.deployments.slice(0, 10))

const stats = computed(() => {
  const onlineNodes = store.nodes.filter(n => n.status === NodeStatus.Online).length
  const totalDeploys = store.deployments.length
  const successDeploys = store.deployments.filter(d => d.status === DeployStatus.Success).length
  const today = new Date().toDateString()
  const todayDeploys = store.deployments.filter(d => new Date(d.createdAt).toDateString() === today).length
  return {
    apps: store.apps.length,
    onlineNodes,
    totalNodes: store.nodes.length,
    todayDeploys,
    successRate: totalDeploys > 0 ? Math.round((successDeploys / totalDeploys) * 100) : 100
  }
})

const statusLabel = (s: DeployStatus) => (['待执行','执行中','成功','失败','回滚中','已回滚'])[s] ?? s
const statusType = (s: DeployStatus) => (['info','primary','success','danger','warning','info'])[s] ?? 'info'
const strategyLabel = (s: DeployStrategy) => (['单机','滚动','蓝绿'])[s] ?? s

onMounted(() => {
  store.fetchApps()
  store.fetchNodes()
  store.fetchDeployments()
})
</script>
