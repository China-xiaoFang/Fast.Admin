# 原子样式处理 --box-shadow 变量的方案对比

## 问题背景

用户提供了一个使用 CSS 变量定义阴影的 Vue 组件，询问如何在原子样式（Atomic CSS/Utility Classes）中处理这个变量。

## 原始代码（使用 CSS 变量）

```vue
<template>
  <div class="fa__card mx-4 mb-4">
    <div class="fa__card__warp">
      <slot />
    </div>
  </div>
</template>

<style scoped lang="less">
.fa__card {
  --box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.04), 0px 2px 4px rgba(0, 0, 0, 0.08), 0px 6px 12px rgba(0, 0, 0, 0.12);
  .fa__card__warp {
    box-shadow: var(--box-shadow);
  }
}

@media (prefers-color-scheme: dark) {
  .fa__card {
    --box-shadow: 0px 1px 2px rgba(255, 255, 255, 0.06), 0px 2px 4px rgba(255, 255, 255, 0.08), 0px 6px 12px rgba(255, 255, 255, 0.12);
  }
}
</style>
```

### 原始方案的问题
1. ❌ 阴影定义在组件的 scoped 样式中，无法复用
2. ❌ 其他组件如需相同阴影，需要重复定义
3. ❌ 不符合原子化 CSS 的设计理念

## 优化方案（使用原子样式类）

### 步骤 1：在全局样式文件中定义原子样式类

在 `src/styles/common/mixin.scss` 中添加：

```scss
/* 阴影样式 - 卡片阴影，支持亮色和暗色模式 */
.fa-card-shadow {
  box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.04), 
              0px 2px 4px rgba(0, 0, 0, 0.08), 
              0px 6px 12px rgba(0, 0, 0, 0.12);
}

@media (prefers-color-scheme: dark) {
  .fa-card-shadow {
    box-shadow: 0px 1px 2px rgba(255, 255, 255, 0.06), 
                0px 2px 4px rgba(255, 255, 255, 0.08), 
                0px 6px 12px rgba(255, 255, 255, 0.12);
  }
}

/* 扩展：提供不同级别的阴影工具类 */
.fa-shadow-sm { box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05); }
.fa-shadow { box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px -1px rgba(0, 0, 0, 0.1); }
.fa-shadow-md { box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.1); }
.fa-shadow-lg { box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -4px rgba(0, 0, 0, 0.1); }
.fa-shadow-xl { box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 8px 10px -6px rgba(0, 0, 0, 0.1); }
```

### 步骤 2：在组件中使用原子样式类

```vue
<template>
  <view class="fa__card mx20 mb20">
    <view class="fa__card__warp fa-card-shadow">
      <slot />
    </view>
  </view>
</template>

<style scoped lang="scss">
.fa__card {
  .fa__card__warp {
    box-sizing: border-box;
    padding: 20rpx;
    border-radius: 30rpx;
    background-color: var(--wot-bg-color-container);
    overflow: hidden;
  }
}
</style>
```

### 优化方案的优势
1. ✅ 阴影样式可在整个项目中复用
2. ✅ 符合原子化 CSS 的设计理念
3. ✅ 类名语义化，易于理解和使用
4. ✅ 自动适配深色模式
5. ✅ 提供多种阴影级别，满足不同场景需求
6. ✅ 减少代码重复，提升维护效率

## 使用示例

### 在 FaCard 组件中使用

```vue
<FaCard title="卡片标题">
  卡片内容
</FaCard>
```

### 在其他组件中复用阴影样式

```vue
<template>
  <!-- 使用卡片阴影 -->
  <view class="custom-box fa-card-shadow">
    内容
  </view>
  
  <!-- 使用小阴影 -->
  <view class="tag fa-shadow-sm">
    标签
  </view>
  
  <!-- 使用大阴影 -->
  <view class="modal fa-shadow-xl">
    弹窗
  </view>
</template>
```

## 核心思想

将 **组件级别的 CSS 变量** 转换为 **全局原子样式类**：

| 方面 | CSS 变量方案 | 原子样式类方案 |
|------|--------------|----------------|
| 作用域 | 组件级别 | 全局可用 |
| 复用性 | 需要重复定义 | 直接复用 |
| 维护性 | 分散在各个组件中 | 集中管理 |
| 语义化 | 较差 (--box-shadow) | 较好 (fa-card-shadow) |
| 扩展性 | 每个组件独立扩展 | 统一扩展 |

## 最佳实践

1. **命名规范**：使用 `fa-` 前缀区分项目自定义的原子样式类
2. **级别划分**：提供 sm、md、lg、xl 等不同级别的工具类
3. **深色模式**：使用媒体查询自动适配深色模式
4. **集中管理**：所有原子样式类统一在 `mixin.scss` 中定义
5. **语义化**：使用有意义的类名，如 `fa-card-shadow`、`fa-shadow-sm`

## 总结

通过将 CSS 变量转换为原子样式类，我们实现了：
- 更好的代码复用
- 更清晰的样式管理
- 更灵活的使用方式
- 更符合现代 CSS 架构的设计理念
