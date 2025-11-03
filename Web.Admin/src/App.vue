<template>
	<el-config-provider v-bind="elConfigProviderProps">
		<router-view></router-view>
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
