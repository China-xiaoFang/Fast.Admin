<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1D1K2Z66L4" rowKey="recordId" :requestApi="visitLogApi.queryVisitLogPaged" stripe>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button plain type="danger" :icon="Delete" @click="handleDeleteLog">删除日志</el-button>
			</template>

			<template #mobile="{ row }: { row?: VisitLogModel }">
				{{ row.nickName }}
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
				<br />
				账号：<span v-iconCopy="row.account">{{ row.account }}</span>
			</template>

			<template #os="{ row }: { row?: VisitLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: VisitLogModel }">
				<span>地区：{{ row.province }} - {{ row.city }}</span>
				<br />
				<span>Ip：{{ row.ip }}</span>
				<br />
				<span>时间：{{ dayjs(row.createdTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { visitLogApi } from "@/api/services/visitLog";
import { VisitLogModel } from "@/api/services/visitLog/models/VisitLogModel";
import { FastTableInstance } from "@/components";
import { Delete } from "@element-plus/icons-vue";
import { dateUtil } from "@fast-china/utils";
import { dayjs, ElMessage, ElMessageBox } from "element-plus";
import { ref } from "vue";

defineOptions({
	name: "SystemVisitLog",
});

const fastTableRef = ref<FastTableInstance>();

/** 处理删除日志 */
const handleDeleteLog = () => {
	ElMessageBox.confirm("确定要删除90天前的访问日志？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await visitLogApi.deleteVisitLog();
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
