<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HDT1KXNTZ" rowKey="recordId" :requestApi="passwordRecordApi.queryPasswordRecordPaged" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { passwordRecordApi } from "@/api/services/passwordRecord";

defineOptions({
	name: "DevPasswordRecord",
});

const fastTableRef = ref<FastTableInstance>();
</script>
