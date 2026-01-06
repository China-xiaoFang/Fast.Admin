<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1D1FT74TNX"
			rowKey="tenantId"
			:requestApi="tenantApi.queryTenantPaged"
			hideSearchTime
			@customCellClick="handleCustomCellClick"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button v-auth="'Tenant:Add'" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryTenantPagedOutput }">
				<el-button v-auth="'Tenant:Detail'" size="small" plain @click="editFormRef.detail(row.tenantId)">详情</el-button>
				<el-button v-auth="'Tenant:Edit'" size="small" plain type="primary" @click="editFormRef.edit(row.tenantId)">编辑</el-button>
				<el-button
					v-if="row.status == CommonStatusEnum.Enable"
					v-auth="'Tenant:Status'"
					size="small"
					plain
					type="danger"
					@click="handleChangeStatus(row)"
				>
					禁用
				</el-button>
				<el-button v-else v-auth="'Tenant:Status'" size="small" plain type="warning" @click="handleChangeStatus(row)">启用</el-button>
			</template>
		</FastTable>
		<TenantEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import TenantEdit from "./edit/index.vue";
import type { QueryTenantPagedOutput } from "@/api/services/tenant/models/QueryTenantPagedOutput";
import type { FastTableInstance } from "@/components";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { tenantApi } from "@/api/services/tenant";

defineOptions({
	name: "SystemTenant",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof TenantEdit>>();

const handleCustomCellClick = (_, { row }: { row: QueryTenantPagedOutput }) => {
	editFormRef.value.detail(row.tenantId);
};

/** 处理状态变更 */
const handleChangeStatus = (row: QueryTenantPagedOutput) => {
	const { tenantId, status, rowVersion } = row;
	ElMessageBox.confirm(`确定${status === CommonStatusEnum.Enable ? "禁用" : "启用"}租户？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await tenantApi.changeStatus({
				tenantId,
				rowVersion,
			});
			ElMessage.success("操作成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
