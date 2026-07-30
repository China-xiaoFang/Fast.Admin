[中](https://gitee.com/FastDotnet/Fast.Admin) | **En**

<p align="center">
  <img src="Fast.png" alt="Fast.Admin Logo" width="120" />
</p>

<h1 align="center">Fast.Admin</h1>

<p align="center">
  A rapid development framework for <strong>.NET Web API</strong> applications — combining best practices, out-of-the-box features, and cutting-edge technology.
</p>

<p align="center">
  <a href="https://gitee.com/FastDotnet/Fast.Admin/blob/master/LICENSE">
    <img src="https://img.shields.io/badge/License-Apache%202.0-blue.svg" alt="License" />
  </a>
  <a href="https://dotnet.microsoft.com/">
    <img src="https://img.shields.io/badge/.NET-6.0%2B-purple.svg" alt=".NET" />
  </a>
  <a href="https://vuejs.org/">
    <img src="https://img.shields.io/badge/Vue-3.x-green.svg" alt="Vue" />
  </a>
</p>

---

## ✨ Features

- **🚀 Out-of-the-Box** — Fully functional admin framework, no additional configuration needed.
- **📦 Unified File Service** — Upload, download, online preview, chunked upload/download with resume support, instant upload (hash dedup).
- **🎨 Multiple Frontends** — Vue 3 admin panel (Web.Admin), React admin (App.Admin), React client (App.Client).
- **🏢 Multi-Tenant** — Complete tenant isolation with per-tenant storage and data filtering.
- **🔐 RBAC** — Role-based access control with JWT authentication.
- **📊 Multi-Database** — Separate databases for Center, Admin, Logs, Gateway, Deploy (SqlSugar ORM).
- **🔌 Modular** — Clean DDD-based layered architecture, each domain independently managed.
- **📄 Rich Document Support** — Image, PDF, Word, Excel, PowerPoint, audio, video, archive file handling.
- **🖼️ Image Processing** — Automatic thumbnail generation (thumb/small/normal) via SixLabors.ImageSharp.
- **📡 Real-Time** — SignalR-based real-time communication.
- **📝 API Documentation** — Swagger + Knife4j UI for interactive API exploration.

## 🏗️ Architecture

```
Fast.Admin/
├── Api.Server/              # .NET Backend
│   └── src/
│       ├── Api/             # Main API entry point
│       ├── File/            # Standalone file service
│       ├── Domain/          # Domain layer (entities, enums, core)
│       ├── Services/        # Business logic layer
│       ├── Shared/          # Shared DTOs and utilities
│       └── Scheduler/       # Background job scheduler
├── Web.Admin/               # Vue 3 Admin Panel
├── App.Admin/               # React Admin App
├── App.Client/              # React Client App
├── docs/                    # Documentation
└── Sql/                     # Database scripts
```

## 📂 File Service

The unified file service module supports:

| Feature | Description |
|---------|-------------|
| Standard Upload | Single file upload for Logo, Avatar, ID Photo, Editor, and general files |
| **Chunked Upload** | Large file upload with chunk splitting, parallel upload, and resume capability |
| **Instant Upload** | MD5 hash-based deduplication — skip upload if file already exists |
| Standard Download | Full file download with original filename |
| **Range Download** | HTTP Range-based partial download for large files |
| Online Preview | Direct file preview in browser (images, PDF, audio, video) |
| Image Thumbnails | Auto-generated thumb (100px), small (300px), normal (600px) variants |

### Supported Formats

| Type | Formats |
|------|---------|
| Images | JPG, JPEG, PNG, GIF, BMP |
| Videos | MP4, MPEG, AVI, WMV, WebM, OGG |
| Audio | MP3, WAV, OGG, M4A, FLAC |
| Documents | PDF, DOC, DOCX, XLS, XLSX, PPT, PPTX |
| Text | TXT, CSV, HTML, Markdown |
| Archives | ZIP, RAR, 7Z, GZIP |

> See [File Service Documentation](docs/guide/file-service.md) for complete API usage guide.

## 🚀 Quick Start

### Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/download) (8.0 recommended)
- [Node.js 18+](https://nodejs.org/) & [pnpm 8+](https://pnpm.io/)
- MySQL 5.7+ / SQL Server 2017+ / PostgreSQL 12+

### Backend

```bash
cd Api.Server
dotnet restore
dotnet build
cd src/Api
dotnet run
```

Access API docs at: `http://localhost:5000/knife4j`

### Frontend

```bash
cd Web.Admin
pnpm install
pnpm dev
```

Access the admin panel at: `http://localhost:5173`

> See [Getting Started Guide](docs/guide/getting-started.md) for detailed setup instructions.

## 📖 Documentation

| Document | Description |
|----------|-------------|
| [Getting Started](docs/guide/getting-started.md) | Environment setup and first run |
| [Architecture](docs/guide/architecture.md) | Project structure and design |
| [File Service](docs/guide/file-service.md) | File upload/download/preview guide |
| [API Reference](docs/api/README.md) | Backend API documentation |
| [Web.Admin Guide](docs/web/README.md) | Frontend development guide |
| [Deployment](docs/guide/deployment.md) | Production deployment |

## 🌿 Branch Strategy

| Branch | Description | Environment |
|--------|-------------|-------------|
| `master` | Stable release | Production |
| `develop` | Active development (unstable) | Development |

> For Fork or modifications, use the `master` branch.

## 🛠️ Tech Stack

### Backend
- **Framework**: [Fast.NET](https://gitee.com/FastDotnet/Fast.NET) + .NET 6/7/8
- **ORM**: SqlSugar (multi-database support)
- **Authentication**: JWT Bearer
- **API Docs**: Swagger + Knife4j UI
- **Image Processing**: SixLabors.ImageSharp
- **Excel**: MiniExcel
- **Real-Time**: SignalR
- **Caching**: Redis (optional)

### Frontend
- **Web.Admin**: Vue 3 + TypeScript + Vite + Element Plus + Pinia
- **App.Admin / App.Client**: React + TypeScript + Vite

## 📝 Changelog

[View Changelog](https://gitee.com/FastDotnet/Fast.Admin/commits/master)

## 📄 License

[Fast.Admin](https://gitee.com/FastDotnet/Fast.Admin) is licensed under [Apache-2.0](LICENSE).

```
Apache License 2.0

Copyright © 2018-Now xiaoFang

Licensed under the Apache License, Version 2.0.
You may use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, subject to the following conditions:

1. All copies must retain this copyright notice and license.
2. Usage must comply with applicable laws and not infringe others' rights.
3. Modified works must clearly indicate the original author and source.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND.
```

## 👥 Team

| Member | Role | Nickname | Motto |
|--------|------|----------|-------|
| 小方 | Full Stack | 1.8K 仔 | Accepting your own mediocrity and ordinariness is a required course for growth |

## 💻 Development Environment

| Tool | Notes |
|------|-------|
| Visual Studio 2022 | Recommended IDE |
| Visual Studio Code | Lightweight alternative |
| ReSharper | Code analysis (comments with `// ReSharper` are auto-generated) |

## ⚠️ Disclaimer

Please do not use this software for projects that violate any national laws or regulations. The author assumes no liability for any legal disputes arising from secondary development.

## ⭐ Support

If this project helps you, please click **Star** in the upper right corner to stay updated. Thank you!

