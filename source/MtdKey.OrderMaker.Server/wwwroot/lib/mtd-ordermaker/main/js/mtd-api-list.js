
const StoreListRequest = (listId) => {

    ActionShowModal(true);

    const form = document.getElementById(`${listId}-form`);
    const formData = CreateFormData(form);

    fetch(form.action, { method: form.method, body: formData })
        .then((response) => {
            return response.json();
        }).then((data) => {
     
            document.getElementById(`${listId}-form-img`).value = data.formImg;

            StoreListCreateTable(data, listId);
            ActionShowModal(false);

        });
}


const StoreListCreateTable = (data, listId) => {

    const area = document.getElementById(`${listId}-table-area`);
    area.innerHTML = '';

    var table = document.createElement('table');
    var thead = document.createElement('thead');
    table.appendChild(thead);
    var trh = document.createElement('tr');
    thead.appendChild(trh);

    for (var i = 0; i < data.columns.length; i++) {
        var th = document.createElement('th');
        var text = document.createTextNode(data.columns[i]);
        th.appendChild(text);
        trh.appendChild(th);
    }

    thead.appendChild(trh);
    table.appendChild(thead);
    var tbody = document.createElement('tbody');
    table.appendChild(tbody);

    var count = data.store.count > 10 ? 10 : data.store.length;

    for (var r = 0; r < count; r++) {
        var tr = document.createElement('tr');        

        tr.addEventListener("click", (e) => {
            var t = e.path.filter(word => word.nodeName === "TR");            
            var storeId = t[0].cells[0].children[0].innerText;
            var docName = t[0].cells[1].children[0].innerText;

            StoreListClickRow(listId, storeId, data.formName, docName,t);
        });

        for (var c = 0; c < data.columns.length; c++) {
            var td = document.createElement('td');
            var div = document.createElement('div');
            td.appendChild(div);
            var text = document.createTextNode(data.store[r].fields[c].value);
            div.appendChild(text);
            td.appendChild(div);
            tr.appendChild(td);
        }

        tbody.appendChild(tr);
    }

    var pageLine = document.getElementById(`${listId}-page-line`);
    pageLine.innerText = data.pageLine;
    var pageNumber = document.getElementById(`${listId}-page-number`);
    pageNumber.value = data.pageNumber;

    var pageCount = document.getElementById(`${listId}-page-count`);
    pageCount.value = data.pageCount;

    table.appendChild(tbody);
    area.appendChild(table);
}

const StoreListPageNavFirst = (listId) => {
    var pageNumber = document.getElementById(`${listId}-page-number`);
    if (pageNumber.value === "1") { return false; }
    pageNumber.value = 1;
    StoreListRequest(listId);
}

const StoreListPageNavPrev = (listId) => {
    var pageNumber = document.getElementById(`${listId}-page-number`);
    if (pageNumber.value === "1") { return false; }

    var value = parseInt(pageNumber.value);
    pageNumber.value = --value;
    StoreListRequest(listId);
}

const StoreListPageNavNext = (listId) => {
    var pageNumber = document.getElementById(`${listId}-page-number`);
    var pageCount = document.getElementById(`${listId}-page-count`);
    if (pageNumber.value === pageCount.value) { return false; }

    var value = parseInt(pageNumber.value);
    pageNumber.value = ++value;
    StoreListRequest(listId);
}

const StoreListPageNavLast = (listId) => {
    var pageNumber = document.getElementById(`${listId}-page-number`);
    var pageCount = document.getElementById(`${listId}-page-count`);
    if (pageNumber.value === pageCount.value) { return false; }

    pageNumber.value = pageCount.value;
    StoreListRequest(listId);
}

const StoreListClickRow = (listId, storeId, formName, docName,t) => {

    document.getElementById(`${listId}-selected-id`).value = storeId;
    t[0].style.backgroundColor = '#EAEDED';
    setTimeout(() => t[0].style.backgroundColor = 'white', 300);

    UpdateTarget(listId, storeId);
    UpdateViewer(listId, formName, docName)
    

}

const StoreListClearSelected = (listId) => {

    document.getElementById(`${listId}-selected-id`).value = "";
    UpdateTarget(listId, "");
    UpdateViewer(listId);
    
}


const StoreListClearFilter = (listId) => {

    document.getElementById(`${listId}-search-number-input`).value = "";
    document.getElementById(`${listId}-search-text-input`).value = "";
    document.getElementById(`${listId}-page-number`).value = "1";

    StoreListClearSelected(listId);

    StoreListRequest(listId);
}

const UpdateViewer = (listId, formName, docName) => {

    document.getElementById(`${listId}-viewer-form-name`).innerText = formName === undefined ? "" : formName;
    document.getElementById(`${listId}-viewer`).style.backgroundColor = '#EAEDED';
    setTimeout(() => document.getElementById(`${listId}-viewer`).style.backgroundColor = 'white', 300);

    if (docName) {

        var src = document.getElementById(`${listId}-form-img`).value;
        document.getElementById(`${listId}-viewer-img`).src = src;
        const selectedId = document.getElementById(`${listId}-selected-id`).value;
        document.getElementById(`${listId}-viewer-on`).innerHTML = `<a href="/workplace/store/details?id=${selectedId}" target="_blank">${docName}</a>`;
        document.getElementById(`${listId}-viewer-on`).style.display = '';        
        document.getElementById(`${listId}-viewer-off`).style.display = 'none';
       
    } else {
        document.getElementById(`${listId}-viewer-on`).innerText = '';
        document.getElementById(`${listId}-viewer-on`).style.display = 'none';
        document.getElementById(`${listId}-viewer-img`).src = '';
        document.getElementById(`${listId}-viewer-off`).style.display = '';
    }

}

const UpdateTarget = (listId, result) => {

    const targetId = document.getElementById(`${listId}-selected-target`).value;
    document.getElementById(targetId).value = result;
}

const sls = document.querySelectorAll("[mtd-store-list]");


if (sls) {
    sls.forEach((list) => {

        const listId = list.getAttribute("mtd-store-list");
        const selectedId = document.getElementById(`${listId}-selected-id`).value;

        UpdateTarget(listId, selectedId);

        const inputNumber = new MTDTextField(`${listId}-search-number`);
        const inputText = new MTDTextField(`${listId}-search-text`);
        const form = document.getElementById(`${listId}-form`);

        inputNumber.input.addEventListener('keydown', (e) => {
            if (e.keyCode == 13) {
                inputText.textField.value = "";
                inputNumber.input.blur();
                const validate = form.reportValidity();
                if (!validate) {                    
                    return false;
                }                
                StoreListRequest(listId);
            }
        });

        inputText.input.addEventListener('keydown', (e) => {
            if (e.keyCode == 13) {      

                inputNumber.textField.value = "";
                inputText.input.blur();

                const validate = form.reportValidity();
                if (!validate) {
                    return false;
                }

                
                StoreListRequest(listId);
            }
        });

        var selectRelated = new MTDSelectList(`${listId}-form-id`);
        selectRelated.selector.listen('MDCSelect:change', () => {
            var pageNumber = document.getElementById(`${listId}-page-number`);
            pageNumber.value = "1";
            StoreListRequest(listId);
        });

        // StoreListRequest(listId);

    });

}