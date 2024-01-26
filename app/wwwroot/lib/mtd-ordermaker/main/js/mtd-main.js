class LocaleDTP {
    constructor() {
        this.lang = document.getElementById("index-dtp-locale-lang").value;
        this.mask = document.getElementById("index-dtp-locale-mask").value;
        this.format = document.getElementById("index-dtp-locale-format").value;
    }
}

function fireEvent(node, eventName) {

    var doc;
    if (node.ownerDocument) {
        doc = node.ownerDocument;
    } else if (node.nodeType == 9) {
        doc = node;
    } else {
        throw new Error("Invalid node passed to fireEvent: " + node.id);
    }

    if (node.dispatchEvent) {
        var eventClass = "";
        switch (eventName) {
            case "click":
            case "mousedown":
            case "mouseup":
                eventClass = "MouseEvents";
                break;

            case "focus":
            case "change":
            case "blur":
            case "select":
                eventClass = "HTMLEvents";
                break;

            default:
                throw "fireEvent: Couldn't find an event class for event '" + eventName + "'.";
                break;
        }
        var event = doc.createEvent(eventClass);

        var bubbles = eventName == "change" ? false : true;
        event.initEvent(eventName, bubbles, true);

        event.synthetic = true;
        node.dispatchEvent(event, true);
    } else if (node.fireEvent) {

        var event = doc.createEventObject();
        event.synthetic = true;
        node.fireEvent("on" + eventName, event);
    }
};

const snackbar = new mdc.snackbar.MDCSnackbar(document.getElementById('main-snack'));

const MainShowSnackBar = (message, error = false) => {
    const div = document.getElementById('main-snack');
    div.classList.add("mdc-snackbar--open");
    snackbar.labelText = message;
    if (error) {
        snackbar.timeoutMs = 10000;
        const surface = div.querySelector(".mdc-snackbar__surface");
        surface.style.backgroundColor = "darkred";
    }
    snackbar.open();
}

const newGuid = () => {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

const rippleFor = (className) => {
    const ripples = document.querySelectorAll(className);
    ripples.forEach((obj) => {
        const ripple = new mdc.ripple.MDCRipple(obj);
        ripple.unbounded = true;
    });
    return true;
}

const ListenerForDataHref = () => {
    const items = document.querySelectorAll('[mtd-data-url]');
    items.forEach((item) => {
        item.addEventListener('click', () => {
            ActionShowModal(true);

            let href = item.getAttribute('mtd-data-url');
            href = window.location.origin + appName.value + href;
            const target = item.getAttribute('mtd_data_href-target');

            if (target) {
                setTimeout(() => { window.open(href, target); }, 500);
                ActionShowModal();
            } else {
                setTimeout(() => { window.location.href = href; }, 500);
            }
        });
    });
}

const CreateFormData = (form) => {

    var formData = new FormData();
    for (var i = 0; i < form.length; i++) {

        if (form[i].getAttribute("type") == 'checkbox') {
            formData.append(form[i].name, form[i].checked);
        } else {
            formData.append(form[i].name, form[i].value);
        }

        if (form[i].files && form[i].files.length > 0) {
            formData.append(form[i].name, form[i].files[0], form[i].files[0].name);
        }
    }

    return formData;
}

const ListenerClickerBy = () => {

    const clickers = document.querySelectorAll('[mtd-data-clicker-by]');

    clickers.forEach((clicker) => {
        const targetId = clicker.getAttribute('mtd-data-clicker-by');
        clicker.addEventListener("click", () => {
            document.getElementById(targetId).click();
        })
    });
}

const ListenerForPostData = () => {

    const forms = document.querySelectorAll("form[mtd-data-form]");
    forms.forEach((form) => {

        const result = form.querySelector('input[mtd-data-result]');
        let action = form.getAttribute('action');
        if (!action) { action = ""; }
        const clickers = form.querySelectorAll('[mtd-data-clicker]');
        const inputs = form.querySelectorAll('input[mtd-input-clicker]');

        clickers.forEach((clicker) => {

            const value = clicker.getAttribute('mtd-data-clicker');
            const location = clicker.getAttribute('mtd-data-location');
            const message = clicker.getAttribute('mtd-data-message');

            clicker.addEventListener('click', () => {

                const validate = form.reportValidity();
                if (!validate) {
                    return false;
                }


                ActionShowModal(true);
                if (value) { result.value = value; }

                const formData = CreateFormData(form);

                const xmlHttp = new XMLHttpRequest();
                xmlHttp.responseType = 'json';
                xmlHttp.open("post", action, true);
                xmlHttp.send(formData);
                xmlHttp.onreadystatechange = function () {

                    if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                        //document.location.reload(location.hash, '');
                        if (message) { sessionStorage.setItem("Message", message); }

                        if (location) {
                            document.location = location;
                        } else {
                            document.location.reload();
                        }
                    }

                    if (xmlHttp.status == 400) {
                        setTimeout(() => { ActionShowModal(); MainShowSnackBar(xmlHttp.response.value, true); }, 1000);
                    }

                    if (xmlHttp.status == 500) {
                        setTimeout(() => { ActionShowModal(); MainShowSnackBar("500 Internal Server Error", true); }, 1000);
                    }
                }
            });
        });

        inputs.forEach((input) => {
            const location = input.getAttribute('mtd-data-location');
            const message = input.getAttribute('mtd-data-message');
            input.addEventListener('keydown', (e) => {
                if (e.keyCode === 13) {

                    const validate = form.reportValidity();
                    if (!validate) {
                        return false;
                    }

                    e.preventDefault();
                    ActionShowModal(true);
                    var xmlHttp = new XMLHttpRequest();
                    xmlHttp.responseType = 'json';
                    xmlHttp.open("post", action, true);

                    var formData = CreateFormData(form);

                    xmlHttp.send(formData);
                    xmlHttp.onreadystatechange = function () {
                        if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
                            //document.location.reload(location.hash, '');

                            if (message) { sessionStorage.setItem("Message", message); }

                            if (location) {
                                document.location = location;
                            } else {
                                document.location.reload();
                            }
                        }

                        if (xmlHttp.status == 400) {
                            setTimeout(() => { ActionShowModal(); MainShowSnackBar(xmlHttp.response.value, true); }, 1000);
                        }

                        if (xmlHttp.status == 500) {
                            setTimeout(() => { ActionShowModal(); MainShowSnackBar("500 Internal Server Error", true); }, 1000);
                        }
                    }
                }
            });
        });

    });
}

const ActionShowModal = (show) => {
    const style = show ? 'block' : 'none';
    document.getElementById('mainModal').style.display = style;
}

const ListenerForAction = () => {

    const button = document.getElementById('main-action-button');
    const action = document.getElementById('main-action-menu');
    if (action) {
        const menu = new mdc.menu.MDCMenu(action);
        menu.setFixedPosition(true);
        button.addEventListener('click', () => { menu.open = true; })
    }
}

const DetectMobile = () => {

    if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/webOS/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/iPod/i)
        || navigator.userAgent.match(/BlackBerry/i)
        || navigator.userAgent.match(/Windows Phone/i)
    ) {
        document.body.style.height = "";
    }
}


//Start
DetectMobile();

if (sessionStorage.getItem("Message")) {

    MainShowSnackBar(sessionStorage.getItem("Message"));
    sessionStorage.removeItem("Message");
}

if (sessionStorage.getItem("ErrorMessage")) {

    MainShowSnackBar(sessionStorage.getItem("ErrorMessage"), true);
    sessionStorage.removeItem("ErrorMessage");
}

ListenerForAction();

rippleFor('.mdc-checkbox');
rippleFor('.mdc-icon-button');
rippleFor('.mdc-list-item');
rippleFor('.mdc-fab');
rippleFor('.mdc-button');
rippleFor('.mdc-card__primary-action');

const toggleButtons = document.querySelectorAll('.mdc-icon-button');
toggleButtons.forEach((item) => {
    new mdc.iconButton.MDCIconButtonToggle(item);
});


const drawer = mdc.drawer.MDCDrawer.attachTo(document.querySelector('.mdc-drawer'));
const topAppBar = mdc.topAppBar.MDCTopAppBar.attachTo(document.getElementById('app-bar'));

topAppBar.listen('MDCTopAppBar:nav', () => {
    drawer.open = !drawer.open;
});

const url = window.location.href.toLowerCase();
var mainMenuItems = document.querySelectorAll("a[name='mainMenuItem']");
var notSelected = true;
var cssActivated = "mdc-list-item--activated";

mainMenuItems.forEach((item, index) => {
    
    if (url.includes(item.href.toLowerCase()) && index !== 0) {

        item.classList.add(cssActivated);
        notSelected = false;
    }
    
});

if (notSelected) { mainMenuItems[0].classList.add(cssActivated); }

ListenerForDataHref();
ListenerForPostData();
ListenerClickerBy();

const mainUserButton = document.getElementById('main-user-button');
if (mainUserButton) {
    const mainUserMenu = new mdc.menuSurface.MDCMenuSurface(document.getElementById('main-user-menu'));

    mainUserMenu.setFixedPosition(true);
    mainUserButton.addEventListener('click', () => {
        mainUserMenu.open();
    });
}

const mainAppsButton = document.getElementById('main-apps-button');
if (mainAppsButton) {
    const mainUserMenu = new mdc.menuSurface.MDCMenuSurface(document.getElementById('main-apps-menu'));

    mainUserMenu.setFixedPosition(true);
    mainAppsButton.addEventListener('click', () => {
        mainUserMenu.open();
    });
}
