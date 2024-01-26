function createRipple(e : any) {

    let ripples = document.createElement('span');
    ripples.style.left = e.offsetX + 'px';
    ripples.style.top = e.offsetY + 'px';
    ripples.classList.add("ripple");
    e.currentTarget.appendChild(ripples);

    setTimeout(() => {
        ripples.remove()
    }, 1000);
}

function getElemPos(elem: any) {
    var xPos = 0,
        yPos = 0;

    while (elem) {
        xPos += (elem.offsetLeft - elem.scrollLeft + elem.clientLeft);
        yPos += (elem.offsetTop - elem.scrollTop + elem.clientTop);
        elem = elem.offsetParent;
    }

    return { x: xPos, y: yPos };
}


export function addRippleEffect(root: HTMLDivElement | HTMLBodyElement) {
    root.querySelectorAll(".btn").forEach(btn => {
        btn.addEventListener("click", createRipple);
    });
}