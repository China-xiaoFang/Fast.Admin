<template>
	<div>
		<FaTable
			ref="faTableRef"
			rowKey="departmentId"
			:columns="tableColumns"
			:requestApi="departmentApi.queryDepartmentPaged"
			hideSearchTime
			:tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.departmentId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.departmentId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FaTable>
		<DepartmentEdit ref="editFormRef" @ok="faTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import DepartmentEdit from "./edit/index.vue";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { departmentApi } from "@/api/services/department";

defineOptions({
	name: "SystemDepartment",
});

const faTableRef = ref<FaTableInstance>();
const editFormRef = ref<InstanceType<typeof DepartmentEdit>>();

/** 处理删除 */
const handleDelete = (row: any) => {
	const { departmentId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除该部门？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await departmentApi.deleteDepartment({ departmentId, rowVersion });
				ElMessage.success("删除成功！");
				faTableRef.value?.refresh();
				done();
			} else {
				done();
			}
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "departmentName",
		label: "部门名称",
		sortable: true,
		link: true,
		click({ row }) {
			editFormRef.value.detail(row.departmentId);
		},
		fixed: "left",
		width: 200,
	},
	{
		prop: "departmentCode",
		label: "部门编码",
		sortable: true,
		copy: true,
		width: 200,
	},
	{
		prop: "leader",
		label: "负责人",
		sortable: true,
		width: 150,
	},
	{
		prop: "phone",
		label: "联系电话",
		sortable: true,
		width: 150,
	},
	{
		prop: "status",
		label: "状态",
		sortable: true,
		tag: true,
		enum: [
			{ label: "正常", value: 1, type: "primary" },
			{ label: "禁用", value: 2, type: "danger" },
		],
		width: 100,
	},
	{
		prop: "remark",
		label: "备注",
		sortable: true,
		width: 280,
	},
	{
		type: "submitInfo",
		prop: "createdTime",
		label: "创建时间",
		sortable: true,
		width: 180,
	},
]);
</script>
