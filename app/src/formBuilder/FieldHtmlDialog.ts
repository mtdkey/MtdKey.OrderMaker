import { MDCTextField } from "@material/textfield";
import bootstrap = require("bootstrap");
import { Actions, ChangeAction } from ".";
import { Values } from "./values";
import { FieldType, GetFileTypeInfo } from "./fieldType";
import { FieldItem } from "./field";

export class FieldHtmlDialog {

    #fieldItem: FieldItem;
    screen: bootstrap.Modal;
    inputName: MDCTextField;
    container: HTMLDivElement;
    values: Values;
    typeInfo: HTMLDivElement;
    inputReadonly: HTMLInputElement;


    constructor(values: Values) {

        this.values = values;
        this.container = document.getElementById("fieldHtmlDialog") as HTMLDivElement;
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new MDCTextField(document.getElementById("fieldHtmlName"));
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`) as HTMLDivElement;
        this.inputReadonly = document.getElementById("swithHtmlReadonly") as HTMLInputElement;

        EventsHandler(this);
    }

    getData(): Readonly<FieldItem> {
        return this.#fieldItem;
    }

    setData(fieldItem: FieldItem) {
        this.#fieldItem = fieldItem;
        this.values.fieldHtmlDialog.inputName.value = this.#fieldItem.name;
        var typeText = GetFileTypeInfo(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);
        this.inputReadonly.checked = this.#fieldItem.readonly;
    }

    getVluesFieldData() {
        var field = this.values.Fields.find(item => item.getData().id === this.getData().id);
        var fieldData = { ...field.getData() };
        return fieldData;
    }

    setValuesFieldData(fieldItem: FieldItem) {
        var field = this.values.Fields.find(item => item.getData().id === this.getData().id);
        field.setData(fieldItem);
    }
}

function EventsHandler(binder: FieldHtmlDialog) {


    binder.inputName.root.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.inputReadonly.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.readonly = binder.inputReadonly.checked;
            binder.setValuesFieldData(fieldData);
        });
    });

}