// For this project development
import "@vue/runtime-core";

// GlobalComponents for Volar
declare module "@vue/runtime-core" {
	export interface GlobalComponents {
		Editor: (typeof import("@/components/Editor/index.vue"))["default"];
		FastTable: (typeof import("@/components/FastTable/index.tsx"))["default"];
		Footer: (typeof import("@/components/Footer/index.vue"))["default"];
		Loading: (typeof import("@/components/Loading/index.vue"))["default"];
		RadioGroup: (typeof import("@/components/RadioGroup/index.vue"))["default"];
		Tag: (typeof import("@/components/Tag/index.vue"))["default"];
		Tendril: (typeof import("@/components/Tendril/index.vue"))["default"];
		Watermark: (typeof import("@/components/Watermark/index.vue"))["default"];
	}

	// interface ComponentCustomProperties {}
}

export {};
