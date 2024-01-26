
//const OnClickFilterColumn = (e) => {

//    if (dragSrcEl) {
//        return false;
//    }

//    const li = e.currentTarget;
//    const id = li.getAttribute("data-value");
//    const aria = li.getAttribute("aria-checked");
//    const input = document.getElementById(`${id}-lc`);
//    li.setAttribute("aria-checked", aria === "true" ? 'false' : 'true');
//    input.checked = !input.checked;

//}

const ListenerFilterColumn = () => {


    document.getElementById("indexButtonColApply").addEventListener('click', (e) => {

        const form = document.getElementById("indexFormColumn");

        ActionShowModal(true);

        const indexListColumn = document.getElementById('indexListColumn');
        const items = indexListColumn.querySelectorAll('li');

        let result = "";
        items.forEach((item) => {
            const state = item.getAttribute("aria-checked");
            if (state === 'true') {
                const dataValue = item.getAttribute("data-value");
                result += `${dataValue},`;
            }
        });

        document.getElementById("indexDataColumnList").value = result;


        const checkBoxNumber = document.getElementById("show-number");
        const checkBoxDate = document.getElementById("show-date");
        const indexDataColumnNumber = document.getElementById("indexDataColumnNumber");
        const indexDataColumnDate = document.getElementById("indexDataColumnDate");
        indexDataColumnNumber.value = checkBoxNumber.checked;
        indexDataColumnDate.value = checkBoxDate.checked;

        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("POST", form.getAttribute("action"), true);
            
        var formData = new FormData();

        for (var i = 0; i < form.length; i++) {
            formData.append(form[i].name, form[i].value);
        }

        xmlHttp.send(formData);
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                document.location.reload(location.hash, '');
            }
        }

    });
}

//let dragSrc = null;
//let dragData = false;

//function handleDragStart(e) {
//    dragData = this.getAttribute('aria-checked');
//    dragSrc = this;
//    e.dataTransfer.setDragImage(new Image(), 0, 0);
//    e.dataTransfer.effectAllowed = 'move';
//    e.dataTransfer.setData('text/html', this.outerHTML);
//}
//function handleDragOver(e) {
//    if (e.preventDefault) {
//        e.preventDefault();
//    }
//    this.classList.add("over");
//    e.dataTransfer.dropEffect = 'move';
//    return false;
//}

//function handleDragEnter(e) {
//    // over item
//}

//function handleDragLeave(e) {
//    this.classList.remove('over');
//}

//function handleDrop(e) {

//    if (e.stopPropagation) {
//        e.stopPropagation();
//    }

//    if (dragSrc != this) {

//        this.parentNode.removeChild(dragSrcEl);
//        var dropHTML = e.dataTransfer.getData('text/html');
//        this.insertAdjacentHTML('beforebegin', dropHTML);
//        var dropElem = this.previousSibling;
//        dropElem.className = "mdc-list-item";
//        new mdc.ripple.MDCRipple(dropElem);
//        addDnDHandlers(dropElem);

//        if (dragData === "true") {

//            const id = dropElem.getAttribute("data-value");
//            const input = document.getElementById(`${id}-lc`);
//            dropElem.setAttribute("aria-checked", "true");
//            input.checked = true;
//        }

//    }

//    this.classList.remove('over');
//    return false;
//}

//function handleDragEnd(e) {
//    this.classList.remove('over');
//    dragSrc = null;
//}

//function addIDnDHandlers(elem) {
//    elem.addEventListener('dragstart', handleDragStart, false);
//    elem.addEventListener('dragenter', handleDragEnter, false)
//    elem.addEventListener('dragover', handleDragOver, false);
//    elem.addEventListener('dragleave', handleDragLeave, false);
//    elem.addEventListener('drop', handleDrop, false);
//    elem.addEventListener('dragend', handleDragEnd, false);
//}

//Start

//var columns = document.querySelectorAll('#indexListColumn .mdc-list-item');
//[].forEach.call(columns, addIDnDHandlers);

ListenerFilterColumn();
