class MTDTextField {

    constructor(id) {        
        this.base = document.getElementById(id);
        if (this.base) {
            
            this.textField = new mdc.textField.MDCTextField(this.base);
           
            this.input = document.getElementById(`${id}-input`);

            this.input.addEventListener("invalid", () => {
                this.input.setCustomValidity(" ");
                this.textField.valid = false;
            });

            this.input.addEventListener("change", () => {
                this.input.setCustomValidity("");
                this.textField.valid = true;
            });

            this.input.addEventListener("input", () => {
                this.input.setCustomValidity("");
                this.textField.valid = true;
            });

        }        
    }
}