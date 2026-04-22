<template>
  <div>
    <div style="margin-bottom:16px;display:flex;gap:12px;align-items:center;">
      <span style="font-size:16px;font-weight:600;">版本管理</span>
      <el-select v-model="selectedAppId" placeholder="选择应用" style="width:220px;" @change="loadVersions">
        <el-option v-for="a in apps" :key="a.id" :label="a.name" :value="a.id" />
      </el-select>
      <el-button type="primary" :disabled="!selectedAppId" :icon="Upload" @click="uploadDialogVisible = true">上传版本包</el-button>
    </div>

    <el-card>
      <el-table :data="versions" v-loading="loading" stripe>
        <el-table-column prop="id" label="ID" width="70" />
        <el-table-column prop="version" label="版本号" width="140" />
        <el-table-column prop="notes" label="备注" show-overflow-tooltip />
        <el-table-column label="状态" width="100">
          <template #default="{ row }">
            <el-tag :type="row.isActive ? 'success' : 'info'" size="small">
              {{ row.isActive ? '激活' : '未激活' }}
            </el-tag>
          </template>
        </el-table-column>
        <el-table-column label="上传时间" width="180">
          <template #default="{ row }">{{ formatDate(row.createdAt) }}</template>
        </el-table-column>
        <el-table-column label="操作" width="160">
          <template #default="{ row }">
            <el-button v-if="!row.isActive" size="small" plain type="success" @click="handleActivate(row)">激活</el-button>
            <el-popconfirm title="确定删除该版本？" @confirm="handleDelete(row.id)">
              <template #reference>
                <el-button size="small" plain type="danger">删除</el-button>
              </template>
            </el-popconfirm>
          </template>
        </el-table-column>
      </el-table>
    </el-card>

    <!-- Upload Dialog -->
    <el-dialog v-model="uploadDialogVisible" title="上传版本包" width="480px">
      <el-form label-width="80px">
        <el-form-item label="版本号" required>
          <el-input v-model="uploadForm.version" placeholder="例如: 1.0.0" />
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="uploadForm.notes" type="textarea" :rows="2" />
        </el-form-item>
        <el-form-item label="包文件">
          <el-upload drag :auto-upload="false" :limit="1" accept=".zip,.tar,.gz,.tgz" :on-change="onFileChange" :file-list="fileList">
            <el-icon style="font-size:48px;"><UploadFilled /></el-icon>
            <div>拖拽或点击上传（.zip / .tar.gz / .tgz）</div>
          </el-upload>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="uploadDialogVisible = false">取消</el-button>
        <el-button type="primary" :loading="uploading" @click="handleUpload">上传</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { Upload, UploadFilled } from '@element-plus/icons-vue'
import { ElMessage } from 'element-plus'
import { appApi } from '@/api/app'
import { versionApi } from '@/api/version'
import type { App, AppVersion } from '@/types'

defineOptions({ name: 'VersionManagement' })

const apps = ref<App[]>([])
const versions = ref<AppVersion[]>([])
const selectedAppId = ref<number | null>(null)
const loading = ref(false)
const uploading = ref(false)
const uploadDialogVisible = ref(false)
const uploadForm = ref({ version: '', notes: '' })
const fileList = ref<any[]>([])
const selectedFile = ref<File | null>(null)

const formatDate = (d: string) => new Date(d).toLocaleString('zh-CN')

const loadVersions = async () => {
  if (!selectedAppId.value) return
  loading.value = true
  try { versions.value = await versionApi.list(selectedAppId.value) } finally { loading.value = false }
}

const onFileChange = (file: any) => { selectedFile.value = file.raw }

const handleUpload = async () => {
  if (!selectedAppId.value) return
  if (!uploadForm.value.version.trim()) { ElMessage.warning('请输入版本号'); return }
  if (!selectedFile.value) { ElMessage.warning('请选择文件'); return }
  uploading.value = true
  try {
    const fd = new FormData()
    fd.append('file', selectedFile.value)
    fd.append('version', uploadForm.value.version)
    fd.append('notes', uploadForm.value.notes)
    await versionApi.upload(selectedAppId.value, fd)
    ElMessage.success('上传成功')
    uploadDialogVisible.value = false
    uploadForm.value = { version: '', notes: '' }
    fileList.value = []
    selectedFile.value = null
    await loadVersions()
  } finally { uploading.value = false }
}

const handleActivate = async (row: AppVersion) => {
  await versionApi.activate(row.appId, row.id)
  ElMessage.success('已激活')
  await loadVersions()
}

const handleDelete = async (id: number) => {
  await versionApi.remove(id)
  ElMessage.success('删除成功')
  await loadVersions()
}

onMounted(async () => { apps.value = await appApi.list() })
</script>
