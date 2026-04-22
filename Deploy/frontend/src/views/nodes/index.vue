<template>
  <div>
    <div style="margin-bottom:16px;display:flex;justify-content:space-between;align-items:center;">
      <span style="font-size:16px;font-weight:600;">节点管理</span>
      <el-button type="primary" :icon="Plus" @click="dialogVisible = true">注册节点</el-button>
    </div>

    <el-card>
      <el-table :data="nodes" v-loading="loading" stripe>
        <el-table-column prop="id" label="ID" width="70" />
        <el-table-column prop="name" label="节点名称" />
        <el-table-column prop="ip" label="IP" width="150" />
        <el-table-column prop="port" label="端口" width="90" />
        <el-table-column label="系统" width="100">
          <template #default="{ row }">
            <el-tag size="small" :type="row.osType === OsType.Windows ? 'primary' : ''">
              {{ row.osType === OsType.Windows ? 'Windows' : 'Linux' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="状态" width="100">
          <template #default="{ row }">
            <el-badge :is-dot="true" :type="row.status === NodeStatus.Online ? 'success' : 'danger'" style="margin-right:6px;" />
            {{ row.status === NodeStatus.Online ? '在线' : '离线' }}
          </template>
        </el-table-column>
        <el-table-column label="最后心跳" width="180">
          <template #default="{ row }">{{ row.lastHeartbeat ? formatDate(row.lastHeartbeat) : '—' }}</template>
        </el-table-column>
        <el-table-column label="操作" width="80">
          <template #default="{ row }">
            <el-popconfirm title="确定删除该节点？" @confirm="handleDelete(row.id)">
              <template #reference>
                <el-button size="small" plain type="danger">删除</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <el-dialog v-model="dialogVisible" title="注册节点" width="480px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="节点名称" required><el-input v-model="form.name" /></el-form-item>
        <el-form-item label="IP 地址" required><el-input v-model="form.ip" /></el-form-item>
        <el-form-item label="端口"><el-input-number v-model="form.port" :min="1" :max="65535" style="width:100%;" /></el-form-item>
        <el-form-item label="Token"><el-input v-model="form.token" placeholder="与 Agent 配置一致" /></el-form-item>
        <el-form-item label="操作系统">
          <el-select v-model="form.osType" style="width:100%;">
            <el-option label="Linux" :value="OsType.Linux" />
            <el-option label="Windows" :value="OsType.Windows" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="handleSave">注册</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { nodeApi } from '@/api/node'
import { OsType, NodeStatus, type Node } from '@/types'

defineOptions({ name: 'NodeManagement' })

const nodes = ref<Node[]>([])
const loading = ref(false)
const saving = ref(false)
const dialogVisible = ref(false)
const form = ref({ name: '', ip: '', port: 5201, token: '', osType: OsType.Linux })

const formatDate = (d: string) => new Date(d).toLocaleString('zh-CN')

const load = async () => {
  loading.value = true
  try { nodes.value = await nodeApi.list() } finally { loading.value = false }
}

const handleSave = async () => {
  if (!form.value.name.trim() || !form.value.ip.trim()) { ElMessage.warning('请填写节点名称和 IP'); return }
  saving.value = true
  try {
    await nodeApi.register(form.value)
    ElMessage.success('节点注册成功')
    dialogVisible.value = false
    form.value = { name: '', ip: '', port: 5201, token: '', osType: OsType.Linux }
    await load()
  } finally { saving.value = false }
}

const handleDelete = async (id: number) => {
  await nodeApi.remove(id)
  ElMessage.success('删除成功')
  await load()
}

onMounted(load)
</script>
