import { MDCTextField } from "@material/textfield";
import bootstrap = require("bootstrap");
import { Actions, ChangeAction } from ".";
import { ImageSelector } from "../imageSelector";
import { Values } from "./values";

export class PartDialog {

    #partId: string;
    screen: bootstrap.Modal;
    inputName: MDCTextField;
    imagePart: ImageSelector;
    container: HTMLDivElement;
    values:  Values;
    inputRadioColumns: HTMLInputElement;
    inputRadioLines: HTMLInputElement;
    inputSwithTitle: HTMLInputElement;

    constructor(values: Values) {
        this.values = values;
        this.container = document.getElementById("partDialog") as HTMLDivElement;
        this.screen = new bootstrap.Modal(this.container, { keyboard: false, backdrop: 'static' });
        this.inputName = new MDCTextField(document.getElementById("partName"));
        this.imagePart = new ImageSelector("imagePart", this.callback.bind(this));
        this.inputRadioColumns = document.getElementById("radioColumns") as HTMLInputElement;
        this.inputRadioLines = document.getElementById("radioLines") as HTMLInputElement;
        this.inputSwithTitle = document.getElementById("swithTitle") as HTMLInputElement;
        //document.querySelector('input[name="genderS"]:checked').value;
        var binder = this;
        this.inputName.root.addEventListener("keyup", (event: any) => {

            if (event.isComposing || event.keyCode === 229) {
                return;
            }
           
            ChangeAction(Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === binder.#partId);                
                var partData = { ...part.getData() };
                partData.name = binder.inputName.value;  
                part.setData(partData);                
            });
        });

        this.inputRadioColumns.addEventListener("change", (e) => {

            ChangeAction(Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === binder.#partId);
                part.items.classList.add(binder.values.styleFormPartColumns);
                var partData = { ...part.getData() };
                partData.styleType = 5;  
                part.setData(partData);  
            });
        });

        this.inputRadioLines.addEventListener("change", (e) => {

            ChangeAction(Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === binder.#partId);
                part.items.classList.remove(binder.values.styleFormPartColumns);
                var partData = { ...part.getData() };
                partData.styleType = 4;
                part.setData(partData);  
            });

        });

        this.inputSwithTitle.addEventListener("change", (e) => {

            var inputTitle = e.currentTarget as HTMLInputElement;
            ChangeAction(Actions.ChangePartParameters, () => {
                var part = values.Parts.find(item => item.container.id === binder.#partId);                
                var partData = { ...part.getData() };
                partData.title = inputTitle.checked;
                part.setData(partData);
            });
        });
    }


    callback() {
        var binder = this;
        ChangeAction(Actions.ChangePartParameters, () => {
          
            var part = binder.values.Parts.find(item => item.container.id === binder.#partId);

            var partData = { ...part.getData() };
            partData.imageData = binder.imagePart.getData().base64string;
            partData.imageSize = binder.imagePart.getData().size;
            partData.imageType = binder.imagePart.getData().type;
            part.setData(partData);

            return;
        });
    }

    setPartId(partId: string) {
        this.#partId = partId;
    }

    getPartId() {
        return this.#partId;
    }


}