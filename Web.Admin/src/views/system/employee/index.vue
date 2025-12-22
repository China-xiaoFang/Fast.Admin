<template>
	<div>
		<FaTable
			ref="faTableRef"
			rowKey="employeeId"
			:columns="tableColumns"
			:requestApi="employeeApi.queryEmployeePaged"
			hideSearchTime
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.employeeId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.employeeId)">编辑</el-button>
				<el-button size="small" plain type="success" @click="setOrgFormRef.open(row.employeeId, row.employeeName)">设置组织</el-button>
				<el-button size="small" plain type="warning" @click="setRoleFormRef.open(row.employeeId, row.employeeName)">设置角色</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FaTable>
		<EmployeeEdit ref="editFormRef" @ok="faTableRef.refresh()" />
		<EmployeeSetOrg ref="setOrgFormRef" />
		<EmployeeSetRole ref="setRoleFormRef" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import EmployeeEdit from "./edit/index.vue";
import EmployeeSetOrg from "./setOrg/index.vue";
import EmployeeSetRole from "./setRole/index.vue";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { employeeApi } from "@/api/services/employee";

defineOptions({
	name: "SystemEmployee",
});

const faTableRef = ref<FaTableInstance>();
const editFormRef = ref<InstanceType<typeof EmployeeEdit>>();
const setOrgFormRef = ref<InstanceType<typeof EmployeeSetOrg>>();
const setRoleFormRef = ref<InstanceType<typeof EmployeeSetRole>>();

/** 处理删除 */
const handleDelete = (row: any) => {
	const { employeeId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除该职员？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await employeeApi.deleteEmployee({ employeeId, rowVersion });
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
		prop: "employeeName",
		label: "职员姓名",
		sortable: true,
		link: true,
		click({ row }) {
			editFormRef.value.detail(row.employeeId);
		},
		fixed: "left",
		width: 150,
	},
	{
		prop: "employeeNo",
		label: "工号",
		sortable: true,
		copy: true,
		width: 150,
	},
	{
		prop: "phone",
		label: "手机号",
		sortable: true,
		width: 150,
	},
	{
		prop: "email",
		label: "邮箱",
		sortable: true,
		width: 200,
	},
	{
		prop: "gender",
		label: "性别",
		sortable: true,
		tag: true,
		enum: [
			{ label: "男", value: 1, type: "primary" },
			{ label: "女", value: 2, type: "danger" },
		],
		width: 80,
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
