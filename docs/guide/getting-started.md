# 快速开始

本指南将帮助您在本地快速启动 Fast.Admin 项目。

## 环境要求

### 后端 (Api.Server)

| 工具 | 版本要求 | 说明 |
|------|---------|------|
| .NET SDK | 6.0+ (推荐 8.0) | [下载地址](https://dotnet.microsoft.com/download) |
| Visual Studio | 2022+ | 推荐使用，支持完整调试 |
| Visual Studio Code | 最新版 | 轻量开发可选 |
| 数据库 | MySQL 5.7+ / SQL Server 2017+ / PostgreSQL 12+ | SqlSugar 多数据库支持 |
| Redis | 6.0+ (可选) | 用于缓存，可关闭 |

### 前端 (Web.Admin)

| 工具 | 版本要求 | 说明 |
|------|---------|------|
| Node.js | 18.0+ | [下载地址](https://nodejs.org/) |
| pnpm | 8.0+ | 推荐的包管理器 |

## 获取代码

```bash
# 从 GitHub 克隆
git clone https://github.com/China-xiaoFang/Fast.Admin.git

# 或从 Gitee 克隆
git clone https://gitee.com/FastDotnet/Fast.Admin.git

cd Fast.Admin
```

## 启动后端

```bash
cd Api.Server

# 还原依赖
dotnet restore

# 编译项目
dotnet build

# 运行 API 服务（主入口）
cd src/Api
dotnet run

# 或运行独立文件服务
cd src/File
dotnet run
```

启动成功后：
- API 文档：`http://localhost:5000/knife4j`
- Swagger UI：`http://localhost:5000/swagger`

### 配置数据库

编辑 `Api.Server/src/Api/appsettings.json`，配置数据库连接：

```json
{
  "ConnectionSettings": {
    "ConnectionString": "Server=localhost;Database=FastAdmin;User=root;Password=yourpassword;"
  }
}
```

## 启动前端

```bash
cd Web.Admin

# 安装依赖
pnpm install

# 启动开发服务器
pnpm dev
```

启动成功后访问：`http://localhost:5173`

### 环境变量

编辑 `.env` 文件配置后端 API 地址：

```env
VITE_API_URL=http://localhost:5000
```

## 项目入口

| 项目 | 路径 | 说明 |
|------|------|------|
| Fast.Api | `Api.Server/src/Api/` | API 主入口（包含所有模块） |
| Fast.File | `Api.Server/src/File/` | 独立文件服务入口 |
| Web.Admin | `Web.Admin/` | Vue 3 管理后台 |
| App.Admin | `App.Admin/` | React 管理端（开发中） |
| App.Client | `App.Client/` | React 客户端（开发中） |

## 下一步

- [了解项目架构](./architecture.md)
- [使用文件服务](./file-service.md)
- [查看 API 文档](../api/README.md)
