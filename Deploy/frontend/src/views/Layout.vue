<template>
  <el-container style="height: 100vh;">
    <!-- Sidebar -->
    <el-aside width="220px" style="background:#001529;">
      <div style="color:#fff;font-size:16px;font-weight:bold;padding:20px 16px;border-bottom:1px solid #1f3c5e;">
        🚀 Fast Deploy
      </div>
      <el-menu
        :default-active="activeMenu"
        background-color="#001529"
        text-color="#a6adb4"
        active-text-color="#fff"
        router
      >
        <el-menu-item index="/dashboard">
          <el-icon><DataAnalysis /></el-icon>
          <span>概览</span>
        </el-menu-item>
        <el-menu-item index="/apps">
          <el-icon><Box /></el-icon>
          <span>应用管理</span>
        </el-menu-item>
        <el-menu-item index="/versions">
          <el-icon><Files /></el-icon>
          <span>版本管理</span>
        </el-menu-item>
        <el-menu-item index="/nodes">
          <el-icon><Monitor /></el-icon>
          <span>节点管理</span>
        </el-menu-item>
        <el-menu-item index="/deployments">
          <el-icon><Promotion /></el-icon>
          <span>发布管理</span>
        </el-menu-item>
        <el-menu-item index="/logs">
          <el-icon><Document /></el-icon>
          <span>部署日志</span>
        </el-menu-item>
      </el-menu>
    </el-aside>

    <!-- Main content -->
    <el-container>
      <el-header style="background:#fff;border-bottom:1px solid #e4e7ed;display:flex;align-items:center;padding:0 20px;">
        <span style="font-size:15px;color:#303133;">Fast Deploy — 轻量级自动化发布系统</span>
      </el-header>
      <el-main style="background:#f5f7fa;padding:20px;">
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'

defineOptions({ name: 'Layout' })

const route = useRoute()
const activeMenu = computed(() => {
  const path = route.path
  if (path.startsWith('/logs')) return '/logs'
  return path
})
</script>

<style>
* { margin: 0; padding: 0; box-sizing: border-box; }
body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif; }
.el-menu { border-right: none !important; }
</style>
