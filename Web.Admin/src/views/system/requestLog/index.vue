<template>
	<div>
		<FastTable tableKey="1D1KMSURSS" rowKey="recordId" :requestApi="requestLogApi.queryRequestLogPaged" stripe>
			<template #mobile="{ row }: { row?: RequestLogModel }">
				{{ row.nickName }}
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
			</template>

			<template #os="{ row }: { row?: RequestLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: RequestLogModel }">
				<span>地区：{{ row.province }} - {{ row.city }}</span>
				<br />
				<span>Ip：{{ row.ip }}</span>
				<br />
				<span>时间：{{ dayjs(row.createdTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small" class="ml5">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>

			<template #param="{ row }: { row?: RequestLogModel }">
				<el-tag
					v-if="row.param && (userInfoStore.isSuperAdmin || userInfoStore.isAdmin)"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '请求参数';
							state.content = row.param || '';
							state.decrypt = false;
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>

			<template #result="{ row }: { row?: RequestLogModel }">
				<el-tag
					v-if="row.result && (userInfoStore.isSuperAdmin || userInfoStore.isAdmin)"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '返回结果';
							state.content = row.result || '';
							state.decrypt = true;
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
		</FastTable>
		<el-dialog v-model="state.visible" :title="state.title" width="1000px" alignCenter draggable destroyOnClose>
			<el-scrollbar>
				<div style="max-height: 500px; padding-bottom: 20px; padding-right: 10px">
					<VueJsonPretty
						:data="jsonContent"
						:deep="3"
						showLength
						showLineNumber
						showIcon
						virtual
						:height="500"
						:theme="configStore.layout.isDark ? 'dark' : 'light'"
					/>
				</div>
			</el-scrollbar>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { computed, reactive } from "vue";
import { dayjs } from "element-plus";
import { cryptoUtil, dateUtil } from "@fast-china/utils";
import VueJsonPretty from "vue-json-pretty";
import { requestLogApi } from "@/api/services/Center/requestLog";
import { RequestLogModel } from "@/api/services/Center/requestLog/models/RequestLogModel";
import { useConfig, useUserInfo } from "@/stores";

defineOptions({
	name: "SystemRequestLog",
});

const configStore = useConfig();
const userInfoStore = useUserInfo();

const state = reactive({
	visible: false,
	decrypt: false,
	title: "日志",
	content: "",
});

const jsonContent = computed(() => {
	try {
		const result = JSON.parse(state.content);
		try {
			if (state.decrypt && result?.value && result?.value?.timestamp && result?.value?.data) {
				result.value.data = cryptoUtil.aes.decrypt(result.value.data, `${result.value.timestamp}`, `FIV${result.value.timestamp}`);
				// 处理 ""xxx"" 这种数据
				if (typeof result.value.data === "string" && result.value.data.startsWith('"') && result.value.data.endsWith('"')) {
					result.value.data = result.value.data.replace(/"/g, "");
				}
			}
			return result;
		} catch {
			return result;
		}
	} catch {
		return state.content;
	}
});
</script>
