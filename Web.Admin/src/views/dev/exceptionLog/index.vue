<template>
	<div>
		<FastTable tableKey="1HDVHRLPGU" rowKey="recordId" :requestApi="exceptionLogApi.queryExceptionLogPaged" />
	</div>
</template>

<script lang="ts" setup>
import { exceptionLogApi } from "@/api/services/exceptionLog";

defineOptions({
	name: "DevExceptionLog",
});
</script>
