
new MTDTextField("user-name");
const pwdNew = new MTDTextField("pwd-new");
const pwdConfirm = new MTDTextField("pwd-confirm");

const CreateFormData = (form) => {

    var formData = new FormData();
    for (var i = 0; i < form.length; i++) {

        if (form[i].getAttribute("type") == 'checkbox') {
            formData.append(form[i].name, form[i].checked);
        } else {
            formData.append(form[i].name, form[i].value);
        }

        if (form[i].files && form[i].files.length > 0) {
            formData.append(form[i].name, form[i].files[0], form[i].files[0].name);
        }
    }

    return formData;
}

const GeneratePassword = () => {

    const form = document.getElementById('generate-password-form');
    const formData = CreateFormData(form);

    fetch(form.action, { method: form.method, body: formData })
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            if (data.value) {
                pwdNew.input.type = "text";
                pwdNew.textField.value = data.value;
                pwdConfirm.textField.value = data.value;
                pwdConfirm.textField.focus();

                fireEvent(pwdNew.input, "change");
                fireEvent(pwdConfirm.input, "change");
            }
        });
}


function fireEvent(node, eventName) {

    var doc;
    if (node.ownerDocument) {
        doc = node.ownerDocument;
    } else if (node.nodeType == 9) {
        doc = node;
    } else {
        throw new Error("Invalid node passed to fireEvent: " + node.id);
    }

    if (node.dispatchEvent) {
        var eventClass = "";
        switch (eventName) {
            case "click":
            case "mousedown":
            case "mouseup":
                eventClass = "MouseEvents";
                break;

            case "focus":
            case "change":
            case "blur":
            case "select":
                eventClass = "HTMLEvents";
                break;

            default:
                throw "fireEvent: Couldn't find an event class for event '" + eventName + "'.";
                break;
        }
        var event = doc.createEvent(eventClass);

        var bubbles = eventName == "change" ? false : true;
        event.initEvent(eventName, bubbles, true);

        event.synthetic = true;
        node.dispatchEvent(event, true);
    } else if (node.fireEvent) {

        var event = doc.createEventObject();
        event.synthetic = true;
        node.fireEvent("on" + eventName, event);
    }
};