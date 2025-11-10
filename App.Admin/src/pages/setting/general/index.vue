<template>
	<view class="page">
		<wd-text customClass="pl20 mb10" customStyle="display: block;" text="主题样式" size="var(--wot-font-size-small)" />
		<wd-cell-group border>
			<wd-picker
				label="主题颜色"
				:columns="predefineColorList"
				v-model="configStore.layout.themeColor"
				alignRight
				@confirm="({ value }) => configStore.setTheme(value)"
			/>
			<wd-cell title="主题色导航栏" label="开启后导航栏颜色随主题色变换" center>
				<wd-switch v-model="configStore.layout.autoThemeNavBar" @change="configStore.setTheme(configStore.layout.themeColor)" />
			</wd-cell>
			<wd-cell title="跟随系统" center>
				<wd-switch v-model="configStore.layout.autoThemMode" @change="configStore.switchAutoThemMode" />
			</wd-cell>
			<wd-cell title="深色模式" center>
				<wd-switch :disabled="configStore.layout.autoThemMode" v-model="configStore.layout.isDark" @change="configStore.switchDark" />
			</wd-cell>
			<wd-cell title="置灰模式" center>
				<wd-switch v-model="configStore.layout.isGrey" @change="({ value }) => configStore.switchGreyOrWeak('grey', value)" />
			</wd-cell>
			<wd-cell title="色弱模式" center>
				<wd-switch v-model="configStore.layout.isWeak" @change="({ value }) => configStore.switchGreyOrWeak('weak', value)" />
			</wd-cell>
		</wd-cell-group>

		<wd-text customClass="pl20 mb10 mt20" customStyle="display: block;" text="界面显示" size="var(--wot-font-size-small)" />
		<wd-cell-group border>
			<wd-cell title="页脚" center>
				<wd-switch v-model="configStore.layout.footer" />
			</wd-cell>
			<wd-cell customClass="mb20" title="水印" label="出于安全考虑，请不要手动禁用水印功能" center border>
				<wd-switch v-model="configStore.layout.watermark" />
			</wd-cell>
		</wd-cell-group>

		<wd-text customClass="pl20 mb10 mt20" customStyle="display: block;" text="列表配置" size="var(--wot-font-size-small)" />
		<wd-cell-group border>
			<wd-cell title="隐藏图片" label="开启后列表所有图片列需要点击才能查看（减少网络资源消耗）" center>
				<wd-switch v-model="configStore.tableLayout.hideImage" />
			</wd-cell>
			<wd-picker
				customClass="mb20"
				label="默认搜索时间范围"
				:columns="dataSearchRangeList"
				v-model="configStore.tableLayout.dataSearchRange"
				alignRight
			/>
		</wd-cell-group>

		<wd-button customClass="mt40 btn__reset" type="error" block :round="false" @tap="configStore.reset">重置</wd-button>
	</view>
</template>

<script setup lang="ts">
import { withDefineType } from "@fast-china/utils";
import { defaultThemeColor, useConfig } from "@/stores";

definePage({
	name: "SettingGeneral",
	layout: "layout",
	style: {
		navigationBarTitleText: "通用",
	},
});
const configStore = useConfig();

const predefineColorList = [
	{
		label: "默认",
		value: defaultThemeColor,
	},
	{ label: "靛蓝", value: "#2A3A93" },
	{ label: "朱红", value: "#EB4537" },
	{ label: "橙色", value: "#FF8225" },
	{ label: "金黄", value: "#FAC230" },
	{ label: "淡紫", value: "#AD6DEF" },
	{ label: "亮蓝色", value: "#4488FE" },
	{ label: "薄荷绿", value: "#55AF7B" },
	{ label: "樱花粉", value: "#FF4191" },
	{ label: "天空蓝", value: "#4286F3" },
];

const dataSearchRangeList: ElSelectorOutput<FaTableDataRange>[] = [
	{
		label: "近1天",
		value: "Past1D",
	},
	{
		label: "近3天",
		value: "Past3D",
	},
	{
		label: "近1周",
		value: "Past1W",
	},
	{
		label: "近1月",
		value: "Past1M",
	},
	{
		label: "近3月",
		value: "Past3M",
	},
	{
		label: "近6月",
		value: "Past6M",
	},
	{
		label: "近1年",
		value: "Past1Y",
	},
	...(import.meta.env.DEV
		? [
				{
					label: "近3年",
					value: withDefineType<FaTableDataRange>("Past3Y"),
				},
			]
		: []),
];
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
