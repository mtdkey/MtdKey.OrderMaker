import { CreateFormData, ShowModalAction, ShowSnackBarAction } from "./actions";
import { TagHandlers } from "./tagHandlers";
import { dataMtdClickerBy, dataMtdClicker } from "./types";



const proxyClickers = document.querySelectorAll(`[${dataMtdClickerBy}]`);

proxyClickers.forEach((clicker) => {
    const targetId = clicker.getAttribute(dataMtdClickerBy);
    clicker.addEventListener("click", () => {
        document.getElementById(targetId).click();
    })
});


const clickers = document.querySelectorAll(`[${dataMtdClicker}]`);

clickers.forEach(async clicker => {

    clicker.addEventListener("click", async (elemnt: any) => {

        
        const form: HTMLFormElement = elemnt.currentTarget.closest("form");

        const validate = form.reportValidity();
        if (!validate) { return false; }

        ShowModalAction(true);

        var formData = CreateFormData(form);
        const response = await fetch(form.action, { method: form.method, body: formData });


        switch (response.status) {
            case 200: {
                TagHandlers.forEach(async handler => await handler(clicker, response));
                setTimeout(() => ShowModalAction(false), 1000);
                break;
            }
            case 400: {
                var value = await response.text();
                setTimeout(() => { ShowModalAction(false); ShowSnackBarAction(value, true); }, 1000);
                break;
            }
            case 500: {
                setTimeout(() => { ShowModalAction(false); ShowSnackBarAction("500 Internal Server Error", true); }, 1000);
                break;
            }
            default: {
                setTimeout(() => { ShowModalAction(false); ShowSnackBarAction(`No handler for status ${response.status}`, true); }, 1000);
            }
        }
    });
});





