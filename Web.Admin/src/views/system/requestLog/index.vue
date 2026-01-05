<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="157RY61K1T" rowKey="recordId" :requestApi="requestLogApi.queryRequestLogPaged" stripe>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button plain type="danger" :icon="Delete" @click="handleDeleteLog">删除日志</el-button>
			</template>

			<template #mobile="{ row }: { row?: RequestLogModel }">
				{{ row.nickName }}
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
				<br />
				账号：<span v-iconCopy="row.account">{{ row.account }}</span>
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
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>

			<template #param="{ row }: { row?: RequestLogModel }">
				<el-tag
					v-if="row.param"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '请求参数';
							state.content = row.param || '';
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
					v-if="row.result"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '返回结果';
							state.content = row.result || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
		</FastTable>
		<el-dialog v-model="state.visible" :title="state.title" width="700px" alignCenter draggable destroyOnClose>
			<el-scrollbar>
				<div style="max-height: 500px; padding-bottom: 20px; padding-right: 10px" v-html="state.content" />
			</el-scrollbar>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { requestLogApi } from "@/api/services/requestLog";
import { RequestLogModel } from "@/api/services/requestLog/models/RequestLogModel";
import { FastTableInstance } from "@/components";
import { Delete } from "@element-plus/icons-vue";
import { dateUtil } from "@fast-china/utils";
import { dayjs, ElMessage, ElMessageBox } from "element-plus";
import { reactive, ref } from "vue";

defineOptions({
	name: "SystemRequestLog",
});

const fastTableRef = ref<FastTableInstance>();

const state = reactive({
	visible: false,
	title: "日志",
	content: "",
});

/** 处理删除日志 */
const handleDeleteLog = () => {
	ElMessageBox.confirm("确定要删除90天前的请求日志？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await requestLogApi.deleteRequestLog();
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
