<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="15L1KF38X7" rowKey="recordId" :requestApi="visitLogApi.queryVisitLogPaged" stripe>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button plain type="danger" :icon="Delete" @click="handleDeleteLog">删除日志</el-button>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { visitLogApi } from "@/api/services/visitLog";
import { FastTableInstance } from "@/components";
import { Delete } from "@element-plus/icons-vue";
import { ElMessage, ElMessageBox } from "element-plus";
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
