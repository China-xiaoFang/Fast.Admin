<template>
	<div class="nav-bar-tab" :style="{ '--height': addUnit(configStore.layout.navBarTabHeight) }">
		<el-icon class="icon-arrow left fa__hover__twinkle" title="向左滚动" @click="handleScrollTo('left')">
			<ArrowLeft />
		</el-icon>
		<el-scrollbar ref="scrollbarRef" @wheel.passive="handleWheelScroll" @scroll="handleScroll">
			<div
				v-for="tag in navTabsStore.state.navBarTabs"
				:key="tag.path"
				ref="tagRefs"
				:class="['nav-bar-tab__item', { 'is-active': route.path === tag.path }]"
				:title="tag.meta?.title"
				@click.prevent="(event) => handleTabClick(event, tag)"
				@mousedown.middle="(event) => handleCloseClick(event, tag)"
				@contextmenu.prevent="(event) => handleContextmenuClick(event, tag)"
			>
				{{ tag.meta?.title }}
				<el-icon
					v-if="!tag.meta?.affix && route.path === tag.path"
					class="icon-close"
					title="关闭"
					@click.prevent.stop="(event) => handleCloseClick(event, tag)"
				>
					<Close />
				</el-icon>
			</div>
		</el-scrollbar>
		<el-icon class="icon-arrow right fa__hover__twinkle" title="向右滚动" @click="handleScrollTo('right')">
			<ArrowRight />
		</el-icon>
		<ScreenFullDropdown />
		<FaContextMenu ref="faContextMenuRef" :data="contextMenuList" />
	</div>
</template>

<script setup lang="ts">
import { nextTick, onMounted, reactive, ref } from "vue";
import { ArrowLeft, ArrowRight, Close } from "@element-plus/icons-vue";
import { type FaContextMenuData, type FaContextMenuInstance } from "fast-element-plus";
import { addUnit } from "@fast-china/utils";
import { onBeforeRouteUpdate, useRoute, useRouter } from "vue-router";
import type { INavBarTab } from "@/stores";
import type { ElScrollbar } from "element-plus";
import type { RouteLocationNormalized } from "vue-router";
import ScreenFullDropdown from "@/layouts/components/ScreenFullDropdown/index.vue";
import { routerUtil } from "@/router";
import { useConfig, useNavTabs } from "@/stores";

defineOptions({
	name: "NavBarTab",
});

const route = useRoute();
const router = useRouter();
const configStore = useConfig();
const navTabsStore = useNavTabs();

const tagRefs = ref<HTMLElement[]>([]);
const scrollbarRef = ref<InstanceType<typeof ElScrollbar>>();
const faContextMenuRef = ref<FaContextMenuInstance>();

const contextMenuList = reactive<FaContextMenuData[]>([
	{
		name: "refresh",
		label: "重新加载",
		disabled: false,
		icon: "el-refresh",
		click: (_, { data }: { data?: INavBarTab }): void => {
			navTabsStore.refreshTab(data);
		},
	},
	{
		name: "close",
		label: "关闭标签",
		disabled: false,
		icon: "el-close",
		click: (_, { data }: { data?: INavBarTab }): void => {
			navTabsStore.closeTab(data);
		},
	},
	{
		name: "closeOther",
		label: "关闭其他标签",
		disabled: false,
		icon: "el-close",
		click: (_, { data }: { data?: INavBarTab }): void => {
			navTabsStore.closeTabs(data);
		},
	},
	{
		name: "closeAll",
		label: "关闭全部标签",
		disabled: false,
		icon: "el-close",
		click: (): void => {
			navTabsStore.closeTabs();
		},
	},
]);

const state = reactive({
	/** 当前滚动条距离左边的距离 */
	currentScrollLeft: 0,
	/** 每次滚动的距离 */
	translateDistance: 100,
	/** 箭头图标宽度 */
	arrowIconWidth: 46,
});

/**
 * 获取可能需要的宽度
 */
const getWidth = () => {
	/** 可滚动内容的长度 */
	const scrollWidth = scrollbarRef.value.wrapRef.scrollWidth;
	/** 滚动可视区宽度 */
	const clientWidth = scrollbarRef.value.wrapRef.clientWidth;
	/** 最后剩余可滚动的宽度 */
	const lastDistance = scrollWidth - clientWidth - state.currentScrollLeft;

	return { scrollWidth, clientWidth, lastDistance };
};

/**
 * 左右滚动
 */
const handleScrollTo = (direction: "left" | "right", distance: number = state.translateDistance) => {
	let scrollLeft = 0;
	const { scrollWidth, clientWidth } = getWidth();
	// 没有横向滚动条，直接结束
	if (clientWidth > scrollWidth) return;
	const currentScrollLeft = state.currentScrollLeft;
	if (direction === "left") {
		scrollLeft = Math.max(0, currentScrollLeft - distance);
	} else {
		scrollLeft = Math.min(currentScrollLeft + distance);
	}
	scrollbarRef.value.setScrollLeft(scrollLeft);
};

/**
 * 鼠标滚轮滚动时触发
 */
const handleWheelScroll = (event: WheelEvent) => {
	if (/^-/.test(event.deltaY.toString())) {
		handleScrollTo("left");
	} else {
		handleScrollTo("right");
	}
};

/**
 * 滚动时触发
 */
const handleScroll = ({ scrollLeft }: { scrollLeft: number }) => {
	state.currentScrollLeft = scrollLeft;
};

/**
 * 移动到目标位置
 */
const handleMoveTo = (to?: RouteLocationNormalized) => {
	nextTick(() => {
		const curTo = to ?? route;
		const findTagRef = tagRefs.value.find((f) => f && f["__vnode"]?.key === curTo.path);
		if (findTagRef) {
			const { offsetWidth, offsetLeft } = findTagRef;
			const { clientWidth } = getWidth();
			// 当前 tag 在可视区域左边时
			if (offsetLeft < state.currentScrollLeft) {
				const distance = state.currentScrollLeft - offsetLeft;
				handleScrollTo("left", distance);
				return;
			}

			// 当前 tag 在可视区域右边时
			const width = clientWidth + state.currentScrollLeft - offsetWidth;
			if (offsetLeft > width) {
				const distance = offsetLeft - width;
				handleScrollTo("right", distance);
				return;
			}
		}
	});
};

const handleTabClick = (event: MouseEvent, tag: INavBarTab): void => {
	if (tag.path === route.path) return;
	// 左键
	routerUtil.routePushSafe(router, { path: tag.path, query: tag.query });
};

const handleContextmenuClick = (event: MouseEvent, tag: INavBarTab): void => {
	// 禁用重新加载
	contextMenuList[0].disabled = tag.path !== route.path;
	// 禁用关闭
	contextMenuList[1].disabled = tag?.meta?.affix === true;
	// 禁用关闭其他和关闭全部
	contextMenuList[2].disabled = contextMenuList[3].disabled = navTabsStore.state.navBarTabs.length === 1 ? true : false;

	contextMenuList.forEach((item) => {
		item.data = tag;
	});

	const { clientX, clientY } = event;
	faContextMenuRef.value.open({ x: clientX, y: clientY });
};

const handleCloseClick = (event: MouseEvent, tag: INavBarTab): void => {
	if (tag.meta?.affix === true) {
		return;
	}
	navTabsStore.closeTab(tag);
	faContextMenuRef.value?.close();
};

onMounted(() => {
	navTabsStore.initNavBarTabs(router);
	navTabsStore.addTab(router.currentRoute.value);
	navTabsStore.setActiveRoute(router.currentRoute.value);
	handleMoveTo(router.currentRoute.value);
});

onBeforeRouteUpdate((to: RouteLocationNormalized) => {
	navTabsStore.addTab(routerUtil.pickByRoute(to));
	navTabsStore.setActiveRoute(to);
	handleMoveTo(to);
});
</script>

<style scoped lang="scss">
@use "./index.scss";
</style>
