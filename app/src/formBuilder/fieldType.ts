

export enum FieldType {
    Text = 1,
    Integer = 2,
    Money = 3,
    TextArea = 4,
    Date = 5,
    DateTime = 6,
    File = 7,
    Image = 8,
    List = 11,
    Checkbox = 12,
    Link = 13,
}

export function GetFileTypeInfo(fieldType: FieldType): HTMLDivElement {
    var result: HTMLDivElement;
    var items = document.querySelectorAll("[data-field]");
    items.forEach((item: HTMLDivElement) => {
        if (item.dataset.field === fieldType.toString()) {

            result = item.cloneNode(true) as HTMLDivElement;
           
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