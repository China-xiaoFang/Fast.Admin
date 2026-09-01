<template>
	<div>
		<FastTable ref="fastTableRef" table-key="1D11KCYJJ9" row-key="fileId" :request-api="fileApi.queryFilePaged">
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
		<el-image-viewer
			v-if="state.previewSrc"
			:url-list="[state.previewSrc]"
			hide-on-click-modal
			teleported
			show-progress
			@close="state.previewSrc = ''"
		/>
	</div>
</template>

<script lang="ts" setup>
import { reactive, useTemplateRef } from "vue";
import { fileApi } from "@/api/services/File";
import { useUserInfo } from "@/stores";
import type { ElSelectorOutput } from "fast-element-plus";
import type { QueryFilePagedOutput } from "@/api/services/File/models/QueryFilePagedOutput";
import type { FastTableInstance } from "@/components";

defineOptions({
	name: "SystemFile",
});

const userInfoStore = useUserInfo();

const fastTableRef = useTemplateRef<FastTableInstance>("fastTableRef");

const state = reactive({
	imageMimeType: ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"],
	previewSrc: "",
});
</script>
