
const pwdCurrent = new MTDTextField("pwd-current");
const pwdNew = new MTDTextField("pwd-new");
const pwdConfirm = new MTDTextField("pwd-confirm");


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


