/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


const ClearCheckboxAll = () => {
    const rows = document.querySelectorAll(`tr[mtd-data-row]`);
    rows.forEach((row) => {

        const id = row.attributes.getNamedItem('mtd-data-row').nodeValue;
        const input = document.getElementById(`${id}-checkbox`);

        const act = document.querySelector(`[mtd-data-action="${id}"]`);
        input.checked = false;
        act.style.display = 'none';
        row.className = "";
    });
}

const ClickEventRow = (row) => {

    const id = row.attributes.getNamedItem('mtd-data-row').nodeValue;
    const input = document.getElementById(`${id}-checkbox`);

    const state = input.checked;
    ClearCheckboxAll();
    if (!state) {
        input.checked = true;
        const act = document.querySelector(`[mtd-data-action="${id}"]`);
        const css = row.attributes.getNamedItem('mtd-data-class').nodeValue;
        act.style.display = 'table-row';
        row.className = css;
    } else {
        input.checked = false;
        const act = document.querySelector(`[mtd-data-action="${id}"]`);
        act.style.display = 'none';
    }
}


//Start

    const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-users-delete'));
    document.querySelectorAll('a[mtd-data-row-delete]').forEach((item) => {

        item.addEventListener('click', () => {
            const id = item.getAttribute('mtd-data-row-delete');
            if (id) {
                document.getElementById('user-delete-id').value = id;
                dialog.open();
            }
        });
    });

    const rows = document.querySelectorAll(`tr[mtd-data-row]`);

    rows.forEach((row) => {
        row.addEventListener('click', (e) => {
            if (e.target.type === 'checkbox') return false;
            ClickEventRow(row);
        });
        const id = row.attributes.getNamedItem('mtd-data-row').nodeValue;
        const input = document.getElementById(`${id}-checkbox`);
        input.addEventListener('click', (e) => {
            e.target.checked = !e.target.checked;
            ClickEventRow(row);
        });

    });
