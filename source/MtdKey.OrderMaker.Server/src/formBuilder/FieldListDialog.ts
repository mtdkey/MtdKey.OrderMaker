import { MDCTextField } from "@material/textfield";
import bootstrap = require("bootstrap");
import { Actions, ChangeAction } from ".";
import { Values } from "./values";
import { FieldType, GetFileTypeInfo } from "./fieldType";
import { FieldItem } from "./field";
import { generateUID } from "./utilities";

export class FieldListDialog {

    values: Values;
    container: HTMLDivElement;
    screen: bootstrap.Modal;
    inputName: MDCTextField;
    #fieldItem: FieldItem;
    inputSearch: MDCTextField;
    buttonAddItem: HTMLButtonElement;
    typeInfo: HTMLDivElement;
    itemPlace: HTMLDivElement;

    constructor(values: Values) {

        this.values = values;
        this.container = document.getElementById("fieldListDialog") as HTMLDivElement;
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new MDCTextField(document.getElementById("fieldListName"));
        this.inputSearch = new MDCTextField(document.getElementById("searchItem"));
        this.buttonAddItem = document.getElementById("buttonAddItem") as HTMLButtonElement;
        this.typeInfo = this.container.querySelector(`.${values.styleFieldType}`) as HTMLDivElement;
        this.itemPlace = document.getElementById("itemPlace") as HTMLDivElement;

        EventsHandler(this);
    }

    getData(): Readonly<FieldItem> {
        return this.#fieldItem;
    }

    setData(fieldItem: FieldItem) {
        this.#fieldItem = fieldItem;
        this.values.fieldListDialog.inputName.value = this.#fieldItem.name;
        var typeText = GetFileTypeInfo(fieldItem.type);
        this.typeInfo.innerHTML = "";
        this.typeInfo.appendChild(typeText);

        this.itemPlace.innerHTML = "";

        fieldItem.listItems.forEach(item => {
            AddItemToPlace(item, this);
        });
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

function AddItemToPlace(listItem: ListItem, binder: FieldListDialog) {

    var template = binder.values.itemTemplate.content.cloneNode(true) as HTMLDivElement;
    var templateItem = template.firstElementChild as HTMLDivElement;
    var inputText = templateItem.querySelector("[data-item-id]") as HTMLInputElement;
    var inputRadio = templateItem.querySelector("[data-item-default]") as HTMLInputElement;
    var buttonRemover = templateItem.querySelector("[data-item-remover]") as HTMLButtonElement;      

    templateItem.dataset.item = listItem.Id;
    inputText.dataset.itemID = listItem.Id;
    inputText.value = listItem.Name;
    inputRadio.dataset.itemDefault = listItem.Id;
    inputRadio.checked = binder.getData().defaultValue == listItem.Id;

    binder.itemPlace.appendChild(templateItem);


    buttonRemover.addEventListener("click", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var currentField = binder.getVluesFieldData();
            var index = currentField.listItems.findIndex(x => x.Id == listItem.Id);
            currentField.listItems.splice(index, 1);

            if (listItem.Id.toLowerCase() === currentField.defaultValue.toLowerCase() && currentField.listItems.length > 0) {
                currentField.defaultValue = currentField.listItems[0].Id;
                var radio = document.querySelector(`[data-item-default='${currentField.listItems[0].Id}']`) as HTMLInputElement;
                radio.checked = true;
            }

            binder.setValuesFieldData(currentField);

            var currentItem = document.querySelector(`[data-item="${listItem.Id}"]`);
            binder.itemPlace.removeChild(currentItem);
        });
    });


    inputRadio.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var currentField = binder.getVluesFieldData();
            currentField.defaultValue = inputRadio.dataset.itemDefault;
            binder.setValuesFieldData(currentField);
        });
    });

    inputText.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var currentField = binder.getVluesFieldData();
            var index = currentField.listItems.findIndex(x => x.Id == listItem.Id);
            currentField.listItems[index].Name = inputText.value;
            binder.setValuesFieldData(currentField);
        });
    });   

}

function EventsHandler(binder: FieldListDialog) {

    binder.inputName.root.addEventListener("change", () => {
        ChangeAction(Actions.ChangeFieldParameters, () => {
            var fieldData = binder.getVluesFieldData();
            fieldData.name = binder.inputName.value;
            binder.setValuesFieldData(fieldData);
        });
    });

    binder.buttonAddItem.addEventListener("click", () => {

        var template = binder.values.itemTemplate.content.cloneNode(true) as HTMLDivElement;
        var item = template.firstElementChild as HTMLDivElement;
        var inputText = item.querySelector("[data-item-id]") as HTMLInputElement;
        var inputRadio = item.querySelector("[data-item-default]") as HTMLInputElement;
        var buttonRemover = item.querySelector("[data-item-remover]") as HTMLButtonElement;

        ChangeAction(Actions.ChangeFieldParameters, () => {


            var itemName = binder.inputSearch.value;
            var fieldItem = binder.getVluesFieldData();
            var isFirstItem = fieldItem.listItems.length === 0;

            const uid = generateUID();
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
                    var radio = document.querySelector(`[data-item-default='${currentField.listItems[0].Id}']`) as HTMLInputElement;
                    radio.checked = true;
                }

                binder.setValuesFieldData(currentField);

                var currentItem = document.querySelector(`[data-item="${uid}"]`);
                binder.itemPlace.removeChild(currentItem);
            });

            inputRadio.addEventListener("change", () => {
                ChangeAction(Actions.ChangeFieldParameters, () => {
                    var currentField = binder.getVluesFieldData();
                    currentField.defaultValue = inputRadio.dataset.itemDefault;
                    binder.setValuesFieldData(currentField);
                });
            });

            inputText.addEventListener("change", () => {
                ChangeAction(Actions.ChangeFieldParameters, () => {
                    var currentField = binder.getVluesFieldData();
                    var index = currentField.listItems.findIndex(x => x.Id == item.dataset.item);
                    currentField.listItems[index].Name = inputText.value;
                    binder.setValuesFieldData(currentField);
                });
            });      
        });
    });

}