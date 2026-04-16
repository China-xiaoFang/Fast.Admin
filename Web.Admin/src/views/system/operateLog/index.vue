<template>
	<div>
		<FastTable tableKey="1D1KHQS53T" rowKey="recordId" :requestApi="operateLogApi.queryOperateLogPaged" stripe>
			<template #employeeNo="{ row }: { row?: OperateLogModel }">
				{{ row.createdUserName }}
				<br />
				工号：<span v-iconCopy="row.employeeNo">{{ row.employeeNo }}</span>
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
			</template>

			<template #os="{ row }: { row?: OperateLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: OperateLogModel }">
				<span>地区：{{ row.province }} - {{ row.city }}</span>
				<br />
				<span>Ip：{{ row.ip }}</span>
				<br />
				<span>时间：{{ dayjs(row.createdTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small" class="ml5">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>
		</FastTable>
	</div>
</template>

<script lang="ts" setup>
import { dayjs } from "element-plus";
import { dateUtil } from "@fast-china/utils";
import { operateLogApi } from "@/api/services/Admin/operateLog";
import { OperateLogModel } from "@/api/services/Admin/operateLog/models/OperateLogModel";

defineOptions({
	name: "SystemOperateLog",
});
</script>
