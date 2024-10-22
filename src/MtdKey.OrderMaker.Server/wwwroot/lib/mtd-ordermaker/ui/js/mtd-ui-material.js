
const autosize = (el) => {
    setTimeout(function () {
        el.style.cssText = 'height:auto; padding:0;';
        el.style.cssText = 'height:' + el.scrollHeight + 'px';
    }, 0);
}

const runMemoResize = () => {
    const tagName = "mtdMemo";
    const items = document.querySelectorAll(`textarea[${tagName}]`);
    items.forEach((item) => {
        autosize(item);
        item.addEventListener('keydown', (event) => {
            if (event.keyCode) {
                autosize(item);
            }
        });
    });
}

//Start

    const selector = '.mdc-card__primary-action';    
    const ripples = [].map.call(document.querySelectorAll(selector), function (el) {
        return new mdc.ripple.MDCRipple(el);
    });

    runMemoResize();


