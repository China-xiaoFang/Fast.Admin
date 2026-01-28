<template>
	<div>
		<div class="fa__display_lr-r">
			<ApplicationTree @change="handleApplicationChange" />
			<div class="fa__display_lr-r">
				<FaTree
					ref="moduleTreeRef"
					title="模块列表"
					width="180"
					:requestApi="() => moduleApi.moduleSelector(fastTableRef?.searchParam?.appId)"
					@change="handleModuleChange"
					@node-contextmenu="(event, data) => handleModuleContextmenu(event as MouseEvent, data)"
				>
					<template #label="{ data }: { data: ElSelectorOutput<number> }">
						<FaIcon v-if="data.data?.icon" style="margin-right: 5px" size="16" :name="data.data.icon" />
						<span>{{ data.label }}</span>
					</template>

					<template #default="{ data }: { data: ElSelectorOutput<number> }">
						<Tag size="small" effect="plain" name="CommonStatusEnum" :value="data.data.status" />
					</template>
				</FaTree>

				<FastTable
					ref="fastTableRef"
					tableKey="1D11Q5S4P2"
					rowKey="menuId"
					:requestApi="menuApi.queryMenuPaged"
					hideSearchTime
					:pagination="false"
					defaultExpandAll
				>
					<!-- 表格按钮操作区域 -->
					<template #header>
						<el-button v-auth="'Menu:Add'" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
					</template>

					<template #menuName="{ row }: { row?: QueryMenuPagedOutput }">
						<FaIcon v-if="row.webIcon" style="margin-right: 5px" size="16" :name="row.webIcon" />
						<span>{{ row.menuName }}</span>
					</template>

					<template #web="{ row }: { row?: QueryMenuPagedOutput }">
						<div style="display: flex; align-items: center; gap: 5px">
							<Tag size="small" name="CommonStatusEnum" :value="row.hasWeb ? CommonStatusEnum.Enable : CommonStatusEnum.Disable" />
							<FaIcon v-if="row.webIcon" size="16" :name="row.webIcon" />
							<el-tag v-if="row.webRouter" type="primary" effect="plain">{{ row.webRouter }}</el-tag>
						</div>
						<el-tag v-if="row.webComponent" type="info" effect="plain">{{ row.webComponent }}</el-tag>
					</template>

					<template #mobile="{ row }: { row?: QueryMenuPagedOutput }">
						<div style="display: flex; align-items: center; gap: 5px">
							<Tag size="small" name="CommonStatusEnum" :value="row.hasMobile ? CommonStatusEnum.Enable : CommonStatusEnum.Disable" />
							<FaImage
								v-if="row.mobileIcon"
								src="https://gitee.com/FastDotnet/Fast.Admin/raw/master/Fast.png@!thumb"
								original
								:preview="false"
							/>
						</div>
						<el-tag v-if="row.mobileRouter" type="info" effect="plain">{{ row.mobileRouter }}</el-tag>
					</template>

					<template #desktop="{ row }: { row?: QueryMenuPagedOutput }">
						<div style="display: flex; align-items: center; gap: 5px">
							<Tag size="small" name="CommonStatusEnum" :value="row.hasDesktop ? CommonStatusEnum.Enable : CommonStatusEnum.Disable" />
							<el-tag v-if="row.desktopIcon" type="primary" effect="plain">{{ row.desktopIcon }}</el-tag>
						</div>
						<el-tag v-if="row.desktopRouter" type="info" effect="plain">{{ row.desktopRouter }}</el-tag>
					</template>

					<template #link="{ row }: { row?: QueryMenuPagedOutput }">
						<el-link type="info" target="_blank" :href="row.link">{{ row.link }}</el-link>
					</template>

					<!-- 表格操作 -->
					<template #operation="{ row }: { row: QueryMenuPagedOutput }">
						<el-button v-auth="'Menu:Edit'" size="small" plain type="primary" @click="editFormRef.edit(row.menuId)">编辑</el-button>
						<el-button v-auth="'Menu:Delete'" size="small" plain type="warning" @click="handleDelete(row)">删除</el-button>
						<el-button
							v-auth="'Menu:Status'"
							v-if="row.status == CommonStatusEnum.Enable"
							size="small"
							plain
							type="danger"
							@click="handleChangeStatus(row)"
						>
							禁用
						</el-button>
						<el-button v-auth="'Menu:Status'" v-else size="small" plain type="warning" @click="handleChangeStatus(row)">启用</el-button>
					</template>
				</FastTable>
			</div>
		</div>
		<FaContextMenu ref="faContextMenuRef" :data="state.contextMenuList" />
		<MenuEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
		<ModuleEdit ref="moduleEditFormRef" @ok="moduleTreeRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { ElSelectorOutput, FaContextMenuData, FaContextMenuInstance, FaTreeInstance } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { menuApi } from "@/api/services/Center/menu";
import { moduleApi } from "@/api/services/Center/module";
import MenuEdit from "./edit/index.vue";
import ModuleEdit from "./edit/moduleEdit.vue";
import type { QueryMenuPagedOutput } from "@/api/services/Center/menu/models/QueryMenuPagedOutput";
import type { FastTableInstance } from "@/components";

defineOptions({
	name: "DevMenu",
});

const fastTableRef = ref<FastTableInstance>();
const moduleTreeRef = ref<FaTreeInstance>();
const faContextMenuRef = ref<FaContextMenuInstance>();
const editFormRef = ref<InstanceType<typeof MenuEdit>>();
const moduleEditFormRef = ref<InstanceType<typeof ModuleEdit>>();

const state = reactive({
	contextMenuList: withDefineType<FaContextMenuData[]>([
		{
			name: "add",
			label: "添加模块",
			icon: "el-icon-FolderAdd",
			click: () => {
				moduleEditFormRef.value.add();
			},
		},
		{
			name: "edit",
			label: "编辑模块",
			icon: "el-icon-EditPen",
			click: (_, { data }: { data?: ElSelectorOutput<number> }) => {
				moduleEditFormRef.value.edit(data.value);
			},
		},
		{
			name: "delete",
			label: "删除模块",
			icon: "el-icon-Delete",
			click: (_, { data }: { data?: ElSelectorOutput<number> }) => {
				ElMessageBox.confirm("确定要删除模块？", {
					type: "warning",
					async beforeClose() {
						await moduleApi.deleteModule({ moduleId: data.value, rowVersion: data.data?.rowVersion });
						ElMessage.success("删除成功！");
						fastTableRef.value?.refresh();
					},
				});
			},
		},
	]),
});

/** 应用更改 */
const handleApplicationChange = (data: ElSelectorOutput) => {
	fastTableRef.value.searchParam.appId = data.value;
	fastTableRef.value.searchParam.moduleId = undefined;
	moduleTreeRef.value.refresh();
	fastTableRef.value.refresh();
};

/** 模块更改 */
const handleModuleChange = (data: ElSelectorOutput<number>) => {
	fastTableRef.value.searchParam.moduleId = data.value;
	fastTableRef.value.refresh();
};

const handleModuleContextmenu = (event: MouseEvent, data: ElSelectorOutput<number>) => {
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
const handleDelete = (row: QueryMenuPagedOutput) => {
	const { menuId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除菜单？", {
		type: "warning",
		async beforeClose() {
			await menuApi.deleteMenu({ menuId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理状态变更 */
const handleChangeStatus = (row: QueryMenuPagedOutput) => {
	const { menuId, status, rowVersion } = row;
	ElMessageBox.confirm(`确定${status === CommonStatusEnum.Enable ? "禁用" : "启用"}菜单？`, {
		type: "warning",
		async beforeClose() {
			await menuApi.changeStatus({
				menuId,
				rowVersion,
			});
			ElMessage.success("操作成功！");
			fastTableRef.value?.refresh();
		},
	});
};
</script>
