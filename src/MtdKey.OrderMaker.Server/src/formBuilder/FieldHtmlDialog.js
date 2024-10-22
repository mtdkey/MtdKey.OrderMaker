"use strict";
var __classPrivateFieldGet = (this && this.__classPrivateFieldGet) || function (receiver, state, kind, f) {
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a getter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
    return kind === "m" ? f : kind === "a" ? f.call(receiver) : f ? f.value : state.get(receiver);
};
var __classPrivateFieldSet = (this && this.__classPrivateFieldSet) || function (receiver, state, value, kind, f) {
    if (kind === "m") throw new TypeError("Private method is not writable");
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a setter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot write private member to an object whose class did not declare it");
    return (kind === "a" ? f.call(receiver, value) : f ? f.value = value : state.set(receiver, value)), value;
};
var _FieldHtmlDialog_fieldItem;
Object.defineProperty(exports, "__esModule", { value: true });
exports.FieldHtmlDialog = void 0;
const textfield_1 = require("@material/textfield");
const bootstrap = require("bootstrap");
const _1 = require(".");
const fieldType_1 = require("./fieldType");
class FieldHtmlDialog {
    constructor(values) {
        _FieldHtmlDialog_fieldItem.set(this, void 0);
        this.values = values;
        this.container = document.getElementById("fieldHtmlDialog");
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new textfield_1.MDCTextField(document.getElementById("fieldHtmlName"));
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`);
        this.inputReadonly = document.getElementById("swithHtmlReadonly");
        EventsHandler(this);
    }
    getData() {
        return __classPrivateFieldGet(this, _FieldHtmlDialog_fieldItem, "f");
    }
    setData(fieldItem) {
        __classPrivateFieldSet(this, _FieldHtmlDialog_fieldItem, fieldItem, "f");
        this.values.fieldHtmlDialog.inputName.value = __classPrivateFieldGet(this, _FieldHtmlDialog_fieldItem, "f").name;
        var typeText = (0, fieldType_1.GetFileTypeInfo)(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);
        this.inputReadonly.checked = __classPrivateFieldGet(this, _FieldHtmlDialog_fieldItem, "f").readonly;
    }
    getVluesFieldData() {
        var field = this.values.Fields.find(item => item.getData().id === this.getData().id);
        var fieldData = Object.assign({}, field.getData());
        return fieldData;
    }
    setValuesFieldData(fieldItem) {
        var field = this.values.Fields.find(item => item.getData().id === this.getData().id);
        field.setData(fieldItem);
    }
}
exports.FieldHtmlDialog = FieldHtmlDialog;
_FieldHtmlDialog_fieldItem = new WeakMap();
function EventsHandler(binder) {
    binder.inputName.root.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputReadonly.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.readonly = binder.inputReadonly.checked;
            binder.setValuesFieldData(fieldData);
        });
    });
}
//# sourceMappingURL=FieldHtmlDialog.js.map