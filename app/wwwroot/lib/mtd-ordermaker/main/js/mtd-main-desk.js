const clickBlockFullScreen = (el) => {

    const id = el.getAttribute('data');
    const content = document.getElementById(id);

    content.classList.toggle('mtd-desk-block-content--full');

    if (content.classList.contains('mtd-desk-block-content--full')) {
        setTimeout(() => BodyShowHide(content), 500);
    } else {
        BodyShowHide(content);
    }    
    
}

const clickBlockToogle = (el) => {
    const id = el.getAttribute('data');
    const content = document.getElementById(id);
    content.classList.toggle('mtd-desk-block-content--colapsed');    
}

const BodyShowHide = (content) => {
    content.querySelector(".mtd-desk-block-body--hidden").classList.toggle("mtd-main-display-none");
}

const fabs = document.querySelectorAll(".mtd-fab");
if (fabs) {
    fabs.forEach((fab) => {
       
        fab.addEventListener("mouseover", () => {
            fab.querySelector(".mdc-fab__label").classList.toggle("mtd-main-display-none");
            fab.classList.toggle("mdc-fab--extended");
        });
        fab.addEventListener("mouseout", () => {
            fab.classList.toggle("mdc-fab--extended");
            fab.querySelector(".mdc-fab__label").classList.toggle("mtd-main-display-none");
        });
    });
}

function delay() {

    return new Promise(resolve => setTimeout(resolve, 20));
}

async function clickBlockDetail (el) {

    const checked = el.getAttribute('aria-checked');
    
    const tableId = el.getAttribute('data');
    const table = document.getElementById(tableId);

    if (checked === 'false') {

        const rows = table.tBodies[0].rows;
        for (let tr of rows) {

            tr.style.display = "";     
            await delay();                          
        }

        el.setAttribute('aria-checked','true');
    }

    if (checked === 'true') {
        let counter = 0;
        const rows = table.tBodies[0].rows;
        for (let tr of rows) {
            counter++;

            tr.style.display = rows.length - counter > 1 ? "none" : "";
            await delay();
        }

        el.setAttribute('aria-checked', 'false');
    }
    
}