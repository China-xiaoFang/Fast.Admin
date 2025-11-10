<template>
	<view class="fa-search-input">
		<view class="fa-search-input__header">
			<slot name="prefix" />
			<wd-input
				v-model="modelValue"
				:placeholder="props.placeholder"
				clearable
				noBorder
				@input="handleInput"
				@focus="(detail) => emit('focus', detail)"
				@blur="(detail) => emit('blur', detail)"
				@confirm="(detail) => emit('confirm', detail)"
				@clear="handleClear"
			>
				<template #prefix>
					<wd-icon customClass="wd-input__icon" classPrefix="iconfont" name="scan" @click="handleScanClick" />
				</template>
			</wd-input>
			<view v-if="props.filter" class="fa-search-input__filter" @click="state.filterVisible = true">
				<wd-icon customClass="wd-input__icon" classPrefix="iconfont" name="filter" />
				<text class="fa-search-input__filter-text">筛选</text>
			</view>
			<slot name="suffix" />
		</view>
		<view class="fa-search-input__footer">
			<slot name="footer" />
		</view>
		<wd-popup
			v-if="props.filter"
			customClass="fa-search-popup"
			v-model="state.filterVisible"
			closable
			position="bottom"
			transition="fade-up"
			safeAreaInsetBottom
		>
			<text class="fa-search-popup__title">高级筛选</text>
			<view class="fa-search-popup__warp">
				<template v-if="!props.hideSearchTime">
					<wd-datetime-picker
						customClass="fa-search-input__date-picker"
						:disable="state.dataSearchRange !== 'custom'"
						type="date"
						label="时间"
						v-model="state.searchTimeList"
						:defaultValue="dateUtil.getDefaultTime().map((m) => m.getTime())"
						alignRight
						@confirm="handleDateTimeConfirm"
					/>
					<wd-radio-group
						class="fa-search-input__date-range"
						v-model="state.dataSearchRange"
						cell
						shape="button"
						@change="handleDateRangeChange"
					>
						<view class="fa-search-input__date-range__warp">
							<wd-radio v-for="(item, index) in dataSearchRangeList" :key="index" :value="item.value">
								{{ item.label }}
							</wd-radio>
							<wd-radio value="custom">自定义</wd-radio>
						</view>
					</wd-radio-group>
				</template>
				<slot name="filter" />
			</view>
			<view class="fa-search-popup__footer">
				<wd-button type="info" plain block :round="false" @click="state.filterVisible = false">取消</wd-button>
				<wd-button type="primary" block :round="false" @click="handleFilterClick">筛选</wd-button>
			</view>
		</wd-popup>
	</view>
</template>

<script setup lang="ts">
import { onMounted, reactive, useModel } from "vue";
import { dateUtil, definePropType, withDefineType } from "@fast-china/utils";
import dayjs from "dayjs";
import { useConfig } from "@/stores";

defineOptions({
	name: "SearchInput",
	options: {
		virtualHost: true,
		addGlobalClass: true,
		styleIsolation: "shared",
	},
});

const props = defineProps({
	/** @description v-model绑定值 */
	modelValue: String,
	/** @description 搜索参数 */
	searchParam: {
		type: definePropType<any>(Object),
		default: {},
	},
	/** @description 占位文本 */
	placeholder: {
		type: String,
		default: "请输入或扫描您想要搜索的内容",
	},
	/** @description 显示筛选图标 */
	filter: {
		type: Boolean,
		default: true,
	},
	/** @description 隐藏搜索时间 */
	hideSearchTime: Boolean,
});

const emit = defineEmits({
	/** @description v-model 回调 */
	"update:modelValue": (value: string) => (boolean) => true,
	/** @description v-model:searchParam 回调 */
	"update:searchParam": (value: any) => (boolean) => true,
	/** @description 监听输入框input事件 */
	input: (detail: UniHelper.InputOnInputDetail): boolean => true,
	/** @description 监听输入框focus事件 */
	focus: (detail: UniHelper.InputOnFocusDetail): boolean => true,
	/** @description 监听输入框blur事件 */
	blur: (detail: UniHelper.InputOnBlurDetail): boolean => true,
	/** @description 点击完成时， 触发 confirm 事件 */
	confirm: (detail: UniHelper.InputOnConfirmDetail): boolean => true,
	/** @description 监听输入框清空按钮事件 */
	clear: (): boolean => true,
	/** @description 弹窗筛选按钮点击事件 */
	search: (): boolean => true,
});

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
];

const configStore = useConfig();

const modelValue = useModel(props, "modelValue");
const searchParam = useModel(props, "searchParam");

const state = reactive({
	filterVisible: false,
	searchTimeList: withDefineType<number[]>([]),
	dataSearchRange: withDefineType<FaTableDataRange | "custom">(),
});

const handleScanClick = () => {
	uni.scanCode({
		onlyFromCamera: true,
		scanType: ["barCode", "qrCode", "datamatrix", "pdf417"],
		success: (res) => {
			modelValue.value = res.result;
			searchParam.value.searchValue = res.result;
		},
	});
};

const handleInput = (detail: UniHelper.InputOnInputDetail) => {
	searchParam.value.searchValue = detail.value;
	emit("input", detail);
};

const handleClear = () => {
	searchParam.value.searchValue = "";
	emit("clear");
};

const handleFilterClick = () => {
	state.filterVisible = false;
	emit("search");
};

const getDateTime = (dataRange: FaTableDataRange | "custom") => {
	const end = new Date();
	const start = new Date();
	switch (dataRange) {
		case "Past1D":
			start.setDate(start.getDate() - 1);
			break;
		case "Past3D":
			start.setDate(start.getDate() - 3);
			break;
		case "Past1W":
			start.setDate(start.getDate() - 7);
			break;
		case "Past1M":
			start.setMonth(start.getMonth() - 1);
			break;
		case "Past3M":
			start.setMonth(start.getMonth() - 3);
			break;
		case "Past6M":
			start.setMonth(start.getMonth() - 6);
			break;
		case "Past1Y":
			start.setFullYear(start.getFullYear() - 1);
			break;
		case "Past3Y":
			start.setFullYear(start.getFullYear() - 3);
			break;
	}
	return {
		start,
		end,
	};
};

const handleDateTimeConfirm = ({ value }: { value: number[] }) => {
	searchParam.value.searchTimeList = [dayjs(value[0]).format("YYYY-MM-DD 00:00:00"), dayjs(value[1]).format("YYYY-MM-DD 23:59:59")];
};

const handleDateRangeChange = ({ value }: { value: FaTableDataRange | "custom" }) => {
	const { start, end } = getDateTime(value);
	state.searchTimeList = [start.getTime(), end.getTime()];
	searchParam.value.searchTimeList = [dayjs(start).format("YYYY-MM-DD 00:00:00"), dayjs(end).format("YYYY-MM-DD 23:59:59")];
};

const defaultSearchTime = () => {
	if (!props.filter || props.hideSearchTime) {
		state.searchTimeList = [];
		searchParam.value.searchTimeList = undefined;
	} else {
		const { start, end } = getDateTime(configStore.tableLayout.dataSearchRange);
		state.dataSearchRange = configStore.tableLayout.dataSearchRange;
		state.searchTimeList = [start.getTime(), end.getTime()];
		searchParam.value.searchTimeList = [dayjs(start).format("YYYY-MM-DD 00:00:00"), dayjs(end).format("YYYY-MM-DD 23:59:59")];
	}
};

onMounted(() => {
	defaultSearchTime();
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
