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
var _FieldListDialog_fieldItem;
Object.defineProperty(exports, "__esModule", { value: true });
exports.FieldListDialog = void 0;
const textfield_1 = require("@material/textfield");
const bootstrap = require("bootstrap");
const _1 = require(".");
const fieldType_1 = require("./fieldType");
const utilities_1 = require("./utilities");
class FieldListDialog {
    constructor(values) {
        _FieldListDialog_fieldItem.set(this, void 0);
        this.values = values;
        this.container = document.getElementById("fieldListDialog");
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new textfield_1.MDCTextField(document.getElementById("fieldListName"));
        this.inputSearch = new textfield_1.MDCTextField(document.getElementById("searchItem"));
        this.buttonAddItem = document.getElementById("buttonAddItem");
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`);
        this.itemPlace = document.getElementById("itemPlace");
        EventsHandler(this);
    }
    getData() {
        return __classPrivateFieldGet(this, _FieldListDialog_fieldItem, "f");
    }
    setData(fieldItem) {
        __classPrivateFieldSet(this, _FieldListDialog_fieldItem, fieldItem, "f");
        this.values.fieldListDialog.inputName.value = __classPrivateFieldGet(this, _FieldListDialog_fieldItem, "f").name;
        var typeText = (0, fieldType_1.GetFileTypeInfo)(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);
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
exports.FieldListDialog = FieldListDialog;
_FieldListDialog_fieldItem = new WeakMap();
function EventsHandler(binder) {
    binder.inputName.root.addEventListener("change", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });
    binder.buttonAddItem.addEventListener("click", () => {
        (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
            var template = binder.values.itemTemplate.content.cloneNode(true);
            var item = template.firstElementChild;
            var inputText = item.querySelector("[data-item-id]");
            var inputRadio = item.querySelector("[data-item-default]");
            var buttonRemover = item.querySelector("[data-item-remover]");
            var uid = (0, utilities_1.generateUID)();
            var itemName = binder.inputSearch.value;
            var fieldItem = binder.getVluesFieldData();
            var isFirstItem = fieldItem.listItems.length === 0;
            item.dataset.item = uid;
            inputRadio.dataset.itemDefault = uid;
            inputText.dataset.itemId = uid;
            inputText.value = itemName;
            if (isFirstItem) {
                inputRadio.checked = isFirstItem;
                fieldItem.defaultValue = uid;
            }
            fieldItem.listItems.push({ Id: uid, Name: itemName });
            binder.setValuesFieldData(fieldItem);
            binder.itemPlace.appendChild(item);
            binder.inputSearch.value = "";
            buttonRemover.addEventListener("click", () => {
                var currentField = binder.getVluesFieldData();
                var index = currentField.listItems.findIndex(x => x.Id == item.dataset.item);
                currentField.listItems.splice(index, 1);
                if (uid.toLowerCase() === currentField.defaultValue.toLowerCase() && currentField.listItems.length > 0) {
                    currentField.defaultValue = currentField.listItems[0].Id;
                    console.log(currentField.listItems);
                    var radio = document.querySelector(`[data-item-default='${currentField.listItems[0].Id}']`);
                    radio.checked = true;
                }
                binder.setValuesFieldData(currentField);
                var currentItem = document.querySelector(`[data-item="${uid}"]`);
                binder.itemPlace.removeChild(currentItem);
            });
            inputRadio.addEventListener("change", () => {
                var currentField = binder.getVluesFieldData();
                currentField.defaultValue = uid;
                binder.setValuesFieldData(currentField);
            });
            inputText.addEventListener("change", () => {
                var currentField = binder.getVluesFieldData();
                var index = currentField.listItems.findIndex(x => x.Id == item.dataset.item);
                currentField.listItems[index].Name = inputText.value;
                binder.setValuesFieldData(currentField);
            });
        });
    });
}
//# sourceMappingURL=FieldListDialog.js.map