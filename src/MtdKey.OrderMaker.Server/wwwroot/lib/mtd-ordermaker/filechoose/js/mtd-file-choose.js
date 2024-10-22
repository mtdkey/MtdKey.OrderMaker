/*
    MTD OrderMaker - http://mtdkey.com
    Copyright (c) 2019 Oleg Bruev <job4bruev@gmail.com>. All rights reserved.
*/

//Start

    document.querySelectorAll("div[mtd-file-upload]").forEach((block) => {
        const id = block.getAttribute("mtd-file-upload");
        const result = document.getElementById(`${id}-file-upload-result`);
        const resultText = result.getAttribute("mtd-file-upload-text");
        const input = document.getElementById(`${id}-file-upload-input`);                
        const cancel = document.getElementById(`${id}-file-upload-cancel`);

        input.addEventListener("change", () => {
            var filename = input.value;
            if (/^\s*$/.test(filename)) {
                block.classList.remove('active');
                result.innerText = resultText;
            }
            else {
                block.classList.add('active');
                result.innerText = filename.replace("C:\\fakepath\\", "");
            }
        });

        cancel.addEventListener('click', () => {
            block.classList.remove('active');
            result.innerText = resultText;
            input.value = "";
            cancel.blur();
        });
        
    });