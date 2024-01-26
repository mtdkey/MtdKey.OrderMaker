//Start
    const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-part-delete'));

    document.querySelector('[mtd-data-delete]').addEventListener('click', () => {
        dialog.open();
    });

new MTDTextField("part-name");
new MTDTextField("part-note");
new MTDSelectList("select-style");