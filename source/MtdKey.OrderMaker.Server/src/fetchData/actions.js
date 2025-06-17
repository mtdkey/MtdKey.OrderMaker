"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ShowSnackBarAction = exports.CreateFormData = void 0;
exports.ShowModalAction = ShowModalAction;
const snackbar_1 = require("@material/snackbar");
function ShowModalAction(show) {
    const style = show ? 'block' : 'none';
    document.getElementById('mainModal').style.display = style;
}
const CreateFormData = (form) => {
    var formData = new FormData();
    var inputs = form.querySelectorAll("input");
    inputs.forEach(input => {
        var _a;
        switch (input.type) {
            case "checkbox": {
                formData.append(input.name, input.checked.toString());
                break;
            }
            default: {
                formData.append(input.name, input.value);
            }
        }
        for (var i = 0; i < ((_a = input.files) === null || _a === void 0 ? void 0 : _a.length); i++) {
            formData.append(input.name, input.files[i], input.files[i].name);
        }
        ;
    });
    return formData;
};
exports.CreateFormData = CreateFormData;
const ShowSnackBarAction = (message, error = false) => {
    const snackbar = new snackbar_1.MDCSnackbar(document.getElementById('main-snack'));
    const div = document.getElementById('main-snack');
    div.classList.add("mdc-snackbar--open");
    snackbar.labelText = message;
    if (error) {
        snackbar.timeoutMs = 10000;
        const surface = div.querySelector(".mdc-snackbar__surface");
        surface.style.backgroundColor = "darkred";
    }
    snackbar.open();
};
exports.ShowSnackBarAction = ShowSnackBarAction;
//# sourceMappingURL=actions.js.map