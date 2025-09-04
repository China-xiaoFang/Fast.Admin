<template>
	<suspense>
		<template #default> </template>
		<template #fallback> </template>
	</suspense>
</template>

<script setup lang="ts">
defineOptions({
	name: "LayoutAsync",
});
</script>

<style scoped lang="scss">
.layout {
	width: 100%;
	height: 100%;
	min-width: 1024px;
}
</style>
