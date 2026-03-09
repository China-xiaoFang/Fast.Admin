<template>
	<suspense>
		<template #default>
			<Watermark v-if="configStore.layout.watermark">
				<component :is="layoutComponents[layoutMode]" class="layout" :class="{ 'is-mobile': isMobile }" />
			</Watermark>
			<component :is="layoutComponents[layoutMode]" v-else class="layout" :class="{ 'is-mobile': isMobile }" />
		</template>
		<template #fallback>
			<Loading loadingText="系统初始化中..." />
		</template>
	</suspense>
	<LayoutConfig ref="layoutConfigRef" />
	<ChangePassword ref="changePasswordRef" />
</template>

<script setup lang="ts">
import { computed, defineAsyncComponent, provide, ref, watch } from "vue";
import { withDefineType } from "@fast-china/utils";
import { useWindowSize } from "@vueuse/core";
import ChangePassword from "@/layouts/components/ChangePassword/index.vue";
import LayoutConfig from "@/layouts/components/Config/index.vue";
import { useConfig } from "@/stores";
import { changePasswordKey, layoutConfigKey } from "./index";
import type { IModeName } from "@/stores";
import type { Component } from "vue";

defineOptions({
	name: "LayoutAsync",
});

const configStore = useConfig();
const windowSize = useWindowSize();

const layoutConfigRef = ref<InstanceType<typeof LayoutConfig>>();
const changePasswordRef = ref<InstanceType<typeof ChangePassword>>();
provide(layoutConfigKey, layoutConfigRef);
provide(changePasswordKey, changePasswordRef);

/** 是否移动端（窗口宽度 <= 768） */
const isMobile = computed(() => windowSize.width.value <= 768);

/** 实际使用的布局模式（移动端强制经典） */
const layoutMode = computed<IModeName>(() => {
	if (isMobile.value) return "Classic";
	return configStore.layout.layoutMode;
});

/** 移动端时自动切换到经典布局 */
watch(isMobile, (newValue) => {
	if (newValue && configStore.layout.layoutMode !== "Classic") {
		configStore.setLayoutMode("Classic");
	}
});

const layoutComponents = withDefineType<Record<IModeName, Component>>({
	Classic: defineAsyncComponent(() => import("@/layouts/LayoutClassic/index.vue")),
	Horizontal: defineAsyncComponent(() => import("@/layouts/LayoutHorizontal/index.vue")),
	Mixed: defineAsyncComponent(() => import("@/layouts/LayoutMixed/index.vue")),
});
</script>

<style scoped lang="scss">
.layout {
	width: 100%;
	height: 100%;
	min-width: 1024px;
}
</style>
