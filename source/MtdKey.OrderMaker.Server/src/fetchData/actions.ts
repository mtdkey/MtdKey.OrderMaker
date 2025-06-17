import { MDCSnackbar } from "@material/snackbar";
export function ShowModalAction(show: boolean) {
    const style = show ? 'block' : 'none';
    document.getElementById('mainModal').style.display = style;
}

export const CreateFormData = (form: HTMLFormElement) => {

    var formData = new FormData();
    
    var inputs: NodeListOf<HTMLInputElement> = form.querySelectorAll("input");
    inputs.forEach(input => {

        switch (input.type) {
            case "checkbox": {
                formData.append(input.name, input.checked.toString());
                break;
            }
            default: {
                formData.append(input.name, input.value);
            }
        }
        

        for (var i = 0; i < input.files?.length; i++) {
            formData.append(input.name, input.files[i], input.files[i].name);
        };
    });
    return formData;
}


export const ShowSnackBarAction = (message: string, error = false) => {
    const snackbar = new MDCSnackbar(document.getElementById('main-snack'));
    const div = document.getElementById('main-snack');
    div.classList.add("mdc-snackbar--open");
    snackbar.labelText = message;
    if (error) {
        snackbar.timeoutMs = 10000;
        const surface: HTMLDivElement = div.querySelector(".mdc-snackbar__surface");
        surface.style.backgroundColor = "darkred";
    }
    snackbar.open();
}