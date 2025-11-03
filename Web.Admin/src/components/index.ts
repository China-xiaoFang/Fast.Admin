import type { DefineComponent } from "vue";
import Editor from "./Editor/index.vue";
import Footer from "./Footer/index.vue";
import Loading from "./Loading/index.vue";
import Tag from "./Tag/index.vue";
import Tendril from "./Tendril/index.vue";
import Watermark from "./Watermark/index.vue";

export * from "./Editor/index.vue";
export * from "./Footer/index.vue";
export * from "./Loading/index.vue";
export * from "./Tag/index.vue";
export * from "./Tendril/index.vue";
export * from "./Watermark/index.vue";

export default [
	Editor,
	Footer,
	Loading,
	Tag,
	Tendril,
	Watermark,
] as unknown as DefineComponent[];
