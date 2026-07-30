# Web.Admin 前端文档

## 技术栈

| 技术 | 版本 | 说明 |
|------|------|------|
| Vue | 3.x | 渐进式 JavaScript 框架 |
| TypeScript | 5.x | 类型安全 |
| Vite | 5.x | 下一代前端构建工具 |
| Element Plus | 最新 | Vue 3 UI 组件库 |
| Pinia | 最新 | Vue 状态管理 |
| pnpm | 8.x | 高效的包管理器 |

## 项目结构

```
Web.Admin/
├── src/
│   ├── api/                    # API 接口封装
│   │   └── services/           # 按模块组织
│   │       └── File/           # 文件服务 API
│   ├── views/                  # 页面视图
│   │   ├── login/              # 登录页（Classic/Simple/Split 三种风格）
│   │   └── system/             # 系统管理
│   │       └── file/           # 文件管理
│   ├── components/             # 公共组件
│   ├── layouts/                # 布局（Classic/Horizontal/Mixed）
│   ├── router/                 # 路由配置
│   ├── stores/                 # Pinia 状态管理
│   ├── styles/                 # 全局样式
│   ├── plugins/                # Vue 插件
│   ├── directives/             # 自定义指令
│   └── icons/                  # SVG 图标
├── types/                      # TypeScript 类型定义
├── public/                     # 静态资源
├── .env                        # 环境变量
├── vite.config.mts             # Vite 配置
├── tsconfig.json               # TypeScript 配置
└── package.json                # 项目依赖
```

## 开发规范

### API 封装规范

每个模块在 `src/api/services/` 下建立独立目录：

```
services/
└── File/
    ├── index.ts                # API 方法导出
    └── models/                 # TypeScript 接口定义
        ├── QueryFilePagedInput.ts
        ├── QueryFilePagedOutput.ts
        └── DownloadFileInput.ts
```

### 组件规范

- 使用 `<script lang="ts" setup>` 语法
- 使用 `defineOptions` 设置组件名称
- 优先使用 Element Plus 组件

### 状态管理

- 使用 Pinia 管理全局状态
- 按功能模块划分 Store

## 功能模块

- [组件说明](./components.md)
- [文件管理页面](./file-management.md)
