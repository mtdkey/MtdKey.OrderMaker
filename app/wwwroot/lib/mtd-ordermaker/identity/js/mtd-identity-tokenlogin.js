window.addEventListener('load',async function () {
    var action = window.location.protocol + "//" + window.location.host + window.location.pathname;
    var formData = CreateFormData(formToken);
    var response = await fetch(action, { method: "POST", body: formData });    
    const result = await response.text();
    waitBlock.style.display = "none";

    if (!response.ok) {        
        errorBlock.style.display = "block";
        errorBlock.innerText = result;
        return;
    }

    completeBlock.style.display = "block";

})