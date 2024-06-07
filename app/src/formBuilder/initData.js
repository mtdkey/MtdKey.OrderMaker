"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Init = void 0;
const _1 = require(".");
const mapper_1 = require("./mapper");
function Init(values) {
    (0, mapper_1.LoadJsonData)(values);
    SetButtonsEvents(values);
    document.querySelectorAll(".element").forEach(function (item) {
        item.addEventListener('dragstart', _1.builderDragStart);
        item.addEventListener('dragend', _1.builderDragEnd);
    });
    values.placeBuilder.addEventListener("dragover", _1.builderDragOver);
    values.placeBuilder.addEventListener("drop", _1.builderDragDrop);
    values.Parts.forEach(part => {
        part.addToPlace();
        values.Fields.find(field => {
            if (field.getData().partId === part.getData().id)
                part.addField(field);
        });
    });
    values.btnClearStorage.addEventListener("click", () => {
        location.reload();
    });
}
exports.Init = Init;
function SetButtonsEvents(values) {
    values.btnFormEditor.addEventListener("click", (e) => {
        values.formDialogName.value = values.Form.name;
        values.formDialog.show();
    });
}
//# sourceMappingURL=initData.js.map