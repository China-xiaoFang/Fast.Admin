<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1D1KQSM6QW" rowKey="complaintId" :requestApi="complaintApi.queryTenantComplaintPaged" hideSearchTime>
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
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import TenantComplaintEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import { complaintApi } from "@/api/services/complaint";
import { QueryComplaintPagedOutput } from "@/api/services/complaint/models/QueryComplaintPagedOutput";

defineOptions({
	name: "SystemTenantComplaint",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof TenantComplaintEdit>>();
</script>
