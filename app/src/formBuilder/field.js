"use strict";
var __classPrivateFieldSet = (this && this.__classPrivateFieldSet) || function (receiver, state, value, kind, f) {
    if (kind === "m") throw new TypeError("Private method is not writable");
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a setter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot write private member to an object whose class did not declare it");
    return (kind === "a" ? f.call(receiver, value) : f ? f.value = value : state.set(receiver, value)), value;
};
var __classPrivateFieldGet = (this && this.__classPrivateFieldGet) || function (receiver, state, kind, f) {
    if (kind === "a" && !f) throw new TypeError("Private accessor was defined without a getter");
    if (typeof state === "function" ? receiver !== state || !f : !state.has(receiver)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
    return kind === "m" ? f : kind === "a" ? f.call(receiver) : f ? f.value : state.get(receiver);
};
var _Field_data, _Field_titleName, _Field_titleType;
Object.defineProperty(exports, "__esModule", { value: true });
exports.FieldItem = exports.checkFieldHelperInfo = exports.Field = void 0;
const _1 = require(".");
const ripple_1 = require("../ripple/ripple");
const utilities_1 = require("./utilities");
const fieldType_1 = require("./fieldType");
const formPart_1 = require("./formPart");
class Field {
    constructor(values) {
        _Field_data.set(this, void 0);
        _Field_titleName.set(this, void 0);
        _Field_titleType.set(this, void 0);
        this.values = values;
        __classPrivateFieldSet(this, _Field_data, new FieldItem(), "f");
        var template = values.fieldTemplate.content.cloneNode(true);
        this.container = template.firstElementChild;
        __classPrivateFieldSet(this, _Field_titleName, this.container.querySelector(`.${values.styleFieldName}`), "f");
        __classPrivateFieldSet(this, _Field_titleType, this.container.querySelector(`.${values.styleFieldType}`), "f");
        this.container.addEventListener("dragstart", _1.builderDragStart);
        this.container.addEventListener("dragend", _1.builderDragEnd);
        this.container.addEventListener("dragover", _1.builderDragOver);
        this.container.addEventListener("dragleave", _1.builderDragLeave);
        this.container.addEventListener("drop", _1.builderDragDrop);
        this.container.id = __classPrivateFieldGet(this, _Field_data, "f").id;
        __classPrivateFieldGet(this, _Field_titleName, "f").textContent = __classPrivateFieldGet(this, _Field_data, "f").name;
        this.btnEditor = this.container.querySelector("[data-btn-editor]");
        this.btnRemover = this.container.querySelector("[data-btn-remover]");
        (0, ripple_1.addRippleEffect)(this.container);
        var binder = this;
        this.btnEditor.addEventListener("click", (e) => {
            switch (__classPrivateFieldGet(this, _Field_data, "f").type) {
                case 11: {
                    values.fieldListDialog.setData(__classPrivateFieldGet(binder, _Field_data, "f"));
                    values.fieldListDialog.screen.show();
                    break;
                }
                case 15: {
                    values.fieldHtmlDialog.setData(__classPrivateFieldGet(binder, _Field_data, "f"));
                    values.fieldHtmlDialog.screen.show();
                    break;
                }
                default: {
                    values.fieldDialog.setData(__classPrivateFieldGet(binder, _Field_data, "f"));
                    values.fieldDialog.screen.show();
                }
            }
        });
        this.btnRemover.addEventListener("click", (e) => {
            (0, _1.ChangeAction)(_1.Actions.ChangeFieldParameters, () => {
                var index = values.Fields.indexOf(binder);
                values.Fields.splice(index, 1);
                var parent = binder.container.closest(`.${binder.values.styleFormPartItems}`);
                parent.removeChild(binder.container);
            });
        });
    }
    setData(data) {
        this.container.id = data.id;
        this.btnEditor.dataset.btnEditor = data.id;
        __classPrivateFieldGet(this, _Field_data, "f").defaultValue = data.defaultValue;
        __classPrivateFieldSet(this, _Field_data, data, "f");
        __classPrivateFieldGet(this, _Field_titleName, "f").textContent = data.name;
        var typeInfo = (0, fieldType_1.GetFileTypeInfo)(data.type);
        __classPrivateFieldGet(this, _Field_titleType, "f").textContent = "";
        __classPrivateFieldGet(this, _Field_titleType, "f").appendChild(typeInfo);
    }
    getData() {
        return __classPrivateFieldGet(this, _Field_data, "f");
    }
    addToFormPart() {
        var type = this.values.overElement.dataset.type;
        //add field if it is over another part
        if (type === this.values.typeActivePart) {
            var formPart = this.values.overElement.querySelector(`.${this.values.styleFormPartItems}`);
            checkFieldHelperInfo(formPart);
            formPart.append(this.container);
        }
        //add field if it is over a field - first find the parent formPart
        if (type === this.values.typeActiveField) {
            var formPart = this.values.overElement.closest(`.${this.values.styleFormPartItems}`);
            checkFieldHelperInfo(formPart);
            formPart.append(this.container);
        }
    }
    static DragOverHandler(values) {
        var typeMoving = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;
        if (typeMoving === values.typePart)
            return;
        //if ((typeMoving === values.typeField || typeMoving == values.typeActiveField) && typeOver === values.typeActivePart) {
        //    values.overElement.classList.add(values.styleOverFromField);
        //}
        if (typeMoving === values.typeField) {
            var formPart = values.overElement.closest(`.${values.styleFormPart}`);
            if (formPart) {
                values.Parts.forEach(part => {
                    var partId = part.getData().id;
                    var elm = document.getElementById(partId);
                    elm.classList.remove(values.styleOverFromField);
                });
                formPart.classList.add(values.styleOverFromField);
            }
        }
        if (typeMoving === values.typeActiveField && typeOver === values.typeActiveField) {
            values.overElement.classList.add(values.styleOverFromPart);
        }
    }
    static DragLeaveHandler(values) {
        if (!values.overElement)
            return;
        values.overElement.classList.remove(values.styleOverFromField);
    }
    static DragDropHandler(values) {
        var type = values.movingElement.dataset.type;
        values.overElement.classList.remove(values.styleOverFromField);
        var typeOver = values.overElement.dataset.type;
        if (typeOver === values.typePlaceBuilder) {
            typeOver = values.typeActivePart;
            if (values.Parts.length == 0) {
                var part = new formPart_1.FormPart(values);
                part.addToPlace();
                if (document.getElementById(part.getData().id))
                    values.Parts.push(part);
            }
            values.overElement = values.Parts[0].container;
            var formPart = values.Parts[0].container;
        }
        if (typeOver !== values.typeActivePart && typeOver !== values.typeActiveField)
            return;
        var formPart = values.overElement.closest(`.${values.styleFormPart}`);
        formPart.classList.remove(values.styleOverFromField);
        //add a field to the form part
        if (type === values.typeField) {
            var field = new Field(values);
            __classPrivateFieldGet(field, _Field_data, "f").partId = formPart.id;
            __classPrivateFieldGet(field, _Field_data, "f").type = Number(values.movingElement.dataset.field);
            __classPrivateFieldGet(field, _Field_data, "f").defaultValue = "";
            __classPrivateFieldGet(field, _Field_data, "f").triggerId = "9C85B07F-9236-4314-A29E-87B20093CF82";
            field.addToFormPart();
            if (document.getElementById(__classPrivateFieldGet(field, _Field_data, "f").id))
                values.Fields.push(field);
        }
        if (type === values.typeActiveField) {
            //self-removal protection
            if (values.movingElement === values.overElement)
                return;
            var items = values.movingElement.closest(`.${values.styleFormPartItems}`);
            items.removeChild(values.movingElement);
            //move a field to the form part
            if (typeOver === values.typeActivePart) {
                items = values.overElement.querySelector(`.${values.styleFormPartItems}`);
                items.insertBefore(values.movingElement, undefined);
            }
            //move a field among the active fields
            if (typeOver === values.typeActiveField) {
                items = values.overElement.closest(`.${values.styleFormPartItems}`);
                items.insertBefore(values.movingElement, values.overElement);
            }
            values.Fields.find(field => {
                if (__classPrivateFieldGet(field, _Field_data, "f").id === values.movingElement.id)
                    __classPrivateFieldGet(field, _Field_data, "f").partId = formPart.id;
            });
        }
        //reindex sequence
        var activeFields = document.querySelectorAll(`.${values.styleFormPartItem}`);
        for (var i = 0; i < activeFields.length; i++) {
            var activeField = values.Fields.find(field => {
                return field.getData().id === activeFields[i].id;
            });
            var data = Object.assign({}, activeField.getData());
            data.sequence = i;
            activeField.setData(data);
        }
    }
}
exports.Field = Field;
_Field_data = new WeakMap(), _Field_titleName = new WeakMap(), _Field_titleType = new WeakMap();
function checkFieldHelperInfo(container) {
    if (container.querySelector(".infoDragField")) {
        container.innerHTML = "";
    }
}
exports.checkFieldHelperInfo = checkFieldHelperInfo;
class FieldItem {
    constructor() {
        this.id = (0, utilities_1.generateUID)();
        this.name = "New Field";
        this.active = true;
        this.listItems = [];
    }
}
exports.FieldItem = FieldItem;
//# sourceMappingURL=field.js.map