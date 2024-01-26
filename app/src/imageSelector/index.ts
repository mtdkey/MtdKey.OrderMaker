import { addRippleEffect } from "../ripple/ripple";
import "./imageSelector.css";

export type ImageInfo = {
    size: number;
    type: string;
    base64string: string;
}

export class ImageSelector {
    container: HTMLDivElement;
    input: HTMLInputElement;
    imageUploadWrap: HTMLDivElement;
    fileUploadImage: HTMLImageElement;
    fileUploadContent: HTMLDivElement;
    buttonRemove: HTMLButtonElement;
    #data: ImageInfo;
    callback: Function;

    constructor(id: string, callback: Function = undefined) {
        this.callback = callback;
        this.container = document.getElementById(id) as HTMLDivElement;
        this.input = this.container.querySelector(".file-upload-input") as HTMLInputElement;
        this.imageUploadWrap = this.container.querySelector(".image-upload-wrap") as HTMLDivElement;
        this.fileUploadImage = this.container.querySelector(".file-upload-image") as HTMLImageElement;
        this.fileUploadContent = this.container.querySelector(".file-upload-content") as HTMLDivElement;
        this.buttonRemove = this.container.querySelector("button") as HTMLButtonElement;
        this.input.addEventListener("change", this.readURL.bind(this));
        this.buttonRemove.addEventListener("click", this.#removeUpload.bind(this));
        this.imageUploadWrap.addEventListener("dragover", this.dragOver.bind(this));
        this.imageUploadWrap.addEventListener("dragleave", this.dragLeave.bind(this));
        this.#data = { base64string: "", size: 0, type: "" };

        addRippleEffect(this.container);
    }

    getImage() {
        return `data:${this.#data.type};base64,${this.#data.base64string}`;
    }
    clearData() {
        this.#data.base64string = "";
        this.#data.size = 0;
        this.#data.type = "";
    }
    setData(data: ImageInfo) {
        this.#data.base64string = data.base64string;
        this.#data.size = data.size;
        this.#data.type = data.type;
    }

    getData(): Readonly<ImageInfo> {
        return this.#data;
    }

    dragOver(e: any) {
        e.preventDefault();
        this.imageUploadWrap.classList.add("image-dropping");
    }

    dragLeave(e: any) {
        e.preventDefault();
        this.imageUploadWrap.classList.remove("image-dropping");
    }

    readURL() {

        if (this.input.files && this.input.files[0]) {

            var reader = new FileReader();
            var imageSelector = this;

            reader.onload = function (e) {
                var imgInfo: ImageInfo = {
                    base64string: e.target.result.toString(),
                    size: imageSelector.input.files[0].size,
                    type: imageSelector.input.files[0].type,
                };
                imgInfo.base64string = imgInfo.base64string.replace(`data:${imgInfo.type};base64,`, "");
                imageSelector.addImage(imgInfo, true);
            };

            reader.readAsDataURL(this.input.files[0]);

        }
    }

    #removeUpload() {
        this.input.value = "";
        this.clearData();
        this.fileUploadContent.style.display = "none";
        this.imageUploadWrap.style.display = "block";

        if (this.callback) this.callback();
    }


    addImage(data: ImageInfo, userInit: boolean = true) {
        
        if (!data.base64string || data.base64string.length === 0) {
            this.input.value = "";
            this.clearData();
            this.fileUploadContent.style.display = "none";
            this.imageUploadWrap.style.display = "block";
            return;
        }
           
        
        this.#data = data;
        this.fileUploadImage.src = this.getImage();
        this.imageUploadWrap.style.display = "none";
        this.fileUploadContent.style.display = "block";

        if (userInit && this.callback)
            this.callback();
    }

}