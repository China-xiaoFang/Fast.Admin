# Fast Deploy — 轻量级自动化发布系统

Fast Deploy 是一个基于 Agent 架构的轻量级发布系统，支持 .NET 和 Vue 项目在 Windows / Linux 的自动化部署。

## 系统架构

```
┌─────────────────────────────────────────────────────┐
│                  管理端 (Vue3 前端)                   │
│   应用管理 | 版本管理 | 节点管理 | 发布管理 | 日志    │
└───────────────────────┬─────────────────────────────┘
                        │ HTTP / WebSocket (SignalR)
┌───────────────────────▼─────────────────────────────┐
│              Fast.Deploy.Server (.NET 10)            │
│  REST API + SignalR Hub + EF Core SQLite             │
│  单机 / 滚动 / 蓝绿 发布编排 + 回滚                 │
└────────────┬──────────────────────┬──────────────────┘
             │ HTTP                 │ HTTP
  ┌──────────▼──────┐     ┌─────────▼────────┐
  │ Agent (节点 1)  │     │  Agent (节点 N)  │
  │ Linux / Windows │     │ Linux / Windows  │
  │ 下载包 → 解压   │     │ 下载包 → 解压    │
  │ start/stop 脚本 │     │ start/stop 脚本  │
  └─────────────────┘     └──────────────────┘
```

## 功能特性

- ✅ **应用管理** — .NET / Vue 应用的增删改查
- ✅ **版本管理** — 包上传（.zip / .tar.gz / .tgz）、激活、版本历史
- ✅ **节点管理** — Agent 节点注册、心跳检测、在线状态
- ✅ **三种发布策略**
  - 单机部署：发布到单一节点
  - 滚动发布：逐节点滚动，每步健康检查通过后才推进
  - 蓝绿发布：先部署 Green 组，健康检查通过后切流量，再部署 Blue 组
- ✅ **零停机发布** — 健康检查 URL + 流量切换
- ✅ **一键回滚** — 自动找到上一次成功的版本并重新部署
- ✅ **实时日志** — SignalR 推送，终端风格查看
- ✅ **跨平台** — Windows / Linux Agent，自动检测 OS 执行对应脚本

## 目录结构

```
Deploy/
├── backend/                  # .NET 10 后端
│   ├── Fast.Deploy.slnx
│   └── src/
│       ├── Fast.Deploy.Shared/   # 枚举 + 模型 + DTOs
│       ├── Fast.Deploy.Server/   # 管理 API + SignalR
│       └── Fast.Deploy.Agent/    # 节点 Agent
├── frontend/                 # Vue3 + Vite 前端
│   └── src/
│       ├── views/            # 页面（应用/版本/节点/发布/日志）
│       ├── api/              # Axios API 层
│       ├── stores/           # Pinia Store
│       └── router/           # Vue Router
├── scripts/
│   ├── windows/              # start.bat / stop.bat / health.bat
│   └── linux/                # start.sh / stop.sh / health.sh
└── README.md
```

## 快速开始

### 1. 启动 Server

```bash
cd Deploy/backend/src/Fast.Deploy.Server
# 编辑 appsettings.json，设置 AgentToken 和 ServerUrl
dotnet run
# 默认监听: http://0.0.0.0:5200
```

### 2. 在每个部署节点启动 Agent

```bash
cd Deploy/backend/src/Fast.Deploy.Agent
# 编辑 appsettings.json，设置与 Server 一致的 AgentToken
dotnet run
# 默认监听: http://0.0.0.0:5201
```

### 3. 启动前端

```bash
cd Deploy/frontend
pnpm install
pnpm dev
# 访问: http://localhost:3200
```

## 部署包规范

包内建议包含以下脚本（Agent 会自动检测并执行）：

| 文件 | 说明 |
|------|------|
| `start.bat` / `start.sh` | 启动应用 |
| `stop.bat` / `stop.sh` | 停止应用 |

参考 `Deploy/scripts/` 目录中的模板。若包内不含脚本，Agent 会自动尝试查找并运行 `.dll` 文件（.NET 应用）。

## API 端点

| 方法 | 路径 | 说明 |
|------|------|------|
| GET | `/api/apps` | 应用列表 |
| POST | `/api/apps` | 创建应用 |
| PUT | `/api/apps/{id}` | 更新应用 |
| DELETE | `/api/apps/{id}` | 删除应用 |
| GET | `/api/apps/{appId}/versions` | 版本列表 |
| POST | `/api/apps/{appId}/versions/upload` | 上传版本包 |
| POST | `/api/apps/{appId}/versions/{id}/activate` | 激活版本 |
| DELETE | `/api/versions/{id}` | 删除版本 |
| GET | `/api/nodes` | 节点列表 |
| POST | `/api/nodes` | 注册节点 |
| POST | `/api/nodes/{id}/heartbeat` | 心跳 |
| DELETE | `/api/nodes/{id}` | 删除节点 |
| GET | `/api/deployments` | 部署列表 |
| POST | `/api/deployments` | 发起部署 |
| POST | `/api/deployments/{id}/rollback` | 回滚 |
| GET | `/api/deployments/{id}/logs` | 日志列表 |
| WebSocket | `/hubs/deploy` | SignalR 实时日志 |

## Agent API

| 方法 | 路径 | 说明 |
|------|------|------|
| POST | `/api/agent/deploy` | 接收部署指令 |
| GET | `/api/agent/health?url=...` | 健康检查 |

## 技术栈

- **后端**: .NET 10, ASP.NET Core, EF Core + SQLite, SignalR
- **前端**: Vue 3.5, Vite, Element Plus, Pinia, @microsoft/signalr
- **通信**: HTTP REST + WebSocket (SignalR)
- **平台**: Windows / Linux
