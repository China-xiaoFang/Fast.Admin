import Editor from "./Editor/index.vue";
import FastTable from "./FastTable/index.tsx";
import Footer from "./Footer/index.vue";
import Loading from "./Loading/index.vue";
import Tag from "./Tag/index.vue";
import Tendril from "./Tendril/index.vue";
import Watermark from "./Watermark/index.vue";

export * from "./Editor/index.vue";
export * from "./Footer/index.vue";
export * from "./Loading/index.vue";
export * from "./FastTable/index.tsx";
export * from "./Tag/index.vue";
export * from "./Tendril/index.vue";
export * from "./Watermark/index.vue";

export type FastTableInstance = InstanceType<typeof FastTable>;

export default [Editor, FastTable, Footer, Loading, Tag, Tendril, Watermark];
