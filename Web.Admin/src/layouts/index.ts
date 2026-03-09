import type ChangePassword from "@/layouts/components/ChangePassword/index.vue";
import type layoutConfig from "@/layouts/components/Config/index.vue";
import type menuSearch from "@/layouts/components/MenuSearch/index.vue";
import type { InjectionKey, Ref } from "vue";

/** 布局配置 */
export const layoutConfigKey: InjectionKey<Ref<InstanceType<typeof layoutConfig>>> = Symbol("layoutConfigKey");
/** 菜单搜索 */
export const menuSearchKey: InjectionKey<Ref<InstanceType<typeof menuSearch>>> = Symbol("menuSearchKey");
/** 修改密码弹窗 */
export const changePasswordKey: InjectionKey<Ref<InstanceType<typeof ChangePassword>>> = Symbol("changePasswordKey");
