<template>
	<div>
		<FastTable tableKey="15U3GNPGNX" rowKey="recordId" :requestApi="sqlTimeoutLogApi.querySqlTimeoutLogPaged" />
	</div>
</template>

<script lang="ts" setup>
import { sqlTimeoutLogApi } from "@/api/services/sqlTimeoutLog";

defineOptions({
	name: "DevSqlTimeoutLog",
});
</script>
