
function createRipple(e) {
    let ripples = document.createElement('span');
    ripples.style.left = e.offsetX + 'px';
    ripples.style.top = e.offsetY + 'px';
    ripples.classList.add("ripple");
    e.currentTarget.appendChild(ripples);
    setTimeout(() => {
        ripples.remove();
    }, 1000);
}
function getElemPos(elem) {
    var xPos = 0, yPos = 0;
    while (elem) {
        xPos += (elem.offsetLeft - elem.scrollLeft + elem.clientLeft);
        yPos += (elem.offsetTop - elem.scrollTop + elem.clientTop);
        elem = elem.offsetParent;
    }
    return { x: xPos, y: yPos };
}
function addRippleEffect(root) {
    root.querySelectorAll(".btn, .card-img-ripple").forEach(btn => {
        btn.addEventListener("click", createRipple);
    });
}

addRippleEffect(document.body);
