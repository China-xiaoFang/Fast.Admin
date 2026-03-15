# 文件服务 API

## 接口列表

### 普通上传

| 方法 | 接口 | 说明 | 认证 |
|------|------|------|------|
| POST | `/fileStorage/uploadLogo` | 上传 Logo | ✅ |
| POST | `/fileStorage/uploadAvatar` | 上传头像 | ✅ |
| POST | `/fileStorage/uploadIdPhoto` | 上传证件照 | ✅ |
| POST | `/fileStorage/uploadEditor` | 上传富文本内容 | ✅ |
| POST | `/fileStorage/uploadFile` | 通用文件上传 | ✅ |

### 分片上传

| 方法 | 接口 | 说明 | 认证 |
|------|------|------|------|
| POST | `/fileStorage/initChunkUpload` | 初始化分片上传 | ✅ |
| POST | `/fileStorage/uploadChunk` | 上传分片 | ✅ |
| GET | `/fileStorage/getChunkUploadProgress` | 获取上传进度 | ✅ |
| POST | `/fileStorage/mergeChunk` | 合并分片 | ✅ |

### 下载 & 预览

| 方法 | 接口 | 说明 | 认证 |
|------|------|------|------|
| POST | `/fileStorage/download` | 下载文件 | ✅ |
| GET | `/fileStorage/range/{fileName}` | 分片下载（Range） | ❌ |
| GET | `/fileStorage/{fileName}` | 预览文件 | ❌ |
| GET | `/fileStorage/{fileName}@!{size}` | 预览指定尺寸图片 | ❌ |

### 文件管理

| 方法 | 接口 | 说明 | 认证 |
|------|------|------|------|
| POST | `/file/queryFilePaged` | 获取文件分页列表 | ✅ |

---

## 接口详情

### POST /fileStorage/uploadFile

上传文件（通用）。

**请求**：`multipart/form-data`

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| file | File | ✅ | 文件内容 |

**响应**：

```json
{
  "code": 200,
  "data": "http://localhost:5000/fileStorage/123456789.pdf"
}
```

**限制**：
- 最大 100MB
- 支持类型：图片、视频、音频、文档、文本、压缩包

---

### POST /fileStorage/initChunkUpload

初始化分片上传任务。

**请求**：`application/json`

```json
{
  "fileName": "large-video.mp4",
  "fileSize": 104857600,
  "chunkSize": 5242880,
  "totalChunks": 20,
  "contentType": "video/mp4",
  "fileHash": "d41d8cd98f00b204e9800998ecf8427e"
}
```

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| fileName | string | ✅ | 文件名称 |
| fileSize | long | ✅ | 文件总大小（字节） |
| chunkSize | long | ✅ | 每个分片大小（字节） |
| totalChunks | int | ✅ | 总分片数 |
| contentType | string | ✅ | 文件 MIME 类型 |
| fileHash | string | ❌ | 文件 MD5 哈希（用于秒传） |

**响应**：

```json
{
  "code": 200,
  "data": {
    "uploadId": "a1b2c3d4e5f6...",
    "uploaded": false,
    "fileLocation": null
  }
}
```

| 字段 | 说明 |
|------|------|
| uploadId | 上传任务标识 |
| uploaded | 是否秒传成功 |
| fileLocation | 秒传成功时返回的文件地址 |

---

### POST /fileStorage/uploadChunk

上传单个分片。

**请求**：`multipart/form-data`

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| uploadId | string | ✅ | 上传任务标识 |
| chunkIndex | int | ✅ | 分片索引（从 0 开始） |
| file | File | ✅ | 分片数据 |

**响应**：

```json
{
  "code": 200,
  "data": null,
  "message": "操作成功"
}
```

---

### GET /fileStorage/getChunkUploadProgress

获取分片上传进度。

**参数**：

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| uploadId | string | ✅ | 上传任务标识 |

**响应**：

```json
{
  "code": 200,
  "data": {
    "uploadId": "a1b2c3d4e5f6...",
    "fileName": "large-video.mp4",
    "totalChunks": 20,
    "uploadedChunks": [0, 1, 2, 3, 4],
    "isComplete": false
  }
}
```

---

### POST /fileStorage/mergeChunk

合并所有分片，生成最终文件。

**请求**：`application/json`

```json
{
  "uploadId": "a1b2c3d4e5f6..."
}
```

**响应**：

```json
{
  "code": 200,
  "data": "http://localhost:5000/fileStorage/123456789.mp4"
}
```

---

### GET /fileStorage/range/{fileName}

支持 HTTP Range 请求的分片下载。

**请求头**：

```http
Range: bytes=0-1048575
```

**响应**：

```
HTTP/1.1 206 Partial Content
Content-Range: bytes 0-1048575/104857600
Accept-Ranges: bytes
Content-Length: 1048576
```

---

### POST /fileStorage/download

下载文件。

**请求**：`application/json`

```json
{
  "fileId": 123456789
}
```

**响应**：文件二进制流，附带 `Content-Disposition` 头。

---

### GET /fileStorage/{fileName}

预览文件。

**示例**：
- 原图：`/fileStorage/123456789.jpg`
- 缩略图：`/fileStorage/123456789.jpg@!thumb`
- 小图：`/fileStorage/123456789.jpg@!small`
- 中图：`/fileStorage/123456789.jpg@!normal`

**响应**：文件二进制流，图片类型附带 `Cache-Control: public,max-age=31536000` 缓存头。

---

### POST /file/queryFilePaged

获取文件分页列表。

**请求**：`application/json`

```json
{
  "pageIndex": 1,
  "pageSize": 20,
  "tenantId": null,
  "isOrderBy": true
}
```

**响应**：

```json
{
  "code": 200,
  "data": {
    "pageIndex": 1,
    "pageSize": 20,
    "totalCount": 100,
    "totalPages": 5,
    "items": [
      {
        "fileId": 123456789,
        "fileObjectName": "123456789.jpg",
        "fileOriginName": "photo.jpg",
        "fileSuffix": "jpg",
        "fileMimeType": "image/jpeg",
        "fileSizeKb": 1024,
        "filePath": "Upload/Default/image/2024/01/15",
        "fileLocation": "http://localhost:5000/fileStorage/123456789.jpg",
        "fileHash": "d41d8cd98f00b204e9800998ecf8427e",
        "uploadDevice": "PC",
        "uploadOS": "Windows 10",
        "uploadBrowser": "Chrome 120",
        "uploadProvince": "广东",
        "uploadCity": "深圳",
        "uploadIp": "192.168.1.1",
        "createdUserName": "管理员",
        "createdTime": "2024-01-15T10:30:00",
        "tenantName": "默认租户"
      }
    ]
  }
}
```
