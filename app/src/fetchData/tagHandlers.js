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
exports.TagHandlers = void 0;
const types_1 = require("./types");
function ResponseHandler(clicker, response) {
    return __awaiter(this, void 0, void 0, function* () {
        var inputId = clicker.getAttribute(`[${types_1.dataResponse}]`);
        if (!inputId)
            return false;
        var input = document.getElementById(inputId);
        if (!input)
            return false;
        input.value = yield response.json();
    });
}
exports.TagHandlers = [ResponseHandler];
//# sourceMappingURL=tagHandlers.js.map