# 项目架构

## 解决方案总览

Fast.Admin 采用分层架构设计，基于 DDD（领域驱动设计）思想，将项目划分为清晰的层次。

```
Fast.Admin/
├── Api.Server/                  # 后端 .NET 解决方案
│   └── src/
│       ├── Shared/              # 共享层：DTO、枚举、扩展方法
│       ├── Domain/              # 领域层：实体、枚举、核心逻辑
│       │   ├── Core/            # 核心框架（配置、中间件、过滤器）
│       │   ├── Center/          # 中心域（文件、租户等）
│       │   ├── Admin/           # 管理域（用户、角色、菜单）
│       │   ├── AdminLog/        # 管理日志域
│       │   ├── CenterLog/       # 中心日志域
│       │   ├── Gateway/         # 网关域
│       │   └── Deploy/          # 部署域
│       ├── Services/            # 服务层：业务逻辑
│       │   ├── Center/          # 中心服务（文件管理等）
│       │   ├── Admin/           # 管理服务
│       │   └── Scheduler/       # 调度服务
│       ├── Api/                 # API 入口（聚合所有模块）
│       ├── File/                # 独立文件服务入口
│       └── Scheduler/           # 定时任务入口
├── Web.Admin/                   # Vue 3 管理后台
├── App.Admin/                   # React 管理端
├── App.Client/                  # React 客户端
├── Sql/                         # 数据库脚本
└── docs/                        # 项目文档
```

## 后端架构

### 分层设计

```
┌─────────────────────────────────────────┐
│              API 入口层                  │
│  (Fast.Api / Fast.File / Fast.Scheduler) │
├─────────────────────────────────────────┤
│              服务层 (Services)            │
│  (Fast.Center.Service / Fast.Admin.Service) │
├─────────────────────────────────────────┤
│              领域层 (Domain)             │
│  Entity（实体） / Enum（枚举）           │
├─────────────────────────────────────────┤
│              核心层 (Core)               │
│  配置 / 中间件 / 过滤器 / 处理器        │
├─────────────────────────────────────────┤
│              共享层 (Shared)             │
│  DTO / 常量 / 扩展方法                  │
├─────────────────────────────────────────┤
│          Fast.NET 框架底层               │
│  DI / ORM / JWT / Swagger / 日志 / 缓存 │
└─────────────────────────────────────────┘
```

### 核心技术栈

| 技术 | 用途 | 说明 |
|------|------|------|
| Fast.NET | 框架底层 | 提供 DI、动态 API、统一返回等 |
| SqlSugar | ORM | 多数据库支持，代码优先 |
| SixLabors.ImageSharp | 图片处理 | 缩略图生成、图片裁剪 |
| MiniExcel | Excel 处理 | 高性能 Excel 导入导出 |
| JWT Bearer | 认证 | Token 认证鉴权 |
| Knife4j | API 文档 | 增强版 Swagger UI |
| SignalR | 实时通信 | WebSocket 实时消息 |
| MailKit | 邮件 | 邮件发送服务 |

### 多数据库设计

Fast.Admin 采用多数据库架构，每个域可使用独立数据库：

| 数据库 | 说明 | 核心表 |
|--------|------|--------|
| Center | 中心库 | 文件(File)、租户(Tenant) |
| Admin | 管理库 | 用户、角色、菜单、部门 |
| AdminLog | 管理日志库 | 操作日志、异常日志 |
| CenterLog | 中心日志库 | 中心操作日志 |
| Gateway | 网关库 | 路由配置 |
| Deploy | 部署库 | 部署记录 |

### 多租户架构

- 每张表均包含 `TenantId` 字段
- 通过 SqlSugar 全局过滤器自动隔离
- 支持超级管理员跨租户查询
- 文件存储按租户编号隔离目录

## 前端架构

### Web.Admin (Vue 3)

```
Web.Admin/src/
├── api/                    # API 接口封装
│   └── services/           # 按模块组织的 API 服务
├── views/                  # 页面视图
│   ├── login/              # 登录页（支持多种登录风格）
│   └── system/             # 系统管理页面
├── components/             # 公共组件
├── layouts/                # 布局组件（Classic/Horizontal/Mixed）
├── router/                 # 路由配置
├── stores/                 # Pinia 状态管理
├── styles/                 # 全局样式
├── plugins/                # 插件
└── directives/             # 自定义指令
```

**技术栈**：Vue 3 + TypeScript + Vite + Element Plus + Pinia

### 布局模式

| 模式 | 说明 |
|------|------|
| Classic | 经典侧边栏布局 |
| Horizontal | 顶部导航布局 |
| Mixed | 混合布局（顶部 + 侧边栏） |

### 登录样式

| 样式 | 说明 |
|------|------|
| classicLogin | 经典登录 |
| simpleLogin | 简约登录 |
| splitLogin | 分栏登录 |

## 下一步

- [文件服务使用指南](./file-service.md)
- [API 文档](../api/README.md)
- [Web 开发指南](../web/README.md)
