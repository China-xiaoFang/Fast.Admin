<template>
	<el-scrollbar>
		<el-menu router :defaultActive="activeMenu" :style="{ '--el-menu-item-height': addUnit(configStore.layout.menuHeight) }">
			<el-menu-item v-for="(item, index) in menuList" :key="index" :index="item.path">
				<FaIcon v-if="item.meta.icon" :name="item.meta.icon" />
				<span>{{ item.meta.title }}</span>
			</el-menu-item>
		</el-menu>
	</el-scrollbar>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { addUnit } from "@fast-china/utils";
import { useRouter } from "vue-router";
import type { RouteRecord } from "vue-router";
import { useConfig, useUserInfo } from "@/stores";

defineOptions({
	name: "LayoutMenu",
});

const router = useRouter();
const configStore = useConfig();
const userInfoStore = useUserInfo();

const activeMenu = computed(() => router.currentRoute.value.path);

const menuList = computed(
	() =>
		router
			.getRoutes()
			.find((f) => f.name === "layout")
			?.children?.filter((f) => {
				if (f.meta.hide) return false;

				if (!f.meta.userType) return true;

				return (f.meta.userType & userInfoStore.userInfo.userType) > 0;
			}) as RouteRecord[]
);
</script>

<style scoped lang="scss">
.el-menu {
	.el-menu-item {
		height: var(--el-menu-item-height);
		&.is-active {
			font-weight: bold;
			color: var(--el-color-white);
			background-color: var(--el-menu-active-color);
		}
	}
}
html.small {
	.el-menu {
		.el-menu-item {
			font-size: var(--el-font-size-extra-small);
			.el-icon {
				font-size: var(--el-font-size-medium);
			}
		}
	}
}
</style>
