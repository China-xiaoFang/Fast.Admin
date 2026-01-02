<template>
	<div>
		<FastTable tableKey="15UVTXHNBW" rowKey="recordId" :requestApi="sqlExceptionLogApi.querySqlExceptionLogPaged" />
	</div>
</template>

<script lang="ts" setup>
import { sqlExceptionLogApi } from "@/api/services/sqlExceptionLog";

defineOptions({
	name: "DevSqlExceptionLog",
});
</script>
