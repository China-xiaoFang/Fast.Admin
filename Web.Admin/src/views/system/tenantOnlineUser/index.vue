<template>
	<div>
		<FastTable
			ref="fastTableRef"
			tableKey="1D1K3NW4XY"
			rowKey="connectionId"
			:requestApi="tenantOnlineUserApi.queryTenantOnlineUserPaged"
			hideSearchTime
		>
			<template #mobile="{ row }: { row?: TenantOnlineUserModel }">
				<span>{{ row.mobile }}</span>
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
			</template>

			<template #employeeNo="{ row }: { row?: TenantOnlineUserModel }">
				<span>{{ row.employeeName }}</span>
				<br />
				工号：<span v-iconCopy="row.employeeNo">{{ row.employeeNo }}</span>
				<br />
				部门：<span>{{ row.departmentName }}</span>
			</template>

			<template #appNo="{ row }: { row?: TenantOnlineUserModel }">
				<span>{{ row.appName }}</span>
				<br />
				编号：<span v-iconCopy="row.employeeNo">{{ row.appNo }}</span>
			</template>

			<template #lastLoginTime="{ row }: { row?: TenantOnlineUserModel }">
				<span>地区：{{ row.lastLoginProvince }} - {{ row.lastLoginCity }}</span>
				<br />
				<span>Ip：{{ row.lastLoginIp }}</span>
				<br />
				<span>时间：{{ dayjs(row.lastLoginTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.lastLoginTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.lastLoginTime)) }}
				</el-tag>
			</template>

			<template #lastLoginOS="{ row }: { row?: TenantOnlineUserModel }">
				<span>设备：{{ row.lastLoginDevice }}</span>
				<br />
				<span>操作系统：{{ row.lastLoginOS }}</span>
				<br />
				<span>浏览器：{{ row.lastLoginTime }}</span>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: TenantOnlineUserModel }">
				<el-button size="small" plain type="warning" @click="handleForceOffline(row)">强制下线</el-button>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { dayjs, ElMessage, ElMessageBox } from "element-plus";
import { dateUtil } from "@fast-china/utils";
import { tenantOnlineUserApi } from "@/api/services/tenantOnlineUser";
import { TenantOnlineUserModel } from "@/api/services/tenantOnlineUser/models/TenantOnlineUserModel";

defineOptions({
	name: "SystemTenantOnlineUser",
});

const fastTableRef = ref<FastTableInstance>();

/** 处理重置密码 */
const handleForceOffline = (row: TenantOnlineUserModel) => {
	const { connectionId, mobile } = row;
	ElMessageBox.confirm(`确定踢掉账号：【${mobile}】`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await tenantOnlineUserApi.forceOffline({
				connectionId,
			});
			ElMessage.success("强制下线成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
