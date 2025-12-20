# FaCard 组件 - 阴影处理方案说明

## 问题分析

原始代码使用了 CSS 变量 `--box-shadow` 来定义阴影效果，并在深色模式下通过媒体查询修改该变量的值：

```css
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
```

## 解决方案：使用原子样式类

### 1. 在 `mixin.scss` 中添加阴影工具类

在 `/src/styles/common/mixin.scss` 文件中添加了阴影相关的原子样式类：

```scss
/* 阴影样式 - 卡片阴影，支持亮色和暗色模式 */
.fa-card-shadow {
	box-shadow: 0px 1px 2px rgba(0, 0, 0, 0.04), 0px 2px 4px rgba(0, 0, 0, 0.08), 0px 6px 12px rgba(0, 0, 0, 0.12);
}

@media (prefers-color-scheme: dark) {
	.fa-card-shadow {
		box-shadow: 0px 1px 2px rgba(255, 255, 255, 0.06), 0px 2px 4px rgba(255, 255, 255, 0.08),
			0px 6px 12px rgba(255, 255, 255, 0.12);
	}
}

/* 其他通用阴影工具类 */
.fa-shadow-sm { /* 小阴影 */ }
.fa-shadow { /* 默认阴影 */ }
.fa-shadow-md { /* 中等阴影 */ }
.fa-shadow-lg { /* 大阴影 */ }
.fa-shadow-xl { /* 超大阴影 */ }
```

### 2. 在组件中使用原子样式类

在 FaCard 组件的模板中直接使用 `fa-card-shadow` 类：

```vue
<template>
  <view class="fa__card__warp fa-card-shadow">
    <slot />
  </view>
</template>
```

### 3. 同时添加了水平外边距工具类

为了支持像 `mx-4` 这样的水平边距，在 `mixin.scss` 中添加了 `mx` 系列类：

```scss
.mx#{$i_step} {
  margin-left: #{$i_step}rpx !important;
  margin-right: #{$i_step}rpx !important;
}
```

## 优势对比

### 原始方案（CSS 变量）
- ❌ 阴影值仅限于当前组件
- ❌ 其他组件需要重复定义
- ❌ 不符合原子化 CSS 的思想

### 原子样式方案
- ✅ 阴影类可在整个项目中复用
- ✅ 符合项目现有的原子化 CSS 架构
- ✅ 易于维护和扩展
- ✅ 自动适配深色模式
- ✅ 语义化的类名（如 `fa-card-shadow`, `fa-shadow-sm` 等）

## 使用示例

### 基础用法

```vue
<template>
  <FaCard title="卡片标题" :more="true" @moreClick="handleClick">
    卡片内容
  </FaCard>
</template>
```

### 使用不同的阴影样式

```vue
<template>
  <!-- 使用卡片专用阴影 -->
  <view class="fa-card-shadow">...</view>
  
  <!-- 使用小阴影 -->
  <view class="fa-shadow-sm">...</view>
  
  <!-- 使用中等阴影 -->
  <view class="fa-shadow-md">...</view>
  
  <!-- 使用大阴影 -->
  <view class="fa-shadow-lg">...</view>
</template>
```

## 文件清单

1. **组件文件**：`/src/components/card/index.vue` - FaCard 组件实现
2. **样式工具类**：`/src/styles/common/mixin.scss` - 阴影和边距原子样式类
3. **示例页面**：`/src/pages_demo/card/index.vue` - 组件使用示例

## 总结

通过将 `--box-shadow` CSS 变量转换为原子样式类 `fa-card-shadow`，我们：

1. 保持了与项目现有工具类风格的一致性（如 `mt20`, `pl20` 等）
2. 实现了样式的复用性
3. 支持了深色模式的自动适配
4. 提供了多种阴影级别供选择

这种方案更符合原子化 CSS 的设计理念，也更易于在项目中推广和使用。
