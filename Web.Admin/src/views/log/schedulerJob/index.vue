<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="LOG_SCHEDULER_JOB"
			rowKey="schedulerJobLogId"
			:requestApi="schedulerJobLogApi.querySchedulerJobLogPaged"
		>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { schedulerJobLogApi } from "@/api/services/schedulerJobLog";

defineOptions({
	name: "LogSchedulerJob",
});

const fastTableRef = ref<FastTableInstance>();
</script>
