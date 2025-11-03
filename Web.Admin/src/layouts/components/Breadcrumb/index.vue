<template>
	<el-breadcrumb separator="/">
		<el-breadcrumb-item to="/">首页</el-breadcrumb-item>
		<el-breadcrumb-item v-for="(item, index) in breadcrumbs" :key="index">
			{{ item }}
		</el-breadcrumb-item>
	</el-breadcrumb>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";

defineOptions({
	name: "Breadcrumb",
});

const route = useRoute();

const breadcrumbs = computed(() => route.matched.filter((f) => !f.meta?.hide && f.meta?.title).map((m) => m.meta?.title));
</script>

<style scoped lang="scss">
.el-breadcrumb {
	padding-left: 10px;
	font-weight: var(--el-font-weight-primary);
	.el-breadcrumb__item {
		:deep() {
			.el-breadcrumb__inner {
				transition: var(--el-transition-color);
			}
		}
		&:last-child {
			:deep() {
				.el-breadcrumb__inner {
					color: var(--el-text-color-placeholder);
				}
			}
		}
	}
}
html.small {
	.el-breadcrumb {
		font-size: var(--el-font-size-extra-small);
	}
}
</style>
