<template>
	<div>
		<FastTable tableKey="1DQCX6TWVL" rowKey="recordId" :requestApi="payRecordApi.queryPasswordMapPaged" stripe />
	</div>
</template>

<script lang="ts" setup>
import { payRecordApi } from "@/api/services/payRecord";

defineOptions({
	name: "SystemPayRecord",
});
</script>
