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
var _FieldDialog_fieldItem;
Object.defineProperty(exports, "__esModule", { value: true });
exports.FieldDialog = void 0;
const textfield_1 = require("@material/textfield");
const bootstrap = require("bootstrap");
const _1 = require(".");
const fieldType_1 = require("./fieldType");
class FieldDialog {
    constructor(values) {
        _FieldDialog_fieldItem.set(this, void 0);
        this.values = values;
        this.container = document.getElementById("fieldDialog");
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new textfield_1.MDCTextField(document.getElementById("fieldName"));
        this.inputDefault = new textfield_1.MDCTextField(document.getElementById("defaultValue"));
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`);
        this.inputRequired = document.getElementById("swithRequired");
        this.inputReadonly = document.getElementById("swithReadonly");
        this.inputNotset = document.getElementById("radioNotset");
        this.inputDateNow = document.getElementById("radioDatetime");
        this.inputUserGroup = document.getElementById("radioUserGroup");
        this.inputUserName = document.getElementById("radioUserName");
        this.textParams = document.getElementById("textParams");
        this.inputSelectForm = document.getElementById("selectForm");
        EventsHandler(this);
    }
    getData() {
        return __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f");
    }
    setData(fieldItem) {
        __classPrivateFieldSet(this, _FieldDialog_fieldItem, fieldItem, "f");
        this.values.fieldDialog.inputName.value = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").name;
        var typeText = (0, fieldType_1.GetFileTypeInfo)(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);
        this.inputDefault.value = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").defaultValue;
        this.inputRequired.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").required;
        this.inputReadonly.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").readonly;
        this.inputNotset.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").triggerId === "9C85B07F-9236-4314-A29E-87B20093CF82";
        this.inputDateNow.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").triggerId === "D3663BC7-FA05-4F64-8EBD-F25414E459B8";
        this.inputUserGroup.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").triggerId === "33E8212E-059B-482D-8CBD-DFDB073E3B63";
        this.inputUserName.checked = __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").triggerId === "08FE6202-45D7-46C2-B343-B79FD4831F27";
        this.listFormParam = document.getElementById("listFormParam");
        if (fieldItem.type < 5) {
            this.textParams.classList.remove("d-none");
        }
        else {
            this.textParams.classList.add("d-none");
        }
        if (fieldItem.type === 11) {
            this.listFormParam.classList.remove("d-none");
            this.inputSelectForm.innerHTML = "";
            this.values.FormInfoModels.forEach(item => {
                var opt = document.createElement('option');
                opt.value = item.Id;
                opt.textContent = item.Name;
                this.inputSelectForm.appendChild(opt);
            });
            __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").listFormId = this.inputSelectForm.value;
        }
        else {
            __classPrivateFieldGet(this, _FieldDialog_fieldItem, "f").listFormId = "";
            this.listFormParam.classList.add("d-none");
        }
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
exports.FieldDialog = FieldDialog;
_FieldDialog_fieldItem = new WeakMap();
function EventsHandler(binder) {
    binder.inputSelectForm.addEventListener("change", (e) => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var elm = e.target;
            var data = binder.getVluesFieldData();
            data.listFormId = elm.value;
            binder.setValuesFieldData(data);
        });
    });
    binder.inputName.root.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputDefault.root.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.defaultValue = binder.inputDefault.value;
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputRequired.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.required = binder.inputRequired.checked;
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
    binder.inputNotset.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "9C85B07F-9236-4314-A29E-87B20093CF82";
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputDateNow.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "33E8212E-059B-482D-8CBD-DFDB073E3B63";
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputUserGroup.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "33E8212E-059B-482D-8CBD-DFDB073E3B63";
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.inputUserName.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "08FE6202-45D7-46C2-B343-B79FD4831F27";
            binder.setValuesFieldData(fieldData);
        });
    });
}
//# sourceMappingURL=FieldDialog.js.map