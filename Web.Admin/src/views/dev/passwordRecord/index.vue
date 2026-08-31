<template>
	<div>
		<FastTable table-key="1DFGFQQD95" row-key="recordId" :request-api="passwordRecordApi.queryPasswordRecordPaged" />
	</div>
</template>

<script lang="ts" setup>
import { passwordRecordApi } from "@/api/services/Center/passwordRecord";

defineOptions({
	name: "DevPasswordRecord",
});
</script>
