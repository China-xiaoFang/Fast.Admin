<template>
	<div>
		<FastTable tableKey="15U9JVNPS8" rowKey="recordId" :requestApi="sqlDiffLogApi.querySqlDiffLogPaged" />
	</div>
</template>

<script lang="ts" setup>
import { sqlDiffLogApi } from "@/api/services/sqlDiffLog";

defineOptions({
	name: "DevSqlDiffLog",
});
</script>
