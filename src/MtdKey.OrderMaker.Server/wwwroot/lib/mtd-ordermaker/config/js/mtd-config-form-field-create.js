//Start

new MTDTextField("field-name");
new MTDTextField("field-note");

const selectType = new MTDSelectList("select-type");
const selectForm = new MTDSelectList("select-form");
const fieldWrapper = document.getElementById("fieldWrapper");

selectType.selector.listen('MDCSelect:change', () => {    
       
    if (selectType.selector.value === '11') {
        fieldWrapper.style.display = "";
    } else {
        fieldWrapper.style.display = 'none';
    }
    });