<template>
	<el-breadcrumb separator="/">
		<transition-group appear name="slide-left" leaveActiveClass="">
			<el-breadcrumb-item key="/" to="/">首页</el-breadcrumb-item>
			<el-breadcrumb-item v-for="item in breadcrumbs" :key="item.path" :to="item.redirect ? item.redirect.toString() : undefined">
				<FaIcon v-if="item.meta?.icon" :name="item.meta.icon" />
				{{ item.meta.title }}
			</el-breadcrumb-item>
		</transition-group>
	</el-breadcrumb>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useRoute } from "vue-router";

defineOptions({
	name: "Breadcrumb",
});

const route = useRoute();

const breadcrumbs = computed(() => route.matched.filter((f) => !f.meta?.hide && f.meta?.title));
</script>

<style scoped lang="scss">
.el-breadcrumb {
	padding-left: 10px;
	font-weight: var(--el-font-weight-primary);
	.el-breadcrumb__item {
		:deep() {
			.el-breadcrumb__inner {
				display: inline-flex;
				gap: 5px;
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
