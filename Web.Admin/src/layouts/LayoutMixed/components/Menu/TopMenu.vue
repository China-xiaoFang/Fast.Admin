<template>
	<el-scrollbar>
		<el-menu
			router
			mode="horizontal"
			:defaultActive="activeModule"
			:style="{ '--el-menu-item-height': addUnit(configStore.layout.navBarHeight) }"
		>
			<el-menu-item index="/dashboard">
				<FaIcon name="fa-icon-Dashboard" />
				<template #title>
					<span>首页</span>
				</template>
			</el-menu-item>
			<el-menu-item
				v-for="(item, idx) in topMenuList"
				:key="idx"
				:index="item.firstChildPath || item.menuCode"
				@click="handleModuleClick(item)"
			>
				<FaIcon v-if="item.icon" :name="item.icon" />
				<template #title>
					<span>{{ item.menuName }}</span>
				</template>
			</el-menu-item>
		</el-menu>
	</el-scrollbar>
</template>

<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { addUnit } from "@fast-china/utils";
import { useRoute, useRouter } from "vue-router";
import { useConfig, useUserInfo } from "@/stores";
import type { AuthMenuInfoDto } from "@/api/services/Auth/auth/models/AuthMenuInfoDto";

defineOptions({
	name: "LayoutMixedTopMenu",
});

const route = useRoute();
const router = useRouter();
const configStore = useConfig();
const userInfoStore = useUserInfo();

const emit = defineEmits<{
	(e: "module-change", menu: AuthMenuInfoDto): void;
}>();

const topMenuList = computed(() => {
	return userInfoStore.menuList.filter((f) => f.visible).map((item) => {
		const firstChild = findFirstLeaf(item);
		return {
			...item,
			firstChildPath: firstChild?.router || "",
		};
	});
});

/** 查找第一个叶子菜单 */
const findFirstLeaf = (menu: AuthMenuInfoDto): AuthMenuInfoDto | null => {
	if (!menu.children || menu.children.length === 0) {
		return menu;
	}
	for (const child of menu.children) {
		const leaf = findFirstLeaf(child);
		if (leaf) return leaf;
	}
	return null;
};

const activeModule = ref("/dashboard");

/** 根据当前路由匹配顶部模块 */
const matchModule = () => {
	const currentPath = route.path;
	for (const item of topMenuList.value) {
		if (isPathInModule(item, currentPath)) {
			activeModule.value = item.firstChildPath || item.menuCode;
			emit("module-change", item);
			return;
		}
	}
	activeModule.value = "/dashboard";
};

/** 判断路径是否在模块中 */
const isPathInModule = (menu: AuthMenuInfoDto, path: string): boolean => {
	if (menu.router === path) return true;
	if (menu.children) {
		for (const child of menu.children) {
			if (isPathInModule(child, path)) return true;
		}
	}
	return false;
};

const handleModuleClick = (item: AuthMenuInfoDto & { firstChildPath: string }) => {
	emit("module-change", item);
	if (item.firstChildPath) {
		router.push(item.firstChildPath);
	}
};

watch(() => route.path, matchModule, { immediate: true });
</script>

<style scoped lang="scss">
.el-scrollbar {
	flex: 1;
	overflow: hidden;
	:deep() {
		.el-scrollbar__wrap {
			overflow-y: hidden !important;
		}
	}
}
.el-menu {
	border: none;
	white-space: nowrap;
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
