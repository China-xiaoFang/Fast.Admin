# 文件服务使用指南

Fast.Admin 提供了统一的文件服务模块，支持文件上传、下载、在线预览、分片上传等功能，可直接作为 COS / OSS 文件存储服务使用。

## 功能概览

| 功能 | 说明 | 状态 |
|------|------|------|
| 普通上传 | 单文件上传（Logo、头像、证件照、富文本、通用） | ✅ |
| 分片上传 | 大文件分片上传，支持断点续传 | ✅ |
| 秒传 | 基于 MD5 哈希的文件去重，已存在文件直接返回 | ✅ |
| 文件下载 | 完整文件下载 | ✅ |
| 分片下载 | 基于 HTTP Range 的分片下载 | ✅ |
| 在线预览 | 图片、PDF、音视频等在线预览 | ✅ |
| 图片缩略图 | 自动生成 thumb/small/normal 三种尺寸 | ✅ |
| 文件去重 | MD5 哈希去重，避免重复存储 | ✅ |
| 多租户隔离 | 按租户编号隔离存储目录 | ✅ |
| 上传追踪 | 记录上传设备、系统、浏览器、IP、地理位置 | ✅ |

## 支持的文件格式

| 类型 | 格式 | MIME 类型 |
|------|------|-----------|
| 图片 | jpg, jpeg, png, gif, bmp | image/* |
| 视频 | mp4, mpeg, avi, wmv, webm, ogg | video/* |
| 音频 | mp3, wav, ogg, m4a, flac | audio/* |
| 文档 | pdf, doc, docx, xls, xlsx, ppt, pptx | application/* |
| 文本 | txt, csv, html, md | text/* |
| 压缩包 | zip, rar, 7z, gz | application/* |

## 普通上传

### 上传接口

| 接口 | 最大大小 | 支持类型 | 说明 |
|------|---------|---------|------|
| `POST /fileStorage/uploadLogo` | 2MB | 图片 | 上传 Logo |
| `POST /fileStorage/uploadAvatar` | 2MB | 图片 | 上传头像 |
| `POST /fileStorage/uploadIdPhoto` | 5MB | jpg/jpeg/png | 上传证件照 |
| `POST /fileStorage/uploadEditor` | 10MB | 图片、视频 | 上传富文本内容 |
| `POST /fileStorage/uploadFile` | 100MB | 所有类型 | 通用文件上传 |

### 请求示例

```bash
curl -X POST http://localhost:5000/fileStorage/uploadFile \
  -H "Authorization: Bearer <token>" \
  -F "file=@/path/to/file.pdf"
```

### 返回值

上传成功返回文件访问地址：

```json
{
  "code": 200,
  "data": "http://localhost:5000/fileStorage/123456789.pdf"
}
```

## 分片上传

分片上传适用于大文件场景，支持断点续传和秒传功能。

### 流程

```
1. 初始化 ──→ 2. 上传分片（并行） ──→ 3. 合并分片
      │                                      │
      └── 秒传（文件已存在）              └── 返回文件地址
```

### 步骤一：初始化分片上传

```
POST /fileStorage/initChunkUpload
Content-Type: application/json
```

**请求体：**

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

**响应：**

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

> 如果 `uploaded` 为 `true`，表示秒传成功，`fileLocation` 即为文件地址，无需后续步骤。

### 步骤二：上传分片

```
POST /fileStorage/uploadChunk
Content-Type: multipart/form-data
```

**表单字段：**

| 字段 | 类型 | 说明 |
|------|------|------|
| uploadId | string | 初始化时返回的上传标识 |
| chunkIndex | int | 分片索引（从 0 开始） |
| file | File | 分片数据 |

> 可并行上传多个分片，服务端会正确处理并发。

### 步骤三：查询上传进度（可选）

```
GET /fileStorage/getChunkUploadProgress?uploadId=a1b2c3d4e5f6...
```

**响应：**

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

### 步骤四：合并分片

```
POST /fileStorage/mergeChunk
Content-Type: application/json
```

```json
{
  "uploadId": "a1b2c3d4e5f6..."
}
```

**响应：**

```json
{
  "code": 200,
  "data": "http://localhost:5000/fileStorage/123456789.mp4"
}
```

## 文件下载

### 普通下载

```
POST /fileStorage/download
Content-Type: application/json
```

```json
{
  "fileId": 123456789
}
```

### 分片下载（Range 请求）

支持 HTTP Range 请求，用于大文件分片下载或断点续传：

```bash
# 下载前 1MB
curl -H "Range: bytes=0-1048575" \
  http://localhost:5000/fileStorage/range/123456789.mp4

# 从 1MB 处继续下载
curl -H "Range: bytes=1048576-" \
  http://localhost:5000/fileStorage/range/123456789.mp4
```

**响应头：**

```
HTTP/1.1 206 Partial Content
Content-Range: bytes 0-1048575/104857600
Content-Length: 1048576
Accept-Ranges: bytes
```

## 在线预览

### 预览地址

```
GET /fileStorage/{fileId}{suffix}
```

示例：`http://localhost:5000/fileStorage/123456789.jpg`

### 图片尺寸

支持获取不同尺寸的图片：

| 尺寸 | 宽度 | URL 格式 |
|------|------|---------|
| 原图 | 原始 | `/fileStorage/123456789.jpg` |
| thumb | 100px | `/fileStorage/123456789.jpg@!thumb` |
| small | 300px | `/fileStorage/123456789.jpg@!small` |
| normal | 600px | `/fileStorage/123456789.jpg@!normal` |

### 前端预览组件

```vue
<!-- 图片预览 -->
<el-image :src="fileLocation" :preview-src-list="[fileLocation]" />

<!-- PDF 预览 -->
<iframe :src="fileLocation" width="100%" height="600px" />

<!-- 音频播放 -->
<audio :src="fileLocation" controls />

<!-- 视频播放 -->
<video :src="fileLocation" controls width="100%" />
```

## 存储结构

文件默认存储在本地文件系统中：

```
Upload/
├── Logo/                           # Logo 文件
├── Avatar/                         # 头像文件
├── IdPhoto/                        # 证件照
├── Editor/                         # 富文本文件
│   └── 2024/01/15/                 # 按日期组织
├── Default/                        # 默认上传
│   └── {TenantNo}/                 # 按租户隔离
│       ├── image/                  # 图片
│       │   └── 2024/01/15/         # 按日期组织
│       ├── video/                  # 视频
│       ├── audio/                  # 音频
│       ├── document/               # 文档
│       ├── text/                   # 文本
│       ├── archive/                # 压缩包
│       └── other/                  # 其他
└── Chunks/                         # 分片临时目录（合并后自动清理）
```

## 配置

在 `appsettings.json` 中自定义上传配置：

```json
{
  "UploadFileSettings": {
    "Logo": {
      "Path": "Upload\\Logo",
      "MaxSize": 2048,
      "ContentType": ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"]
    },
    "Default": {
      "Path": "Upload\\Default",
      "MaxSize": 102400,
      "UseTypeFolder": true,
      "UseDateFolder": true,
      "ContentType": [
        "image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp",
        "video/mp4", "video/mpeg", "video/webm",
        "audio/mpeg", "audio/wav",
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
      ]
    }
  }
}
```

## 前端集成示例

### 普通上传

```typescript
import { fileApi } from "@/api/services/File";

const formData = new FormData();
formData.append("file", file);
const fileLocation = await fileApi.uploadFile(formData);
```

### 分片上传

```typescript
import { fileApi } from "@/api/services/File";

// 1. 初始化分片上传
const initResult = await fileApi.initChunkUpload({
  fileName: file.name,
  fileSize: file.size,
  chunkSize: 5 * 1024 * 1024, // 5MB per chunk
  totalChunks: Math.ceil(file.size / (5 * 1024 * 1024)),
  contentType: file.type,
  fileHash: await calculateMD5(file), // optional, for instant upload
});

// 2. 秒传检查
if (initResult.data.uploaded) {
  console.log("File already exists:", initResult.data.fileLocation);
  return;
}

const uploadId = initResult.data.uploadId;
const chunkSize = 5 * 1024 * 1024;

// 3. 上传分片（可并行）
for (let i = 0; i < totalChunks; i++) {
  const start = i * chunkSize;
  const end = Math.min(start + chunkSize, file.size);
  const chunk = file.slice(start, end);

  const formData = new FormData();
  formData.append("uploadId", uploadId);
  formData.append("chunkIndex", i.toString());
  formData.append("file", chunk);

  await fileApi.uploadChunk(formData);
}

// 4. 合并分片
const fileLocation = await fileApi.mergeChunk({ uploadId });
```

## 下一步

- [API 接口详情](../api/file-storage.md)
- [Web 文件管理页面](../web/file-management.md)
