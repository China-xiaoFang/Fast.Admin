<template>
	<div>
		<FastTable tableKey="1DFGFQQD95" rowKey="recordId" :requestApi="passwordRecordApi.queryPasswordRecordPaged" />
	</div>
</template>

<script lang="ts" setup>
import { passwordRecordApi } from "@/api/services/Center/passwordRecord";

defineOptions({
	name: "DevPasswordRecord",
});
</script>
