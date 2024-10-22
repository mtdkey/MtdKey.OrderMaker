class ToastContainer {
    constructor() {}
    /**
     * 
     * @param {any} key Unique message id to reject multiple windows for the same information
     * @param {any} info Informational text
     * @param {any} code Message type when 200 for success and 400 for error
     */
    showToast(key, info, code = 200) {

        var toastContainer = document.getElementById("toastContainer");
        if (!toastContainer) return

        let template = toastOk.content.cloneNode(true).firstElementChild;

        if (code === 400)
            template = toastError.content.cloneNode(true).firstElementChild;

        const id = `${key}-toast`;
        var oldToast = document.getElementById(id);

        if (oldToast && oldToast.innerText === info) {

            var inst = bootstrap.Toast.getInstance(oldToast)
            inst.show();
            return
        }

        template.id = id;
        toastContainer.appendChild(template);
        var toastE1 = document.getElementById(id);
        toastE1.querySelector(".toast-body").innerText = info;
        var toast = new bootstrap.Toast(toastE1);
        toast.show();
    }
}


(() => {
    if (sessionStorage.getItem("message-200")) {
        var toastContainer = new ToastContainer();
        toastContainer.showToast("m200", sessionStorage.getItem("message-200"), 200);
        sessionStorage.removeItem("message-200");
    }

    if (sessionStorage.getItem("message-400")) {
        var toastContainer = new ToastContainer();
        toastContainer.showToast("m400", sessionStorage.getItem("message-400"), 400);
        sessionStorage.removeItem("message-400");
    }
})();