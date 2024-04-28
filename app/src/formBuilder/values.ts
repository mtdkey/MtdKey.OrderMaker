import bootstrap = require("bootstrap");
import { Field, FieldItem } from "./field";
import { Form } from "./form";
import { FormPart, FormPartItem } from "./formPart";
import { MDCTextField } from '@material/textfield';
import { PartDialog } from "./partDialog";
import { FieldDialog } from "./FieldDialog";
import { FieldListDialog } from "./FieldListDialog";


export class Values {

    movingElement: HTMLDivElement;
    overElement: HTMLDivElement;
    styleFormPart: string;
    styleOverFromPart: string;
    styleOverFromField: string;
    styleFormPartItems: string;
    styleFormPartTitle: string;
    styleFieldType: string;
    typePart: string;
    typeActivePart: string;
    typePlaceBuilder: string;
    typeField: string;
    typeActiveField: string;
    Form: Form;
    Parts: Array<FormPart>;
    Fields: Array<Field>;
    styleTaken: string;
    placeBuilder: HTMLDivElement;
    formPartTemplate: HTMLTemplateElement;
    fieldTemplate: HTMLTemplateElement;
    itemTemplate: HTMLTemplateElement;
    styleFieldName: string;
    styleFormPartItem: string;

    formDialog: bootstrap.Modal;
    btnFormEditor: HTMLButtonElement; 
    formDialogName: MDCTextField;
    titleFormName: HTMLDivElement;
    inputJsonData: HTMLInputElement;
    warnMessage: HTMLDivElement;
    btnClearStorage: HTMLButtonElement;

    partDialog: PartDialog;
    styleFormPartColumns: string;
    fieldDialog: FieldDialog;
    fieldListDialog: FieldListDialog;
    FormInfoModels: FormInfoModel[];

    constructor() {

        this.placeBuilder = document.getElementById("placeBuilder") as HTMLDivElement;
        this.movingElement = undefined;
        this.overElement = this.placeBuilder;
        this.formPartTemplate = document.getElementById("formPartTemplate") as HTMLTemplateElement;
        this.fieldTemplate = document.getElementById("fieldTemplate") as HTMLTemplateElement;
        this.itemTemplate = document.getElementById("itemTemplate") as HTMLTemplateElement;
        this.styleTaken = "taken";
        this.styleFormPart = "formPart";
        this.styleOverFromPart = "overFromPart";
        this.styleOverFromField = "overFromField";
        this.styleFormPartItems = "formPart_items";
        this.styleFormPartItem = "formPart_item";
        this.styleFormPartTitle = "formPart_title";
        this.styleFieldType = "formPart_item_title_type";
        this.styleFieldName = "formPart_item_title_name";
        this.styleFormPartColumns = "formPart_items_columns";
        this.typePart = "part";
        this.typeActivePart = "activePart";
        this.typePlaceBuilder = "placeBuilder";
        this.typeField = "field";
        this.typeActiveField = "activeField";   

        this.formDialog = new bootstrap.Modal(document.getElementById("formDialog"), { keyboard: false, backdrop: 'static' });
        this.btnFormEditor = document.getElementById("btnFormEditor") as HTMLButtonElement;

        this.titleFormName = document.querySelector(".form_name") as HTMLDivElement;
        this.formDialogName = new MDCTextField(document.getElementById("formName"));

        this.inputJsonData = document.getElementById("jsonData") as HTMLInputElement;
        this.warnMessage = document.getElementById("warnMessage") as HTMLDivElement;
        this.btnClearStorage = document.getElementById("btnClearStorage") as HTMLButtonElement;

        this.Form = new Form(this);
        this.Parts = new Array<FormPart>();
        this.Fields = new Array<Field>();    
        this.FormInfoModels = new Array<FormInfoModel>();

        this.partDialog = new PartDialog(this);
        this.fieldDialog = new FieldDialog(this);
        this.fieldListDialog = new FieldListDialog(this);
    }
}
