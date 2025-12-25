<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HDT5H3D62" :requestApi="passwordMapApi.queryPasswordMapPaged" hideSearchTime />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { passwordMapApi } from "@/api/services/passwordMap";

defineOptions({
	name: "DevPasswordMap",
});

const fastTableRef = ref<FastTableInstance>();
</script>
