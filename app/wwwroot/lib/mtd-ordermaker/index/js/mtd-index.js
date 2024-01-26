/*
    OrderMaker - http://mtdkey.com
    Copyright(c) 2019 Oleg Bruev. All rights reserved.
*/


const IndexShowModal = (show = true, indicator = true) => {

    const fab = document.getElementById("indexCreator");
    const modal = document.getElementById("indexModal");
    const progress = document.getElementById("indexProgress");

    modal.style.display = show ? "" : "none";
    if (fab) { fab.style.display = show ? "none" : ""; }
    progress.style.display = indicator ? "" : "none";
}

const ListenerPageMenu = () => {

    const divMenu = document.getElementById('indexPageMenu')
    if (!divMenu) { return false; }
    const indexPageMenu = new mdc.menu.MDCMenu(divMenu);
    const pb = document.querySelector('[mtd-data-page]');
    const formId = pb.attributes.getNamedItem('mtd-data-page').nodeValue;
    pb.addEventListener('click', () => {
        indexPageMenu.open = true;
    });


    const ms = document.getElementById("indexMenuSize");
    ms.addEventListener('click', (e) => {
        const pages = e.target.getAttribute("data-value");
        document.body.scrollTop = document.documentElement.scrollTop = 0;
        ActionShowModal(true);
        var xmlHttp = new XMLHttpRequest();
        xmlHttp.open("POST", `/api/index/${formId}/pagesize/${pages}`, true);

        xmlHttp.send();
        xmlHttp.onreadystatechange = function () {
            if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                //document.location.reload(location.hash, '');
                document.location.reload();
            }
        }
    });
}

const FileDownload = (formId) => {
    ActionShowModal(true);
    var form = document.getElementById(formId);

    var formData = CreateFormData(form);
    
    var xhr = new XMLHttpRequest();
    xhr.open('POST', form.action, true);
    //xmlHttp.responseType = 'json';
    xhr.responseType = 'arraybuffer';
    xhr.onload = function () {
        if (this.status === 200) {
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
            }
            var type = xhr.getResponseHeader('Content-Type');

            var blob;
            if (typeof File === 'function') {
                try {
                    blob = new File([this.response], filename, { type: type });
                } catch (e) { /* Edge */ }
            }
            if (typeof blob === 'undefined') {
                blob = new Blob([this.response], { type: type });
            }

            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                window.navigator.msSaveBlob(blob, filename);
            } else {
                var URL = window.URL || window.webkitURL;
                var downloadUrl = URL.createObjectURL(blob);

                if (filename) {
                    // use HTML5 a[download] attribute to specify filename
                    var a = document.createElement("a");
                    // safari doesn't support this yet
                    if (typeof a.download === 'undefined') {
                        window.location.href = downloadUrl;
                    } else {
                        a.href = downloadUrl;
                        a.download = filename;
                        document.body.appendChild(a);
                        a.click();
                    }
                } else {
                    window.location.href = downloadUrl;
                }

                setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
                ActionShowModal();
            }
        }

        if (this.status == 400) {      
            var textError = new TextDecoder().decode(this.response);
            var textJson = JSON.parse(textError);            
            setTimeout(() => { ActionShowModal(); MainShowSnackBar(textJson.value, true); }, 1000);
        }

        if (this.status == 500) {
            setTimeout(() => { ActionShowModal(); MainShowSnackBar("500 Internal Server Error", true); }, 1000);
        }

    };
    //xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
    xhr.send(formData);
}




//Start

ListenerPageMenu();

const storeIds = document.getElementById("nav-store-ids");
if (storeIds) {
    sessionStorage.setItem("storeIds", storeIds.value);
}

new MTDTextField("search-text");
new MTDTextField("search-number");
