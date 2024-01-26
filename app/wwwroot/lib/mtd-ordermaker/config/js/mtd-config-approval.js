
//Start
    const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-delete'));
    if (dialog) {
        document.querySelector('[mtd-data-delete]').addEventListener('click', () => {
            dialog.open();
        });
    }

new MTDTextField("field-name");
new MTDTextField("field-note");
new MTDTextField("field-name-start");
new MTDTextField("field-name-returned");
new MTDTextField("field-name-required");
new MTDTextField("field-name-waiting");
new MTDTextField("field-name-approved");
new MTDTextField("field-name-rejected");
