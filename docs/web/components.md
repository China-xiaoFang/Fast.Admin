# 内置组件说明

## FastTable

核心表格组件，封装了分页、搜索、排序等功能。

### 基本用法

```vue
<FastTable
  ref="fastTableRef"
  tableKey="uniqueKey"
  rowKey="id"
  :requestApi="api.queryPaged"
>
  <template #header>
    <!-- 搜索区域 -->
  </template>
  <template #operation="{ row }">
    <!-- 操作按钮 -->
  </template>
</FastTable>
```

### Props

| 属性 | 类型 | 说明 |
|------|------|------|
| tableKey | string | 表格唯一标识（用于列配置持久化） |
| rowKey | string | 行数据主键字段 |
| requestApi | Function | 分页查询 API 方法 |

### Slots

| 插槽 | 说明 |
|------|------|
| header | 表格头部搜索/操作区域 |
| operation | 行操作按钮 |

## TenantSelectPage

租户选择器组件，用于超级管理员筛选数据。

### 用法

```vue
<TenantSelectPage
  width="280"
  @change="(value) => handleTenantChange(value)"
/>
```

## el-image-viewer

Element Plus 图片查看器，支持缩放、旋转等操作。

```vue
<el-image-viewer
  v-if="previewSrc"
  :urlList="[previewSrc]"
  hideOnClickModal
  teleported
  showProgress
  @close="previewSrc = ''"
/>
```
