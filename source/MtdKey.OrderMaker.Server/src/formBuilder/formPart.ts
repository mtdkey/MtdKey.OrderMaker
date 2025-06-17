import bootstrap = require("bootstrap");
import { Actions, ChangeAction, builderDragDrop, builderDragEnd, builderDragLeave, builderDragOver, builderDragStart } from ".";
import { addRippleEffect } from "../ripple/ripple";
import { Field, FieldItem, checkFieldHelperInfo } from "./field";
import { generateUID } from "./utilities";
import { Values } from "./values";
import { ImageInfo } from "../imageSelector";


export class FormPart {

    values: Values;
    container: HTMLDivElement;
    items: HTMLDivElement;
    #data: FormPartItem;
    #title: HTMLDivElement;
    actions: bootstrap.Dropdown;
    btnActions: HTMLButtonElement;
    btnPartEditor: HTMLLinkElement;
    btnPartRemover: HTMLLinkElement;

    constructor(values: Values) {
        this.values = values;
        var template = values.formPartTemplate.content.cloneNode(true) as HTMLDivElement;
        this.container = template.firstElementChild as HTMLDivElement;
        this.#data = new FormPartItem();
        this.#data.formId = values.Form.id;
        this.items = this.container.querySelector(`.${values.styleFormPartItems}`);
        this.#title = this.container.querySelector(`.${values.styleFormPartTitle}`);

        this.container.addEventListener("dragstart", builderDragStart);
        this.container.addEventListener("dragend", builderDragEnd);
        this.container.addEventListener("dragover", builderDragOver);
        this.container.addEventListener("dragleave", builderDragLeave);
        this.container.addEventListener("drop", builderDragDrop);

        this.container.id = this.#data.id;
        this.#title.textContent = this.#data.name;
        this.btnActions = this.container.querySelector(".dropdown-toggle") as HTMLButtonElement;
        this.actions = new bootstrap.Dropdown(this.container.querySelector(".dropdown-toggle"));
        this.btnPartEditor = this.container.querySelector("[data-action-editor]") as HTMLLinkElement;
        this.btnPartRemover = this.container.querySelector("[data-action-remover]") as HTMLLinkElement;

        var binder = this;

        this.btnActions.addEventListener("click", () => {
            this.actions.toggle();
            values.partDialog.inputName.value = binder.#data.name;
            values.partDialog.setPartId(binder.container.id);

            var imageInfo: ImageInfo = {
                base64string: binder.#data.imageData,
                size: binder.#data.imageSize,
                type: binder.#data.imageType
            }

            values.partDialog.imagePart.addImage(imageInfo, false);

            values.partDialog.inputRadioColumns.checked = binder.#data.styleType == 5;
            values.partDialog.inputRadioLines.checked = binder.#data.styleType == 4 || binder.#data.styleType === undefined;
            values.partDialog.inputSwithTitle.checked = binder.#data.title;
        });

        this.btnPartEditor.addEventListener("click", (e) => {
            values.partDialog.screen.show();
        });

        this.btnPartRemover.addEventListener("click", (e) => {

            ChangeAction(Actions.ChangePartParameters, () => {
                var index = values.Parts.indexOf(binder);
                values.Parts.splice(index, 1);
                var fields = new Array<Field>();
                values.Fields.forEach(item => {
                    if (item.getData().partId === binder.#data.id)
                        fields.push(item);
                });
                fields.forEach(field => { values.Fields.splice(values.Fields.indexOf(field), 1); });
                values.placeBuilder.removeChild(binder.container);
            });
        });

        addRippleEffect(this.container);
    }

    setData(data: FormPartItem) {
        this.container.id = data.id;
        this.#data = data;
        this.#title.textContent = data.name;
    }

    getData(): Readonly<FormPartItem> {
        return this.#data;
    }

    addFields(fields: Array<Field>) {
        fields.forEach(field => this.addField(field));
    }

    addField(field: Field) {
        checkFieldHelperInfo(this.items);
        this.items.append(field.container);
    }

    addToPlace() {

        checkPartHelperInfo(this.values);

        var beforeElement = this.values.placeBuilder === this.values.overElement ? undefined : this.values.overElement;
        this.values.placeBuilder.insertBefore(this.container, beforeElement);

        if (this.#data.styleType === 5)
            this.items.classList.add(this.values.styleFormPartColumns);
        else
            this.items.classList.remove(this.values.styleFormPartColumns);
    }

    static DragOverHandler(values: Values) {

        var typeMoving = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;

        if (typeOver !== values.typeActivePart) return;

        if (typeMoving === values.typeActivePart || typeMoving === values.typePart) {
            values.overElement.classList.add(values.styleOverFromPart);
        }
    }

    static DragLeaveHandler(values: Values) {
        if (!values.overElement) return;
        values.overElement.classList.remove(values.styleOverFromPart);
    }

    static DragDropHandler(values: Values) {

        var type = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;

        values.overElement.classList.remove(values.styleOverFromPart);

        if (typeOver !== values.typeActivePart && values.overElement !== values.placeBuilder) return;

        if (type === values.typePart) {
            var formPart = new FormPart(values);
            formPart.addToPlace();
            if (document.getElementById(formPart.#data.id))
                values.Parts.push(formPart);
        }

        if (type === values.typeActivePart) {

            //self-removal protection
            if (values.movingElement === values.overElement || typeOver === values.typeActiveField) return;

            values.placeBuilder.removeChild(values.movingElement);
            var beforeElement = values.placeBuilder === values.overElement ? undefined : values.overElement;
            values.placeBuilder.insertBefore(values.movingElement, beforeElement);
        }

        var parts = document.querySelectorAll(`.${values.styleFormPart}`);
        for (var i = 0; i < parts.length; i++) {

            var part = values.Parts.find(part =>
                part.getData().id === parts[i].id
            );

            var data = { ...part.getData() };
            data.sequence = i;
            part.setData(data);
        }
    }
}

export function checkPartHelperInfo(values: Values) {

    if (document.querySelector(".infoFirstOfAll")) {
        values.placeBuilder.innerHTML = "";
    }
}

export class FormPartItem {
    id: string;
    name: string;
    description: string;
    sequence: number;
    styleType: number;
    formId: string;
    title: boolean;
    active: boolean;
    imageData: string;
    imageSize: number;
    imageType: string;

    constructor() {
        this.id = generateUID();
        this.name = "New Form Part";
        this.active = true;
    }
}
