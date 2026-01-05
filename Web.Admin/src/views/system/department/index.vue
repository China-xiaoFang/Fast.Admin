<template>
	<div>
		<div class="fa__display_lr-r">
			<FaTree
				ref="orgTreeRef"
				title="机构列表"
				width="240"
				:requestApi="organizationApi.organizationSelector"
				@change="handleOrgChange"
				@node-contextmenu="handleOrgContextmenu"
			>
				<template #label="{ data }: { data: ElTreeOutput<number> }">
					<span>{{ data.label }}</span>
				</template>
				<template #default="{ data }: { data: ElTreeOutput<number> }">
					<el-text type="info">{{ data.data.orgCode }}</el-text>
				</template>
			</FaTree>
			<FastTable
				ref="fastTableRef"
				tableKey="1DGHHUL3RL"
				rowKey="departmentId"
				:requestApi="departmentApi.queryDepartmentPaged"
				hideSearchTime
				:pagination="false"
				defaultExpandAll
				@custom-cell-click="handleCustomCellClick"
			>
				<!-- 表格按钮操作区域 -->
				<template #header>
					<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
				</template>

				<!-- 表格操作 -->
				<template #operation="{ row }: { row: QueryDepartmentPagedOutput }">
					<el-button size="small" plain @click="editFormRef.detail(row.departmentId)">详情</el-button>
					<el-button size="small" plain type="primary" @click="editFormRef.edit(row.departmentId)">编辑</el-button>
					<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
				</template>
			</FastTable>
		</div>
		<FaContextMenu ref="faContextMenuRef" :data="state.contextMenuList" />
		<DepartmentEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
		<OrgEdit ref="orgEditFormRef" @ok="orgTreeRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import DepartmentEdit from "./edit/index.vue";
import OrgEdit from "./edit/orgEdit.vue";
import type { FastTableInstance } from "@/components";
import { organizationApi } from "@/api/services/organization";
import { ElTreeOutput, FaContextMenuData, FaContextMenuInstance, FaTreeInstance } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import { departmentApi } from "@/api/services/department";
import { QueryDepartmentPagedOutput } from "@/api/services/department/models/QueryDepartmentPagedOutput";

defineOptions({
	name: "SystemDepartment",
});

const fastTableRef = ref<FastTableInstance>();
const orgTreeRef = ref<FaTreeInstance>();
const faContextMenuRef = ref<FaContextMenuInstance>();
const editFormRef = ref<InstanceType<typeof DepartmentEdit>>();
const orgEditFormRef = ref<InstanceType<typeof OrgEdit>>();

const state = reactive({
	contextMenuList: withDefineType<FaContextMenuData[]>([
		{
			name: "add",
			label: "添加机构",
			icon: "el-icon-FolderAdd",
			click: (_, { data }: { data?: ElTreeOutput<number> }) => {
				orgEditFormRef.value.add();
			},
		},
		{
			name: "edit",
			label: "编辑机构",
			icon: "el-icon-EditPen",
			click: (_, { data }: { data?: ElTreeOutput<number> }) => {
				orgEditFormRef.value.edit(data.value);
			},
		},
		{
			name: "delete",
			label: "删除机构",
			icon: "el-icon-Delete",
			click: (_, { data }: { data?: ElTreeOutput<number> }) => {
				ElMessageBox.confirm("确定要删除机构？", {
					type: "warning",
					async beforeClose(action, instance, done) {
						await organizationApi.deleteOrganization({ orgId: data.value, rowVersion: data.data?.rowVersion });
						ElMessage.success("删除成功！");
						fastTableRef.value?.refresh();
					},
				});
			},
		},
	]),
});

const handleCustomCellClick = (_, { row }: { row: QueryDepartmentPagedOutput }) => {
	editFormRef.value.detail(row.departmentId);
};

/** 机构更改 */
const handleOrgChange = (data: ElTreeOutput<number>) => {
	fastTableRef.value.searchParam.orgId = data.value;
	fastTableRef.value.refresh();
};

const handleOrgContextmenu = (event: MouseEvent, data: ElTreeOutput<number>) => {
	if (data.all) {
		state.contextMenuList[0].hide = false;
		state.contextMenuList[1].hide = true;
		state.contextMenuList[2].hide = true;
	} else {
		state.contextMenuList[0].hide = true;
		state.contextMenuList[1].hide = false;
		state.contextMenuList[2].hide = false;
	}
	state.contextMenuList.forEach((item) => {
		item.data = data;
	});
	faContextMenuRef.value.open({ x: event.clientX, y: event.clientY });
};

/** 处理删除 */
const handleDelete = (row: QueryDepartmentPagedOutput) => {
	const { departmentId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除部门？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await departmentApi.deleteDepartment({ departmentId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
