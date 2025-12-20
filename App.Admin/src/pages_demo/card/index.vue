<template>
	<view class="page">
		<FaCard title="卡片标题示例" :more="true" @moreClick="handleMoreClick">
			<view class="fa-page__card">
				<view class="page__row">
					<view class="title">名称：</view>
					<view class="content">这是卡片内容</view>
				</view>
				<view class="page__row">
					<view class="title">描述：</view>
					<view class="content">使用原子样式类 fa-card-shadow 实现阴影效果</view>
				</view>
			</view>
		</FaCard>

		<FaCard title="无更多按钮" :more="false">
			<view class="demo-content">
				<wd-text text="这个卡片没有更多按钮" />
			</view>
		</FaCard>

		<FaCard title="图表容器示例">
			<view class="eChart__warp">
				<wd-text text="图表区域 (最小高度 360rpx)" />
			</view>
		</FaCard>

		<view class="mx20 mb20">
			<wd-text customClass="mb10" customStyle="display: block;" text="阴影工具类示例：" size="var(--wot-font-size-lg)" />

			<view class="shadow-demo fa-shadow-sm mb20">
				<wd-text text="小阴影 (fa-shadow-sm)" />
			</view>

			<view class="shadow-demo fa-shadow mb20">
				<wd-text text="默认阴影 (fa-shadow)" />
			</view>

			<view class="shadow-demo fa-shadow-md mb20">
				<wd-text text="中等阴影 (fa-shadow-md)" />
			</view>

			<view class="shadow-demo fa-shadow-lg mb20">
				<wd-text text="大阴影 (fa-shadow-lg)" />
			</view>

			<view class="shadow-demo fa-shadow-xl mb20">
				<wd-text text="超大阴影 (fa-shadow-xl)" />
			</view>

			<view class="shadow-demo fa-card-shadow">
				<wd-text text="卡片阴影 (fa-card-shadow)" />
			</view>
		</view>
	</view>
</template>

<script setup lang="ts">
import { useToast } from "@/hooks";

definePage({
	name: "DemoCard",
	layout: "layout",
	style: {
		navigationBarTitleText: "FaCard 组件示例",
	},
});

const handleMoreClick = () => {
	useToast.info("点击了更多按钮");
};
</script>

<style scoped lang="scss">
.demo-content {
	padding: 20rpx;
	text-align: center;
}

.shadow-demo {
	padding: 30rpx;
	background-color: var(--wot-bg-color-container);
	border-radius: 20rpx;
}
</style>
