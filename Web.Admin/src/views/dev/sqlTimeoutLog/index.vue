<template>
	<div>
		<FastTable tableKey="15U3GNPGNX" rowKey="recordId" :requestApi="sqlTimeoutLogApi.querySqlTimeoutLogPaged" stripe>
			<template #mobile="{ row }: { row?: SqlTimeoutLogModel }">
				{{ row.nickName }}
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
				<br />
				账号：<span v-iconCopy="row.account">{{ row.account }}</span>
			</template>

			<template #os="{ row }: { row?: SqlTimeoutLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: SqlTimeoutLogModel }">
				<span>地区：{{ row.province }} - {{ row.city }}</span>
				<br />
				<span>Ip：{{ row.ip }}</span>
				<br />
				<span>时间：{{ dayjs(row.createdTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>

			<template #rawSql="{ row }: { row?: SqlTimeoutLogModel }">
				<el-tag
					v-if="row.rawSql"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '原始Sql';
							state.content = row.rawSql || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>

			<template #parameters="{ row }: { row?: SqlTimeoutLogModel }">
				<el-tag
					v-if="row.parameters"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = 'Sql参数';
							state.content = row.parameters || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>

			<template #pureSql="{ row }: { row?: SqlTimeoutLogModel }">
				<el-tag
					v-if="row.pureSql"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '纯Sql';
							state.content = row.pureSql || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
		</FastTable>
		<el-dialog v-model="state.visible" :title="state.title" width="700px" alignCenter draggable destroyOnClose>
			<el-scrollbar>
				<div style="max-height: 500px; padding-bottom: 20px; padding-right: 10px" v-html="state.content" />
			</el-scrollbar>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { sqlTimeoutLogApi } from "@/api/services/sqlTimeoutLog";
import { SqlTimeoutLogModel } from "@/api/services/sqlTimeoutLog/models/SqlTimeoutLogModel";
import { dateUtil } from "@fast-china/utils";
import { dayjs } from "element-plus";
import { reactive } from "vue";

defineOptions({
	name: "DevSqlTimeoutLog",
});

const state = reactive({
	visible: false,
	title: "日志",
	content: "",
});
</script>
