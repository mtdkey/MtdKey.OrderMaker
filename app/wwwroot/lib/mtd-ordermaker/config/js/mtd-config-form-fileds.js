

//Start
const selectPart = new MTDSelectList("select-part");

selectPart.selector.listen('MDCSelect:change', (e) => {
    ActionShowModal(true);
    const form = document.getElementById("configPartSelector");
    const curr = document.getElementById("configCurrentPart");
    curr.value = selectPart.selector.value;
    form.submit();
});

