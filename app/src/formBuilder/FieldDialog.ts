import { MDCTextField } from "@material/textfield";
import bootstrap = require("bootstrap");
import { Actions, ChangeAction } from ".";
import { Values } from "./values";
import { FieldType, GetFileTypeInfo } from "./fieldType";
import { FieldItem } from "./field";

export class FieldDialog {

    #fieldItem: FieldItem;
    screen: bootstrap.Modal;
    inputName: MDCTextField;
    inputDefault: MDCTextField;
    container: HTMLDivElement;
    values: Values;
    typeInfo: HTMLDivElement;
    inputRequired: HTMLInputElement;
    inputReadonly: HTMLInputElement;
    inputNotset: HTMLInputElement;
    inputDateNow: HTMLInputElement;
    inputUserGroup: HTMLInputElement;
    inputUserName: HTMLInputElement;
    textParams: HTMLDivElement;
    inputSelectForm: HTMLSelectElement;
    listFormParam: HTMLElement;


    constructor(values: Values) {

        this.values = values;
        this.container = document.getElementById("fieldDialog") as HTMLDivElement;
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new MDCTextField(document.getElementById("fieldName"));
        this.inputDefault = new MDCTextField(document.getElementById("defaultValue"));
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`) as HTMLDivElement;
        this.inputRequired = document.getElementById("swithRequired") as HTMLInputElement;
        this.inputReadonly = document.getElementById("swithReadonly") as HTMLInputElement;
        this.inputNotset = document.getElementById("radioNotset") as HTMLInputElement;
        this.inputDateNow = document.getElementById("radioDatetime") as HTMLInputElement;
        this.inputUserGroup = document.getElementById("radioUserGroup") as HTMLInputElement;
        this.inputUserName = document.getElementById("radioUserName") as HTMLInputElement;
        this.textParams = document.getElementById("textParams") as HTMLDivElement;
        this.inputSelectForm = document.getElementById("selectForm") as HTMLSelectElement;

        EventsHandler(this);
    }

    getData(): Readonly<FieldItem> {
        return this.#fieldItem;
    }

    setData(fieldItem: FieldItem) {
        this.#fieldItem = fieldItem;
        this.values.fieldDialog.inputName.value = this.#fieldItem.name;
        var typeText = GetFileTypeInfo(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);
        this.inputDefault.value = this.#fieldItem.defaultValue;
        this.inputRequired.checked = this.#fieldItem.required;
        this.inputReadonly.checked = this.#fieldItem.readonly;
        this.inputNotset.checked = this.#fieldItem.triggerId === "9C85B07F-9236-4314-A29E-87B20093CF82";
        this.inputDateNow.checked = this.#fieldItem.triggerId === "D3663BC7-FA05-4F64-8EBD-F25414E459B8";
        this.inputUserGroup.checked = this.#fieldItem.triggerId === "33E8212E-059B-482D-8CBD-DFDB073E3B63";
        this.inputUserName.checked = this.#fieldItem.triggerId === "08FE6202-45D7-46C2-B343-B79FD4831F27";
        this.listFormParam = document.getElementById("listFormParam");

        if (fieldItem.type < 5) {
            this.textParams.classList.remove("d-none");
        } else {
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
            this.#fieldItem.listFormId = this.inputSelectForm.value;
        } else {
            this.#fieldItem.listFormId = "";
            this.listFormParam.classList.add("d-none");
        }
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

function EventsHandler(binder: FieldDialog) {

    binder.inputSelectForm.addEventListener("change", (e) => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var elm = e.target as HTMLSelectElement;
            var data = binder.getVluesFieldData();
            data.listFormId = elm.value;
            binder.setValuesFieldData(data);
        });
    });

    binder.inputName.root.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.inputDefault.root.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.defaultValue = binder.inputDefault.value;
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.inputRequired.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.required = binder.inputRequired.checked;
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

    binder.inputNotset.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "9C85B07F-9236-4314-A29E-87B20093CF82";
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.inputDateNow.addEventListener("change", () => {

        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "33E8212E-059B-482D-8CBD-DFDB073E3B63";
            binder.setValuesFieldData(fieldData);
        });

    });

    binder.inputUserGroup.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "33E8212E-059B-482D-8CBD-DFDB073E3B63";
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.inputUserName.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.triggerId = "08FE6202-45D7-46C2-B343-B79FD4831F27";
            binder.setValuesFieldData(fieldData);
        });
    });

}