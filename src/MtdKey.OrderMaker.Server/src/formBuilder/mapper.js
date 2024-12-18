"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LoadJsonData = LoadJsonData;
exports.SaveHistory = SaveHistory;
const field_1 = require("./field");
const formPart_1 = require("./formPart");
function LoadJsonData(values) {
    if (!values.inputJsonData.value)
        return;
    var jsonData = JSON.parse(values.inputJsonData.value);
    values.titleFormName.textContent = jsonData.FormModel.Name;
    values.Form.id = jsonData.FormModel.Id;
    values.Form.name = jsonData.FormModel.Name;
    values.Form.description = jsonData.FormModel.Description;
    values.Form.active = jsonData.FormModel.Active;
    values.Form.sequence = jsonData.FormModel.Sequence;
    values.Form.visibleDate = jsonData.FormModel.VisibleDate;
    values.Form.visibleNumber = jsonData.FormModel.VisibleNumber;
    var imageBackInfo = {
        base64string: jsonData.FormModel.ImageBack,
        size: jsonData.FormModel.ImageBackSize,
        type: jsonData.FormModel.ImageBackType
    };
    values.Form.imageBack.addImage(imageBackInfo, false);
    var imageLogoInfo = {
        base64string: jsonData.FormModel.ImageLogo,
        size: jsonData.FormModel.ImageLogoSize,
        type: jsonData.FormModel.ImageLogoType
    };
    values.Form.imageLogo.addImage(imageLogoInfo, false);
    jsonData.PartModels.forEach(partModel => {
        var part = new formPart_1.FormPart(values);
        var partItem = new formPart_1.FormPartItem();
        partItem.id = partModel.Id;
        partItem.formId = values.Form.id;
        partItem.name = partModel.Name;
        partItem.description = partModel.Description;
        partItem.sequence = partModel.Sequence;
        partItem.styleType = partModel.StyleType;
        partItem.title = partModel.Title;
        partItem.active = partModel.Active;
        partItem.imageData = partModel.ImageData;
        partItem.imageSize = partModel.ImageSize;
        partItem.imageType = partModel.ImageType;
        part.setData(partItem);
        values.Parts.push(part);
    });
    jsonData.FieldModels.forEach(fieldModel => {
        var field = new field_1.Field(values);
        var fieldItem = new field_1.FieldItem();
        fieldItem.id = fieldModel.Id;
        fieldItem.name = fieldModel.Name;
        fieldItem.description = fieldModel.Description;
        fieldItem.partId = fieldModel.PartId;
        fieldItem.readonly = fieldModel.Readonly;
        fieldItem.required = fieldModel.Required;
        fieldItem.sequence = fieldModel.Sequence;
        fieldItem.active = fieldModel.Active;
        fieldItem.type = fieldModel.SysType;
        fieldItem.triggerId = fieldModel.TriggerId;
        fieldItem.defaultValue = fieldModel.DefaultValue;
        fieldItem.listFormId = fieldModel.ListFormId;
        fieldItem.listItems = fieldModel.ListItems;
        field.setData(fieldItem);
        values.Fields.push(field);
    });
    values.FormInfoModels = jsonData.FormInfoModels;
}
function SaveHistory(values) {
    if (!values.inputJsonData.value)
        return;
    const jsonData = {
        FormModel: {
            Id: "",
            Name: "",
            Description: "",
            Active: false,
            MtdCategory: "",
            Sequence: 0,
            Parent: "",
            VisibleNumber: false,
            VisibleDate: false,
            ImageBack: "",
            ImageLogo: "",
            ImageBackType: "",
            ImageBackSize: 0,
            ImageLogoType: "",
            ImageLogoSize: 0
        },
        PartModels: [],
        FieldModels: [],
        FormInfoModels: []
    };
    jsonData.FormModel.Id = values.Form.id;
    jsonData.FormModel.Name = values.Form.name;
    jsonData.FormModel.Description = values.Form.description;
    jsonData.FormModel.Active = values.Form.active;
    jsonData.FormModel.Sequence = values.Form.sequence;
    jsonData.FormModel.VisibleDate = values.Form.visibleDate;
    jsonData.FormModel.VisibleNumber = values.Form.visibleNumber;
    var imageBackInfo = values.Form.imageBack.getData();
    jsonData.FormModel.ImageBack = imageBackInfo.base64string;
    jsonData.FormModel.ImageBackType = imageBackInfo.type;
    jsonData.FormModel.ImageBackSize = imageBackInfo.size;
    var imageLogoInfo = values.Form.imageLogo.getData();
    jsonData.FormModel.ImageLogo = imageLogoInfo.base64string;
    jsonData.FormModel.ImageLogoType = imageLogoInfo.type;
    jsonData.FormModel.ImageLogoSize = imageLogoInfo.size;
    values.Parts.forEach(item => {
        const partModel = {
            Id: "",
            Name: "",
            Description: "",
            FormId: "",
            Sequence: 0,
            StyleType: 0,
            Title: false,
            Active: false,
            ImageData: "",
            ImageSize: 0,
            ImageType: ""
        };
        var partItem = item.getData();
        partModel.Id = partItem.id;
        partModel.FormId = partItem.formId;
        partModel.Name = partItem.name;
        partModel.Description = partItem.description;
        partModel.Sequence = partItem.sequence;
        partModel.StyleType = partItem.styleType;
        partModel.Title = partItem.title;
        partModel.Active = partItem.active;
        partModel.ImageData = partItem.imageData;
        partModel.ImageSize = partItem.imageSize;
        partModel.ImageType = partItem.imageType;
        jsonData.PartModels.push(partModel);
    });
    values.Fields.forEach(item => {
        var fieldModel = {
            Id: "",
            Name: "",
            Description: "",
            PartId: "",
            Readonly: false,
            Required: false,
            Sequence: 0,
            Active: false,
            SysType: 0,
            TriggerId: "",
            DefaultValue: "",
            ListFormId: "",
            ListItems: []
        };
        var fieldItem = item.getData();
        fieldModel.Id = fieldItem.id;
        fieldModel.Name = fieldItem.name;
        fieldModel.Description = fieldItem.description;
        fieldModel.PartId = fieldItem.partId;
        fieldModel.Readonly = fieldItem.readonly;
        fieldModel.Required = fieldItem.required;
        fieldModel.Sequence = fieldItem.sequence;
        fieldModel.Active = fieldItem.active;
        fieldModel.SysType = fieldItem.type;
        fieldModel.TriggerId = fieldItem.triggerId;
        fieldModel.DefaultValue = fieldItem.defaultValue;
        fieldModel.ListFormId = fieldItem.listFormId;
        fieldModel.ListItems = fieldItem.listItems;
        jsonData.FieldModels.push(fieldModel);
    });
    var jsonDataString = JSON.stringify(jsonData);
    values.inputJsonData.value = jsonDataString;
}
//# sourceMappingURL=mapper.js.map