<template>
	<div>
		<FastTable tableKey="15USXWYRJ4" rowKey="recordId" :requestApi="sqlExecutionLogApi.querySqlExecutionLogPaged" />
	</div>
</template>

<script lang="ts" setup>
import { sqlExecutionLogApi } from "@/api/services/sqlExecutionLog";

defineOptions({
	name: "DevSqlExecutionLog",
});
</script>
