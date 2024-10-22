"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const actions_1 = require("./actions");
const tagHandlers_1 = require("./tagHandlers");
const types_1 = require("./types");
const proxyClickers = document.querySelectorAll(`[${types_1.dataMtdClickerBy}]`);
proxyClickers.forEach((clicker) => {
    const targetId = clicker.getAttribute(types_1.dataMtdClickerBy);
    clicker.addEventListener("click", () => {
        document.getElementById(targetId).click();
    });
});
const clickers = document.querySelectorAll(`[${types_1.dataMtdClicker}]`);
clickers.forEach((clicker) => __awaiter(void 0, void 0, void 0, function* () {
    clicker.addEventListener("click", (elemnt) => __awaiter(void 0, void 0, void 0, function* () {
        const form = elemnt.currentTarget.closest("form");
        const validate = form.reportValidity();
        if (!validate) {
            return false;
        }
        (0, actions_1.ShowModalAction)(true);
        var formData = (0, actions_1.CreateFormData)(form);
        const response = yield fetch(form.action, { method: form.method, body: formData });
        switch (response.status) {
            case 200: {
                tagHandlers_1.TagHandlers.forEach((handler) => __awaiter(void 0, void 0, void 0, function* () { return yield handler(clicker, response); }));
                setTimeout(() => (0, actions_1.ShowModalAction)(false), 1000);
                break;
            }
            case 400: {
                var value = yield response.text();
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)(value, true); }, 1000);
                break;
            }
            case 500: {
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)("500 Internal Server Error", true); }, 1000);
                break;
            }
            default: {
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)(`No handler for status ${response.status}`, true); }, 1000);
            }
        }
    }));
}));
//# sourceMappingURL=index.js.map