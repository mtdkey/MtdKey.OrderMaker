import { Values } from "./values";
import { builderDragDrop, builderDragEnd, builderDragOver, builderDragStart } from ".";
import { LoadJsonData } from "./mapper";

export function Init(values: Values) {

    LoadJsonData(values);
    SetButtonsEvents(values);

    document.querySelectorAll(".element").forEach(function (item) {
        item.addEventListener('dragstart', builderDragStart);
        item.addEventListener('dragend', builderDragEnd);
    });

    values.placeBuilder.addEventListener("dragover", builderDragOver);
    values.placeBuilder.addEventListener("drop", builderDragDrop);

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




function SetButtonsEvents(values: Values) {
    values.btnFormEditor.addEventListener("click", (e) => {
        values.formDialogName.value = values.Form.name;
        values.formDialog.show();
    });
}