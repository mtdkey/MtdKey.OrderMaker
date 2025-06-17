import "./configFormBuilder.css";
import "../imageSelector/imageSelector.css";
import { Field, FieldItem } from "./field";
import { FormPart } from "./formPart";
import { Init } from "./initData";
import { Values } from "./values";
import { SaveHistory } from "./mapper";


var values: Values = undefined;
export enum Actions { 
    DragDrop, ChangeFormParameters, ChangePartParameters, ChangeFieldParameters
}


export function builderDragStart(e: any) {    
    e.stopPropagation();
    var elm = e.currentTarget as HTMLDivElement    
    values.movingElement = elm;
    elm.style.opacity = "0.4";
    elm.classList.add(values.styleTaken);
}

export function builderDragEnd(e : any) {
    e.currentTarget.style.opacity = "1";
    e.currentTarget.classList.remove(values.styleTaken);
    values.movingElement = undefined;
    values.Parts.forEach(part => {
        var elm = document.getElementById(part.getData().id);
        elm.classList.remove(values.styleOverFromField);
    });
}

export function builderDragOver(e: any) {
    e.preventDefault();
    e.stopPropagation();
    values.overElement = e.currentTarget;
    FormPart.DragOverHandler(values);
    Field.DragOverHandler(values);
}

export function builderDragLeave(e : any) {
    e.preventDefault();
    e.stopPropagation();
    FormPart.DragLeaveHandler(values);
    Field.DragLeaveHandler(values);
    values.overElement = undefined;
}


export function builderDragDrop(e: any) {
    e.preventDefault();
    e.stopPropagation();

    ChangeAction(Actions.DragDrop, () => {
        FormPart.DragDropHandler(values);
        Field.DragDropHandler(values);
    });
}

export function ChangeAction(action: Actions, callback: Function) {
    //console.info(Actions[action]);
    callback();

    SaveHistory(values);
    values.warnMessage.classList.add("warn-message-on");
    values.btnClearStorage.classList.remove("d-none");
}

values = new Values();

setTimeout(() => { Init(values); }, 600);


function propertyOf<TObj>(name: keyof TObj) {
    return name;
}

Object.keys(values.Fields).forEach((item, curr) => {
    values.Fields[curr].getData()     
});
