const MtdCheckBoxClick = (id) => {    
    const icon = document.getElementById(`${id}-mtd-checkbox-icon`);
    const input = document.getElementById(`${id}-mtd-checkbox-input`);
    
    input.checked = !input.checked;
    icon.className = input.checked ? "mtd-checkbox-icon-on" : "mtd-checkbox-icon-off";
}