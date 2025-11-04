<template>
	<el-sub-menu v-if="props.menu.children?.length > 0" :index="props.menu.children[0].router" :title="props.menu.menuTitle || props.menu.menuName">
		<template #title>
			<FaIcon :name="props.menu.icon || 'el-icon-Menu'" />
			<span>{{ props.menu.menuName }}</span>
		</template>
		<MenuItem v-for="(item, idx) in props.menu.children" :key="idx" :menu="item" />
	</el-sub-menu>
	<el-menu-item v-else :index="props.menu.router">
		<FaIcon :name="props.menu.icon || 'el-icon-Menu'" />
		<template #title>
			<span>{{ props.menu.menuName }}</span>
		</template>
	</el-menu-item>
</template>

<script setup lang="ts">
import { definePropType } from "@fast-china/utils";
import MenuItem from "./index.vue";
import type { AuthMenuInfoDto } from "@/api/services/auth/models/AuthMenuInfoDto";

defineOptions({
	name: "LayoutMenuItem",
});

const props = defineProps({
	/** 菜单 */
	menu: definePropType<AuthMenuInfoDto>(Object),
});
</script>

<style scoped lang="scss">
.el-sub-menu {
	&.is-active {
		:deep() {
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
	}
	:deep() {
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
</style>
