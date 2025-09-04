<template>
	<el-config-provider v-bind="elConfigProviderProps">
		<suspense>
			<template #default>
				<Watermark>
					<el-container>
						<el-main :style="{ '--el-main-padding': configStore.layout.mainPadding }">
							<el-scrollbar>
								<router-view v-slot="{ Component, route }">
									<transition mode="out-in" :name="configStore.layout.mainAnimation">
										<component :is="Component" :key="route.name" class="main" />
									</transition>
								</router-view>
							</el-scrollbar>
						</el-main>
						<el-footer :style="{ '--el-footer-height': configStore.layout.footerHeight }">
							<Footer />
						</el-footer>
					</el-container>
				</Watermark>
			</template>
			<template #fallback>
				<Loading loadingText="系统初始化中..." />
			</template>
		</suspense>
	</el-config-provider>
</template>

<script lang="ts" setup>
import { onMounted, reactive, watch } from "vue";
import { type componentSizes } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { useWindowSize } from "@vueuse/core";
import { useConfig } from "@/stores";

defineOptions({
	name: "App",
});

const windowSize = useWindowSize();
const configStore = useConfig();

const elConfigProviderProps = reactive({
	// 语言
	locale: undefined,
	button: {
		// 自动插入空格
		autoInsertSpace: false,
	},
	emptyValues: [undefined, null],
	valueOnClear: null,
	size: withDefineType<(typeof componentSizes)[number]>("default"),
});

// 初始化主题
configStore.initTheme();

/** 监听页面宽度，设置组件大小 */
watch(
	() => [windowSize.width.value, windowSize.height.value],
	() => {
		const html = document.documentElement;
		if (windowSize.width.value > 1366) {
			// 1366 以上使用默认
			elConfigProviderProps.size = "default";
			html.classList.remove("small");
			html.classList.add("default");
			configStore.setDefaultLayoutSize();
		} else {
			elConfigProviderProps.size = "small";
			html.classList.remove("default");
			html.classList.add("small");
			configStore.setSmallLayoutSize();
		}
	},
	{ immediate: true }
);

onMounted(async () => {
	if (import.meta.env.DEV) {
		const { default: zhCn } = await import("element-plus/es/locale/lang/zh-cn");
		elConfigProviderProps.locale = zhCn;
	} else {
		elConfigProviderProps.locale = ElementPlusLocaleZhCn;
	}
});
</script>

<style scoped lang="scss">
html.small {
	.el-container {
		.el-footer {
			font-size: var(--el-font-size-extra-small);
			flex-direction: column;
			justify-content: space-evenly;
			height: calc(var(--el-footer-height) * 1.7);
		}
	}
}

@media (max-width: 800px) {
	.el-container {
		min-width: 500px;
	}
}

.el-container {
	width: 100%;
	height: 100%;
	min-width: 1024px;
	.el-main {
		box-sizing: border-box;
		overflow-x: hidden;
		background-color: var(--el-bg-color-page);
		:deep(.el-scrollbar) {
			.el-scrollbar__wrap {
				.el-scrollbar__view {
					height: 100%;
					.main {
						width: 100%;
						height: 100%;
					}
				}
			}
		}
	}
	.el-footer {
		font-size: var(--el-font-size-base);
		color: var(--el-text-color-secondary);
		text-decoration: none;
		letter-spacing: 0.5px;
		padding: 0;
		display: flex;
		align-items: center;
		justify-content: center;
		transition: height var(--el-transition-duration);
		border-top: var(--el-border-width) solid var(--el-border-color);
		background-color: var(--el-bg-color);
		overflow: hidden;
	}
}
</style>
