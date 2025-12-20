# FaCard 组件与原子样式阴影工具类

## 📋 概述

本次更新为 Fast.Admin 项目的 App.Admin 模块添加了 FaCard 组件，并实现了一套完整的原子样式阴影工具类系统。

## 🎯 解决的问题

**原始问题**：如何将组件中的 CSS 变量 `--box-shadow` 转换为原子样式（Atomic CSS）的形式？

**解决方案**：将组件级别的 CSS 变量转换为全局可复用的原子样式工具类，使其符合项目现有的工具类架构（如 `mt20`、`pl20` 等）。

## 📦 新增内容

### 1. FaCard 组件
**位置**：`src/components/card/index.vue`

一个通用的卡片组件，支持：
- 自定义标题
- 可选的"更多"按钮
- 自动适配深色模式的阴影效果
- 图表容器样式支持

### 2. 阴影工具类
**位置**：`src/styles/common/mixin.scss`

新增 6 种阴影工具类：

| 类名 | 用途 | 阴影强度 |
|------|------|----------|
| `fa-card-shadow` | 卡片专用阴影 | 中等 |
| `fa-shadow-sm` | 小组件阴影 | 最弱 |
| `fa-shadow` | 默认阴影 | 弱 |
| `fa-shadow-md` | 中等阴影 | 中等 |
| `fa-shadow-lg` | 大组件阴影 | 强 |
| `fa-shadow-xl` | 弹窗/模态框阴影 | 最强 |

所有阴影类都自动适配深色模式 ✨

### 3. 增强的间距工具类

新增 4 种双向间距工具类：

| 类名 | 说明 | 示例 |
|------|------|------|
| `mx{step}` | 水平外边距 | `mx20` = 左右各 20rpx |
| `my{step}` | 垂直外边距 | `my30` = 上下各 30rpx |
| `px{step}` | 水平内边距 | `px15` = 左右各 15rpx |
| `py{step}` | 垂直内边距 | `py25` = 上下各 25rpx |

步长范围：5rpx - 100rpx（5 的倍数）

### 4. 示例页面
**位置**：`src/pages_demo/card/index.vue`

展示了 FaCard 组件和所有阴影工具类的实际效果。

### 5. 文档
- **CARD_SHADOW_SOLUTION.md** - 阴影处理方案详解
- **ATOMIC_CSS_GUIDE.md** - 原子样式完整指南（含对比）
- **IMPLEMENTATION_SUMMARY.md** - 实现总结

## 🚀 快速开始

### 使用 FaCard 组件

```vue
<template>
  <FaCard title="用户信息" :more="true" @moreClick="handleMoreClick">
    <view class="fa-page__card">
      <view class="page__row">
        <view class="title">姓名：</view>
        <view class="content">张三</view>
      </view>
      <view class="page__row">
        <view class="title">邮箱：</view>
        <view class="content">zhangsan@example.com</view>
      </view>
    </view>
  </FaCard>
</template>

<script setup lang="ts">
const handleMoreClick = () => {
  console.log('点击了更多按钮');
};
</script>
```

### 使用阴影工具类

```vue
<template>
  <!-- 卡片阴影 -->
  <view class="product-card fa-card-shadow">
    商品信息
  </view>
  
  <!-- 小阴影 - 适合标签 -->
  <view class="tag fa-shadow-sm">
    热门
  </view>
  
  <!-- 大阴影 - 适合弹窗 -->
  <view class="modal fa-shadow-xl">
    确认删除吗？
  </view>
</template>
```

### 使用间距工具类

```vue
<template>
  <!-- 水平外边距 -->
  <view class="mx20">左右外边距 20rpx</view>
  
  <!-- 垂直内边距 + 小阴影 -->
  <view class="py30 fa-shadow-sm">上下内边距 30rpx</view>
  
  <!-- 组合使用 -->
  <view class="mx15 my20 px25 py15 fa-card-shadow">
    组合间距和阴影
  </view>
</template>
```

## 🎨 设计理念

### 原子化 CSS
每个工具类只做一件事，通过组合使用实现复杂效果。

### 语义化命名
- `fa-` 前缀：表示 Fast 项目的自定义类
- `shadow-sm/md/lg/xl`：表示阴影强度级别
- `mx/my/px/py`：表示方向（x=水平，y=垂直）

### 自动适配
所有阴影工具类通过媒体查询自动适配深色模式，无需额外代码。

### 渐进增强
在原有 `mt/mr/mb/ml/pt/pr/pb/pl` 基础上增加双向间距类，保持向后兼容。

## 📊 技术栈

- **框架**：uni-app
- **UI 库**：wot-design-uni
- **样式**：SCSS
- **类型**：TypeScript

## 🔄 兼容性

- ✅ 小程序（微信、支付宝等）
- ✅ H5
- ✅ App（iOS、Android）
- ✅ 亮色/深色模式自动切换

## 📝 最佳实践

1. **优先使用工具类**：能用工具类的场景，避免编写自定义样式
2. **组合使用**：通过组合多个工具类实现复杂效果
3. **语义优先**：选择最符合语义的阴影级别（如卡片用 `fa-card-shadow`）
4. **保持一致**：整个项目使用统一的阴影和间距标准

## 🔗 相关文档

- [阴影处理方案详解](./CARD_SHADOW_SOLUTION.md)
- [原子样式完整指南](./ATOMIC_CSS_GUIDE.md)
- [实现总结](./IMPLEMENTATION_SUMMARY.md)

## 📄 文件清单

```
App.Admin/
├── src/
│   ├── components/
│   │   └── card/
│   │       └── index.vue              # FaCard 组件
│   ├── styles/
│   │   └── common/
│   │       └── mixin.scss             # 工具类（含阴影和间距）
│   └── pages_demo/
│       └── card/
│           └── index.vue              # 使用示例
├── CARD_SHADOW_SOLUTION.md            # 方案详解
├── ATOMIC_CSS_GUIDE.md                # 完整指南
├── IMPLEMENTATION_SUMMARY.md          # 实现总结
└── README_CARD_COMPONENT.md           # 本文档
```

## 🎉 总结

通过这次更新，我们：

1. ✅ 创建了可复用的 FaCard 组件
2. ✅ 建立了完整的阴影工具类系统（6 种级别）
3. ✅ 增强了间距工具类（新增 4 种双向类）
4. ✅ 提供了详尽的文档和示例
5. ✅ 保持了与项目现有架构的一致性
6. ✅ 实现了深色模式的自动适配

这套方案将 CSS 变量转换为原子样式类，提升了代码的复用性和维护性，为项目提供了更加现代化的样式解决方案。🚀
