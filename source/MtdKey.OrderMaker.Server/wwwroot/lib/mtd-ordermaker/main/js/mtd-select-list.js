class MTDSelectList {

    constructor(id) {

        this.div = document.getElementById(id);

        if (this.div) {

            this.selector = new mdc.select.MDCSelect(document.getElementById(id));
            this.input = document.getElementById(`${id}-input`);
            this.input.value = this.selector.value;
            this.lis = document.getElementById(`${id}-ul`).getElementsByTagName('li');

            this.selector.listen('MDCSelect:change', () => {
                this.input.value = this.selector.value;
            });

            this.itemTemplate = document.getElementById("mtd-select-list-helper-item");
        }

        const inputSearch = document.getElementById(`${id}-search`);
        if (inputSearch) {
            inputSearch.addEventListener("keyup", (e) => {

                if (e.target.value === "") {
                    for (let li of this.lis) {
                        li.style.display = "";
                    }
                }

                if (e.target.value !== "") {
                    for (let li of this.lis) {
                        if (li.dataset.value != "search-li") {
                            const text = li.querySelector(".mdc-list-item__text").innerText;
                            if (text.toUpperCase().includes(e.target.value.toUpperCase())) {
                                li.style.display = "";
                            } else {
                                li.style.display = "none";
                            }                            
                        }                        
                    }
                }               

            });
        }

    }

    AddItem(id, name, selected = false) {

        const li = this.itemTemplate.querySelector("li");

        let clone = li.cloneNode(true);
        clone.setAttribute("data-value", id);
        const ripple = new mdc.ripple.MDCRipple(clone);
        let text = clone.querySelector(".mdc-list-item__text");
        text.textContent = name;
        const ul = this.div.querySelector(".mdc-list");
        ul.appendChild(clone);

        if (selected) {
            const index = ul.getElementsByTagName("li").length;
            this.selector.selectedIndex = index - 1;
        }

    }

    RemoveItems() {
        this.selector.selectedIndex = -1;
        const ul = this.div.querySelector(".mdc-list");
        ul.innerHTML = "";
        while (ul.firstChild) {
            ul.removeChild(ul.firstChild);
        }

    }
}