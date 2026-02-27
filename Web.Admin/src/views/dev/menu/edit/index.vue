<template>
	<FaDialog
		ref="faDialogRef"
		width="1000"
		fullHeight
		:title="state.dialogTitle"
		:showConfirmButton="!state.formDisabled"
		:showBeforeClose="!state.formDisabled"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="faFormRef.resetFields()"
	>
		<FaForm ref="faFormRef" :model="state.formData" :rules="state.formRules" :disabled="state.formDisabled" cols="2" labelWidth="100">
			<FaFormItem prop="appId" label="应用">
				<ApplicationSelect v-model="state.formData.appId" v-model:appName="state.formData.appName" />
			</FaFormItem>
			<FaFormItem prop="moduleId" label="模块">
				<ModuleSelect
					:disabled="!state.formData?.appId"
					:appId="state.formData.appId"
					v-model="state.formData.moduleId"
					v-model:moduleName="state.formData.moduleName"
					@change="handleModuleChange"
				/>
			</FaFormItem>
			<FaFormItem prop="menuType" label="菜单类型">
				<RadioGroup name="MenuTypeEnum" v-model="state.formData.menuType" />
			</FaFormItem>
			<FaFormItem prop="parentId" label="父级">
				<el-cascader
					:disabled="!state.formData?.moduleId"
					v-model="state.formData.parentId"
					:options="state.menuList"
					placeholder="请选择父级菜单"
					filterable
					clearable
					:props="{ checkStrictly: true, checkOnClickNode: true, emitPath: false }"
				/>
			</FaFormItem>
			<FaFormItem prop="edition" label="版本" span="2">
				<RadioGroup name="EditionEnum" v-model="state.formData.edition" />
			</FaFormItem>
			<FaFormItem prop="menuCode" label="菜单编码">
				<el-input v-model="state.formData.menuCode" maxlength="50" placeholder="请输入菜单编码" />
			</FaFormItem>
			<FaFormItem prop="menuName" label="菜单名称">
				<el-input v-model="state.formData.menuName" maxlength="20" placeholder="请输入菜单名称" />
			</FaFormItem>
			<FaFormItem prop="menuTitle" label="菜单标题">
				<el-input v-model="state.formData.menuTitle" maxlength="20" placeholder="请输入菜单标题" />
			</FaFormItem>
			<FaFormItem prop="sort" label="排序" tips="从小到大">
				<el-input-number v-model="state.formData.sort" :min="1" :max="9999" placeholder="请输入排序" />
			</FaFormItem>
			<FaFormItem prop="visible" label="显示">
				<RadioGroup button name="BooleanEnum" v-model="state.formData.visible" />
			</FaFormItem>
			<FaFormItem v-if="state.dialogState !== 'add'" prop="status" label="状态">
				<RadioGroup button name="CommonStatusEnum" v-model="state.formData.status" />
			</FaFormItem>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">Web端</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="hasWeb" label="Web端">
				<el-checkbox v-model="state.formData.hasWeb">Web端</el-checkbox>
			</FaFormItem>
			<template v-if="state.formData.hasWeb">
				<FaFormItem prop="webIcon" label="图标">
					<IconSelect v-model="state.formData.webIcon" placeholder="请选择Web端图标" :showAllLevels="false" :props="{ emitPath: false }" />
				</FaFormItem>
				<template v-if="state.formData.menuType === MenuTypeEnum.Menu">
					<FaFormItem prop="webComponent" label="组件地址">
						<el-cascader
							v-model="state.componentValue"
							:options="state.componentList"
							placeholder="请选择Web端组件地址"
							filterable
							clearable
							@change="handleComponentChange"
						/>
					</FaFormItem>
					<FaFormItem prop="webRouter" label="路由地址">
						<el-input v-model="state.formData.webRouter" maxlength="200" placeholder="请输入Web端路由地址" />
					</FaFormItem>
					<FaFormItem prop="webTab" label="导航栏">
						<RadioGroup button name="BooleanEnum" v-model="state.formData.webTab" />
					</FaFormItem>
					<FaFormItem prop="webKeepAlive" label="KeepAlive">
						<RadioGroup button name="BooleanEnum" v-model="state.formData.webKeepAlive" />
					</FaFormItem>
				</template>
			</template>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">移动端</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="hasMobile" label="移动端" span="2">
				<el-checkbox v-model="state.formData.hasMobile">移动端</el-checkbox>
			</FaFormItem>
			<template v-if="state.formData.hasMobile">
				<FaFormItem prop="mobileIcon" label="图标" span="2">
					<el-input type="textarea" v-model="state.formData.mobileIcon" :rows="2" maxlength="200" placeholder="请输入移动端图标" />
				</FaFormItem>
				<FaFormItem v-if="state.formData.menuType === MenuTypeEnum.Menu" prop="mobileRouter" label="路由地址" span="2">
					<el-input v-model="state.formData.mobileRouter" maxlength="200" placeholder="请输入移动端路由地址" />
				</FaFormItem>
			</template>

			<FaLayoutGridItem span="2">
				<el-divider contentPosition="left">桌面端</el-divider>
			</FaLayoutGridItem>
			<FaFormItem prop="hasDesktop" label="桌面端" span="2">
				<el-checkbox v-model="state.formData.hasDesktop">桌面端</el-checkbox>
			</FaFormItem>
			<template v-if="state.formData.hasDesktop">
				<FaFormItem prop="desktopIcon" label="图标" span="2">
					<el-input type="textarea" v-model="state.formData.desktopIcon" :rows="2" maxlength="200" placeholder="请输入桌面端图标" />
				</FaFormItem>
				<FaFormItem v-if="state.formData.menuType === MenuTypeEnum.Menu" prop="desktopRouter" label="路由地址" span="2">
					<el-input v-model="state.formData.desktopRouter" maxlength="200" placeholder="请输入桌面端路由地址" />
				</FaFormItem>
			</template>

			<template v-if="state.formData.menuType === MenuTypeEnum.Internal || state.formData.menuType === MenuTypeEnum.Outside">
				<FaLayoutGridItem span="2">
					<el-divider contentPosition="left">其他</el-divider>
				</FaLayoutGridItem>
				<FaFormItem prop="link" label="内链/外链地址" span="2">
					<el-input type="textarea" v-model="state.formData.link" :rows="2" maxlength="200" placeholder="请输入内链/外链地址" />
				</FaFormItem>
			</template>

			<template v-if="state.formData.menuType === MenuTypeEnum.Menu && state.dialogState !== 'add'">
				<FaLayoutGridItem span="2">
					<el-divider contentPosition="left">按钮</el-divider>
				</FaLayoutGridItem>
				<FaLayoutGridItem span="2" style="min-height: 300px; max-height: 500px">
					<ButtonTable v-model="state.formData.buttonList" :disabled="state.formDisabled" />
				</FaLayoutGridItem>
			</template>
		</FaForm>
	</FaDialog>
</template>

<script lang="ts" setup>
import { onMounted, reactive, ref } from "vue";
import { CascaderValue, ElMessage, type FormRules } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { menuApi } from "@/api/services/Center/menu";
import { AddMenuInput } from "@/api/services/Center/menu/models/AddMenuInput";
import { EditMenuInput } from "@/api/services/Center/menu/models/EditMenuInput";
import routerPath from "@/router/index.json";
import ButtonTable from "./components/buttonTable.vue";
import type { ElSelectorOutput, FaDialogInstance, FaFormInstance } from "fast-element-plus";

defineOptions({
	name: "DevMenuEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();
const faFormRef = ref<FaFormInstance>();

const state = reactive({
	formData: withDefineType<EditMenuInput & AddMenuInput & { appId?: number; appName?: string; moduleName?: string }>({}),
	formRules: withDefineType<FormRules>({
		moduleId: [{ required: true, message: "请选择模块", trigger: "change" }],
		menuType: [{ required: true, message: "请选择菜单类型", trigger: "change" }],
		menuCode: [{ required: true, message: "请输入菜单编码", trigger: "blur" }],
		menuName: [{ required: true, message: "请输入菜单名称", trigger: "blur" }],
		menuTitle: [{ required: true, message: "请输入菜单标题", trigger: "blur" }],
		// desktopIcon: [{ required: true, message: "请输入桌面端图标", trigger: "blur" }],
		// desktopRouter: [{ required: true, message: "请输入桌面端路由地址", trigger: "blur" }],
		// webIcon: [{ required: true, message: "请输入Web端图标", trigger: "blur" }],
		webRouter: [{ required: true, message: "请输入Web端路由地址", trigger: "blur" }],
		webComponent: [{ required: true, message: "请选择Web端组件地址", trigger: "change" }],
		// mobileIcon: [{ required: true, message: "请输入移动端图标", trigger: "blur" }],
		// mobileRouter: [{ required: true, message: "请输入移动端路由地址", trigger: "blur" }],
		link: [{ required: true, message: "请输入内链/外链地址", trigger: "blur" }],
		sort: [{ required: true, message: "请输入排序", trigger: "blur" }],
	}),
	formDisabled: false,
	dialogState: withDefineType<IPageStateType>("detail"),
	dialogTitle: "菜单",
	componentList: withDefineType<ElSelectorOutput<string>[]>([]),
	componentValue: [],
	menuList: withDefineType<ElSelectorOutput<number>[]>([]),
});

const handleModuleChange = async (value: ElSelectorOutput) => {
	state.menuList = await menuApi.menuSelector(value?.value);
};

const handleComponentChange = (value: CascaderValue) => {
	if (!Array.isArray(value) || value.length === 0) {
		state.formData.webRouter = "";
		state.formData.webComponent = "";
		return;
	}

	// 拼接路径并去掉 .vue
	const componentPath = value.join("/").replace(/\.vue$/, "");

	if (routerPath[`/src/views/${componentPath}.vue`]) {
		// 去掉末尾 /index
		state.formData.webRouter = `/${componentPath}`;
		state.formData.webComponent = componentPath;
	} else {
		state.formData.webRouter = "";
		state.formData.webComponent = "";
	}
};

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await faFormRef.value.validateScrollToField();
		switch (state.dialogState) {
			case "add":
				await menuApi.addMenu(state.formData);
				ElMessage.success("新增成功！");
				break;
			case "edit":
				await menuApi.editMenu(state.formData);
				ElMessage.success("保存成功！");
				break;
		}
		emit("ok");
	});
};

const add = () => {
	faDialogRef.value.open(() => {
		state.dialogState = "add";
		state.dialogTitle = "添加菜单";
		state.formDisabled = false;
		state.formData = {
			menuType: MenuTypeEnum.Catalog,
			edition: EditionEnum.None,
			visible: true,
			hasWeb: true,
			webTab: false,
			webKeepAlive: false,
			hasMobile: false,
			hasDesktop: false,
			status: CommonStatusEnum.Enable,
			buttonList: [],
		};
	});
};

const edit = (menuId: number) => {
	faDialogRef.value.open(async () => {
		state.dialogState = "edit";
		state.formDisabled = false;
		const apiRes = await menuApi.queryMenuDetail(menuId);
		state.formData = apiRes;
		if (apiRes.moduleId) {
			state.menuList = await menuApi.menuSelector(apiRes.moduleId);
		}
		state.componentValue = apiRes.webComponent
			? apiRes.webComponent.split("/").map((part, index, arr) => (index === arr.length - 1 ? `${part}.vue` : part))
			: [];
		state.dialogTitle = `编辑菜单 - ${apiRes.menuName}`;
	});
};

onMounted(() => {
	state.componentList = [];

	for (const path in routerPath) {
		const label = routerPath[path];
		const cleanPath = path.replace(/^\/src\/views\//, "");
		const parts = cleanPath.split("/").filter(Boolean);

		let currentLevel = state.componentList;

		for (let i = 0; i < parts.length; i++) {
			const part = parts[i];
			let node = currentLevel.find((item) => item.value === part);

			if (!node) {
				node = {
					value: part,
					label: part,
					children: [],
				};
				currentLevel.push(node);
			}

			if (i === parts.length - 1) {
				node.label = label;
				delete node.children;
			}

			currentLevel = node.children || [];
		}
	}
});

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	add,
	edit,
});
</script>
