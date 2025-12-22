<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="LOG_LOGIN" rowKey="loginLogId" :requestApi="loginLogApi.queryLoginLogPaged"> </FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { loginLogApi } from "@/api/services/loginLog";

defineOptions({
	name: "LogLogin",
});

const fastTableRef = ref<FastTableInstance>();
</script>
