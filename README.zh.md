**中** | [En](https://github.com/China-xiaoFang/Fast.Admin)

<p align="center">
  <img src="Fast.png" alt="Fast.Admin Logo" width="120" />
</p>

<h1 align="center">Fast.Admin</h1>

<p align="center">
  集百家所长的 <strong>.NET Web API</strong> 快速开发框架 — 开箱即用、紧随前沿技术。
</p>

<p align="center">
  <a href="https://gitee.com/FastDotnet/Fast.Admin/blob/master/LICENSE">
    <img src="https://img.shields.io/badge/许可证-Apache%202.0-blue.svg" alt="许可证" />
  </a>
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET-6.0%2B-purple.svg" alt=".NET" />
  </a>
  <a href="https://vuejs.org/">
    <img src="https://img.shields.io/badge/Vue-3.x-green.svg" alt="Vue" />
  </a>
</p>

---

## ✨ 项目特点

- **🚀 开箱即用** — 功能齐全的管理后台框架，无需额外配置即可快速构建项目。
- **📦 统一文件服务** — 上传、下载、在线预览、分片上传/下载（断点续传）、秒传（哈希去重）。
- **🎨 多端支持** — Vue 3 管理后台（Web.Admin）、React 管理端（App.Admin）、React 客户端（App.Client）。
- **🏢 多租户架构** — 完整的租户隔离，按租户独立存储和数据过滤。
- **🔐 RBAC 权限** — 基于角色的访问控制，JWT 认证鉴权。
- **📊 多数据库** — 中心库、管理库、日志库、网关库、部署库独立管理（SqlSugar ORM）。
- **🔌 模块化设计** — 基于 DDD 的清晰分层架构，各领域独立管理。
- **📄 丰富的文件格式** — 支持图片、PDF、Word、Excel、PowerPoint、音频、视频、压缩包。
- **🖼️ 图片处理** — 自动生成缩略图（thumb/small/normal），基于 SixLabors.ImageSharp。
- **📡 实时通信** — 基于 SignalR 的实时消息推送。
- **📝 API 文档** — Swagger + Knife4j UI 交互式 API 文档。

## 🏗️ 项目架构

```
Fast.Admin/
├── Api.Server/              # .NET 后端服务
│   └── src/
│       ├── Api/             # API 主入口（聚合所有模块）
│       ├── File/            # 独立文件服务入口
│       ├── Domain/          # 领域层（实体、枚举、核心逻辑）
│       ├── Services/        # 服务层（业务逻辑）
│       ├── Shared/          # 共享层（DTO、工具类）
│       └── Scheduler/       # 定时任务调度
├── Web.Admin/               # Vue 3 管理后台
├── App.Admin/               # React 管理端
├── App.Client/              # React 客户端
├── docs/                    # 项目文档
└── Sql/                     # 数据库脚本
```

## 📂 统一文件服务

统一文件服务模块，可直接作为 COS / OSS 文件存储服务使用：

| 功能 | 说明 |
|------|------|
| 普通上传 | 支持 Logo、头像、证件照、富文本、通用文件上传 |
| **分片上传** | 大文件分片上传，支持并行上传和断点续传 |
| **秒传** | 基于 MD5 哈希去重，已存在文件直接返回 |
| 普通下载 | 完整文件下载，保留原始文件名 |
| **分片下载** | 基于 HTTP Range 的分片下载，支持断点续传 |
| 在线预览 | 浏览器直接预览图片、PDF、音频、视频 |
| 图片缩略图 | 自动生成 thumb(100px)、small(300px)、normal(600px) 三种尺寸 |

### 支持的文件格式

| 类型 | 格式 |
|------|------|
| 图片 | JPG, JPEG, PNG, GIF, BMP |
| 视频 | MP4, MPEG, AVI, WMV, WebM, OGG |
| 音频 | MP3, WAV, OGG, M4A, FLAC |
| 文档 | PDF, DOC, DOCX, XLS, XLSX, PPT, PPTX |
| 文本 | TXT, CSV, HTML, Markdown |
| 压缩包 | ZIP, RAR, 7Z, GZIP |

> 详细使用指南请查看 [文件服务文档](docs/guide/file-service.md)。

## 🚀 快速开始

### 环境要求

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)（推荐 8.0）
- [Node.js 18+](https://nodejs.org/) & [pnpm 8+](https://pnpm.io/)
- MySQL 5.7+ / SQL Server 2017+ / PostgreSQL 12+

### 启动后端

```bash
cd Api.Server
dotnet restore
dotnet build
cd src/Api
dotnet run
```

API 文档访问：`http://localhost:5000/knife4j`

### 启动前端

```bash
cd Web.Admin
pnpm install
pnpm dev
```

管理后台访问：`http://localhost:5173`

> 详细说明请查看 [快速开始指南](docs/guide/getting-started.md)。

## 📖 文档

| 文档 | 说明 |
|------|------|
| [快速开始](docs/guide/getting-started.md) | 环境准备与首次运行 |
| [项目架构](docs/guide/architecture.md) | 解决方案结构与设计说明 |
| [文件服务](docs/guide/file-service.md) | 上传/下载/预览完整指南 |
| [API 文档](docs/api/README.md) | 后端接口文档 |
| [Web.Admin 指南](docs/web/README.md) | 前端开发指南 |
| [部署指南](docs/guide/deployment.md) | 生产环境部署 |

## 🌿 分支说明

| 分支 | 说明 | 环境 |
|------|------|------|
| `master` | 主分支，稳定版本 | 生产环境 |
| `develop` | 开发分支，快速迭代（未经测试） | 开发环境 |

> 如需 Fork 或修改，请拉取 `master` 分支代码。

## 🛠️ 技术栈

### 后端
- **框架**：[Fast.NET](https://gitee.com/FastDotnet/Fast.NET) + .NET 6/7/8
- **ORM**：SqlSugar（多数据库支持）
- **认证**：JWT Bearer Token
- **API 文档**：Swagger + Knife4j UI
- **图片处理**：SixLabors.ImageSharp
- **Excel**：MiniExcel
- **实时通信**：SignalR
- **缓存**：Redis（可选）

### 前端
- **Web.Admin**：Vue 3 + TypeScript + Vite + Element Plus + Pinia
- **App.Admin / App.Client**：React + TypeScript + Vite

## 📝 更新日志

[查看更新日志](https://gitee.com/FastDotnet/Fast.Admin/commits/master)

## 📄 开源协议

[Fast.Admin](https://gitee.com/FastDotnet/Fast.Admin) 遵循 [Apache-2.0](LICENSE) 开源协议，欢迎大家提交 `PR` 或 `Issue`。

```
Apache开源许可证

版权所有 © 2018-Now 小方

许可授权：
本协议授予任何获得本软件及其相关文档（以下简称"软件"）副本的个人或组织。
在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
3.修改或衍生作品须明确标注原作者及原软件出处。

特别声明：
- 本软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
- 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
- 包括但不限于数据丢失、业务中断等情况。

免责条款：
禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
```

## 👥 团队成员

| 成员 | 技术 | 昵称 | 座右铭 |
|------|------|------|--------|
| 小方 | 全栈 | 1.8K 仔 | 接受自己的平庸和普通，是成长的必修课<br>你羡慕的生活都是你没熬过的苦<br>当你的能力还撑不起你的野心时，你就需要静下心来好好学习 |

## 💻 编码环境

| 名称 | 备注 |
|------|------|
| Visual Studio 2022 | 推荐 IDE |
| Visual Studio Code | 轻量开发可选 |
| ReSharper | 代码分析工具（代码中的 `// ReSharper` 注释由此应用生成） |

## ⚠️ 免责申明

请勿用于违反我国法律的项目上。对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。

## ⭐ 支持

如果对您有帮助，您可以点右上角 **Star** 收藏一下，获取第一时间更新，谢谢！
