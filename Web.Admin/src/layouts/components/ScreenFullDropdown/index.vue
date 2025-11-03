<template>
	<el-dropdown class="screen-full fa__hover__twinkle">
		<div>
			<el-icon>
				<FullScreenExit v-if="navTabsStore.state.contentLarge" />
				<FullScreen v-else />
			</el-icon>
		</div>
		<template #dropdown>
			<el-dropdown-menu>
				<el-dropdown-item @click="handleContentLargeClick">
					{{ navTabsStore.state.contentLarge ? "内容区复原" : "内容区放大" }}
				</el-dropdown-item>
				<el-dropdown-item @click="handleContentFullClick">内容区全屏</el-dropdown-item>
			</el-dropdown-menu>
		</template>
	</el-dropdown>
	<div v-if="navTabsStore.state.contentFull" class="screen-full__close fa__hover__twinkle" title="关闭内容区全屏" @click="handleContentFullClick">
		<el-icon>
			<Exit />
		</el-icon>
	</div>
</template>

<script setup lang="ts">
import { Exit, FullScreen, FullScreenExit } from "@fast-element-plus/icons-vue";
import { useNavTabs } from "@/stores";

defineOptions({
	name: "ScreenFullDropdown",
});

const navTabsStore = useNavTabs();

const handleContentLargeClick = (): void => {
	navTabsStore.setContentLarge(!navTabsStore.state.contentLarge);
};

const handleContentFullClick = (): void => {
	navTabsStore.setContentFull(!navTabsStore.state.contentFull);
};
</script>

<style scoped lang="scss">
.screen-full {
	box-shadow: -5px 0 5px -6px var(--el-text-color-secondary);
	.el-icon {
		width: 100%;
		height: 100%;
		font-size: var(--el-font-size-large);
	}
}
.screen-full__close {
	position: fixed;
	top: -25px;
	right: -25px;
	z-index: 999;
	width: 55px;
	height: 55px;
	cursor: pointer;
	background-color: var(--el-color-info);
	border-radius: 50%;
	opacity: 0.9;
	.el-icon {
		position: relative;
		top: 50%;
		left: 20%;
		font-size: 14px;
		color: var(--el-color-white);
	}
}
html.small {
	.screen-full {
		.el-icon {
			font-size: var(--el-font-size-medium);
		}
	}
}
</style>
