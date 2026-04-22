<template>
  <div>
    <div style="margin-bottom:16px;display:flex;justify-content:space-between;align-items:center;">
      <span style="font-size:16px;font-weight:600;">应用管理</span>
      <el-button type="primary" :icon="Plus" @click="openAdd">新增应用</el-button>
    </div>

    <el-card>
      <el-table :data="apps" v-loading="loading" stripe>
        <el-table-column prop="id" label="ID" width="70" />
        <el-table-column prop="name" label="应用名称" />
        <el-table-column prop="description" label="描述" show-overflow-tooltip />
        <el-table-column label="类型" width="100">
          <template #default="{ row }">
            <el-tag :type="row.appType === AppType.DotNet ? 'primary' : 'success'" size="small">
              {{ row.appType === AppType.DotNet ? '.NET' : 'Vue' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="创建时间" width="180">
          <template #default="{ row }">{{ formatDate(row.createdAt) }}</template>
        </el-table-column>
        <el-table-column label="操作" width="160">
          <template #default="{ row }">
            <el-button size="small" plain type="primary" @click="openEdit(row)">编辑</el-button>
            <el-popconfirm title="确定删除该应用？" @confirm="handleDelete(row.id)">
              <template #reference>
                <el-button size="small" plain type="danger">删除</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- Add / Edit Dialog -->
    <el-dialog v-model="dialogVisible" :title="editingId ? '编辑应用' : '新增应用'" width="480px">
      <el-form :model="form" label-width="80px">
        <el-form-item label="应用名称" required>
          <el-input v-model="form.name" placeholder="请输入应用名称" />
        </el-form-item>
        <el-form-item label="描述">
          <el-input v-model="form.description" type="textarea" :rows="3" placeholder="应用描述（可选）" />
        </el-form-item>
        <el-form-item label="应用类型" v-if="!editingId">
          <el-select v-model="form.appType" style="width:100%;">
            <el-option label=".NET 应用" :value="AppType.DotNet" />
            <el-option label="Vue 应用" :value="AppType.Vue" />
          </el-select>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="saving" @click="handleSave">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { appApi } from '@/api/app'
import { AppType, type App } from '@/types'

defineOptions({ name: 'AppManagement' })

const apps = ref<App[]>([])
const loading = ref(false)
const saving = ref(false)
const dialogVisible = ref(false)
const editingId = ref<number | null>(null)
const form = ref({ name: '', description: '', appType: AppType.DotNet })

const formatDate = (d: string) => new Date(d).toLocaleString('zh-CN')

const load = async () => {
  loading.value = true
  try { apps.value = await appApi.list() } finally { loading.value = false }
}

const openAdd = () => {
  editingId.value = null
  form.value = { name: '', description: '', appType: AppType.DotNet }
  dialogVisible.value = true
}

const openEdit = (row: App) => {
  editingId.value = row.id
  form.value = { name: row.name, description: row.description ?? '', appType: row.appType }
  dialogVisible.value = true
}

const handleSave = async () => {
  if (!form.value.name.trim()) { ElMessage.warning('请输入应用名称'); return }
  saving.value = true
  try {
    if (editingId.value) {
      await appApi.update(editingId.value, { name: form.value.name, description: form.value.description })
    } else {
      await appApi.create(form.value)
    }
    ElMessage.success('保存成功')
    dialogVisible.value = false
    await load()
  } finally { saving.value = false }
}

const handleDelete = async (id: number) => {
  await appApi.remove(id)
  ElMessage.success('删除成功')
  await load()
}

onMounted(load)
</script>
