/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/


const OnDeleteGroup = (groupId) => {

    const groupContainer = document.getElementById('group-container');
    const groupBlock = document.getElementById(`${groupId}-group-block`);
    groupContainer.value = groupContainer.value.replace(`&${groupId}`, "");
    groupBlock.remove();

    if (groupContainer.value.length == 0) {
        document.getElementById('group-not-selected').style.display = "";
    }
}


const OnAddGroup = () => {

    const groupContainer = document.getElementById('group-container');
    const groupId = groupList.selector.value;
    fetch(`/api/users/admin/groups/group/${groupId}`)
        .then((resp) => resp.json())
        .then(function (data) {
            if (data.id == "null" || groupContainer.value.includes(data.id)) { return; }

            tmpl.content.querySelector("[data-content=root]").id = `${data.id}-group-block`;
            tmpl.content.querySelector("[data-content=groupName]").innerText = data.groupName;
            tmpl.content.querySelector("button").setAttribute('onclick', `OnDeleteGroup('${data.id}')`);

            var clone = tmpl.content.cloneNode(true);
            document.getElementById("group-selected-list").append(clone);
            document.getElementById('group-not-selected').style.display = "none";


            groupContainer.value += `&${data.id}`;

        });
}

//Start

    const dialog = new mdc.dialog.MDCDialog(document.getElementById('dialog-users-delete'));
    document.getElementById('users-open-dialog').addEventListener('click', () => {
        dialog.open();
    });

    //const selectPart = new mdc.select.MDCSelect(document.getElementById("users-edit-role"));

new MTDTextField("login-title-group");
new MTDTextField("login-title");
new MTDTextField("login-phone")
const email = new MTDTextField("login-email");
const confirm = document.getElementById("email-confirm");

new MTDSelectList("select-policy");
new MTDSelectList("select-role");
new MTDSelectList("select-role-cpq");
new MTDSelectList("user-recipient-id");

const groupList = new MTDSelectList("group-list");