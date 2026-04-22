import { createRouter, createWebHistory } from 'vue-router'
import Layout from '@/views/Layout.vue'

const routes = [
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [
      { path: 'dashboard', name: 'Dashboard', component: () => import('@/views/Dashboard.vue'), meta: { title: '概览' } },
      { path: 'apps', name: 'Apps', component: () => import('@/views/apps/index.vue'), meta: { title: '应用管理' } },
      { path: 'versions', name: 'Versions', component: () => import('@/views/versions/index.vue'), meta: { title: '版本管理' } },
      { path: 'nodes', name: 'Nodes', component: () => import('@/views/nodes/index.vue'), meta: { title: '节点管理' } },
      { path: 'deployments', name: 'Deployments', component: () => import('@/views/deployments/index.vue'), meta: { title: '发布管理' } },
      { path: 'logs/:deploymentId?', name: 'Logs', component: () => import('@/views/logs/index.vue'), meta: { title: '部署日志' } }
    ]
  }
]

export default createRouter({ history: createWebHistory(), routes })
