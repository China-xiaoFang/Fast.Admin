<template>
	<el-scrollbar>
		<el-menu
			router
			:defaultActive="activeMenu"
			:collapse="configStore.layout.menuCollapse"
			:style="{ '--el-menu-item-height': addUnit(configStore.layout.menuHeight) }"
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
import { useConfig, useNavTabs, useUserInfo } from "@/stores";
import MenuItem from "../MenuItem/index.vue";

defineOptions({
	name: "LayoutMenu",
});

const router = useRouter();
const configStore = useConfig();
const navTabStore = useNavTabs();
const userInfoStore = useUserInfo();

const activeMenu = computed(() => router.currentRoute.value.path);

const menuList = computed(() => {
	const activeModuleId = navTabStore.activeModuleId || userInfoStore.menuList[0].moduleId;
	const _menuList = userInfoStore.menuList.find((f) => f.moduleId === activeModuleId) ?? userInfoStore.menuList[0];
	return (_menuList?.children ?? []).filter((f) => f.visible);
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
	:deep() {
		.el-sub-menu {
			&.is-active {
				.el-sub-menu__title.el-tooltip__trigger {
					font-weight: var(--el-font-weight-primary);
					.el-icon {
						color: var(--el-color-white);
					}
					// background-color: var(--el-menu-active-color);
					background: none;
					* {
						z-index: 2;
					}
					&::before {
						content: "";
						position: absolute;
						top: 5px;
						bottom: 5px;
						left: 0;
						right: 0;
						background-color: var(--el-menu-active-color);
						border-radius: 3px;
						z-index: 1;
					}
				}
			}
			.el-sub-menu__title {
				* {
					z-index: 2;
				}
				&:hover {
					.el-icon {
						color: var(--el-menu-text-color);
					}
					// background-color: var(--el-menu-hover-bg-color);
					background: none;
					&::before {
						content: "";
						position: absolute;
						top: 5px;
						bottom: 5px;
						left: 0;
						right: 0;
						background-color: var(--el-menu-hover-bg-color);
						border-radius: 3px;
						z-index: 1;
					}
				}
			}
		}
		.el-menu-item {
			* {
				z-index: 2;
			}
			&:hover {
				// background-color: var(--el-menu-hover-bg-color);
				background: none;
				&::before {
					content: "";
					position: absolute;
					top: 5px;
					bottom: 5px;
					left: 0;
					right: 0;
					background-color: var(--el-menu-hover-bg-color);
					border-radius: 3px;
					z-index: 1;
				}
			}

			&.is-active {
				font-weight: var(--el-font-weight-primary);
				color: var(--el-color-white);
				// background-color: var(--el-menu-active-color);
				background: none;
				&::before {
					content: "";
					position: absolute;
					top: 5px;
					bottom: 5px;
					left: 0;
					right: 0;
					background-color: var(--el-menu-active-color);
					border-radius: 3px;
					z-index: 1;
				}
			}
		}
	}
}
.el-menu--collapse {
	--el-menu-base-level-padding: 10px;
}
html.small {
	.el-menu {
		--el-menu-item-font-size: var(--el-font-size-small);
	}
}
</style>
