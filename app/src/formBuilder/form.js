"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Form = void 0;
const utilities_1 = require("./utilities");
const index_1 = require("../imageSelector/index");
const _1 = require(".");
class Form {
    constructor(values) {
        this.id = (0, utilities_1.generateUID)();
        this.name = "New Form";
        this.active = true;
        this.visibleDate = true;
        this.visibleNumber = true;
        this.imageBack = new index_1.ImageSelector("imageBack", this.callback);
        this.imageLogo = new index_1.ImageSelector("imageLogo", this.callback);
        values.formDialogName.root.addEventListener("keyup", (event) => {
            if (event.isComposing || event.keyCode === 229) {
                return;
            }
            (0, _1.ChangeAction)(_1.Actions.ChangeFormParameters, () => {
                values.titleFormName.textContent = values.formDialogName.value;
                values.Form.name = values.formDialogName.value;
            });
        });
    }
    callback() {
        (0, _1.ChangeAction)(_1.Actions.ChangeFormParameters, () => { return; });
    }
}
exports.Form = Form;
//# sourceMappingURL=form.js.map