<template>
	<div>
		<FastTable
			ref="fastTableRef"
			table-key="1D11MFM59S"
			row-key="serialRuleId"
			:request-api="sysSerialApi.querySysSerialRulePaged"
			hide-search-time
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button v-auth="'SysSerial:Add'" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QuerySysSerialRulePagedOutput }">
				<el-button v-auth="'SysSerial:Detail'" size="small" plain @click="editFormRef.detail(row.serialRuleId)">详情</el-button>
				<el-button v-auth="'SysSerial:Edit'" size="small" plain type="primary" @click="editFormRef.edit(row.serialRuleId)">编辑</el-button>
			</template>
		</FastTable>
		<SysSerialEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { useTemplateRef } from "vue";
import { Plus } from "@element-plus/icons-vue";
import { sysSerialApi } from "@/api/services/Center/sysSerial";
import SysSerialEdit from "./edit/index.vue";
import type { QuerySysSerialRulePagedOutput } from "@/api/services/Center/sysSerial/models/QuerySysSerialRulePagedOutput";
import type { FastTableInstance } from "@/components";

defineOptions({
	name: "DevSysSerial",
});

const fastTableRef = useTemplateRef<FastTableInstance>("fastTableRef");
const editFormRef = useTemplateRef<InstanceType<typeof SysSerialEdit>>("editFormRef");
</script>
