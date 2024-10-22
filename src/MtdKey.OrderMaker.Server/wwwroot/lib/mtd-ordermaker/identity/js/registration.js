

var apiToken = new MTDTextField("apiKey");
var policy = new MTDSelectList("policyId");
var group = new MTDSelectList("groupId");


function GenerateStringBase64Token() {
    apiToken.textField.value = [..."abcdefghijklmnopqrsuvwxyz0123456789"].map((e, i, a) => a[Math.floor(Math.random() * a.length)]).join('');
    
}


//function OnChangePolicy() {
//    policyName.innerText = policy.selector.selectedText.innerText;
//    policyTitle.innerText = policy.selector.selectedText.innerText;
//}

