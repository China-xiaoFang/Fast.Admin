# 认证鉴权 API

## 概述

Fast.Admin 使用 JWT Bearer Token 进行认证鉴权。

## 接口列表

| 方法 | 接口 | 说明 | 认证 |
|------|------|------|------|
| POST | `/auth/login` | 用户登录 | ❌ |
| POST | `/auth/logout` | 用户登出 | ✅ |
| GET | `/auth/userInfo` | 获取当前用户信息 | ✅ |

## 认证流程

```
1. 登录 ──→ 获取 JWT Token
2. 请求 ──→ Header 携带 Token
3. 服务端 ──→ 校验 Token 有效性
4. 通过 ──→ 返回数据
```

## Token 使用

在需要认证的接口请求头中添加：

```http
Authorization: Bearer <your-jwt-token>
```

## 权限控制

Fast.Admin 使用基于角色的权限控制（RBAC）：

- **超级管理员**：拥有所有权限
- **管理员**：拥有当前租户下的管理权限
- **普通用户**：仅拥有分配的权限

权限通过 `[Permission]` 特性标注在接口方法上。

> 详细接口请参考在线 Swagger/Knife4j 文档。
