//Start
    const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-field-delete'));

    document.querySelector('[mtd-data-delete]').addEventListener('click', () => {
        dialog.open();
    });


new MTDTextField("field-name");
new MTDTextField("field-note");
new MTDTextField("field-default");

new MTDSelectList("select-part");
new MTDSelectList("select-trigger");