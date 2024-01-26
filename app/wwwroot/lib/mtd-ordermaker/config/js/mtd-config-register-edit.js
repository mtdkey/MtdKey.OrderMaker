new MTDTextField("form-name");
new MTDTextField("form-note");
new MTDSelectList("select-form");
new MTDSelectList("select-field");

const ChangeRegisterAction = (target) => {

    let id = target.id.replace('-income', '').replace('-expense', '');
 
    const income = document.getElementById(`${id}-income`);
    const expense = document.getElementById(`${id}-expense`);
    const linked = document.getElementById(`${id}-linked`);

    income.checked = income.id == target.id;
    expense.checked = expense.id == target.id;

    linked.checked = income.checked != false || expense.checked != false;
    
}

const ChangeRegisterLinked = (target) => {

    const id = target.id.replace('-linked', '');
    const income = document.getElementById(`${id}-income`);
    const expense = document.getElementById(`${id}-expense`);

    if (!target.checked) {
        
        income.checked = false;
        expense.checked = false;

    } else {

        income.checked = true;
    }
}

const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-delete'));
if (dialog) {
    document.querySelector('[mtd-data-delete]').addEventListener('click', () => {
        dialog.open();
    });
}