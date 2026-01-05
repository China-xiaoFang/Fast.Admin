<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="151636M25M"
			rowKey="roleId"
			:requestApi="roleApi.queryRolePaged"
			hideSearchTime
			@custom-cell-click="(_, { row }: { row: QueryRolePagedOutput }) => editFormRef.detail(row.roleId)"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryRolePagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.roleId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.roleId)">编辑</el-button>
				<el-button v-if="row.roleType === RoleTypeEnum.Normal" size="small" plain type="success" @click="authEditRef.open(row.roleId)">
					授权
				</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FastTable>
		<RoleEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
		<AuthEdit ref="authEditRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import RoleEdit from "./edit/index.vue";
import AuthEdit from "./edit/authEdit.vue";
import type { FastTableInstance } from "@/components";
import { roleApi } from "@/api/services/role";
import { QueryRolePagedOutput } from "@/api/services/role/models/QueryRolePagedOutput";
import { RoleTypeEnum } from "@/api/enums/RoleTypeEnum";

defineOptions({
	name: "SystemRole",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof RoleEdit>>();
const authEditRef = ref<InstanceType<typeof AuthEdit>>();

/** 处理删除 */
const handleDelete = (row: QueryRolePagedOutput) => {
	const { roleId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除角色？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await roleApi.deleteRole({ roleId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
