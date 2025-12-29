<template>
	<div>
		<FastTable tableKey="1HDT5H3D62" :requestApi="passwordMapApi.queryPasswordMapPaged" hideSearchTime />
	</div>
</template>

<script lang="ts" setup>
import { passwordMapApi } from "@/api/services/passwordMap";

defineOptions({
	name: "DevPasswordMap",
});
</script>
