let dragSrcEl = null;
let dragVaule = false;

const handleDragStart = e => {
    dragVaule = e.target.attributes.getNamedItem('aria-checked').nodeValue;
    dragSrcEl = e.target;
    e.dataTransfer.setDragImage(new Image(), 0, 0);
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', e.target.outerHTML);
}

const handleDragOver = e => {

    if (e.preventDefault) {
        e.preventDefault();
    }

    e.target.classList.add("over");
    e.dataTransfer.dropEffect = 'move';
    return false;
}

const handleDragEnter = (e) => {
    // over item
}

const handleDragLeave =(e) => {
    e.target.classList.remove('over');
};

const handleDrop = e => {

    if (e.stopPropagation) {
        e.stopPropagation();
    }

    if (dragSrcEl != e.target) {

        e.target.parentNode.removeChild(dragSrcEl);
        var dropHTML = e.dataTransfer.getData('text/html');
        e.target.insertAdjacentHTML('beforebegin', dropHTML);
        var dropElem = e.target.previousSibling;
        //dropElem.className = "mtd-desk-draggable-item mdc-list-item";
        dropElem.className = "mdc-list-item";
        new mdc.ripple.MDCRipple(dropElem);
        addDnDHandlers(dropElem);
        if (dragVaule === "true") {
            //dropElem.click();
            const id = dropElem.getAttribute("data-value");
            const input = document.getElementById(`${id}-lc`);
            dropElem.setAttribute("aria-checked", "true");
            input.checked = true;
        }

    }
    e.target.classList.remove('over');
    return false;
}

const handleDragEnd = (e) => {
    e.target.classList.remove('over');
    dragSrcEl = null;

    const formSequence = document.querySelector("[mtd-data-form='sequence']");
    if (formSequence) {

        let strData = "";
        const list = document.querySelector("[mtd-data-draggable]");
        const clicker = formSequence.querySelector("[mtd-data-clicker]");
        const data = formSequence.querySelector("[mtd-data-sequence]");

        list.querySelectorAll('[data-value]').forEach((item) => {
            const d = item.getAttribute("data-value");
            strData += `${d}&`;
        });

        data.value = strData;
        clicker.click();

    }




}

const OnClickFilterColumn = (e) => {

    if (dragSrcEl) {
        return false;
    }

    const li = e.currentTarget;
    const id = li.getAttribute("data-value");
    const aria = li.getAttribute("aria-checked");
    const input = document.getElementById(`${id}-lc`);
    li.setAttribute("aria-checked", aria === "true" ? 'false' : 'true');
    input.checked = !input.checked;

}


const addDnDHandlers = elem => {

    elem.addEventListener('dragstart', handleDragStart, false);
    elem.addEventListener('dragover', handleDragOver, false);
    elem.addEventListener('dragleave', handleDragLeave, false);
    elem.addEventListener('drop', handleDrop, false);
    elem.addEventListener('dragend', handleDragEnd, false);
}

//Start
const cols = document.querySelectorAll('[mtd-data-draggable] .mdc-list-item');
[].forEach.call(cols, addDnDHandlers);
