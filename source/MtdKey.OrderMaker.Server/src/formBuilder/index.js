"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Actions = void 0;
exports.builderDragStart = builderDragStart;
exports.builderDragEnd = builderDragEnd;
exports.builderDragOver = builderDragOver;
exports.builderDragLeave = builderDragLeave;
exports.builderDragDrop = builderDragDrop;
exports.ChangeAction = ChangeAction;
require("./configFormBuilder.css");
require("../imageSelector/imageSelector.css");
const field_1 = require("./field");
const formPart_1 = require("./formPart");
const initData_1 = require("./initData");
const values_1 = require("./values");
const mapper_1 = require("./mapper");
var values = undefined;
var Actions;
(function (Actions) {
    Actions[Actions["DragDrop"] = 0] = "DragDrop";
    Actions[Actions["ChangeFormParameters"] = 1] = "ChangeFormParameters";
    Actions[Actions["ChangePartParameters"] = 2] = "ChangePartParameters";
    Actions[Actions["ChangeFieldParameters"] = 3] = "ChangeFieldParameters";
})(Actions || (exports.Actions = Actions = {}));
function builderDragStart(e) {
    e.stopPropagation();
    var elm = e.currentTarget;
    values.movingElement = elm;
    elm.style.opacity = "0.4";
    elm.classList.add(values.styleTaken);
}
function builderDragEnd(e) {
    e.currentTarget.style.opacity = "1";
    e.currentTarget.classList.remove(values.styleTaken);
    values.movingElement = undefined;
    values.Parts.forEach(part => {
        var elm = document.getElementById(part.getData().id);
        elm.classList.remove(values.styleOverFromField);
    });
}
function builderDragOver(e) {
    e.preventDefault();
    e.stopPropagation();
    values.overElement = e.currentTarget;
    formPart_1.FormPart.DragOverHandler(values);
    field_1.Field.DragOverHandler(values);
}
function builderDragLeave(e) {
    e.preventDefault();
    e.stopPropagation();
    formPart_1.FormPart.DragLeaveHandler(values);
    field_1.Field.DragLeaveHandler(values);
    values.overElement = undefined;
}
function builderDragDrop(e) {
    e.preventDefault();
    e.stopPropagation();
    ChangeAction(Actions.DragDrop, () => {
        formPart_1.FormPart.DragDropHandler(values);
        field_1.Field.DragDropHandler(values);
    });
}
function ChangeAction(action, callback) {
    //console.info(Actions[action]);
    callback();
    (0, mapper_1.SaveHistory)(values);
    values.warnMessage.classList.add("warn-message-on");
    values.btnClearStorage.classList.remove("d-none");
}
values = new values_1.Values();
setTimeout(() => { (0, initData_1.Init)(values); }, 600);
function propertyOf(name) {
    return name;
}
Object.keys(values.Fields).forEach((item, curr) => {
    values.Fields[curr].getData();
});
//# sourceMappingURL=index.js.map