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
var _PartDialog_partId;
Object.defineProperty(exports, "__esModule", { value: true });
exports.PartDialog = void 0;
const textfield_1 = require("@material/textfield");
const bootstrap = require("bootstrap");
const _1 = require(".");
const imageSelector_1 = require("../imageSelector");
class PartDialog {
    constructor(values) {
        _PartDialog_partId.set(this, void 0);
        this.values = values;
        this.container = document.getElementById("partDialog");
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new textfield_1.MDCTextField(document.getElementById("partName"));
        this.imagePart = new imageSelector_1.ImageSelector("imagePart", this.callback.bind(this));
        this.inputRadioColumns = document.getElementById("radioColumns");
        this.inputRadioLines = document.getElementById("radioLines");
        this.inputSwithTitle = document.getElementById("swithTitle");
        //document.querySelector('input[name="genderS"]:checked').value;
        var binder = this;
        this.inputName.root.addEventListener("keyup", (event) => {
            if (event.isComposing || event.keyCode === 229) {
                return;
            }
            (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === __classPrivateFieldGet(binder, _PartDialog_partId, "f"));
                var partData = Object.assign({}, part.getData());
                partData.name = binder.inputName.value;
                part.setData(partData);
            });
        });
        this.inputRadioColumns.addEventListener("change", (e) => {
            (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === __classPrivateFieldGet(binder, _PartDialog_partId, "f"));
                part.items.classList.add(binder.values.styleFormPartColumns);
                var partData = Object.assign({}, part.getData());
                partData.styleType = 5;
                part.setData(partData);
            });
        });
        this.inputRadioLines.addEventListener("change", (e) => {
            (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === __classPrivateFieldGet(binder, _PartDialog_partId, "f"));
                part.items.classList.remove(binder.values.styleFormPartColumns);
                var partData = Object.assign({}, part.getData());
                partData.styleType = 4;
                part.setData(partData);
            });
        });
        this.inputSwithTitle.addEventListener("change", (e) => {
            var inputTitle = e.currentTarget;
            (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === __classPrivateFieldGet(binder, _PartDialog_partId, "f"));
                var partData = Object.assign({}, part.getData());
                partData.title = inputTitle.checked;
                part.setData(partData);
            });
        });
    }
    callback() {
        var binder = this;
        (0, _1.ChangeAction)(_1.Actions.ChangePartParameters, () => {
            var part = binder.values.Parts.find(item => item.container.id === __classPrivateFieldGet(binder, _PartDialog_partId, "f"));
            var partData = Object.assign({}, part.getData());
            partData.imageData = binder.imagePart.getData().base64string;
            partData.imageSize = binder.imagePart.getData().size;
            partData.imageType = binder.imagePart.getData().type;
            part.setData(partData);
            return;
        });
    }
    setPartId(partId) {
        __classPrivateFieldSet(this, _PartDialog_partId, partId, "f");
    }
    getPartId() {
        return __classPrivateFieldGet(this, _PartDialog_partId, "f");
    }
}
exports.PartDialog = PartDialog;
_PartDialog_partId = new WeakMap();
//# sourceMappingURL=partDialog.js.map