<template>
	<el-menu
		uniqueOpened
		mode="horizontal"
		:defaultActive="activeMenu"
		:style="{ '--el-menu-horizontal-height': addUnit(configStore.layout.navBarHeight) }"
	>
		<el-menu-item index="/dashboard" @click="router.push('/dashboard')">
			<FaIcon name="fa-icon-Dashboard" />
			<template #title>
				<span>首页</span>
			</template>
		</el-menu-item>
		<MenuItem v-for="(item, idx) in menuList" :key="idx" :menu="item" />
	</el-menu>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { addUnit } from "@fast-china/utils";
import { useRouter } from "vue-router";
import MenuItem from "@/layouts/components/MenuItem/index.vue";
import { useConfig, useUserInfo } from "@/stores";

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
.el-menu {
	width: 100%;
	border: none;
	white-space: nowrap;
	overflow: hidden;
	:deep() {
		.el-sub-menu {
			&.is-active {
				.el-sub-menu__title {
					font-weight: var(--el-font-weight-primary);
					border-bottom: 3px solid var(--el-menu-active-color);
				}
			}
		}
		.el-menu-item {
			&.is-active {
				font-weight: var(--el-font-weight-primary);
				border-bottom: 3px solid var(--el-menu-active-color);
			}
		}
	}
}
</style>
