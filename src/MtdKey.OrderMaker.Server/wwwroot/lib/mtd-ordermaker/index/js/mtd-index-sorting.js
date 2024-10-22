var btns = document.querySelectorAll("button[data-sort]");

btns.forEach(btn => {
    btn.addEventListener("click", () => {        
        clickerSort.value = btn.dataset.sort;
        indexOrder.value = btn.dataset.order;
        clickerSort.click();
    });
});