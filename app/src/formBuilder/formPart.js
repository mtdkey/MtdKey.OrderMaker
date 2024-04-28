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
var _FormPart_data, _FormPart_title;
Object.defineProperty(exports, "__esModule", { value: true });
exports.FormPartItem = exports.checkPartHelperInfo = exports.FormPart = void 0;
const bootstrap = require("bootstrap");
const _1 = require(".");
const ripple_1 = require("../ripple/ripple");
const field_1 = require("./field");
const utilities_1 = require("./utilities");
class FormPart {
    constructor(values) {
        _FormPart_data.set(this, void 0);
        _FormPart_title.set(this, void 0);
        this.values = values;
        var template = values.formPartTemplate.content.cloneNode(true);
        this.container = template.firstElementChild;
        __classPrivateFieldSet(this, _FormPart_data, new FormPartItem(), "f");
        __classPrivateFieldGet(this, _FormPart_data, "f").formId = values.Form.id;
        this.items = this.container.querySelector(`.${values.styleFormPartItems}`);
        __classPrivateFieldSet(this, _FormPart_title, this.container.querySelector(`.${values.styleFormPartTitle}`), "f");
        this.container.addEventListener("dragstart", _1.builderDragStart);
        this.container.addEventListener("dragend", _1.builderDragEnd);
        this.container.addEventListener("dragover", _1.builderDragOver);
        this.container.addEventListener("dragleave", _1.builderDragLeave);
        this.container.addEventListener("drop", _1.builderDragDrop);
        this.container.id = __classPrivateFieldGet(this, _FormPart_data, "f").id;
        __classPrivateFieldGet(this, _FormPart_title, "f").textContent = __classPrivateFieldGet(this, _FormPart_data, "f").name;
        this.btnActions = this.container.querySelector(".dropdown-toggle");
        this.actions = new bootstrap.Dropdown(this.container.querySelector(".dropdown-toggle"));
        this.btnPartEditor = this.container.querySelector("[data-action-editor]");
        this.btnPartRemover = this.container.querySelector("[data-action-remover]");
        var binder = this;
        this.btnActions.addEventListener("click", () => {
            this.actions.toggle();
            values.partDialog.inputName.value = __classPrivateFieldGet(binder, _FormPart_data, "f").name;
            values.partDialog.setPartId(binder.container.id);
            var imageInfo = {
                base64string: __classPrivateFieldGet(binder, _FormPart_data, "f").imageData,
                size: __classPrivateFieldGet(binder, _FormPart_data, "f").imageSize,
                type: __classPrivateFieldGet(binder, _FormPart_data, "f").imageType
            };
            values.partDialog.imagePart.addImage(imageInfo, false);
            values.partDialog.inputRadioColumns.checked = __classPrivateFieldGet(binder, _FormPart_data, "f").styleType == 5;
            values.partDialog.inputRadioLines.checked = __classPrivateFieldGet(binder, _FormPart_data, "f").styleType == 4 || __classPrivateFieldGet(binder, _FormPart_data, "f").styleType === undefined;
            values.partDialog.inputSwithTitle.checked = __classPrivateFieldGet(binder, _FormPart_data, "f").title;
        });
        this.btnPartEditor.addEventListener("click", (e) => {
            values.partDialog.screen.show();
        });
        this.btnPartRemover.addEventListener("click", (e) => {
            (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
                var index = values.Parts.indexOf(binder);
                values.Parts.splice(index, 1);
                var fields = new Array();
                values.Fields.forEach(item => {
                    if (item.getData().partId === __classPrivateFieldGet(binder, _FormPart_data, "f").id)
                        fields.push(item);
                });
                fields.forEach(field => { values.Fields.splice(values.Fields.indexOf(field), 1); });
                values.placeBuilder.removeChild(binder.container);
            });
        });
        (0, ripple_1.addRippleEffect)(this.container);
    }
    setData(data) {
        this.container.id = data.id;
        __classPrivateFieldSet(this, _FormPart_data, data, "f");
        __classPrivateFieldGet(this, _FormPart_title, "f").textContent = data.name;
    }
    getData() {
        return __classPrivateFieldGet(this, _FormPart_data, "f");
    }
    addFields(fields) {
        fields.forEach(field => this.addField(field));
    }
    addField(field) {
        (0, field_1.checkFieldHelperInfo)(this.items);
        this.items.append(field.container);
    }
    addToPlace() {
        checkPartHelperInfo(this.values);
        var beforeElement = this.values.placeBuilder === this.values.overElement ? undefined : this.values.overElement;
        this.values.placeBuilder.insertBefore(this.container, beforeElement);
        if (__classPrivateFieldGet(this, _FormPart_data, "f").styleType === 5)
            this.items.classList.add(this.values.styleFormPartColumns);
        else
            this.items.classList.remove(this.values.styleFormPartColumns);
    }
    static DragOverHandler(values) {
        var typeMoving = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;
        if (typeOver !== values.typeActivePart)
            return;
        if (typeMoving === values.typeActivePart || typeMoving === values.typePart) {
            values.overElement.classList.add(values.styleOverFromPart);
        }
    }
    static DragLeaveHandler(values) {
        if (!values.overElement)
            return;
        values.overElement.classList.remove(values.styleOverFromPart);
    }
    static DragDropHandler(values) {
        var type = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;
        values.overElement.classList.remove(values.styleOverFromPart);
        if (typeOver !== values.typeActivePart && values.overElement !== values.placeBuilder)
            return;
        if (type === values.typePart) {
            var formPart = new FormPart(values);
            formPart.addToPlace();
            if (document.getElementById(__classPrivateFieldGet(formPart, _FormPart_data, "f").id))
                values.Parts.push(formPart);
        }
        if (type === values.typeActivePart) {
            //self-removal protection
            if (values.movingElement === values.overElement || typeOver === values.typeActiveField)
                return;
            values.placeBuilder.removeChild(values.movingElement);
            var beforeElement = values.placeBuilder === values.overElement ? undefined : values.overElement;
            values.placeBuilder.insertBefore(values.movingElement, beforeElement);
        }
        var parts = document.querySelectorAll(`.${values.styleFormPart}`);
        for (var i = 0; i < parts.length; i++) {
            var part = values.Parts.find(part => part.getData().id === parts[i].id);
            var data = Object.assign({}, part.getData());
            data.sequence = i;
            part.setData(data);
        }
    }
}
exports.FormPart = FormPart;
_FormPart_data = new WeakMap(), _FormPart_title = new WeakMap();
function checkPartHelperInfo(values) {
    if (document.querySelector(".infoFirstOfAll")) {
        values.placeBuilder.innerHTML = "";
    }
}
exports.checkPartHelperInfo = checkPartHelperInfo;
class FormPartItem {
    constructor() {
        this.id = (0, utilities_1.generateUID)();
        this.name = "New Form Part";
        this.active = true;
    }
}
exports.FormPartItem = FormPartItem;
//# sourceMappingURL=formPart.js.map