<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="146GLML9MD" rowKey="accountId" :requestApi="accountApi.queryAccountPaged" hideSearchTime>
			<template #mobile="{ row }: { row?: QueryAccountPagedOutput }">
				<div>
					<ElIcon class="fa__copy-icon" title="复制" v-copy="row.mobile">
						<CopyDocument />
					</ElIcon>
					手机：
					<el-text class="el-link is-hover-underline fa-table__link-column__text" @click="editFormRef.detail(row.accountId)">
						{{ row.mobile }}
					</el-text>
				</div>
				<div>
					<ElIcon class="fa__copy-icon" title="复制" v-copy="row.email">
						<CopyDocument />
					</ElIcon>
					邮箱：
					{{ row.email }}
				</div>
			</template>
			<template #firstLoginTime="{ row }: { row?: QueryAccountPagedOutput }">
				<div>地区：{{ row.firstLoginProvince }} - {{ row.firstLoginCity }}</div>
				<div>Ip：{{ row.firstLoginIp }}</div>
				<div>时间：{{ dayjs(row.firstLoginTime).format("YYYY-MM-DD HH:mm:ss") }}</div>
				<el-tag v-if="row.firstLoginTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.firstLoginTime)) }}
				</el-tag>
			</template>
			<template #firstLoginOS="{ row }: { row?: QueryAccountPagedOutput }">
				<div>设备：{{ row.firstLoginDevice }}</div>
				<div>操作系统：{{ row.firstLoginOS }}</div>
				<div>浏览器：{{ row.firstLoginBrowser }}</div>
			</template>
			<template #lastLoginTime="{ row }: { row?: QueryAccountPagedOutput }">
				<div>地区：{{ row.lastLoginProvince }} - {{ row.lastLoginCity }}</div>
				<div>Ip：{{ row.lastLoginIp }}</div>
				<div>时间：{{ dayjs(row.lockEndTime).format("YYYY-MM-DD HH:mm:ss") }}</div>
				<el-tag v-if="row.lastLoginTime" type="info" round effect="light" size="small">
					{{ dateUtil.dateTimeFix(String(row.lastLoginTime)) }}
				</el-tag>
			</template>
			<template #lastLoginOS="{ row }: { row?: QueryAccountPagedOutput }">
				<div>设备：{{ row.lastLoginDevice }}</div>
				<div>操作系统：{{ row.lastLoginOS }}</div>
				<div>浏览器：{{ row.lastLoginTime }}</div>
			</template>
			<template #lockStartTime="{ row }: { row?: QueryAccountPagedOutput }">
				<template v-if="row.isLock">
					<el-tag type="warning">已锁定</el-tag>
					<div>错误次数：{{ row.passwordErrorTime }}次</div>
					<div>开始时间：{{ dayjs(row.lockStartTime).format("YYYY-MM-DD HH:mm:ss") }}</div>
					<div>结束时间：{{ dayjs(row.lockEndTime).format("YYYY-MM-DD HH:mm:ss") }}</div>
				</template>
				<template v-else>
					<el-tag type="info">未锁定</el-tag>
				</template>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QueryAccountPagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.accountId)">详情</el-button>
				<el-button size="small" plain type="warning" @click="handleResetPassword(row)">重置密码</el-button>
				<el-button v-if="row.isLock" size="small" plain type="warning" @click="handleUnlock(row)">解锁</el-button>
				<el-button v-if="row.status == CommonStatusEnum.Enable" size="small" plain type="danger" @click="handleChangeStatus(row)">
					禁用
				</el-button>
				<el-button v-else size="small" plain type="warning" @click="handleChangeStatus(row)">启用</el-button>
			</template>
		</FastTable>
		<AccountEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import AccountEdit from "./edit/index.vue";
import type { FastTableInstance } from "@/components";
import { accountApi } from "@/api/services/account";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { QueryAccountPagedOutput } from "@/api/services/account/models/QueryAccountPagedOutput";
import { dayjs, ElMessage, ElMessageBox } from "element-plus";
import { dateUtil } from "@fast-china/utils";
import { CopyDocument } from "@element-plus/icons-vue";

defineOptions({
	name: "SystemAccount",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof AccountEdit>>();

/** 处理重置密码 */
const handleResetPassword = (row: QueryAccountPagedOutput) => {
	const { accountId, rowVersion } = row;
	ElMessageBox.confirm(`确定解锁账号？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await accountApi.resetPassword({
				accountId,
				rowVersion,
			});
			ElMessage.success("操作成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理解锁 */
const handleUnlock = (row: QueryAccountPagedOutput) => {
	const { accountId, rowVersion } = row;
	ElMessageBox.confirm(`确定解锁账号？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await accountApi.unlock({
				accountId,
				rowVersion,
			});
			ElMessage.success("操作成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理状态变更 */
const handleChangeStatus = (row: QueryAccountPagedOutput) => {
	const { accountId, status, rowVersion } = row;
	ElMessageBox.confirm(`确定${status === CommonStatusEnum.Enable ? "禁用" : "启用"}账号？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await accountApi.changeStatus({
				accountId,
				rowVersion,
			});
			ElMessage.success("操作成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
