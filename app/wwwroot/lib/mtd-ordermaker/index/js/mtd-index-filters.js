
class ServiceForm {

    constructor() {

        this.locale = document.getElementById("index-locale-for-date");
        this.dateStart = new MTDTextField("index-filter-date-start");
        this.dateFinish = new MTDTextField("index-filter-date-finish");

        this.number = new MTDTextField("index-filter-related-number");

        this.period = document.getElementById("index-filter-service-period");
        this.owners = document.getElementById("index-filter-service-owners");
        this.related = document.getElementById("index-filter-service-related");

        this.select = new MTDSelectList("index-filter-service-list");
        this.ownersList = new MTDSelectList("index-filter-service-owners-list");

        this.relatedDocs = new MTDSelectList("index-filter-service-related-list");
    }

}

class ExctensionForm {
    constructor() {

        this.selectScript = new MTDSelectList("index-filter-extension-list");
        this.scriptId = document.getElementById("index-filter-extension-script");
    }
}

class CustomForm {
    constructor() {

        this.resultId = document.getElementById("index-filter-custom-result-id");
        this.resultType = document.getElementById("index-filter-custom-result-type");
        this.resultAction = document.getElementById("index-filter-custom-result-action");
        this.resultValue = document.getElementById("index-filter-custom-result-value");
        this.resultValueExt = document.getElementById("index-filter-custom-result-value-ext");

        this.selectAction = new MTDSelectList("index-filter-custom-action");
        this.selectValue = new MTDSelectList("index-filter-custom-list-value");
        this.selectFields = new MTDSelectList("index-filter-custom-fields");

        this.textValue = new MTDTextField("index-filter-custom-text-value");
        this.intValue = new MTDTextField("index-filter-custom-int-value");
        this.boolValue = new MTDSelectList("index-filter-custom-bool-value");
        this.dateStartValue = new MTDTextField("index-filter-custom-date-start-value");
        this.dateFinishValue = new MTDTextField("index-filter-custom-date-finish-value");
    }
}


const IndexFilterShowTab = (tabShow) => {

    const tabs = ['custom', 'service', 'extension'];
    tabs.forEach((tabName) => {
        const div = document.getElementById(`index-filter-${tabName}`);
        div.style.display = tabName === tabShow ? "block" : "none";
    });
}

const ShowCustomFieldTypes = (fiedShow) => {

    const tabs = ['text', 'int', 'date', 'list', 'bool', 'text'];
    tabs.forEach((tabName) => {
        const div = document.getElementById(`index-filter-custom-${tabName}`);
        div.style.display = tabName === fiedShow ? "block" : "none";
    });
}

const Service = () => {

    const sf = new ServiceForm();

    sf.select.selector.listen('MDCSelect:change', (e) => {

        const val = sf.select.selector.value;

        sf.period.style.display = val === "DateCreated" ? "" : "none"; 
        sf.owners.style.display = val === "DocumentOwner" ? "" : "none";

        if (sf.related) {
            sf.related.style.display = val === "DocumentBased" ? "" : "none";
        }
        

    });

    const dtp = new LocaleDTP();

    $(sf.dateStart.input).datetimepicker({
        timepicker: false,
        lang: dtp.lang,
        mask: dtp.mask,
        format: dtp.format,
        defaultDate: new Date(),
    });
    
    $(sf.dateFinish.input).datetimepicker({
        timepicker: false,
        lang: dtp.lang,
        mask: dtp.mask,
        format: dtp.format,
        defaultDate: new Date(),
    });
}

const GetCustomPartForType = (dataType) => {
    let result;
    switch (dataType) {
        case "2":
        case "3": {
            result = "int";
            break;
        }
        case "5":
        case "6": {
            result = "date";
            break;
        }
        case "11": {
            result = "list";
            break;
        }
        case "12": {
            result = "bool";
            break;
        }
        default: {
            result = "text";
            break;
        }
    }

    return result;
}

const RequestFieldList = (idField, selectValue) => {

    const form = document.getElementById('index-filter-custom-list-form');
    const inputField = form.querySelector("input[id='id-field']");
    inputField.value = idField;
    const formData = CreateFormData(form);

    fetch(form.action, { method: form.method, body: formData })
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            if (data.value) {

                selectValue.RemoveItems();
                data.value.forEach((item, index) => {
                    selected = false;
                    if (index === 0) { selected = true; }
                    selectValue.AddItem(item.id, item.value, selected);
                });
            }
        });
}

const Custom = () => {

    const cf = new CustomForm();

    cf.resultAction.value = cf.selectAction.selector.value;
    cf.resultId.value = cf.selectFields.selector.value;

   
    cf.selectAction.selector.listen('MDCSelect:change', () => {
        cf.resultAction.value = cf.selectAction.selector.value;
    });
 

    cf.selectValue.selector.listen('MDCSelect:change', () => {
        cf.resultValue.value = cf.selectValue.selector.value;
    });

    cf.boolValue.selector.listen('MDCSelect:change', () => {
        cf.resultValue.value = cf.boolValue.selector.value;
    });

    cf.textValue.input.addEventListener("input", () => {
        cf.resultValue.value = cf.textValue.textField.value;
    });

    cf.intValue.input.addEventListener("input", () => {
        cf.resultValue.value = cf.intValue.textField.value;
    });

    const dtp = new LocaleDTP();

    $(cf.dateStartValue.input).datetimepicker({
        timepicker: false,
        locale: dtp.locale,
        mask: dtp.mask,
        format: dtp.format,
        defaultDate: new Date(),
        //value: new Date(),
        onSelectDate: function ($dtp, current, input) {
            cf.resultValue.value = cf.dateStartValue.textField.value;
        },
    });

    $(cf.dateFinishValue.input).datetimepicker({
        timepicker: false,
        locale: dtp.locale,
        mask: dtp.mask,
        format: dtp.format,
        defaultDate: new Date(),
        //value: new Date(),
        onSelectDate: function ($dtp, current, input) {
            cf.resultValueExt.value = cf.dateFinishValue.textField.value;
        },
    });

    const li = cf.selectFields.div.querySelector(`[data-value='${cf.selectFields.selector.value}']`);
    const fieldShow = GetCustomPartForType(li);
    ShowCustomFieldTypes(fieldShow);

    cf.selectFields.selector.listen('MDCSelect:change', () => {

        const li = cf.selectFields.div.querySelector(`[data-value='${cf.selectFields.selector.value}']`);
        if (!li) return;

        const dataType = li.getAttribute("data-type");
        const fieldShow = GetCustomPartForType(dataType);

        cf.resultType.value = dataType;
        cf.resultId.value = cf.selectFields.selector.value;
        cf.textValue.textField.value = "";
        cf.intValue.textField.value = "";
        cf.boolValue.selector.value = "1";
        cf.dateStartValue.textField.value = "";
        cf.dateFinishValue.textField.value = "";

        cf.resultValue.value = "";
        cf.resultValueExt.value = "";

        if (dataType === '11') { RequestFieldList(cf.selectFields.selector.value, cf.selectValue); }

        cf.selectAction.div.classList.remove("mtd-main-display-none");
        const separ = document.getElementById("custom-separ");
        separ.classList.remove("mtd-main-display-none");
        
        if (dataType === "12" || dataType === '11' || dataType === "5" || dataType === "6") {
            cf.selectAction.div.classList.add("mtd-main-display-none");
            separ.classList.add("mtd-main-display-none");
        }

        if (dataType === "12") {
            cf.resultValue.value = "1";
        }

        if (dataType === '2' || dataType === "3") { cf.resultValue.value = "10"; }

        ShowCustomFieldTypes(fieldShow);

    });

    cf.selectFields.selector.emit("MDCSelect:change");
}


const Extension = () => {

    const ef = new ExctensionForm();
    ef.scriptId.value = ef.selectScript.selector.value;

    ef.selectScript.selector.listen('MDCSelect:change', () => {
        ef.scriptId.value = ef.selectScript.selector.value;
    });
}

//Start
const tabBar = document.getElementById('index-filter-tabs');
new mdc.tabBar.MDCTabBar(tabBar);

Service();
Custom();
Extension();

