<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1D11KCYJJ9" rowKey="fileId" :requestApi="fileApi.queryFilePaged">
			<!-- 表格按钮操作区域 -->
			<template #header v-if="userInfoStore.isSuperAdmin">
				<TenantSelectPage
					width="280"
					@change="
						(value: ElSelectorOutput) => {
							if (value) {
								fastTableRef.searchParam.tenantId = value.value;
							} else {
								fastTableRef.searchParam.tenantId = undefined;
							}
							fastTableRef.refresh();
						}
					"
				/>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryFilePagedOutput }">
				<el-button v-if="state.imageMimeType.includes(row.fileMimeType)" size="small" plain @click="state.previewSrc = row.fileLocation">
					预览
				</el-button>
			</template>
		</FastTable>
		<el-image-viewer v-if="state.previewSrc" :urlList="[state.previewSrc]" showProgress @close="state.previewSrc = ''" />
	</div>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElSelectorOutput } from "fast-element-plus";
import { fileApi } from "@/api/services/File";
import { QueryFilePagedOutput } from "@/api/services/File/models/QueryFilePagedOutput";
import { FastTableInstance } from "@/components";
import { useUserInfo } from "@/stores";

defineOptions({
	name: "SystemFile",
});

const userInfoStore = useUserInfo();

const fastTableRef = ref<FastTableInstance>();

const state = reactive({
	imageMimeType: ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"],
	previewSrc: "",
});
</script>
