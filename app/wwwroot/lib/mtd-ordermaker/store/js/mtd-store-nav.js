class NavButtons {
    constructor() {
        this.NavDiv = document.getElementById("nav-div");
        this.NavModal = document.getElementById("nav-modal");

        this.DivMain = document.getElementById("nav-div-main");
        this.ButtonMain = document.getElementById("nav-button-main");
        this.DivList = document.getElementById("nav-div-list");
        this.ButtonList = document.getElementById("nav-button-list");
        this.DivBack = document.getElementById("nav-div-back");
        this.ButtonBack = document.getElementById("nav-button-back");
        this.DivNext = document.getElementById("nav-div-next");
        this.ButtonNext = document.getElementById("nav-button-next");
        this.MiniButons = document.getElementById("nav-div").querySelectorAll(".mdc-fab--mini");
        this.CurrentId = document.getElementById("nav-current-id");
    }
}

//Start

NavButtons = new NavButtons();
let back, next, counter = 0;

const storeIds = sessionStorage.getItem("storeIds");

if (storeIds) {
    const arrayIds = storeIds.split("&");
    arrayIds.forEach((item) => {
        if (item === NavButtons.CurrentId.value) {
            back = arrayIds[counter - 1];
            next = arrayIds[counter + 1];
        }
        counter++;
    });
}

if (back) {
    NavButtons.DivBack.style.display = "";
    NavButtons.ButtonBack.addEventListener('click', () => {
        ActionShowModal(true);
        const href = `/workplace/store/details?id=${back}`;
        window.location.href = href;
    });
} else {
    NavButtons.DivBack.style.display = "none";
}

if (next) {
    NavButtons.DivNext.style.display = "";
    NavButtons.ButtonNext.addEventListener('click', () => {
        ActionShowModal(true);
        const href = `/workplace/store/details?id=${next}`;
        window.location.href = href;
    });
} else {
    NavButtons.DivNext.style.display = "none";
}


NavButtons.ButtonMain.addEventListener("click", () => {
    NavButtons.MiniButons.forEach((item) => {
        item.classList.toggle("mdc-fab--exited");
        NavButtons.NavModal.classList.toggle("mtd-main-display-none");
    });

    NavButtons.ButtonMain.querySelectorAll(".mdc-fab__icon").forEach((item) => {
        item.classList.toggle("mtd-main-display-none");
    });
});

NavButtons.MiniButons.forEach((item) => {
    item.addEventListener("mouseover", () => {
        const label = item.querySelector(".mdc-fab__label");
        item.classList.toggle("mdc-fab--extended");
        label.classList.toggle("mtd-main-display-none");
    });
    item.addEventListener("mouseout", () => {
        const label = item.querySelector(".mdc-fab__label");
        item.classList.toggle("mdc-fab--extended");
        label.classList.toggle("mtd-main-display-none");
    });
});

