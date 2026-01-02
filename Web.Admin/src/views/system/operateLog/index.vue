<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="15LKUKQQ6T" rowKey="recordId" :requestApi="operateLogApi.queryOperateLogPaged" stripe>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button plain type="danger" :icon="Delete" @click="handleDeleteLog">删除日志</el-button>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { operateLogApi } from "@/api/services/operateLog";
import { FastTableInstance } from "@/components";
import { Delete } from "@element-plus/icons-vue";
import { ElMessage, ElMessageBox } from "element-plus";
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
