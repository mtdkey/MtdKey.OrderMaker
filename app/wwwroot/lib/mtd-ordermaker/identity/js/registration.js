
var acc = document.getElementsByClassName("mtd-settings-item");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var data = this.querySelector(".mtd-settings-chevron").querySelector(".bi");
        
        if (this.classList.contains('active')) {
            data.classList.replace('bi-chevron-down', 'bi-chevron-up');
        } else {
            data.classList.replace('bi-chevron-up', 'bi-chevron-down');
        }

        var panel = this.nextElementSibling;
        if (panel.style.maxHeight) {
            panel.style.maxHeight = null;
        } else {
            panel.style.maxHeight = panel.scrollHeight + "px";
        }
    });
}