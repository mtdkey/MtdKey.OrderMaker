"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Values = void 0;
const bootstrap = require("bootstrap");
const form_1 = require("./form");
const textfield_1 = require("@material/textfield");
const partDialog_1 = require("./partDialog");
const FieldDialog_1 = require("./FieldDialog");
const FieldListDialog_1 = require("./FieldListDialog");
class Values {
    constructor() {
        this.placeBuilder = document.getElementById("placeBuilder");
        this.movingElement = undefined;
        this.overElement = this.placeBuilder;
        this.formPartTemplate = document.getElementById("formPartTemplate");
        this.fieldTemplate = document.getElementById("fieldTemplate");
        this.itemTemplate = document.getElementById("itemTemplate");
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
        this.btnFormEditor = document.getElementById("btnFormEditor");
        this.titleFormName = document.querySelector(".form_name");
        this.formDialogName = new textfield_1.MDCTextField(document.getElementById("formName"));
        this.inputJsonData = document.getElementById("jsonData");
        this.warnMessage = document.getElementById("warnMessage");
        this.btnClearStorage = document.getElementById("btnClearStorage");
        this.Form = new form_1.Form(this);
        this.Parts = new Array();
        this.Fields = new Array();
        this.FormInfoModels = new Array();
        this.partDialog = new partDialog_1.PartDialog(this);
        this.fieldDialog = new FieldDialog_1.FieldDialog(this);
        this.fieldListDialog = new FieldListDialog_1.FieldListDialog(this);
    }
}
exports.Values = Values;
//# sourceMappingURL=values.js.map