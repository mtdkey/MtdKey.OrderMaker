let dialogResolution;
let dialogDelete;

function clickResolutionAdd() {
    clearDialog();
    document.getElementById('button-dialog-create').parentElement.style.display = "block";
    document.getElementById('button-dialog-apply').parentElement.style.display = "none";
    const guid = newGuid();
    document.getElementById('dialog-resolution-id').value = guid;
    dialogResolution.open();
}

const clickResolutionEdit = (event) => {
    event.stopPropagation();
    let guid = event.target.getAttribute('id');
    if (guid === null) { guid = event.target.parentElement.getAttribute('id'); }
    guid = guid.substring(0,36);
    
    document.getElementById('dialog-resolution-id').value = guid;
    document.getElementById('button-dialog-create').parentElement.style.display = "none";
    document.getElementById('button-dialog-apply').parentElement.style.display = "block";

    document.getElementById('dialog-resolution-name').value = document.getElementById(`${guid}-resolution-name`).value;
    document.getElementById('dialog-resolution-note').value = document.getElementById(`${guid}-resolution-note`).value;
    document.getElementById('dialog-resolution-color').value = document.getElementById(`${guid}-resolution-color`).value;
    document.getElementById('dialog-resolution-number').value = document.getElementById(`${guid}-resolution-number`).value;
    
    dialogResolution.open();
}

function changeResolution() {

    const guid = document.getElementById('dialog-resolution-id').value;

    document.getElementById(`${guid}-resolution-name`).value = document.getElementById('dialog-resolution-name').value;
    document.getElementById(`${guid}-resolution-note`).value = document.getElementById('dialog-resolution-note').value;
    document.getElementById(`${guid}-resolution-color`).value = document.getElementById('dialog-resolution-color').value;
    document.getElementById(`${guid}-resolution-number`).value = document.getElementById('dialog-resolution-number').value;

    updateList(guid);
 
}

function clickDeleteDialog(event) {
    event.stopPropagation();
    const guid = event.target.getAttribute('id');
    document.getElementById('dialog-resolution-delete-guid').value = guid.substring(0,36);
    dialogDelete.open();

}


function clearDialog() {
    document.getElementById('dialog-resolution-name').value = "";
    document.getElementById('dialog-resolution-note').value = "";
    document.getElementById('dialog-resolution-color').value = "";
    document.getElementById('dialog-resolution-number').value = "";
}

function createResolution() {

    const ulFact = document.getElementById('ul-resolution');
    const ulFake = document.getElementById('ul-resolution-fake');
    const guid = document.getElementById('dialog-resolution-id').value;
    let html = ulFake.innerHTML;
    html = html.replace(/ID/g, guid);
    const liFake = document.createElement('li');
    liFake.innerHTML = html;
    const liFact = liFake.querySelector('li>li');
    ulFact.appendChild(liFact);

    document.getElementById(`${guid}-resolution`).value = guid;
    document.getElementById(`${guid}-resolution-name`).value = document.getElementById('dialog-resolution-name').value;
    document.getElementById(`${guid}-resolution-note`).value = document.getElementById('dialog-resolution-note').value;
    document.getElementById(`${guid}-resolution-color`).value = document.getElementById('dialog-resolution-color').value;
    document.getElementById(`${guid}-resolution-number`).value = document.getElementById('dialog-resolution-number').value;


    const rippleLi = new mdc.ripple.MDCRipple(liFact);
    rippleLi.unbounded = true;

    const rippleDel = new mdc.ripple.MDCRipple(document.getElementById(`${guid}-delete-button`));
    rippleDel.unbounded = true;

    updateList(guid);

}



function deleteResolution() {
    const guid = document.getElementById('dialog-resolution-delete-guid').value;
    const li = document.getElementById(`${guid}-resolution-li`);
    const ul = document.getElementById('ul-resolution');
    ul.removeChild(li);
}


function updateList(guid) {

    const name = document.getElementById(`${guid}-span-name`);
    name.innerText = document.getElementById(`${guid}-resolution-name`).value;
    name.style.color = document.getElementById(`${guid}-resolution-color`).value; 

    const note = document.getElementById(`${guid}-span-note`);
    note.innerText = document.getElementById(`${guid}-resolution-note`).value;
    note.style.color = document.getElementById(`${guid}-resolution-color`).value; 
}

//Start
    dialogResolution = new mdc.dialog.MDCDialog(document.getElementById('dialog-resolution'));
    dialogDelete = new mdc.dialog.MDCDialog(document.getElementById('dialog-resolution-delete'));
