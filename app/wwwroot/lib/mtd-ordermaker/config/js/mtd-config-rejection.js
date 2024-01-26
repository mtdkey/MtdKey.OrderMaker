let dialogRejection;
let dialogRejectionDelete;

function clickRejectionAdd() {
    clearRejectionDialog();
    document.getElementById('button-dialog-create-rejection').parentElement.style.display = "block";
    document.getElementById('button-dialog-apply-rejection').parentElement.style.display = "none";
    const guid = newGuid();
    document.getElementById('dialog-rejection-id').value = guid;
    dialogRejection.open();
}

const clickRejectionEdit = (event) => {
    event.stopPropagation();
    let guid = event.target.getAttribute('id');
    if (guid === null) { guid = event.target.parentElement.getAttribute('id'); }
    guid = guid.substring(0,36);
    
    document.getElementById('dialog-rejection-id').value = guid;
    document.getElementById('button-dialog-create-rejection').parentElement.style.display = "none";
    document.getElementById('button-dialog-apply-rejection').parentElement.style.display = "block";

    document.getElementById('dialog-rejection-name').value = document.getElementById(`${guid}-rejection-name`).value;
    document.getElementById('dialog-rejection-note').value = document.getElementById(`${guid}-rejection-note`).value;
    document.getElementById('dialog-rejection-color').value = document.getElementById(`${guid}-rejection-color`).value;
    document.getElementById('dialog-rejection-number').value = document.getElementById(`${guid}-rejection-number`).value;
    
    dialogRejection.open();
}

function changeRejection() {

    const guid = document.getElementById('dialog-rejection-id').value;

    document.getElementById(`${guid}-rejection-name`).value = document.getElementById('dialog-rejection-name').value;
    document.getElementById(`${guid}-rejection-note`).value = document.getElementById('dialog-rejection-note').value;
    document.getElementById(`${guid}-rejection-color`).value = document.getElementById('dialog-rejection-color').value;
    document.getElementById(`${guid}-rejection-number`).value = document.getElementById('dialog-rejection-number').value;

    updateRejectionList(guid);
 
}

function clickDeleteRejectionDialog(event) {
    event.stopPropagation();
    const guid = event.target.getAttribute('id');
    document.getElementById('dialog-rejection-delete-guid').value = guid.substring(0,36);
    dialogRejectionDelete.open();
}


function clearRejectionDialog() {
    document.getElementById('dialog-rejection-name').value = "";
    document.getElementById('dialog-rejection-note').value = "";
    document.getElementById('dialog-rejection-color').value = "";
    document.getElementById('dialog-rejection-number').value = "";
}

function createRejection() {

    const ulFact = document.getElementById('ul-rejection');
    const ulFake = document.getElementById('ul-rejection-fake');
    const guid = document.getElementById('dialog-rejection-id').value;
    let html = ulFake.innerHTML;
    html = html.replace(/ID/g, guid);
    const liFake = document.createElement('li');
    liFake.innerHTML = html;
    const liFact = liFake.querySelector('li>li');
    ulFact.appendChild(liFact);

    document.getElementById(`${guid}-rejection`).value = guid;
    document.getElementById(`${guid}-rejection-name`).value = document.getElementById('dialog-rejection-name').value;
    document.getElementById(`${guid}-rejection-note`).value = document.getElementById('dialog-rejection-note').value;
    document.getElementById(`${guid}-rejection-color`).value = document.getElementById('dialog-rejection-color').value;
    document.getElementById(`${guid}-rejection-number`).value = document.getElementById('dialog-rejection-number').value;


    const rippleLi = new mdc.ripple.MDCRipple(liFact);
    rippleLi.unbounded = true;

    const rippleDel = new mdc.ripple.MDCRipple(document.getElementById(`${guid}-delete-button-rejection`));
    rippleDel.unbounded = true;

    updateRejectionList(guid);

}

function deleteRejection() {
    const guid = document.getElementById('dialog-rejection-delete-guid').value;
    const li = document.getElementById(`${guid}-rejection-li`);
    const ul = document.getElementById('ul-rejection');
    ul.removeChild(li);
}

function updateRejectionList(guid) {

    const name = document.getElementById(`${guid}-span-name-rejection`);
    name.innerText = document.getElementById(`${guid}-rejection-name`).value;
    name.style.color = document.getElementById(`${guid}-rejection-color`).value; 

    const note = document.getElementById(`${guid}-span-note-rejection`);
    note.innerText = document.getElementById(`${guid}-rejection-note`).value;
    note.style.color = document.getElementById(`${guid}-rejection-color`).value; 
}

//Start
    dialogRejection = new mdc.dialog.MDCDialog(document.getElementById('dialog-rejection'));
    dialogRejectionDelete = new mdc.dialog.MDCDialog(document.getElementById('dialog-rejection-delete'));
