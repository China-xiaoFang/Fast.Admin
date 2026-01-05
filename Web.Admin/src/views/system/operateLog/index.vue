<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="15LKUKQQ6T" rowKey="recordId" :requestApi="operateLogApi.queryOperateLogPaged" stripe>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button plain type="danger" :icon="Delete" @click="handleDeleteLog">删除日志</el-button>
			</template>

			<template #mobile="{ row }: { row?: OperateLogModel }">
				{{ row.employeeName }}
				<br />
				工号：<span v-iconCopy="row.employeeNo">{{ row.employeeNo }}</span>
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
			</template>

			<template #os="{ row }: { row?: OperateLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: OperateLogModel }">
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
import { operateLogApi } from "@/api/services/operateLog";
import { OperateLogModel } from "@/api/services/operateLog/models/OperateLogModel";
import { FastTableInstance } from "@/components";
import { Delete } from "@element-plus/icons-vue";
import { dateUtil } from "@fast-china/utils";
import { dayjs, ElMessage, ElMessageBox } from "element-plus";
import { ref } from "vue";

defineOptions({
	name: "SystemOperateLog",
});

const fastTableRef = ref<FastTableInstance>();

/** 处理删除日志 */
const handleDeleteLog = () => {
	ElMessageBox.confirm("确定要删除90天前的操作日志？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await operateLogApi.deleteOperateLog();
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
