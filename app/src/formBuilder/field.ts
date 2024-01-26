import { Values } from "./values";
import { Actions, ChangeAction, builderDragDrop, builderDragEnd, builderDragLeave, builderDragOver, builderDragStart } from ".";
import { addRippleEffect } from "../ripple/ripple";
import { generateUUID } from "./utilities";
import { FieldType, GetFileTypeInfo } from "./fieldType";
import bootstrap = require("bootstrap");
import { Form } from "./form";
import { FormPart } from "./formPart";


export class Field {

    values: Values;
    container: HTMLDivElement;
    #data: FieldItem;
    #titleName: HTMLDivElement;
    #titleType: HTMLDivElement;
    btnEditor: HTMLButtonElement;
    btnRemover: HTMLButtonElement;


    constructor(values: Values) {
        this.values = values;
        this.#data = new FieldItem();

        var template = values.fieldTemplate.content.cloneNode(true) as HTMLDivElement;
        this.container = template.firstElementChild as HTMLDivElement;

        this.#titleName = this.container.querySelector(`.${values.styleFieldName}`);
        this.#titleType = this.container.querySelector(`.${values.styleFieldType}`);
        this.container.addEventListener("dragstart", builderDragStart);
        this.container.addEventListener("dragend", builderDragEnd);
        this.container.addEventListener("dragover", builderDragOver);
        this.container.addEventListener("dragleave", builderDragLeave);
        this.container.addEventListener("drop", builderDragDrop);
        this.container.id = this.#data.id;
        this.#titleName.textContent = this.#data.name;
        this.btnEditor = this.container.querySelector("[data-btn-editor]") as HTMLButtonElement;
        this.btnRemover = this.container.querySelector("[data-btn-remover]") as HTMLButtonElement;

        addRippleEffect(this.container);
        var binder = this;

        this.btnEditor.addEventListener("click", (e) => {
            values.fieldDialog.setData(binder.#data);
            values.fieldDialog.screen.show();
        });

        this.btnRemover.addEventListener("click", (e) => {
            ChangeAction(Actions.ChangeFieldParameters, () => {
                var index = values.Fields.indexOf(binder);
                values.Fields.splice(index, 1);
                var parent = binder.container.closest(`.${binder.values.styleFormPartItems}`);
                parent.removeChild(binder.container);
            });

        });
    }

    setData(data: FieldItem) {
        this.container.id = data.id;
        this.btnEditor.dataset.btnEditor = data.id;
        this.#data.defaultValue = data.defaultValue;
        this.#data = data;
        this.#titleName.textContent = data.name;

        var typeInfo = GetFileTypeInfo(data.type);

        this.#titleType.textContent = "";
        this.#titleType.appendChild(typeInfo);
    }

    getData(): Readonly<FieldItem> {
        return this.#data;
    }

    addToFormPart() {

        var type = this.values.overElement.dataset.type;

        //add field if it is over another part
        if (type === this.values.typeActivePart) {
            var formPart = this.values.overElement.querySelector(`.${this.values.styleFormPartItems}`) as HTMLDivElement;
            checkFieldHelperInfo(formPart);
            formPart.append(this.container);
        }

        //add field if it is over a field - first find the parent formPart
        if (type === this.values.typeActiveField) {
            var formPart = this.values.overElement.closest(`.${this.values.styleFormPartItems}`) as HTMLDivElement;
            checkFieldHelperInfo(formPart);
            formPart.append(this.container);
        }

    }

    static DragOverHandler(values: Values) {

        var typeMoving = values.movingElement.dataset.type;
        var typeOver = values.overElement.dataset.type;

        if (typeMoving === values.typePart) return;

        //if ((typeMoving === values.typeField || typeMoving == values.typeActiveField) && typeOver === values.typeActivePart) {
        //    values.overElement.classList.add(values.styleOverFromField);
        //}

        if (typeMoving === values.typeField) {
            var formPart = values.overElement.closest(`.${values.styleFormPart}`);
            if (formPart) {
                values.Parts.forEach(part => {
                    var partId = part.getData().id;
                    var elm = document.getElementById(partId);
                    elm.classList.remove(values.styleOverFromField);
                });

                formPart.classList.add(values.styleOverFromField);
            }
        }

        if (typeMoving === values.typeActiveField && typeOver === values.typeActiveField) {
            values.overElement.classList.add(values.styleOverFromPart);
        }

    }

    static DragLeaveHandler(values: Values) {
        if (!values.overElement) return;
        values.overElement.classList.remove(values.styleOverFromField);
    }


    static DragDropHandler(values: Values) {

        var type = values.movingElement.dataset.type;
        values.overElement.classList.remove(values.styleOverFromField);
        var typeOver = values.overElement.dataset.type;

        if (typeOver === values.typePlaceBuilder) {
            typeOver = values.typeActivePart;
            if (values.Parts.length == 0) {
                var part = new FormPart(values);
                part.addToPlace();
                if (document.getElementById(part.getData().id))
                    values.Parts.push(part);
            }

            values.overElement = values.Parts[0].container;
            var formPart = values.Parts[0].container;
        }

        if (typeOver !== values.typeActivePart && typeOver !== values.typeActiveField) return;

        var formPart = values.overElement.closest(`.${values.styleFormPart}`) as HTMLDivElement;
        formPart.classList.remove(values.styleOverFromField);

        //add a field to the form part
        if (type === values.typeField) {
            var field = new Field(values);
            field.#data.partId = formPart.id;
            field.#data.type = Number(values.movingElement.dataset.field);
            field.#data.defaultValue = "";
            field.#data.triggerId = "9C85B07F-9236-4314-A29E-87B20093CF82";

            field.addToFormPart();
            if (document.getElementById(field.#data.id))
                values.Fields.push(field);
        }

        if (type === values.typeActiveField) {

            //self-removal protection
            if (values.movingElement === values.overElement) return;

            var items = values.movingElement.closest(`.${values.styleFormPartItems}`);
            items.removeChild(values.movingElement);

            //move a field to the form part
            if (typeOver === values.typeActivePart) {
                items = values.overElement.querySelector(`.${values.styleFormPartItems}`);
                items.insertBefore(values.movingElement, undefined);
            }

            //move a field among the active fields
            if (typeOver === values.typeActiveField) {
                items = values.overElement.closest(`.${values.styleFormPartItems}`);
                items.insertBefore(values.movingElement, values.overElement);
            }

            values.Fields.find(field => {
                if (field.#data.id === values.movingElement.id)
                    field.#data.partId = formPart.id;
            });

        }

        //reindex sequence
        var activeFields = document.querySelectorAll(`.${values.styleFormPartItem}`);
        for (var i = 0; i < activeFields.length; i++) {
            var activeField = values.Fields.find(field => {
                return field.getData().id === activeFields[i].id
            });

            var data = { ...activeField.getData() };
            data.sequence = i;
            activeField.setData(data);
        }
    }
}

export function checkFieldHelperInfo(container: HTMLDivElement) {
    if (container.querySelector(".infoDragField")) {
        container.innerHTML = "";
    }
}

export class FieldItem {
    id: string;
    name: string;
    description: string;
    sequence: number;
    required: boolean;
    readonly: boolean;
    type: number;
    partId: string;
    triggerId: string;
    active: boolean;
    defaultValue: string;
    listFormId: string;

    constructor() {
        this.id = generateUUID();
        this.name = "New Field";
        this.active = true;
    }
}