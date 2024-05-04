//Start
const dialogDeleteForm = new mdc.dialog.MDCDialog(document.getElementById('dialog-form-delete'));
new MTDTextField("form-name");
new MTDTextField("form-note");

var actionDelete = document.querySelector('[mtd-data-delete]');

actionDelete.addEventListener('click', () => {
    dialogDeleteForm.open();
});


