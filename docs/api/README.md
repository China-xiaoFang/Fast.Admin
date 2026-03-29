# API 文档

Fast.Admin 后端基于 Fast.NET 动态 API 框架，所有接口均支持 Swagger/Knife4j 文档自动生成。

## 接口分组

| 分组 | 前缀 | 说明 |
|------|------|------|
| File Storage | `/fileStorage` | 文件上传、下载、预览、分片上传 |
| File | `/file` | 文件管理（分页查询等） |
| Auth | `/auth` | 认证鉴权（登录、Token） |
| Admin | `/admin` | 后台管理（用户、角色、菜单等） |
| Center | `/center` | 中心服务（租户管理等） |

## 认证方式

所有需要认证的接口使用 JWT Bearer Token：

```http
Authorization: Bearer <your-jwt-token>
```

### 获取 Token

```
POST /auth/login
Content-Type: application/json

{
  "account": "admin",
  "password": "123456"
}
```

## 统一返回格式

```json
{
  "code": 200,
  "success": true,
  "data": {},
  "message": "操作成功",
  "extras": null,
  "timestamp": 1710000000000
}
```

### 错误返回

```json
{
  "code": 500,
  "success": false,
  "data": null,
  "message": "错误信息",
  "extras": null,
  "timestamp": 1710000000000
}
```

## 在线文档

启动 API 服务后访问：

- **Knife4j UI**：`http://localhost:5000/knife4j`
- **Swagger UI**：`http://localhost:5000/swagger`

## API 详情

- [文件服务 API](./file-storage.md)
- [认证鉴权 API](./authentication.md)
