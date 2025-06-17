/* Translate */
class FileStorage {
    constructor(id) {
        this.id = id;
        this.files = []
    }
}

var FileStorages = [];
let isFirefox = navigator.userAgent.search("Firefox");

function FileListItems(files) {
    var b = new ClipboardEvent("").clipboardData || new DataTransfer()
    for (var i = 0, len = files.length; i < len; i++) b.items.add(files[i])
    return b.files
}

function GetFileStorage(id) {
    var store = FileStorages.filter(store => store.id === id);
    if (!store[0]) return new FileStorage(id);
    return FileStorages.filter(store => store.id === id)[0];
}

document.querySelectorAll(".attachedFiles").forEach(dropZone => {

    var id = dropZone.id.replace("-zone", "");
    var input = document.getElementById(id);
    var toastContainer = new ToastContainer();

    FileStorages.push(new FileStorage(dropZone.id));

    var buttonExplorer = document.getElementById(`${id}-addExplorer`);
    buttonExplorer.addEventListener("click", () => {
        input.click();
    });

    var buttonBuffer = document.getElementById(`${id}-addBuffer`)

    if (isFirefox > -1)
        buttonBuffer.addEventListener("click", () => {
            toastContainer.showToast("firefox", "Данная функция не поддерживается в браузере Firefox.", 400);
        });
    else
        buttonBuffer.addEventListener("click", async (ev) => {

            //const permission = await navigator.permissions.query({ name: 'clipboard-read' });
            //if (permission.state === 'denied') {
            //    throw new Error('Not allowed to read clipboard.');
            //}
            let clipboardContents;
            try {
               clipboardContents = await navigator.clipboard.read();
            } catch (error) {
                console.log(error);
                if (error.message.includes("No valid data on clipboard")) {
                    toastContainer.showToast("noImage", "Только для изображений! В буфере обмена нет изображений. Сделайте снимок экрана и попробуйте снова.", 400);
                }
                throw new Error('Clipboard contains non-image data.');
            }
            
            
            for (const item of clipboardContents) {
                if (!item.types.includes('image/png')) {

                    toastContainer.showToast("noImage", "Только для изображений! В буфере обмена нет изображений. Сделайте снимок экрана и попробуйте снова.", 400);
                    throw new Error('Clipboard contains non-image data.');
                }

                const blob = await item.getType('image/png');
                var file = new File([blob], "image.png", {
                    type: "image/png",
                });

                var id = dropZone.id.replace("-zone", "");
                var input = document.getElementById(id);
                var fileStore = GetFileStorage(dropZone.id);
                fileStore.files.push(file);
                input.files = new FileListItems(fileStore.files);
                addFile(dropZone, file);
            }
        });

    input.addEventListener("change", (e) => {
        if (input.files.length > 0) {
            var fileStore = GetFileStorage(dropZone.id);
            for (var i = 0; i < input.files.length; i++) {

                if (input.files[i].size < input.dataset.size) {
                    fileStore.files.push(input.files[i]);
                    addFile(dropZone, input.files[i]);
                } else {
                    toastContainer.showToast("bigFile", `Лимит размера файла до ${input.dataset.size / 1000000}МБ!`, 400);
                }
            }
            input.files = new FileListItems(fileStore.files);
        }
    });
});


function dropHandler(ev) {
    var toastContainer = new ToastContainer();
    ev.preventDefault();
    var id = ev.currentTarget.id.replace("-zone", "");
    var input = document.getElementById(id);
    var fileStore = GetFileStorage(ev.currentTarget.id);
    if (ev.dataTransfer.items) {
        for (var i = 0; i < ev.dataTransfer.items.length; i++) {
            if (ev.dataTransfer.items[i].kind === 'file') {
                var file = ev.dataTransfer.items[i].getAsFile();

                if (file.size < input.dataset.size) {
                    fileStore.files.push(file);
                    addFile(ev.currentTarget, file);
                } else {
                    toastContainer.showToast("bigFile", `Лимит размера файла до ${input.dataset.size / 1000000}МБ!`, 400);
                }
            }
        }
    } else {
        for (var i = 0; i < ev.dataTransfer.files.length; i++) {

            if (ev.dataTransfer.files[i].size < input.dataset.size) {
                fileStore.files.push(ev.dataTransfer.files[i]);
                addFile(ev.currentTarget, ev.dataTransfer.files[i]);
            } else {
                toastContainer.showToast("bigFile", `Лимит размера файла до ${input.dataset.size / 1000000}МБ!`, 400);
            }
        }
    }

    input.files = new FileListItems(fileStore.files);
    ev.currentTarget.classList.remove("attachedFiles--hover");
}

function dragOverHandler(ev) {
    ev.preventDefault();
    ev.currentTarget.classList.add("attachedFiles--hover");
}

function dragLeaveHandler(ev) {
    ev.preventDefault();
    if (ev.target.classList.contains("attachedFiles--hover")) {
        ev.target.classList.remove("attachedFiles--hover");
    }
}

//function dragEnd(ev) {
//    console.log("SDFSDF");
//}

function RemoveFile(dropZone, file) {
    var fileStore = GetFileStorage(dropZone.id);
    var index = fileStore.files.indexOf(file);
    fileStore.files.splice(index, 1);
    var id = dropZone.id.replace("-zone", "");
    var input = document.getElementById(id);
    input.files = new FileListItems(fileStore.files);
}

function addFile(dropZone, file) {
    const ItemDiv = document.createElement("div");
    const RemovingDiv = document.createElement("div");
    ItemDiv.classList.add("attachedFiles__item");
    RemovingDiv.classList.add("attachedFiles__removing");

    ItemDiv.appendChild(RemovingDiv);

    ItemDiv.textContent = file.name;
    ItemDiv.appendChild(RemovingDiv);
    dropZone.appendChild(ItemDiv);

    RemovingDiv.addEventListener("click", (e) => {
        e.stopPropagation();
        e.preventDefault();
        dropZone.removeChild(ItemDiv);
        RemoveFile(dropZone, file);
    });
}

function clearDropZone(dropZone) {
    var items = dropZone.querySelectorAll(".attachedFiles__item");
    items.forEach(item => {
        dropZone.removeChild(item);
    });
}

var fileItems = document.querySelectorAll(".attachedFiles__removing");
fileItems.forEach(div => {
    div.addEventListener("click", (e) => {
        e.stopPropagation();
        e.preventDefault();
        var root = div.closest(".attachedFiles__item");
        var input = root.querySelector("input");
        input.name = input.name.replace("-attached", "-delete");
        root.style.display="none";
        //div.parentNode.remove();
    });
});

function dragPaste(ev) {
    ev.preventDefault();
    var items = ev.clipboardData.items;
    const dT = new ClipboardEvent('').clipboardData || new DataTransfer();
    var blob = items[0].getAsFile();

    var id = ev.currentTarget.id.replace("-zone", "");
    var input = document.getElementById(id);
    var fileStore = GetFileStorage(ev.currentTarget.id);
    fileStore.files.push(blob);
    input.files = new FileListItems(fileStore.files);
    addFile(ev.currentTarget, blob);
}


document.querySelectorAll(".attachedFiles__column").forEach(item => {

    item.addEventListener("click", (e) => {
        e.preventDefault();
        e.stopPropagation();

        if (e.target.href) {
            window.open(e.target.href, '_blank').focus();
        }
       
    });    
});