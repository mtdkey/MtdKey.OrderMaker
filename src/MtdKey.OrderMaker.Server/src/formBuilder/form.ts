import { generateUID } from "./utilities";
import { ImageSelector } from "../imageSelector/index";
import { Values } from "./values";
import { Actions, ChangeAction } from ".";

export class Form  {

    id: string;
    name: string;
    description: string;
    active: boolean;
    sequence: number;
    visibleNumber: boolean;
    visibleDate: boolean;
    imageBack: ImageSelector;
    imageLogo: ImageSelector;


    constructor(values: Values) {
        this.id = generateUID();
        this.name = "New Form";
        this.active = true;
        this.visibleDate = true;
        this.visibleNumber = true;
        this.imageBack = new ImageSelector("imageBack", this.callback);
        this.imageLogo = new ImageSelector("imageLogo", this.callback);


        values.formDialogName.root.addEventListener("keyup", (event: any) => {
            if (event.isComposing || event.keyCode === 229) {
                return;
            }

            ChangeAction(Actions.ChangeFormParameters, () => {
                values.titleFormName.textContent = values.formDialogName.value;
                values.Form.name = values.formDialogName.value;
            });
          
        });
    }

    callback() {
        ChangeAction(Actions.ChangeFormParameters, () => { return; });
    }

}