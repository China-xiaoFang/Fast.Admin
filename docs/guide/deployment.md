# 部署指南

## 后端部署

### 使用 Docker 部署

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Api.Server/", "Api.Server/"]
RUN dotnet restore "Api.Server/src/Api/Fast.Api.csproj"
RUN dotnet build "Api.Server/src/Api/Fast.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Server/src/Api/Fast.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fast.Api.dll"]
```

### 使用 IIS 部署

1. 发布项目：
```bash
cd Api.Server
dotnet publish src/Api/Fast.Api.csproj -c Release -o ./publish
```

2. 在 IIS 中创建站点，指向 `publish` 目录
3. 确保已安装 ASP.NET Core Hosting Bundle
4. 配置应用程序池为"无托管代码"

### 使用 Nginx 反向代理

```nginx
server {
    listen 80;
    server_name api.yourdomain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        client_max_body_size 200m;
    }
}
```

> 注意：`client_max_body_size` 需根据文件上传大小限制调整。

## 前端部署

### 构建

```bash
cd Web.Admin
pnpm install
pnpm build
```

构建产物在 `dist/` 目录。

### Nginx 配置

```nginx
server {
    listen 80;
    server_name admin.yourdomain.com;
    root /var/www/fast-admin/dist;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        client_max_body_size 200m;
    }
}
```

## 环境变量

### 后端

| 变量 | 说明 | 默认值 |
|------|------|--------|
| ASPNETCORE_ENVIRONMENT | 运行环境 | Production |
| ASPNETCORE_URLS | 监听地址 | http://+:5000 |

### 前端

| 变量 | 说明 | 默认值 |
|------|------|--------|
| VITE_API_URL | API 地址 | http://localhost:5000 |

## 下一步

- [项目架构](./architecture.md)
- [文件服务配置](./file-service.md)
