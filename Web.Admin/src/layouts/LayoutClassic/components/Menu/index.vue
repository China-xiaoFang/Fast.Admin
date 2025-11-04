<template>
	<el-scrollbar>
		<el-menu
			router
			:defaultActive="activeMenu"
			:collapse="configStore.layout.menuCollapse"
			:style="{ '--el-menu-item-height': addUnit(configStore.layout.menuHeight) }"
		>
			<MenuItem v-for="(item, idx) in menuList" :key="idx" :menu="item" />
		</el-menu>
	</el-scrollbar>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { addUnit } from "@fast-china/utils";
import { useRouter } from "vue-router";
import MenuItem from "../MenuItem/index.vue";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { useConfig, useNavTabs, useUserInfo } from "@/stores";

defineOptions({
	name: "LayoutMenu",
});

const router = useRouter();
const configStore = useConfig();
const navTabStore = useNavTabs();
const userInfoStore = useUserInfo();

const activeMenu = computed(() => router.currentRoute.value.path);

const menuList = computed(() => {
	const activeModuleId = navTabStore.activeModuleId || userInfoStore.menuList[0].id;
	const _menuList = userInfoStore.menuList.find((f) => f.id === activeModuleId) ?? userInfoStore.menuList[0];
	return (_menuList?.children ?? []).filter((f) => f.visible === YesOrNotEnum.Y);
});
</script>

<style scoped lang="scss">
.el-scrollbar {
	flex: 1;
	:deep() {
		.el-scrollbar__view {
			padding: 0 5px;
		}
	}
}
.el-menu {
	border: none;
	height: 100%;
}
html.small {
	.el-menu {
		--el-menu-item-font-size: var(--el-font-size-small);
	}
}
</style>
