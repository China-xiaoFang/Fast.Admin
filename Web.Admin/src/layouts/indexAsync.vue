<template>
	<suspense>
		<template #default>
			<Watermark v-if="configStore.layout.watermark">
				<component :is="layoutComponents[configStore.layout.layoutMode]" class="layout" />
			</Watermark>
			<component :is="layoutComponents[configStore.layout.layoutMode]" v-else class="layout" />
		</template>
		<template #fallback>
			<Loading loadingText="系统初始化中..." />
		</template>
	</suspense>
	<LayoutConfig ref="layoutConfigRef" />
</template>

<script setup lang="ts">
import { defineAsyncComponent, provide, ref } from "vue";
import { withDefineType } from "@fast-china/utils";
import { layoutConfigKey } from "./index";
import type { IModeName } from "@/stores";
import type { Component } from "vue";
import LayoutConfig from "@/layouts/components/Config/index.vue";
import { useConfig } from "@/stores";

defineOptions({
	name: "LayoutAsync",
});

const configStore = useConfig();

const layoutConfigRef = ref<InstanceType<typeof LayoutConfig>>();
provide(layoutConfigKey, layoutConfigRef);

const layoutComponents = withDefineType<Record<IModeName, Component>>({
	Classic: defineAsyncComponent(() => import("@/layouts/LayoutClassic/index.vue")),
});
</script>

<style scoped lang="scss">
.layout {
	width: 100%;
	height: 100%;
	min-width: 1024px;
}
</style>
