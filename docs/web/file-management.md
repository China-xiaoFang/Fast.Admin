# 文件管理页面

## 概述

文件管理页面位于 `系统管理 → 文件管理`，提供已上传文件的查看、预览和管理功能。

## 功能

- 文件列表分页查询
- 超级管理员可按租户筛选
- 图片在线预览（支持缩放、旋转）
- 文件信息展示（大小、类型、上传来源等）

## 页面源码

位置：`Web.Admin/src/views/system/file/index.vue`

### 核心代码

```vue
<template>
  <div>
    <FastTable ref="fastTableRef" tableKey="1D11KCYJJ9" rowKey="fileId" :requestApi="fileApi.queryFilePaged">
      <!-- 租户筛选（仅超管可见） -->
      <template #header v-if="userInfoStore.isSuperAdmin">
        <TenantSelectPage width="280" @change="handleTenantChange" />
      </template>

      <!-- 行操作 -->
      <template #operation="{ row }">
        <el-button v-if="isImage(row.fileMimeType)" size="small" plain @click="previewImage(row)">
          预览
        </el-button>
      </template>
    </FastTable>

    <!-- 图片预览 -->
    <el-image-viewer
      v-if="state.previewSrc"
      :urlList="[state.previewSrc]"
      hideOnClickModal
      teleported
      showProgress
      @close="state.previewSrc = ''"
    />
  </div>
</template>
```

## API 调用

```typescript
import { fileApi } from "@/api/services/File";

// 分页查询
fileApi.queryFilePaged({ pageIndex: 1, pageSize: 20 });

// 下载文件
fileApi.download({ fileId: 123456789 });

// 上传文件
const formData = new FormData();
formData.append("file", file);
fileApi.uploadFile(formData);
```

## 文件预览方式

| 文件类型 | 预览方式 |
|---------|---------|
| 图片 (jpg/png/gif/bmp) | `el-image-viewer` 组件预览 |
| PDF | 浏览器内置 PDF 查看器 / `<iframe>` |
| 音频 (mp3/wav/ogg) | `<audio>` 标签播放 |
| 视频 (mp4/webm) | `<video>` 标签播放 |
| Office 文档 | 通过预览地址直接访问 |

## 扩展

如需自定义文件管理功能，可修改 `Web.Admin/src/views/system/file/index.vue`，或参考 [文件服务 API 文档](../api/file-storage.md) 调用后端接口。
