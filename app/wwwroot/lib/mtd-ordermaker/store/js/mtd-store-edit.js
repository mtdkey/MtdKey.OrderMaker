/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


//Start

const tagName = "mtdSelector";
const items = document.querySelectorAll(`div[${tagName}]`);

items.forEach((item) => {

    const id = item.attributes.getNamedItem(tagName).nodeValue;
    const input = document.getElementById(id);
    const href = document.getElementById(`${id}-href`);
    const select = document.getElementById(`${id}-select`);
    const strike = document.getElementById(`${id}-strike`);

    const actionDelete = document.getElementById(`${id}-action-delete`);
    const actionUndo = document.getElementById(`${id}-action-undo`);
    const fixDelete = document.getElementById(`${id}-delete`);

    const isFile = href.firstElementChild.textContent;
    if (isFile) actionDelete.hidden = false;

    item.addEventListener("click", () => {
        input.click();
    });

    input.addEventListener("change", (event) => {
        select.innerText = event.target.files[0].name;
        href.hidden = true; select.hidden = false; strike.hidden = true;
        actionDelete.hidden = false;
        actionUndo.hidden = true;
        fixDelete.checked = false;
    });

    actionDelete.addEventListener("click", () => {

        if (input.value) {
            href.hidden = false; select.hidden = true; strike.hidden = true;
            input.value = null;
            if (!isFile) {
                actionDelete.hidden = true;
            }
        } else {
            href.hidden = true; select.hidden = true; strike.hidden = false;
            actionDelete.hidden = true;
            actionUndo.hidden = false;
            fixDelete.checked = true;
        }
    });

    actionUndo.addEventListener("click", () => {
        href.hidden = false; select.hidden = true; strike.hidden = true;
        actionDelete.hidden = false;
        actionUndo.hidden = true;
        fixDelete.checked = false;
    });

});

document.querySelectorAll('select[datalink]').forEach((datalink) => {
    const id = datalink.attributes.getNamedItem("datalink").nodeValue;
    const input = document.getElementById(`${id}-datalink`);

    const dlv = datalink.options[datalink.selectedIndex];
    if (dlv) {
        input.value = dlv.textContent;
        datalink.addEventListener('change', (e) => {
            document.getElementById(`${id}-datalink`).value = e.target.options[e.target.selectedIndex].textContent;
        });
    }

});

const dialog = document.getElementById('dialog-info');
if (dialog) {
    const dialogInfo = new mdc.dialog.MDCDialog(dialog);
    const dialogInfoContent = document.getElementById('dialog-info-content');
    const dialogInfoTitle = document.getElementById('dialog-info-title');
    document.querySelectorAll('[mtd-info]').forEach((item) => {
        item.addEventListener('click', (e) => {
            const note = item.getAttribute('mtd-info');
            dialogInfoTitle.innerText = e.target.textContent;
            dialogInfoContent.innerText = note;
            dialogInfo.open();

        });
    });
}

const textFields = document.querySelectorAll(".mdc-text-field");

textFields.forEach((textField) => {
    new MTDTextField(textField.id)
});


const divields = document.querySelectorAll(".mtd-select-list");
divields.forEach((divField) => {
    var selectField =  divField.querySelector(".mdc-select");
    var select = new MTDSelectList(selectField.id);
    document.getElementById(`${selectField.id}-datalink`).value = select.selector.selectedText.textContent;
    select.selector.listen('MDCSelect:change', () => {
        document.getElementById(`${selectField.id}-datalink`).value = select.selector.selectedText.textContent;
    });
});


const toggleParts = document.querySelectorAll("[mtd-button-toggle]");

toggleParts.forEach((button) => {

    button.addEventListener("click", () => {
        const id = button.getAttribute("mtd-button-toggle");
        const parts = document.querySelectorAll(`[mtd-div-toggle='${id}']`);
        parts.forEach((part) => {
            part.classList.toggle("mtd-main-display-none");
        });
    });
});

const PartsOpenAll = () => {
    toggleParts.forEach((button) => {
        button.classList.remove("mdc-icon-button--on");
        const id = button.getAttribute("mtd-button-toggle");
        const parts = document.querySelectorAll(`[mtd-div-toggle='${id}']`);
        parts.forEach((part) => {
            part.classList.remove("mtd-main-display-none");
        });
    });
}
