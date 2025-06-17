"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.FieldType = void 0;
exports.GetFileTypeInfo = GetFileTypeInfo;
var FieldType;
(function (FieldType) {
    FieldType[FieldType["Text"] = 1] = "Text";
    FieldType[FieldType["Integer"] = 2] = "Integer";
    FieldType[FieldType["Money"] = 3] = "Money";
    FieldType[FieldType["TextArea"] = 4] = "TextArea";
    FieldType[FieldType["Date"] = 5] = "Date";
    FieldType[FieldType["DateTime"] = 6] = "DateTime";
    FieldType[FieldType["File"] = 7] = "File";
    FieldType[FieldType["Image"] = 8] = "Image";
    FieldType[FieldType["List"] = 11] = "List";
    FieldType[FieldType["Checkbox"] = 12] = "Checkbox";
    FieldType[FieldType["Link"] = 13] = "Link";
    FieldType[FieldType["FileStorage"] = 14] = "FileStorage";
    FieldType[FieldType["HtmlEditor"] = 15] = "HtmlEditor";
})(FieldType || (exports.FieldType = FieldType = {}));
function GetFileTypeInfo(fieldType) {
    var result;
    var items = document.querySelectorAll("[data-field]");
    items.forEach((item) => {
        if (item.dataset.field === fieldType.toString()) {
            result = item.cloneNode(true);
            Object.keys(result.dataset).forEach(key => {
                delete result.dataset[key];
            });
            result.removeAttribute("draggable");
            result.removeAttribute("class");
            result.removeAttribute("style");
        }
    });
    return result;
}
//# sourceMappingURL=fieldType.js.map