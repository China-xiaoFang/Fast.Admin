<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HDRYP6NFH" rowKey="serialRuleId" :requestApi="sysSerialApi.querySysSerialRulePaged" hideSearchTime>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QuerySysSerialRulePagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.serialRuleId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.serialRuleId)">编辑</el-button>
			</template>
		</FastTable>
		<SysSerialEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { Plus } from "@element-plus/icons-vue";
import SysSerialEdit from "./edit/index.vue";
import type { QuerySysSerialRulePagedOutput } from "@/api/services/sysSerial/models/QuerySysSerialRulePagedOutput";
import type { FastTableInstance } from "@/components";
import { sysSerialApi } from "@/api/services/sysSerial";

defineOptions({
	name: "DevSysSerial",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof SysSerialEdit>>();
</script>
