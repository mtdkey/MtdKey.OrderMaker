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
var _ImageSelector_instances, _ImageSelector_data, _ImageSelector_removeUpload;
Object.defineProperty(exports, "__esModule", { value: true });
exports.ImageSelector = void 0;
const ripple_1 = require("../ripple/ripple");
require("./imageSelector.css");
class ImageSelector {
    constructor(id, callback = undefined) {
        _ImageSelector_instances.add(this);
        _ImageSelector_data.set(this, void 0);
        this.callback = callback;
        this.container = document.getElementById(id);
        this.input = this.container.querySelector(".file-upload-input");
        this.imageUploadWrap = this.container.querySelector(".image-upload-wrap");
        this.fileUploadImage = this.container.querySelector(".file-upload-image");
        this.fileUploadContent = this.container.querySelector(".file-upload-content");
        this.buttonRemove = this.container.querySelector("button");
        this.input.addEventListener("change", this.readURL.bind(this));
        this.buttonRemove.addEventListener("click", __classPrivateFieldGet(this, _ImageSelector_instances, "m", _ImageSelector_removeUpload).bind(this));
        this.imageUploadWrap.addEventListener("dragover", this.dragOver.bind(this));
        this.imageUploadWrap.addEventListener("dragleave", this.dragLeave.bind(this));
        __classPrivateFieldSet(this, _ImageSelector_data, { base64string: "", size: 0, type: "" }, "f");
        (0, ripple_1.addRippleEffect)(this.container);
    }
    getImage() {
        return `data:${__classPrivateFieldGet(this, _ImageSelector_data, "f").type};base64,${__classPrivateFieldGet(this, _ImageSelector_data, "f").base64string}`;
    }
    clearData() {
        __classPrivateFieldGet(this, _ImageSelector_data, "f").base64string = "";
        __classPrivateFieldGet(this, _ImageSelector_data, "f").size = 0;
        __classPrivateFieldGet(this, _ImageSelector_data, "f").type = "";
    }
    setData(data) {
        __classPrivateFieldGet(this, _ImageSelector_data, "f").base64string = data.base64string;
        __classPrivateFieldGet(this, _ImageSelector_data, "f").size = data.size;
        __classPrivateFieldGet(this, _ImageSelector_data, "f").type = data.type;
    }
    getData() {
        return __classPrivateFieldGet(this, _ImageSelector_data, "f");
    }
    dragOver(e) {
        e.preventDefault();
        this.imageUploadWrap.classList.add("image-dropping");
    }
    dragLeave(e) {
        e.preventDefault();
        this.imageUploadWrap.classList.remove("image-dropping");
    }
    readURL() {
        if (this.input.files && this.input.files[0]) {
            var reader = new FileReader();
            var imageSelector = this;
            reader.onload = function (e) {
                var imgInfo = {
                    base64string: e.target.result.toString(),
                    size: imageSelector.input.files[0].size,
                    type: imageSelector.input.files[0].type,
                };
                imgInfo.base64string = imgInfo.base64string.replace(`data:${imgInfo.type};base64,`, "");
                imageSelector.addImage(imgInfo, true);
            };
            reader.readAsDataURL(this.input.files[0]);
        }
    }
    addImage(data, userInit = true) {
        if (!data.base64string || data.base64string.length === 0) {
            this.input.value = "";
            this.clearData();
            this.fileUploadContent.style.display = "none";
            this.imageUploadWrap.style.display = "block";
            return;
        }
        __classPrivateFieldSet(this, _ImageSelector_data, data, "f");
        this.fileUploadImage.src = this.getImage();
        this.imageUploadWrap.style.display = "none";
        this.fileUploadContent.style.display = "block";
        if (userInit && this.callback)
            this.callback();
    }
}
exports.ImageSelector = ImageSelector;
_ImageSelector_data = new WeakMap(), _ImageSelector_instances = new WeakSet(), _ImageSelector_removeUpload = function _ImageSelector_removeUpload() {
    this.input.value = "";
    this.clearData();
    this.fileUploadContent.style.display = "none";
    this.imageUploadWrap.style.display = "block";
    if (this.callback)
        this.callback();
};
//# sourceMappingURL=index.js.map