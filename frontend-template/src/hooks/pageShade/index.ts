/**
 * 页面遮罩层
 */

import { useEventListener } from "@vueuse/core";

/*
 * 显示页面遮罩
 */
export const showShade = function (className = "shade", closeCallBack: Function): void {
    const containerEl = document.querySelector(".fast-layout-container") as HTMLElement;
    const shadeDiv = document.createElement("div");
    shadeDiv.setAttribute("class", "fast-layout-shade " + className);
    containerEl.appendChild(shadeDiv);
    useEventListener(shadeDiv, "click", () => closeShade(closeCallBack));
};

/*
 * 隐藏页面遮罩
 */
export const closeShade = function (closeCallBack: Function = () => {}): void {
    const shadeEl = document.querySelector(".fast-layout-shade") as HTMLElement;
    shadeEl && shadeEl.remove();

    closeCallBack();
};
