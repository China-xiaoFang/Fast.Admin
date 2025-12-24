import type { RouteRecordRaw } from "vue-router";

/**
 * System Management Routes
 */
export const systemRoute: RouteRecordRaw = {
	path: "/system",
	name: "System",
	redirect: "/system/role",
	meta: {
		title: "系统管理",
		icon: "fa fa-cogs",
	},
	children: [
		{
			path: "role",
			name: "SystemRole",
			component: () => import("@/views/system/role/index.vue"),
			meta: {
				title: "角色管理",
				icon: "fa fa-user-circle",
			},
		},
		{
			path: "department",
			name: "SystemDepartment",
			component: () => import("@/views/system/department/index.vue"),
			meta: {
				title: "部门管理",
				icon: "fa fa-sitemap",
			},
		},
		{
			path: "employee",
			name: "SystemEmployee",
			component: () => import("@/views/system/employee/index.vue"),
			meta: {
				title: "职员管理",
				icon: "fa fa-users",
			},
		},
		{
			path: "position",
			name: "SystemPosition",
			component: () => import("@/views/system/position/index.vue"),
			meta: {
				title: "职位管理",
				icon: "fa fa-briefcase",
			},
		},
		{
			path: "jobLevel",
			name: "SystemJobLevel",
			component: () => import("@/views/system/jobLevel/index.vue"),
			meta: {
				title: "职级管理",
				icon: "fa fa-signal",
			},
		},
		{
			path: "organization",
			name: "SystemOrganization",
			component: () => import("@/views/system/organization/index.vue"),
			meta: {
				title: "组织机构",
				icon: "fa fa-building",
			},
		},
	],
};
