<template>
	<el-sub-menu v-if="props.menu.children?.length > 0" :index="props.menu.children[0].router" :title="props.menu.menuTitle || props.menu.menuName">
		<template #title>
			<FaIcon v-if="props.menu.icon" :name="props.menu.icon" />
			<span>{{ props.menu.menuName }}</span>
		</template>
		<MenuItem v-for="(item, idx) in props.menu.children" :key="idx" :menu="item" />
	</el-sub-menu>
	<el-menu-item v-else :index="props.menu.router || props.menu.link" @click="handleMenuClick">
		<FaIcon v-if="props.menu.icon" :name="props.menu.icon" />
		<template #title>
			<span>{{ props.menu.menuName }}</span>
		</template>
	</el-menu-item>
</template>

<script setup lang="ts">
import { definePropType } from "@fast-china/utils";
import { useRouter } from "vue-router";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { loadComponentName, parseRouterUrl } from "@/router";
import MenuItem from "./index.vue";
import type { AuthMenuInfoDto } from "@/api/services/Auth/auth/models/AuthMenuInfoDto";

defineOptions({
	name: "LayoutMenuItem",
});

const router = useRouter();

const props = defineProps({
	/** 菜单 */
	menu: definePropType<AuthMenuInfoDto>(Object),
});

const handleMenuClick = () => {
	switch (props.menu.menuType) {
		case MenuTypeEnum.Catalog:
			break;
		case MenuTypeEnum.Menu: {
			// 通过路由 name 导航，确保 to.name / to.meta 正确反映当前菜单项
			// 同时将 router 地址中的查询参数单独提取，避免同一路径不同查询参数的菜单共用同一路由记录
			const { query } = parseRouterUrl(props.menu.router || "");
			const routeName = loadComponentName(props.menu.menuCode || props.menu.component);
			if (routeName) {
				router.push({ name: routeName, query });
			} else {
				router.push(props.menu.router || "");
			}
			break;
		}
		case MenuTypeEnum.Internal:
			router.push({
				path: "/iframe",
				query: { url: props.menu.link },
			});
			break;
		case MenuTypeEnum.Outside:
			window.open(props.menu.link, "_blank");
			break;
	}
};
</script>
