<template>
	<view :style="{ height: addUnit(state.height) }">
		<view class="fa-tabbar">
			<view
				v-for="(item, index) in userInfoStore.tabBars"
				:key="index"
				:class="[
					'fa-tabbar-item',

					{
						'fa-tabbar-item__bulge': item.bulge,
						'is-active': userInfoStore.activeTabBar === item.path,
					},
				]"
				@click="handleTabBarClick(item.path, item.disable)"
			>
				<template v-if="item.bulge">
					<view class="item__bulge-warp">
						<FaIcon :name="item.icon" />
						<text class="item__title">{{ item.title }}</text>
					</view>
				</template>
				<template v-else>
					<wd-badge>
						<FaIcon :name="item.icon" />
						<text class="item__title">{{ item.title }}</text>
					</wd-badge>
				</template>
				<FaIcon v-if="item.disable" name="lock" />
			</view>
		</view>
	</view>
</template>

<script setup lang="ts">
import { getCurrentInstance, nextTick, onBeforeMount, onMounted, reactive } from "vue";
import { addUnit, withDefineType } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import { useUserInfo } from "@/stores";

defineOptions({
	name: "TabBar",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const { proxy } = getCurrentInstance();

const router = useRouter();
const userInfoStore = useUserInfo();

const state = reactive({
	height: withDefineType<number>(),
});

const handlePlaceholderHeight = () => {
	let query: UniApp.SelectorQuery;
	if (proxy) {
		query = uni.createSelectorQuery().in(proxy);
	} else {
		query = uni.createSelectorQuery();
	}

	query
		.select(".fa-tabbar")
		.boundingClientRect((res) => {
			state.height = (res as UniApp.NodeInfo)?.height - 1;
		})
		.exec();
};

const handleTabBarClick = (value: string, disable?: boolean) => {
	if (disable) return;
	router.pushTab({
		path: value,
	});
};

onMounted(() => {
	nextTick(() => {
		handlePlaceholderHeight();
	});
});

onBeforeMount(() => {
	// #ifndef MP-WEIXIN
	// 隐藏原生tabbar
	uni.hideTabBar();
	// #endif
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
