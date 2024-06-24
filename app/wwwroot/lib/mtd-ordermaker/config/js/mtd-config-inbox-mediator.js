const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-delete'));

if (dialog) {
    document.querySelectorAll('[mtd-data-delete]').forEach(btn =>
        btn.addEventListener("click", (e) => {
            configEmail.value = e.currentTarget.getAttribute("mtd-data-delete");
            dialog.open();
        }));
}

document.querySelectorAll("[data-test]").forEach(btn => {
    btn.addEventListener("click", () => {
        testEmail.value = btn.dataset.test;
        testClicker.click();
    });
});
