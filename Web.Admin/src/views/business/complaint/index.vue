<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="BUSINESS_COMPLAINT" rowKey="complaintId" :requestApi="complaintApi.queryComplaintPaged">
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="handleView(row)">查看</el-button>
				<el-button v-if="!row.handleTime" size="small" plain type="success" @click="handleProcess(row)">处理</el-button>
			</template>
		</FastTable>
		<HandleDialog ref="handleDialogRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import HandleDialog from "./handle/index.vue";
import type { QueryComplaintPagedOutput } from "@/api/services/complaint/models/QueryComplaintPagedOutput";
import type { FastTableInstance } from "@/components";
import { complaintApi } from "@/api/services/complaint";

defineOptions({
	name: "BusinessComplaint",
});

const fastTableRef = ref<FastTableInstance>();
const handleDialogRef = ref<InstanceType<typeof HandleDialog>>();

/** 查看详情 */
const handleView = (row: QueryComplaintPagedOutput) => {
	handleDialogRef.value?.detail(row);
};

/** 处理投诉 */
const handleProcess = (row: QueryComplaintPagedOutput) => {
	handleDialogRef.value?.handle(row);
};
</script>
