import { TagHandler, dataResponse } from "./types";

async function ResponseHandler(clicker: Element, response: Response) {
    var inputId = clicker.getAttribute(`[${dataResponse}]`);
    if (!inputId) return false;
    var input: HTMLInputElement = document.getElementById(inputId) as HTMLInputElement;
    if (!input) return false;
    input.value = await response.json();
}



export const TagHandlers: Array<TagHandler> = [ResponseHandler];
