<template>
	<div>
		<FastTable
			ref="fastTableRef"
			table-key="1D1KQSM6QW"
			row-key="complaintId"
			:request-api="complaintApi.queryTenantComplaintPaged"
			hide-search-time
		>
			<template #attachmentImages="{ row }: { row?: QueryComplaintPagedOutput }">
				<el-button size="small" plain @click="state.previewSrcList = row.attachmentImages"> 查看 </el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryComplaintPagedOutput }">
				<el-button
					v-if="!row.handleTime"
					v-auth="'Complaint:TenantHandle'"
					size="small"
					plain
					type="primary"
					@click="editFormRef.open(row.complaintId)"
				>
					处理
				</el-button>
			</template>
		</FastTable>
		<TenantComplaintEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
		<el-image-viewer
			v-if="state.previewSrcList.length > 0"
			:url-list="state.previewSrcList"
			hide-on-click-modal
			teleported
			show-progress
			@close="state.previewSrcList = []"
		/>
	</div>
</template>

<script lang="ts" setup>
import { reactive, useTemplateRef } from "vue";
import { complaintApi } from "@/api/services/Center/complaint";
import TenantComplaintEdit from "./edit/index.vue";
import type { QueryComplaintPagedOutput } from "@/api/services/Center/complaint/models/QueryComplaintPagedOutput";
import type { FastTableInstance } from "@/components";

defineOptions({
	name: "SystemTenantComplaint",
});

const fastTableRef = useTemplateRef<FastTableInstance>("fastTableRef");
const editFormRef = useTemplateRef<InstanceType<typeof TenantComplaintEdit>>("editFormRef");

const state = reactive({
	previewSrcList: [],
});
</script>
