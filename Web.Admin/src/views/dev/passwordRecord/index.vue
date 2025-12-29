<template>
	<div>
		<FastTable tableKey="1HDT1KXNTZ" rowKey="recordId" :requestApi="passwordRecordApi.queryPasswordRecordPaged" />
	</div>
</template>

<script lang="ts" setup>
import { passwordRecordApi } from "@/api/services/passwordRecord";

defineOptions({
	name: "DevPasswordRecord",
});
</script>
