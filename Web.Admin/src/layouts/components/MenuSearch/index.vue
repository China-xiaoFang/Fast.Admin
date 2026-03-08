<template>
	<div>
		<el-icon class="menu-search fa__hover__twinkle" title="搜索菜单" @click="handleOpen">
			<Search />
		</el-icon>
		<el-dialog
			v-model="state.visible"
			:showClose="false"
			width="600px"
			top="15vh"
			appendToBody
			:closeOnClickModal="true"
			:closeOnPressEscape="true"
			class="menu-search-dialog"
		>
			<el-input
				ref="inputRef"
				v-model="state.keyword"
				size="large"
				placeholder="搜索菜单..."
				:prefixIcon="Search"
				clearable
				@input="handleSearch"
			/>
			<div class="search-result">
				<el-scrollbar maxHeight="400px">
					<div v-if="state.resultList.length === 0 && state.keyword" class="search-empty">
						<el-empty description="暂无搜索结果" :imageSize="80" />
					</div>
					<div
						v-for="(item, idx) in state.resultList"
						:key="idx"
						:class="['search-item', { 'is-active': state.activeIndex === idx }]"
						@click="handleSelect(item)"
						@mouseenter="state.activeIndex = idx"
					>
						<el-icon>
							<FaIcon v-if="item.icon" :name="item.icon" />
							<Menu v-else />
						</el-icon>
						<div class="search-item__info">
							<span class="search-item__title">{{ item.menuName }}</span>
							<span v-if="item.parentName" class="search-item__path">{{ item.parentName }}</span>
						</div>
						<el-icon class="search-item__enter">
							<Right />
						</el-icon>
					</div>
				</el-scrollbar>
			</div>
			<div class="search-footer">
				<span class="search-footer__tip">
					<kbd>↑</kbd> <kbd>↓</kbd> 导航
				</span>
				<span class="search-footer__tip">
					<kbd>↵</kbd> 确认
				</span>
				<span class="search-footer__tip">
					<kbd>Esc</kbd> 关闭
				</span>
			</div>
		</el-dialog>
	</div>
</template>

<script setup lang="ts">
import { nextTick, onMounted, onUnmounted, reactive, ref } from "vue";
import { Menu, Right, Search } from "@element-plus/icons-vue";
import { useRouter } from "vue-router";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { useUserInfo } from "@/stores";
import type { AuthMenuInfoDto } from "@/api/services/Auth/auth/models/AuthMenuInfoDto";
import type { InputInstance } from "element-plus";

defineOptions({
	name: "MenuSearch",
});

type ISearchItem = {
	menuName: string;
	icon?: string;
	router?: string;
	link?: string;
	menuType?: number;
	parentName?: string;
};

const router = useRouter();
const userInfoStore = useUserInfo();
const inputRef = ref<InputInstance>();

const state = reactive({
	visible: false,
	keyword: "",
	resultList: [] as ISearchItem[],
	activeIndex: 0,
	allMenus: [] as ISearchItem[],
});

/** 扁平化菜单 */
const flattenMenus = (menus: AuthMenuInfoDto[], parentName = ""): ISearchItem[] => {
	const result: ISearchItem[] = [];
	for (const menu of menus) {
		if (!menu.visible) continue;
		if (menu.children && menu.children.length > 0) {
			result.push(...flattenMenus(menu.children, menu.menuName));
		} else if (menu.router || menu.link) {
			result.push({
				menuName: menu.menuName,
				icon: menu.icon,
				router: menu.router,
				link: menu.link,
				menuType: menu.menuType,
				parentName,
			});
		}
	}
	return result;
};

const handleOpen = () => {
	state.visible = true;
	state.keyword = "";
	state.resultList = [];
	state.activeIndex = 0;
	state.allMenus = flattenMenus(userInfoStore.menuList);
	nextTick(() => {
		inputRef.value?.focus();
	});
};

const handleSearch = () => {
	state.activeIndex = 0;
	if (!state.keyword.trim()) {
		state.resultList = [];
		return;
	}
	const keyword = state.keyword.toLowerCase();
	state.resultList = state.allMenus.filter(
		(item) =>
			item.menuName.toLowerCase().includes(keyword) || (item.parentName && item.parentName.toLowerCase().includes(keyword))
	);
};

const handleSelect = (item: ISearchItem) => {
	state.visible = false;
	if (item.menuType === MenuTypeEnum.Outside && item.link) {
		window.open(item.link, "_blank");
	} else if (item.menuType === MenuTypeEnum.Internal && item.link) {
		router.push({ path: "/iframe", query: { url: item.link } });
	} else if (item.router) {
		router.push(item.router);
	}
};

const handleKeydown = (event: KeyboardEvent) => {
	if (!state.visible) {
		// Ctrl+K 或 Cmd+K 打开搜索
		if ((event.ctrlKey || event.metaKey) && event.key === "k") {
			event.preventDefault();
			handleOpen();
		}
		return;
	}
	switch (event.key) {
		case "ArrowUp":
			event.preventDefault();
			state.activeIndex = state.activeIndex > 0 ? state.activeIndex - 1 : state.resultList.length - 1;
			break;
		case "ArrowDown":
			event.preventDefault();
			state.activeIndex = state.activeIndex < state.resultList.length - 1 ? state.activeIndex + 1 : 0;
			break;
		case "Enter":
			event.preventDefault();
			if (state.resultList.length > 0) {
				handleSelect(state.resultList[state.activeIndex]);
			}
			break;
	}
};

onMounted(() => {
	document.addEventListener("keydown", handleKeydown);
});

onUnmounted(() => {
	document.removeEventListener("keydown", handleKeydown);
});
</script>

<style scoped lang="scss">
.menu-search {
	height: 100%;
	width: 35px;
	font-size: var(--el-font-size-large);
	cursor: pointer;
	&:hover {
		background-color: var(--el-text-color-disabled);
	}
}
</style>

<style lang="scss">
.menu-search-dialog {
	border-radius: 12px;
	.el-dialog__header {
		display: none;
	}
	.el-dialog__body {
		padding: 0;
	}
	.el-input {
		padding: 14px 16px;
		border-bottom: var(--el-border);
	}
	.search-result {
		min-height: 50px;
		.search-empty {
			padding: 20px 0;
		}
		.search-item {
			display: flex;
			align-items: center;
			padding: 10px 16px;
			cursor: pointer;
			transition: background-color var(--el-transition-duration);
			gap: 10px;
			&:hover,
			&.is-active {
				background-color: var(--el-fill-color-light);
			}
			.el-icon {
				font-size: 18px;
				color: var(--el-text-color-secondary);
			}
			.search-item__info {
				flex: 1;
				display: flex;
				flex-direction: column;
				.search-item__title {
					font-size: var(--el-font-size-base);
					color: var(--el-text-color-primary);
				}
				.search-item__path {
					font-size: var(--el-font-size-extra-small);
					color: var(--el-text-color-secondary);
				}
			}
			.search-item__enter {
				font-size: 14px;
				color: var(--el-text-color-placeholder);
			}
		}
	}
	.search-footer {
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 16px;
		padding: 10px 16px;
		border-top: var(--el-border);
		.search-footer__tip {
			display: flex;
			align-items: center;
			gap: 4px;
			font-size: var(--el-font-size-extra-small);
			color: var(--el-text-color-secondary);
			kbd {
				display: inline-flex;
				align-items: center;
				justify-content: center;
				min-width: 20px;
				height: 20px;
				padding: 0 4px;
				border: 1px solid var(--el-border-color);
				border-radius: 4px;
				font-size: 11px;
				background-color: var(--el-fill-color);
			}
		}
	}
}
</style>
