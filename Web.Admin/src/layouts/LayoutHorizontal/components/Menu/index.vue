<template>
	<el-scrollbar>
		<el-menu
			router
			mode="horizontal"
			:defaultActive="activeMenu"
			:style="{ '--el-menu-item-height': addUnit(configStore.layout.navBarHeight) }"
		>
			<el-menu-item index="/dashboard">
				<FaIcon name="fa-icon-Dashboard" />
				<template #title>
					<span>首页</span>
				</template>
			</el-menu-item>
			<MenuItem v-for="(item, idx) in menuList" :key="idx" :menu="item" />
		</el-menu>
	</el-scrollbar>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { addUnit } from "@fast-china/utils";
import { useRouter } from "vue-router";
import { useConfig, useUserInfo } from "@/stores";
import MenuItem from "@/layouts/LayoutClassic/components/MenuItem/index.vue";

defineOptions({
	name: "LayoutHorizontalMenu",
});

const router = useRouter();
const configStore = useConfig();
const userInfoStore = useUserInfo();

const activeMenu = computed(() => router.currentRoute.value.path);

const menuList = computed(() => userInfoStore.menuList.filter((f) => f.visible));
</script>

<style scoped lang="scss">
.el-scrollbar {
	flex: 1;
	overflow: hidden;
}
.el-menu {
	border: none;
	:deep() {
		.el-sub-menu {
			&.is-active {
				.el-sub-menu__title {
					font-weight: var(--el-font-weight-primary);
					border-bottom: 2px solid var(--el-menu-active-color);
				}
			}
		}
		.el-menu-item {
			&.is-active {
				font-weight: var(--el-font-weight-primary);
				border-bottom: 2px solid var(--el-menu-active-color);
			}
		}
	}
}
</style>
