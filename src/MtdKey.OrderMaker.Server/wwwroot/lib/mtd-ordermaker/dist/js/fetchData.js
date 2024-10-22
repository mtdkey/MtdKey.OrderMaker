var OrderMaker;
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./node_modules/@material/base/component.js":
/*!**************************************************!*\
  !*** ./node_modules/@material/base/component.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   MDCComponent: () => (/* binding */ MDCComponent),
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _foundation__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./foundation */ "./node_modules/@material/base/foundation.js");
/**
 * @license
 * Copyright 2016 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


var MDCComponent = /** @class */ (function () {
    function MDCComponent(root, foundation) {
        var args = [];
        for (var _i = 2; _i < arguments.length; _i++) {
            args[_i - 2] = arguments[_i];
        }
        this.root = root;
        this.initialize.apply(this, (0,tslib__WEBPACK_IMPORTED_MODULE_0__.__spread)(args));
        // Note that we initialize foundation here and not within the constructor's default param so that
        // this.root_ is defined and can be used within the foundation class.
        this.foundation =
            foundation === undefined ? this.getDefaultFoundation() : foundation;
        this.foundation.init();
        this.initialSyncWithDOM();
    }
    MDCComponent.attachTo = function (root) {
        // Subclasses which extend MDCBase should provide an attachTo() method that takes a root element and
        // returns an instantiated component with its root set to that element. Also note that in the cases of
        // subclasses, an explicit foundation class will not have to be passed in; it will simply be initialized
        // from getDefaultFoundation().
        return new MDCComponent(root, new _foundation__WEBPACK_IMPORTED_MODULE_1__.MDCFoundation({}));
    };
    /* istanbul ignore next: method param only exists for typing purposes; it does not need to be unit tested */
    MDCComponent.prototype.initialize = function () {
        var _args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            _args[_i] = arguments[_i];
        }
        // Subclasses can override this to do any additional setup work that would be considered part of a
        // "constructor". Essentially, it is a hook into the parent constructor before the foundation is
        // initialized. Any additional arguments besides root and foundation will be passed in here.
    };
    MDCComponent.prototype.getDefaultFoundation = function () {
        // Subclasses must override this method to return a properly configured foundation class for the
        // component.
        throw new Error('Subclasses must override getDefaultFoundation to return a properly configured ' +
            'foundation class');
    };
    MDCComponent.prototype.initialSyncWithDOM = function () {
        // Subclasses should override this method if they need to perform work to synchronize with a host DOM
        // object. An example of this would be a form control wrapper that needs to synchronize its internal state
        // to some property or attribute of the host DOM. Please note: this is *not* the place to perform DOM
        // reads/writes that would cause layout / paint, as this is called synchronously from within the constructor.
    };
    MDCComponent.prototype.destroy = function () {
        // Subclasses may implement this method to release any resources / deregister any listeners they have
        // attached. An example of this might be deregistering a resize event from the window object.
        this.foundation.destroy();
    };
    MDCComponent.prototype.listen = function (evtType, handler, options) {
        this.root.addEventListener(evtType, handler, options);
    };
    MDCComponent.prototype.unlisten = function (evtType, handler, options) {
        this.root.removeEventListener(evtType, handler, options);
    };
    /**
     * Fires a cross-browser-compatible custom event from the component root of the given type, with the given data.
     */
    MDCComponent.prototype.emit = function (evtType, evtData, shouldBubble) {
        if (shouldBubble === void 0) { shouldBubble = false; }
        var evt;
        if (typeof CustomEvent === 'function') {
            evt = new CustomEvent(evtType, {
                bubbles: shouldBubble,
                detail: evtData,
            });
        }
        else {
            evt = document.createEvent('CustomEvent');
            evt.initCustomEvent(evtType, shouldBubble, false, evtData);
        }
        this.root.dispatchEvent(evt);
    };
    return MDCComponent;
}());

// tslint:disable-next-line:no-default-export Needed for backward compatibility with MDC Web v0.44.0 and earlier.
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (MDCComponent);
//# sourceMappingURL=component.js.map

/***/ }),

/***/ "./node_modules/@material/base/foundation.js":
/*!***************************************************!*\
  !*** ./node_modules/@material/base/foundation.js ***!
  \***************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   MDCFoundation: () => (/* binding */ MDCFoundation),
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/**
 * @license
 * Copyright 2016 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
var MDCFoundation = /** @class */ (function () {
    function MDCFoundation(adapter) {
        if (adapter === void 0) { adapter = {}; }
        this.adapter = adapter;
    }
    Object.defineProperty(MDCFoundation, "cssClasses", {
        get: function () {
            // Classes extending MDCFoundation should implement this method to return an object which exports every
            // CSS class the foundation class needs as a property. e.g. {ACTIVE: 'mdc-component--active'}
            return {};
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCFoundation, "strings", {
        get: function () {
            // Classes extending MDCFoundation should implement this method to return an object which exports all
            // semantic strings as constants. e.g. {ARIA_ROLE: 'tablist'}
            return {};
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCFoundation, "numbers", {
        get: function () {
            // Classes extending MDCFoundation should implement this method to return an object which exports all
            // of its semantic numbers as constants. e.g. {ANIMATION_DELAY_MS: 350}
            return {};
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCFoundation, "defaultAdapter", {
        get: function () {
            // Classes extending MDCFoundation may choose to implement this getter in order to provide a convenient
            // way of viewing the necessary methods of an adapter. In the future, this could also be used for adapter
            // validation.
            return {};
        },
        enumerable: true,
        configurable: true
    });
    MDCFoundation.prototype.init = function () {
        // Subclasses should override this method to perform initialization routines (registering events, etc.)
    };
    MDCFoundation.prototype.destroy = function () {
        // Subclasses should override this method to perform de-initialization routines (de-registering events, etc.)
    };
    return MDCFoundation;
}());

// tslint:disable-next-line:no-default-export Needed for backward compatibility with MDC Web v0.44.0 and earlier.
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (MDCFoundation);
//# sourceMappingURL=foundation.js.map

/***/ }),

/***/ "./node_modules/@material/dom/ponyfill.js":
/*!************************************************!*\
  !*** ./node_modules/@material/dom/ponyfill.js ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   closest: () => (/* binding */ closest),
/* harmony export */   estimateScrollWidth: () => (/* binding */ estimateScrollWidth),
/* harmony export */   matches: () => (/* binding */ matches)
/* harmony export */ });
/**
 * @license
 * Copyright 2018 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
/**
 * @fileoverview A "ponyfill" is a polyfill that doesn't modify the global prototype chain.
 * This makes ponyfills safer than traditional polyfills, especially for libraries like MDC.
 */
function closest(element, selector) {
    if (element.closest) {
        return element.closest(selector);
    }
    var el = element;
    while (el) {
        if (matches(el, selector)) {
            return el;
        }
        el = el.parentElement;
    }
    return null;
}
function matches(element, selector) {
    var nativeMatches = element.matches
        || element.webkitMatchesSelector
        || element.msMatchesSelector;
    return nativeMatches.call(element, selector);
}
/**
 * Used to compute the estimated scroll width of elements. When an element is
 * hidden due to display: none; being applied to a parent element, the width is
 * returned as 0. However, the element will have a true width once no longer
 * inside a display: none context. This method computes an estimated width when
 * the element is hidden or returns the true width when the element is visble.
 * @param {Element} element the element whose width to estimate
 */
function estimateScrollWidth(element) {
    // Check the offsetParent. If the element inherits display: none from any
    // parent, the offsetParent property will be null (see
    // https://developer.mozilla.org/en-US/docs/Web/API/HTMLElement/offsetParent).
    // This check ensures we only clone the node when necessary.
    var htmlEl = element;
    if (htmlEl.offsetParent !== null) {
        return htmlEl.scrollWidth;
    }
    var clone = htmlEl.cloneNode(true);
    clone.style.setProperty('position', 'absolute');
    clone.style.setProperty('transform', 'translate(-9999px, -9999px)');
    document.documentElement.appendChild(clone);
    var scrollWidth = clone.scrollWidth;
    document.documentElement.removeChild(clone);
    return scrollWidth;
}
//# sourceMappingURL=ponyfill.js.map

/***/ }),

/***/ "./node_modules/@material/snackbar/component.js":
/*!******************************************************!*\
  !*** ./node_modules/@material/snackbar/component.js ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   MDCSnackbar: () => (/* binding */ MDCSnackbar)
/* harmony export */ });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _material_base_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @material/base/component */ "./node_modules/@material/base/component.js");
/* harmony import */ var _material_dom_ponyfill__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @material/dom/ponyfill */ "./node_modules/@material/dom/ponyfill.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./constants */ "./node_modules/@material/snackbar/constants.js");
/* harmony import */ var _foundation__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./foundation */ "./node_modules/@material/snackbar/foundation.js");
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./util */ "./node_modules/@material/snackbar/util.js");
/**
 * @license
 * Copyright 2018 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */






var SURFACE_SELECTOR = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.SURFACE_SELECTOR, LABEL_SELECTOR = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.LABEL_SELECTOR, ACTION_SELECTOR = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.ACTION_SELECTOR, DISMISS_SELECTOR = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.DISMISS_SELECTOR, OPENING_EVENT = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.OPENING_EVENT, OPENED_EVENT = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.OPENED_EVENT, CLOSING_EVENT = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.CLOSING_EVENT, CLOSED_EVENT = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.CLOSED_EVENT;
var MDCSnackbar = /** @class */ (function (_super) {
    (0,tslib__WEBPACK_IMPORTED_MODULE_1__.__extends)(MDCSnackbar, _super);
    function MDCSnackbar() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MDCSnackbar.attachTo = function (root) {
        return new MDCSnackbar(root);
    };
    MDCSnackbar.prototype.initialize = function (announcerFactory) {
        if (announcerFactory === void 0) { announcerFactory = function () { return _util__WEBPACK_IMPORTED_MODULE_2__.announce; }; }
        this.announce_ = announcerFactory();
    };
    MDCSnackbar.prototype.initialSyncWithDOM = function () {
        var _this = this;
        this.surfaceEl_ = this.root.querySelector(SURFACE_SELECTOR);
        this.labelEl_ = this.root.querySelector(LABEL_SELECTOR);
        this.actionEl_ = this.root.querySelector(ACTION_SELECTOR);
        this.handleKeyDown_ = function (evt) { return _this.foundation.handleKeyDown(evt); };
        this.handleSurfaceClick_ = function (evt) {
            var target = evt.target;
            if (_this.isActionButton_(target)) {
                _this.foundation.handleActionButtonClick(evt);
            }
            else if (_this.isActionIcon_(target)) {
                _this.foundation.handleActionIconClick(evt);
            }
        };
        this.registerKeyDownHandler_(this.handleKeyDown_);
        this.registerSurfaceClickHandler_(this.handleSurfaceClick_);
    };
    MDCSnackbar.prototype.destroy = function () {
        _super.prototype.destroy.call(this);
        this.deregisterKeyDownHandler_(this.handleKeyDown_);
        this.deregisterSurfaceClickHandler_(this.handleSurfaceClick_);
    };
    MDCSnackbar.prototype.open = function () {
        this.foundation.open();
    };
    /**
     * @param reason Why the snackbar was closed. Value will be passed to CLOSING_EVENT and CLOSED_EVENT via the
     *     `event.detail.reason` property. Standard values are REASON_ACTION and REASON_DISMISS, but custom
     *     client-specific values may also be used if desired.
     */
    MDCSnackbar.prototype.close = function (reason) {
        if (reason === void 0) { reason = ''; }
        this.foundation.close(reason);
    };
    MDCSnackbar.prototype.getDefaultFoundation = function () {
        var _this = this;
        // DO NOT INLINE this variable. For backward compatibility, foundations take a Partial<MDCFooAdapter>.
        // To ensure we don't accidentally omit any methods, we need a separate, strongly typed adapter variable.
        var adapter = {
            addClass: function (className) { return _this.root.classList.add(className); },
            announce: function () { return _this.announce_(_this.labelEl_); },
            notifyClosed: function (reason) { return _this.emit(CLOSED_EVENT, reason ? { reason: reason } : {}); },
            notifyClosing: function (reason) { return _this.emit(CLOSING_EVENT, reason ? { reason: reason } : {}); },
            notifyOpened: function () { return _this.emit(OPENED_EVENT, {}); },
            notifyOpening: function () { return _this.emit(OPENING_EVENT, {}); },
            removeClass: function (className) { return _this.root.classList.remove(className); },
        };
        return new _foundation__WEBPACK_IMPORTED_MODULE_3__.MDCSnackbarFoundation(adapter);
    };
    Object.defineProperty(MDCSnackbar.prototype, "timeoutMs", {
        get: function () {
            return this.foundation.getTimeoutMs();
        },
        set: function (timeoutMs) {
            this.foundation.setTimeoutMs(timeoutMs);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbar.prototype, "closeOnEscape", {
        get: function () {
            return this.foundation.getCloseOnEscape();
        },
        set: function (closeOnEscape) {
            this.foundation.setCloseOnEscape(closeOnEscape);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbar.prototype, "isOpen", {
        get: function () {
            return this.foundation.isOpen();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbar.prototype, "labelText", {
        get: function () {
            // This property only returns null if the node is a document, DOCTYPE, or notation.
            // On Element nodes, it always returns a string.
            return this.labelEl_.textContent;
        },
        set: function (labelText) {
            this.labelEl_.textContent = labelText;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbar.prototype, "actionButtonText", {
        get: function () {
            return this.actionEl_.textContent;
        },
        set: function (actionButtonText) {
            this.actionEl_.textContent = actionButtonText;
        },
        enumerable: true,
        configurable: true
    });
    MDCSnackbar.prototype.registerKeyDownHandler_ = function (handler) {
        this.listen('keydown', handler);
    };
    MDCSnackbar.prototype.deregisterKeyDownHandler_ = function (handler) {
        this.unlisten('keydown', handler);
    };
    MDCSnackbar.prototype.registerSurfaceClickHandler_ = function (handler) {
        this.surfaceEl_.addEventListener('click', handler);
    };
    MDCSnackbar.prototype.deregisterSurfaceClickHandler_ = function (handler) {
        this.surfaceEl_.removeEventListener('click', handler);
    };
    MDCSnackbar.prototype.isActionButton_ = function (target) {
        return Boolean((0,_material_dom_ponyfill__WEBPACK_IMPORTED_MODULE_4__.closest)(target, ACTION_SELECTOR));
    };
    MDCSnackbar.prototype.isActionIcon_ = function (target) {
        return Boolean((0,_material_dom_ponyfill__WEBPACK_IMPORTED_MODULE_4__.closest)(target, DISMISS_SELECTOR));
    };
    return MDCSnackbar;
}(_material_base_component__WEBPACK_IMPORTED_MODULE_5__.MDCComponent));

//# sourceMappingURL=component.js.map

/***/ }),

/***/ "./node_modules/@material/snackbar/constants.js":
/*!******************************************************!*\
  !*** ./node_modules/@material/snackbar/constants.js ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   cssClasses: () => (/* binding */ cssClasses),
/* harmony export */   numbers: () => (/* binding */ numbers),
/* harmony export */   strings: () => (/* binding */ strings)
/* harmony export */ });
/**
 * @license
 * Copyright 2018 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
var cssClasses = {
    CLOSING: 'mdc-snackbar--closing',
    OPEN: 'mdc-snackbar--open',
    OPENING: 'mdc-snackbar--opening',
};
var strings = {
    ACTION_SELECTOR: '.mdc-snackbar__action',
    ARIA_LIVE_LABEL_TEXT_ATTR: 'data-mdc-snackbar-label-text',
    CLOSED_EVENT: 'MDCSnackbar:closed',
    CLOSING_EVENT: 'MDCSnackbar:closing',
    DISMISS_SELECTOR: '.mdc-snackbar__dismiss',
    LABEL_SELECTOR: '.mdc-snackbar__label',
    OPENED_EVENT: 'MDCSnackbar:opened',
    OPENING_EVENT: 'MDCSnackbar:opening',
    REASON_ACTION: 'action',
    REASON_DISMISS: 'dismiss',
    SURFACE_SELECTOR: '.mdc-snackbar__surface',
};
var numbers = {
    DEFAULT_AUTO_DISMISS_TIMEOUT_MS: 5000,
    INDETERMINATE: -1,
    MAX_AUTO_DISMISS_TIMEOUT_MS: 10000,
    MIN_AUTO_DISMISS_TIMEOUT_MS: 4000,
    // These variables need to be kept in sync with the values in _variables.scss.
    SNACKBAR_ANIMATION_CLOSE_TIME_MS: 75,
    SNACKBAR_ANIMATION_OPEN_TIME_MS: 150,
    /**
     * Number of milliseconds to wait between temporarily clearing the label text
     * in the DOM and subsequently restoring it. This is necessary to force IE 11
     * to pick up the `aria-live` content change and announce it to the user.
     */
    ARIA_LIVE_DELAY_MS: 1000,
};

//# sourceMappingURL=constants.js.map

/***/ }),

/***/ "./node_modules/@material/snackbar/foundation.js":
/*!*******************************************************!*\
  !*** ./node_modules/@material/snackbar/foundation.js ***!
  \*******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   MDCSnackbarFoundation: () => (/* binding */ MDCSnackbarFoundation),
/* harmony export */   "default": () => (__WEBPACK_DEFAULT_EXPORT__)
/* harmony export */ });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _material_base_foundation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @material/base/foundation */ "./node_modules/@material/base/foundation.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./constants */ "./node_modules/@material/snackbar/constants.js");
/**
 * @license
 * Copyright 2018 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */



var OPENING = _constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.OPENING, OPEN = _constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.OPEN, CLOSING = _constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.CLOSING;
var REASON_ACTION = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.REASON_ACTION, REASON_DISMISS = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.REASON_DISMISS;
var MDCSnackbarFoundation = /** @class */ (function (_super) {
    (0,tslib__WEBPACK_IMPORTED_MODULE_1__.__extends)(MDCSnackbarFoundation, _super);
    function MDCSnackbarFoundation(adapter) {
        var _this = _super.call(this, (0,tslib__WEBPACK_IMPORTED_MODULE_1__.__assign)((0,tslib__WEBPACK_IMPORTED_MODULE_1__.__assign)({}, MDCSnackbarFoundation.defaultAdapter), adapter)) || this;
        _this.isOpen_ = false;
        _this.animationFrame_ = 0;
        _this.animationTimer_ = 0;
        _this.autoDismissTimer_ = 0;
        _this.autoDismissTimeoutMs_ = _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.DEFAULT_AUTO_DISMISS_TIMEOUT_MS;
        _this.closeOnEscape_ = true;
        return _this;
    }
    Object.defineProperty(MDCSnackbarFoundation, "cssClasses", {
        get: function () {
            return _constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbarFoundation, "strings", {
        get: function () {
            return _constants__WEBPACK_IMPORTED_MODULE_0__.strings;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbarFoundation, "numbers", {
        get: function () {
            return _constants__WEBPACK_IMPORTED_MODULE_0__.numbers;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MDCSnackbarFoundation, "defaultAdapter", {
        get: function () {
            return {
                addClass: function () { return undefined; },
                announce: function () { return undefined; },
                notifyClosed: function () { return undefined; },
                notifyClosing: function () { return undefined; },
                notifyOpened: function () { return undefined; },
                notifyOpening: function () { return undefined; },
                removeClass: function () { return undefined; },
            };
        },
        enumerable: true,
        configurable: true
    });
    MDCSnackbarFoundation.prototype.destroy = function () {
        this.clearAutoDismissTimer_();
        cancelAnimationFrame(this.animationFrame_);
        this.animationFrame_ = 0;
        clearTimeout(this.animationTimer_);
        this.animationTimer_ = 0;
        this.adapter.removeClass(OPENING);
        this.adapter.removeClass(OPEN);
        this.adapter.removeClass(CLOSING);
    };
    MDCSnackbarFoundation.prototype.open = function () {
        var _this = this;
        this.clearAutoDismissTimer_();
        this.isOpen_ = true;
        this.adapter.notifyOpening();
        this.adapter.removeClass(CLOSING);
        this.adapter.addClass(OPENING);
        this.adapter.announce();
        // Wait a frame once display is no longer "none", to establish basis for animation
        this.runNextAnimationFrame_(function () {
            _this.adapter.addClass(OPEN);
            _this.animationTimer_ = setTimeout(function () {
                var timeoutMs = _this.getTimeoutMs();
                _this.handleAnimationTimerEnd_();
                _this.adapter.notifyOpened();
                if (timeoutMs !== _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.INDETERMINATE) {
                    _this.autoDismissTimer_ = setTimeout(function () {
                        _this.close(REASON_DISMISS);
                    }, timeoutMs);
                }
            }, _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.SNACKBAR_ANIMATION_OPEN_TIME_MS);
        });
    };
    /**
     * @param reason Why the snackbar was closed. Value will be passed to CLOSING_EVENT and CLOSED_EVENT via the
     *     `event.detail.reason` property. Standard values are REASON_ACTION and REASON_DISMISS, but custom
     *     client-specific values may also be used if desired.
     */
    MDCSnackbarFoundation.prototype.close = function (reason) {
        var _this = this;
        if (reason === void 0) { reason = ''; }
        if (!this.isOpen_) {
            // Avoid redundant close calls (and events), e.g. repeated interactions as the snackbar is animating closed
            return;
        }
        cancelAnimationFrame(this.animationFrame_);
        this.animationFrame_ = 0;
        this.clearAutoDismissTimer_();
        this.isOpen_ = false;
        this.adapter.notifyClosing(reason);
        this.adapter.addClass(_constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.CLOSING);
        this.adapter.removeClass(_constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.OPEN);
        this.adapter.removeClass(_constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.OPENING);
        clearTimeout(this.animationTimer_);
        this.animationTimer_ = setTimeout(function () {
            _this.handleAnimationTimerEnd_();
            _this.adapter.notifyClosed(reason);
        }, _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.SNACKBAR_ANIMATION_CLOSE_TIME_MS);
    };
    MDCSnackbarFoundation.prototype.isOpen = function () {
        return this.isOpen_;
    };
    MDCSnackbarFoundation.prototype.getTimeoutMs = function () {
        return this.autoDismissTimeoutMs_;
    };
    MDCSnackbarFoundation.prototype.setTimeoutMs = function (timeoutMs) {
        // Use shorter variable names to make the code more readable
        var minValue = _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.MIN_AUTO_DISMISS_TIMEOUT_MS;
        var maxValue = _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.MAX_AUTO_DISMISS_TIMEOUT_MS;
        var indeterminateValue = _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.INDETERMINATE;
        if (timeoutMs === _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.INDETERMINATE || (timeoutMs <= maxValue && timeoutMs >= minValue)) {
            this.autoDismissTimeoutMs_ = timeoutMs;
        }
        else {
            throw new Error("\n        timeoutMs must be an integer in the range " + minValue + "\u2013" + maxValue + "\n        (or " + indeterminateValue + " to disable), but got '" + timeoutMs + "'");
        }
    };
    MDCSnackbarFoundation.prototype.getCloseOnEscape = function () {
        return this.closeOnEscape_;
    };
    MDCSnackbarFoundation.prototype.setCloseOnEscape = function (closeOnEscape) {
        this.closeOnEscape_ = closeOnEscape;
    };
    MDCSnackbarFoundation.prototype.handleKeyDown = function (evt) {
        var isEscapeKey = evt.key === 'Escape' || evt.keyCode === 27;
        if (isEscapeKey && this.getCloseOnEscape()) {
            this.close(REASON_DISMISS);
        }
    };
    MDCSnackbarFoundation.prototype.handleActionButtonClick = function (_evt) {
        this.close(REASON_ACTION);
    };
    MDCSnackbarFoundation.prototype.handleActionIconClick = function (_evt) {
        this.close(REASON_DISMISS);
    };
    MDCSnackbarFoundation.prototype.clearAutoDismissTimer_ = function () {
        clearTimeout(this.autoDismissTimer_);
        this.autoDismissTimer_ = 0;
    };
    MDCSnackbarFoundation.prototype.handleAnimationTimerEnd_ = function () {
        this.animationTimer_ = 0;
        this.adapter.removeClass(_constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.OPENING);
        this.adapter.removeClass(_constants__WEBPACK_IMPORTED_MODULE_0__.cssClasses.CLOSING);
    };
    /**
     * Runs the given logic on the next animation frame, using setTimeout to factor in Firefox reflow behavior.
     */
    MDCSnackbarFoundation.prototype.runNextAnimationFrame_ = function (callback) {
        var _this = this;
        cancelAnimationFrame(this.animationFrame_);
        this.animationFrame_ = requestAnimationFrame(function () {
            _this.animationFrame_ = 0;
            clearTimeout(_this.animationTimer_);
            _this.animationTimer_ = setTimeout(callback, 0);
        });
    };
    return MDCSnackbarFoundation;
}(_material_base_foundation__WEBPACK_IMPORTED_MODULE_2__.MDCFoundation));

// tslint:disable-next-line:no-default-export Needed for backward compatibility with MDC Web v0.44.0 and earlier.
/* harmony default export */ const __WEBPACK_DEFAULT_EXPORT__ = (MDCSnackbarFoundation);
//# sourceMappingURL=foundation.js.map

/***/ }),

/***/ "./node_modules/@material/snackbar/index.js":
/*!**************************************************!*\
  !*** ./node_modules/@material/snackbar/index.js ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   MDCSnackbar: () => (/* reexport safe */ _component__WEBPACK_IMPORTED_MODULE_1__.MDCSnackbar),
/* harmony export */   MDCSnackbarFoundation: () => (/* reexport safe */ _foundation__WEBPACK_IMPORTED_MODULE_3__.MDCSnackbarFoundation),
/* harmony export */   cssClasses: () => (/* reexport safe */ _constants__WEBPACK_IMPORTED_MODULE_2__.cssClasses),
/* harmony export */   numbers: () => (/* reexport safe */ _constants__WEBPACK_IMPORTED_MODULE_2__.numbers),
/* harmony export */   strings: () => (/* reexport safe */ _constants__WEBPACK_IMPORTED_MODULE_2__.strings),
/* harmony export */   util: () => (/* reexport module object */ _util__WEBPACK_IMPORTED_MODULE_0__)
/* harmony export */ });
/* harmony import */ var _util__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./util */ "./node_modules/@material/snackbar/util.js");
/* harmony import */ var _component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./component */ "./node_modules/@material/snackbar/component.js");
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./constants */ "./node_modules/@material/snackbar/constants.js");
/* harmony import */ var _foundation__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./foundation */ "./node_modules/@material/snackbar/foundation.js");
/**
 * @license
 * Copyright 2019 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */





//# sourceMappingURL=index.js.map

/***/ }),

/***/ "./node_modules/@material/snackbar/util.js":
/*!*************************************************!*\
  !*** ./node_modules/@material/snackbar/util.js ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   announce: () => (/* binding */ announce)
/* harmony export */ });
/* harmony import */ var _constants__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./constants */ "./node_modules/@material/snackbar/constants.js");
/**
 * @license
 * Copyright 2018 Google Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

var ARIA_LIVE_DELAY_MS = _constants__WEBPACK_IMPORTED_MODULE_0__.numbers.ARIA_LIVE_DELAY_MS;
var ARIA_LIVE_LABEL_TEXT_ATTR = _constants__WEBPACK_IMPORTED_MODULE_0__.strings.ARIA_LIVE_LABEL_TEXT_ATTR;
function announce(ariaEl, labelEl) {
    if (labelEl === void 0) { labelEl = ariaEl; }
    var priority = ariaEl.getAttribute('aria-live');
    // Trim text to ignore `&nbsp;` (see below).
    // textContent is only null if the node is a document, DOCTYPE, or notation.
    var labelText = labelEl.textContent.trim();
    if (!labelText || !priority) {
        return;
    }
    // Temporarily disable `aria-live` to prevent JAWS+Firefox from announcing the message twice.
    ariaEl.setAttribute('aria-live', 'off');
    // Temporarily clear `textContent` to force a DOM mutation event that will be detected by screen readers.
    // `aria-live` elements are only announced when the element's `textContent` *changes*, so snackbars
    // sent to the browser in the initial HTML response won't be read unless we clear the element's `textContent` first.
    // Similarly, displaying the same snackbar message twice in a row doesn't trigger a DOM mutation event,
    // so screen readers won't announce the second message unless we first clear `textContent`.
    //
    // We have to clear the label text two different ways to make it work in all browsers and screen readers:
    //
    //   1. `textContent = ''` is required for IE11 + JAWS
    //   2. `innerHTML = '&nbsp;'` is required for Chrome + JAWS and NVDA
    //
    // All other browser/screen reader combinations support both methods.
    //
    // The wrapper `<span>` visually hides the space character so that it doesn't cause jank when added/removed.
    // N.B.: Setting `position: absolute`, `opacity: 0`, or `height: 0` prevents Chrome from detecting the DOM change.
    //
    // This technique has been tested in:
    //
    //   * JAWS 2019:
    //       - Chrome 70
    //       - Firefox 60 (ESR)
    //       - IE 11
    //   * NVDA 2018:
    //       - Chrome 70
    //       - Firefox 60 (ESR)
    //       - IE 11
    //   * ChromeVox 53
    labelEl.textContent = '';
    labelEl.innerHTML = '<span style="display: inline-block; width: 0; height: 1px;">&nbsp;</span>';
    // Prevent visual jank by temporarily displaying the label text in the ::before pseudo-element.
    // CSS generated content is normally announced by screen readers
    // (except in IE 11; see https://tink.uk/accessibility-support-for-css-generated-content/);
    // however, `aria-live` is turned off, so this DOM update will be ignored by screen readers.
    labelEl.setAttribute(ARIA_LIVE_LABEL_TEXT_ATTR, labelText);
    setTimeout(function () {
        // Allow screen readers to announce changes to the DOM again.
        ariaEl.setAttribute('aria-live', priority);
        // Remove the message from the ::before pseudo-element.
        labelEl.removeAttribute(ARIA_LIVE_LABEL_TEXT_ATTR);
        // Restore the original label text, which will be announced by screen readers.
        labelEl.textContent = labelText;
    }, ARIA_LIVE_DELAY_MS);
}

//# sourceMappingURL=util.js.map

/***/ }),

/***/ "./src/fetchData/actions.ts":
/*!**********************************!*\
  !*** ./src/fetchData/actions.ts ***!
  \**********************************/
/***/ ((__unused_webpack_module, exports, __webpack_require__) => {


Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.ShowSnackBarAction = exports.CreateFormData = void 0;
exports.ShowModalAction = ShowModalAction;
const snackbar_1 = __webpack_require__(/*! @material/snackbar */ "./node_modules/@material/snackbar/index.js");
function ShowModalAction(show) {
    const style = show ? 'block' : 'none';
    document.getElementById('mainModal').style.display = style;
}
const CreateFormData = (form) => {
    var formData = new FormData();
    var inputs = form.querySelectorAll("input");
    inputs.forEach(input => {
        var _a;
        switch (input.type) {
            case "checkbox": {
                formData.append(input.name, input.checked.toString());
                break;
            }
            default: {
                formData.append(input.name, input.value);
            }
        }
        for (var i = 0; i < ((_a = input.files) === null || _a === void 0 ? void 0 : _a.length); i++) {
            formData.append(input.name, input.files[i], input.files[i].name);
        }
        ;
    });
    return formData;
};
exports.CreateFormData = CreateFormData;
const ShowSnackBarAction = (message, error = false) => {
    const snackbar = new snackbar_1.MDCSnackbar(document.getElementById('main-snack'));
    const div = document.getElementById('main-snack');
    div.classList.add("mdc-snackbar--open");
    snackbar.labelText = message;
    if (error) {
        snackbar.timeoutMs = 10000;
        const surface = div.querySelector(".mdc-snackbar__surface");
        surface.style.backgroundColor = "darkred";
    }
    snackbar.open();
};
exports.ShowSnackBarAction = ShowSnackBarAction;


/***/ }),

/***/ "./src/fetchData/index.ts":
/*!********************************!*\
  !*** ./src/fetchData/index.ts ***!
  \********************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {


var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
const actions_1 = __webpack_require__(/*! ./actions */ "./src/fetchData/actions.ts");
const tagHandlers_1 = __webpack_require__(/*! ./tagHandlers */ "./src/fetchData/tagHandlers.ts");
const types_1 = __webpack_require__(/*! ./types */ "./src/fetchData/types.ts");
const proxyClickers = document.querySelectorAll(`[${types_1.dataMtdClickerBy}]`);
proxyClickers.forEach((clicker) => {
    const targetId = clicker.getAttribute(types_1.dataMtdClickerBy);
    clicker.addEventListener("click", () => {
        document.getElementById(targetId).click();
    });
});
const clickers = document.querySelectorAll(`[${types_1.dataMtdClicker}]`);
clickers.forEach((clicker) => __awaiter(void 0, void 0, void 0, function* () {
    clicker.addEventListener("click", (elemnt) => __awaiter(void 0, void 0, void 0, function* () {
        const form = elemnt.currentTarget.closest("form");
        const validate = form.reportValidity();
        if (!validate) {
            return false;
        }
        (0, actions_1.ShowModalAction)(true);
        var formData = (0, actions_1.CreateFormData)(form);
        const response = yield fetch(form.action, { method: form.method, body: formData });
        switch (response.status) {
            case 200: {
                tagHandlers_1.TagHandlers.forEach((handler) => __awaiter(void 0, void 0, void 0, function* () { return yield handler(clicker, response); }));
                setTimeout(() => (0, actions_1.ShowModalAction)(false), 1000);
                break;
            }
            case 400: {
                var value = yield response.text();
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)(value, true); }, 1000);
                break;
            }
            case 500: {
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)("500 Internal Server Error", true); }, 1000);
                break;
            }
            default: {
                setTimeout(() => { (0, actions_1.ShowModalAction)(false); (0, actions_1.ShowSnackBarAction)(`No handler for status ${response.status}`, true); }, 1000);
            }
        }
    }));
}));


/***/ }),

/***/ "./src/fetchData/tagHandlers.ts":
/*!**************************************!*\
  !*** ./src/fetchData/tagHandlers.ts ***!
  \**************************************/
/***/ (function(__unused_webpack_module, exports, __webpack_require__) {


var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.TagHandlers = void 0;
const types_1 = __webpack_require__(/*! ./types */ "./src/fetchData/types.ts");
function ResponseHandler(clicker, response) {
    return __awaiter(this, void 0, void 0, function* () {
        var inputId = clicker.getAttribute(`[${types_1.dataResponse}]`);
        if (!inputId)
            return false;
        var input = document.getElementById(inputId);
        if (!input)
            return false;
        input.value = yield response.json();
    });
}
exports.TagHandlers = [ResponseHandler];


/***/ }),

/***/ "./src/fetchData/types.ts":
/*!********************************!*\
  !*** ./src/fetchData/types.ts ***!
  \********************************/
/***/ ((__unused_webpack_module, exports) => {


Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.dataMtdClickerBy = exports.dataResponse = exports.dataMtdClicker = void 0;
exports.dataMtdClicker = "data-mtd-clicker";
exports.dataResponse = "data-mtd-response";
exports.dataMtdClickerBy = "data-mtd-clicker-by";


/***/ }),

/***/ "./node_modules/tslib/tslib.es6.js":
/*!*****************************************!*\
  !*** ./node_modules/tslib/tslib.es6.js ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   __assign: () => (/* binding */ __assign),
/* harmony export */   __asyncDelegator: () => (/* binding */ __asyncDelegator),
/* harmony export */   __asyncGenerator: () => (/* binding */ __asyncGenerator),
/* harmony export */   __asyncValues: () => (/* binding */ __asyncValues),
/* harmony export */   __await: () => (/* binding */ __await),
/* harmony export */   __awaiter: () => (/* binding */ __awaiter),
/* harmony export */   __classPrivateFieldGet: () => (/* binding */ __classPrivateFieldGet),
/* harmony export */   __classPrivateFieldSet: () => (/* binding */ __classPrivateFieldSet),
/* harmony export */   __createBinding: () => (/* binding */ __createBinding),
/* harmony export */   __decorate: () => (/* binding */ __decorate),
/* harmony export */   __exportStar: () => (/* binding */ __exportStar),
/* harmony export */   __extends: () => (/* binding */ __extends),
/* harmony export */   __generator: () => (/* binding */ __generator),
/* harmony export */   __importDefault: () => (/* binding */ __importDefault),
/* harmony export */   __importStar: () => (/* binding */ __importStar),
/* harmony export */   __makeTemplateObject: () => (/* binding */ __makeTemplateObject),
/* harmony export */   __metadata: () => (/* binding */ __metadata),
/* harmony export */   __param: () => (/* binding */ __param),
/* harmony export */   __read: () => (/* binding */ __read),
/* harmony export */   __rest: () => (/* binding */ __rest),
/* harmony export */   __spread: () => (/* binding */ __spread),
/* harmony export */   __spreadArrays: () => (/* binding */ __spreadArrays),
/* harmony export */   __values: () => (/* binding */ __values)
/* harmony export */ });
/*! *****************************************************************************
Copyright (c) Microsoft Corporation.

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR
OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR
PERFORMANCE OF THIS SOFTWARE.
***************************************************************************** */
/* global Reflect, Promise */

var extendStatics = function(d, b) {
    extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return extendStatics(d, b);
};

function __extends(d, b) {
    extendStatics(d, b);
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
}

var __assign = function() {
    __assign = Object.assign || function __assign(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p)) t[p] = s[p];
        }
        return t;
    }
    return __assign.apply(this, arguments);
}

function __rest(s, e) {
    var t = {};
    for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p) && e.indexOf(p) < 0)
        t[p] = s[p];
    if (s != null && typeof Object.getOwnPropertySymbols === "function")
        for (var i = 0, p = Object.getOwnPropertySymbols(s); i < p.length; i++) {
            if (e.indexOf(p[i]) < 0 && Object.prototype.propertyIsEnumerable.call(s, p[i]))
                t[p[i]] = s[p[i]];
        }
    return t;
}

function __decorate(decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
}

function __param(paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
}

function __metadata(metadataKey, metadataValue) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(metadataKey, metadataValue);
}

function __awaiter(thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
}

function __generator(thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
}

function __createBinding(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}

function __exportStar(m, exports) {
    for (var p in m) if (p !== "default" && !exports.hasOwnProperty(p)) exports[p] = m[p];
}

function __values(o) {
    var s = typeof Symbol === "function" && Symbol.iterator, m = s && o[s], i = 0;
    if (m) return m.call(o);
    if (o && typeof o.length === "number") return {
        next: function () {
            if (o && i >= o.length) o = void 0;
            return { value: o && o[i++], done: !o };
        }
    };
    throw new TypeError(s ? "Object is not iterable." : "Symbol.iterator is not defined.");
}

function __read(o, n) {
    var m = typeof Symbol === "function" && o[Symbol.iterator];
    if (!m) return o;
    var i = m.call(o), r, ar = [], e;
    try {
        while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
    }
    catch (error) { e = { error: error }; }
    finally {
        try {
            if (r && !r.done && (m = i["return"])) m.call(i);
        }
        finally { if (e) throw e.error; }
    }
    return ar;
}

function __spread() {
    for (var ar = [], i = 0; i < arguments.length; i++)
        ar = ar.concat(__read(arguments[i]));
    return ar;
}

function __spreadArrays() {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};

function __await(v) {
    return this instanceof __await ? (this.v = v, this) : new __await(v);
}

function __asyncGenerator(thisArg, _arguments, generator) {
    if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
    var g = generator.apply(thisArg, _arguments || []), i, q = [];
    return i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i;
    function verb(n) { if (g[n]) i[n] = function (v) { return new Promise(function (a, b) { q.push([n, v, a, b]) > 1 || resume(n, v); }); }; }
    function resume(n, v) { try { step(g[n](v)); } catch (e) { settle(q[0][3], e); } }
    function step(r) { r.value instanceof __await ? Promise.resolve(r.value.v).then(fulfill, reject) : settle(q[0][2], r); }
    function fulfill(value) { resume("next", value); }
    function reject(value) { resume("throw", value); }
    function settle(f, v) { if (f(v), q.shift(), q.length) resume(q[0][0], q[0][1]); }
}

function __asyncDelegator(o) {
    var i, p;
    return i = {}, verb("next"), verb("throw", function (e) { throw e; }), verb("return"), i[Symbol.iterator] = function () { return this; }, i;
    function verb(n, f) { i[n] = o[n] ? function (v) { return (p = !p) ? { value: __await(o[n](v)), done: n === "return" } : f ? f(v) : v; } : f; }
}

function __asyncValues(o) {
    if (!Symbol.asyncIterator) throw new TypeError("Symbol.asyncIterator is not defined.");
    var m = o[Symbol.asyncIterator], i;
    return m ? m.call(o) : (o = typeof __values === "function" ? __values(o) : o[Symbol.iterator](), i = {}, verb("next"), verb("throw"), verb("return"), i[Symbol.asyncIterator] = function () { return this; }, i);
    function verb(n) { i[n] = o[n] && function (v) { return new Promise(function (resolve, reject) { v = o[n](v), settle(resolve, reject, v.done, v.value); }); }; }
    function settle(resolve, reject, d, v) { Promise.resolve(v).then(function(v) { resolve({ value: v, done: d }); }, reject); }
}

function __makeTemplateObject(cooked, raw) {
    if (Object.defineProperty) { Object.defineProperty(cooked, "raw", { value: raw }); } else { cooked.raw = raw; }
    return cooked;
};

function __importStar(mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result.default = mod;
    return result;
}

function __importDefault(mod) {
    return (mod && mod.__esModule) ? mod : { default: mod };
}

function __classPrivateFieldGet(receiver, privateMap) {
    if (!privateMap.has(receiver)) {
        throw new TypeError("attempted to get private field on non-instance");
    }
    return privateMap.get(receiver);
}

function __classPrivateFieldSet(receiver, privateMap, value) {
    if (!privateMap.has(receiver)) {
        throw new TypeError("attempted to set private field on non-instance");
    }
    privateMap.set(receiver, value);
    return value;
}


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module is referenced by other modules so it can't be inlined
/******/ 	var __webpack_exports__ = __webpack_require__("./src/fetchData/index.ts");
/******/ 	OrderMaker = __webpack_exports__;
/******/ 	
/******/ })()
;
//# sourceMappingURL=data:application/json;charset=utf-8;base64,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QUEwQiw2QkFBNkIsb0JBQW9CLGdEQUFnRCxrQkFBa0I7QUFDN0k7QUFDQTtBQUNPO0FBQ1A7QUFDQTtBQUNBLDJHQUEyRyx1RkFBdUYsY0FBYztBQUNoTix1QkFBdUIsOEJBQThCLGdEQUFnRCx3REFBd0Q7QUFDN0osNkNBQTZDLHNDQUFzQyxVQUFVLG1CQUFtQixJQUFJO0FBQ3BIO0FBQ0E7QUFDTztBQUNQLGlDQUFpQyx1Q0FBdUMsWUFBWSxLQUFLLE9BQU87QUFDaEc7QUFDQTtBQUNBO0FBQ087QUFDUDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNPO0FBQ1AsNkNBQTZDO0FBQzdDO0FBQ0E7QUFDTztBQUNQO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNPO0FBQ1A7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7O1VDek5BO1VBQ0E7O1VBRUE7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7O1VBRUE7VUFDQTs7VUFFQTtVQUNBO1VBQ0E7Ozs7O1dDdEJBO1dBQ0E7V0FDQTtXQUNBO1dBQ0EseUNBQXlDLHdDQUF3QztXQUNqRjtXQUNBO1dBQ0E7Ozs7O1dDUEE7Ozs7O1dDQUE7V0FDQTtXQUNBO1dBQ0EsdURBQXVELGlCQUFpQjtXQUN4RTtXQUNBLGdEQUFnRCxhQUFhO1dBQzdEOzs7OztVRU5BO1VBQ0E7VUFDQTtVQUNBIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vT3JkZXJNYWtlci8uL25vZGVfbW9kdWxlcy9AbWF0ZXJpYWwvYmFzZS9jb21wb25lbnQuanMiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci8uL25vZGVfbW9kdWxlcy9AbWF0ZXJpYWwvYmFzZS9mb3VuZGF0aW9uLmpzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9ub2RlX21vZHVsZXMvQG1hdGVyaWFsL2RvbS9wb255ZmlsbC5qcyIsIndlYnBhY2s6Ly9PcmRlck1ha2VyLy4vbm9kZV9tb2R1bGVzL0BtYXRlcmlhbC9zbmFja2Jhci9jb21wb25lbnQuanMiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci8uL25vZGVfbW9kdWxlcy9AbWF0ZXJpYWwvc25hY2tiYXIvY29uc3RhbnRzLmpzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9ub2RlX21vZHVsZXMvQG1hdGVyaWFsL3NuYWNrYmFyL2ZvdW5kYXRpb24uanMiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci8uL25vZGVfbW9kdWxlcy9AbWF0ZXJpYWwvc25hY2tiYXIvaW5kZXguanMiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci8uL25vZGVfbW9kdWxlcy9AbWF0ZXJpYWwvc25hY2tiYXIvdXRpbC5qcyIsIndlYnBhY2s6Ly9PcmRlck1ha2VyLy4vc3JjL2ZldGNoRGF0YS9hY3Rpb25zLnRzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9zcmMvZmV0Y2hEYXRhL2luZGV4LnRzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9zcmMvZmV0Y2hEYXRhL3RhZ0hhbmRsZXJzLnRzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9zcmMvZmV0Y2hEYXRhL3R5cGVzLnRzIiwid2VicGFjazovL09yZGVyTWFrZXIvLi9ub2RlX21vZHVsZXMvdHNsaWIvdHNsaWIuZXM2LmpzIiwid2VicGFjazovL09yZGVyTWFrZXIvd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci93ZWJwYWNrL3J1bnRpbWUvZGVmaW5lIHByb3BlcnR5IGdldHRlcnMiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci93ZWJwYWNrL3J1bnRpbWUvaGFzT3duUHJvcGVydHkgc2hvcnRoYW5kIiwid2VicGFjazovL09yZGVyTWFrZXIvd2VicGFjay9ydW50aW1lL21ha2UgbmFtZXNwYWNlIG9iamVjdCIsIndlYnBhY2s6Ly9PcmRlck1ha2VyL3dlYnBhY2svYmVmb3JlLXN0YXJ0dXAiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci93ZWJwYWNrL3N0YXJ0dXAiLCJ3ZWJwYWNrOi8vT3JkZXJNYWtlci93ZWJwYWNrL2FmdGVyLXN0YXJ0dXAiXSwic291cmNlc0NvbnRlbnQiOlsiLyoqXG4gKiBAbGljZW5zZVxuICogQ29weXJpZ2h0IDIwMTYgR29vZ2xlIEluYy5cbiAqXG4gKiBQZXJtaXNzaW9uIGlzIGhlcmVieSBncmFudGVkLCBmcmVlIG9mIGNoYXJnZSwgdG8gYW55IHBlcnNvbiBvYnRhaW5pbmcgYSBjb3B5XG4gKiBvZiB0aGlzIHNvZnR3YXJlIGFuZCBhc3NvY2lhdGVkIGRvY3VtZW50YXRpb24gZmlsZXMgKHRoZSBcIlNvZnR3YXJlXCIpLCB0byBkZWFsXG4gKiBpbiB0aGUgU29mdHdhcmUgd2l0aG91dCByZXN0cmljdGlvbiwgaW5jbHVkaW5nIHdpdGhvdXQgbGltaXRhdGlvbiB0aGUgcmlnaHRzXG4gKiB0byB1c2UsIGNvcHksIG1vZGlmeSwgbWVyZ2UsIHB1Ymxpc2gsIGRpc3RyaWJ1dGUsIHN1YmxpY2Vuc2UsIGFuZC9vciBzZWxsXG4gKiBjb3BpZXMgb2YgdGhlIFNvZnR3YXJlLCBhbmQgdG8gcGVybWl0IHBlcnNvbnMgdG8gd2hvbSB0aGUgU29mdHdhcmUgaXNcbiAqIGZ1cm5pc2hlZCB0byBkbyBzbywgc3ViamVjdCB0byB0aGUgZm9sbG93aW5nIGNvbmRpdGlvbnM6XG4gKlxuICogVGhlIGFib3ZlIGNvcHlyaWdodCBub3RpY2UgYW5kIHRoaXMgcGVybWlzc2lvbiBub3RpY2Ugc2hhbGwgYmUgaW5jbHVkZWQgaW5cbiAqIGFsbCBjb3BpZXMgb3Igc3Vic3RhbnRpYWwgcG9ydGlvbnMgb2YgdGhlIFNvZnR3YXJlLlxuICpcbiAqIFRIRSBTT0ZUV0FSRSBJUyBQUk9WSURFRCBcIkFTIElTXCIsIFdJVEhPVVQgV0FSUkFOVFkgT0YgQU5ZIEtJTkQsIEVYUFJFU1MgT1JcbiAqIElNUExJRUQsIElOQ0xVRElORyBCVVQgTk9UIExJTUlURUQgVE8gVEhFIFdBUlJBTlRJRVMgT0YgTUVSQ0hBTlRBQklMSVRZLFxuICogRklUTkVTUyBGT1IgQSBQQVJUSUNVTEFSIFBVUlBPU0UgQU5EIE5PTklORlJJTkdFTUVOVC4gSU4gTk8gRVZFTlQgU0hBTEwgVEhFXG4gKiBBVVRIT1JTIE9SIENPUFlSSUdIVCBIT0xERVJTIEJFIExJQUJMRSBGT1IgQU5ZIENMQUlNLCBEQU1BR0VTIE9SIE9USEVSXG4gKiBMSUFCSUxJVFksIFdIRVRIRVIgSU4gQU4gQUNUSU9OIE9GIENPTlRSQUNULCBUT1JUIE9SIE9USEVSV0lTRSwgQVJJU0lORyBGUk9NLFxuICogT1VUIE9GIE9SIElOIENPTk5FQ1RJT04gV0lUSCBUSEUgU09GVFdBUkUgT1IgVEhFIFVTRSBPUiBPVEhFUiBERUFMSU5HUyBJTlxuICogVEhFIFNPRlRXQVJFLlxuICovXG5pbXBvcnQgeyBfX3JlYWQsIF9fc3ByZWFkIH0gZnJvbSBcInRzbGliXCI7XG5pbXBvcnQgeyBNRENGb3VuZGF0aW9uIH0gZnJvbSAnLi9mb3VuZGF0aW9uJztcbnZhciBNRENDb21wb25lbnQgPSAvKiogQGNsYXNzICovIChmdW5jdGlvbiAoKSB7XG4gICAgZnVuY3Rpb24gTURDQ29tcG9uZW50KHJvb3QsIGZvdW5kYXRpb24pIHtcbiAgICAgICAgdmFyIGFyZ3MgPSBbXTtcbiAgICAgICAgZm9yICh2YXIgX2kgPSAyOyBfaSA8IGFyZ3VtZW50cy5sZW5ndGg7IF9pKyspIHtcbiAgICAgICAgICAgIGFyZ3NbX2kgLSAyXSA9IGFyZ3VtZW50c1tfaV07XG4gICAgICAgIH1cbiAgICAgICAgdGhpcy5yb290ID0gcm9vdDtcbiAgICAgICAgdGhpcy5pbml0aWFsaXplLmFwcGx5KHRoaXMsIF9fc3ByZWFkKGFyZ3MpKTtcbiAgICAgICAgLy8gTm90ZSB0aGF0IHdlIGluaXRpYWxpemUgZm91bmRhdGlvbiBoZXJlIGFuZCBub3Qgd2l0aGluIHRoZSBjb25zdHJ1Y3RvcidzIGRlZmF1bHQgcGFyYW0gc28gdGhhdFxuICAgICAgICAvLyB0aGlzLnJvb3RfIGlzIGRlZmluZWQgYW5kIGNhbiBiZSB1c2VkIHdpdGhpbiB0aGUgZm91bmRhdGlvbiBjbGFzcy5cbiAgICAgICAgdGhpcy5mb3VuZGF0aW9uID1cbiAgICAgICAgICAgIGZvdW5kYXRpb24gPT09IHVuZGVmaW5lZCA/IHRoaXMuZ2V0RGVmYXVsdEZvdW5kYXRpb24oKSA6IGZvdW5kYXRpb247XG4gICAgICAgIHRoaXMuZm91bmRhdGlvbi5pbml0KCk7XG4gICAgICAgIHRoaXMuaW5pdGlhbFN5bmNXaXRoRE9NKCk7XG4gICAgfVxuICAgIE1EQ0NvbXBvbmVudC5hdHRhY2hUbyA9IGZ1bmN0aW9uIChyb290KSB7XG4gICAgICAgIC8vIFN1YmNsYXNzZXMgd2hpY2ggZXh0ZW5kIE1EQ0Jhc2Ugc2hvdWxkIHByb3ZpZGUgYW4gYXR0YWNoVG8oKSBtZXRob2QgdGhhdCB0YWtlcyBhIHJvb3QgZWxlbWVudCBhbmRcbiAgICAgICAgLy8gcmV0dXJucyBhbiBpbnN0YW50aWF0ZWQgY29tcG9uZW50IHdpdGggaXRzIHJvb3Qgc2V0IHRvIHRoYXQgZWxlbWVudC4gQWxzbyBub3RlIHRoYXQgaW4gdGhlIGNhc2VzIG9mXG4gICAgICAgIC8vIHN1YmNsYXNzZXMsIGFuIGV4cGxpY2l0IGZvdW5kYXRpb24gY2xhc3Mgd2lsbCBub3QgaGF2ZSB0byBiZSBwYXNzZWQgaW47IGl0IHdpbGwgc2ltcGx5IGJlIGluaXRpYWxpemVkXG4gICAgICAgIC8vIGZyb20gZ2V0RGVmYXVsdEZvdW5kYXRpb24oKS5cbiAgICAgICAgcmV0dXJuIG5ldyBNRENDb21wb25lbnQocm9vdCwgbmV3IE1EQ0ZvdW5kYXRpb24oe30pKTtcbiAgICB9O1xuICAgIC8qIGlzdGFuYnVsIGlnbm9yZSBuZXh0OiBtZXRob2QgcGFyYW0gb25seSBleGlzdHMgZm9yIHR5cGluZyBwdXJwb3NlczsgaXQgZG9lcyBub3QgbmVlZCB0byBiZSB1bml0IHRlc3RlZCAqL1xuICAgIE1EQ0NvbXBvbmVudC5wcm90b3R5cGUuaW5pdGlhbGl6ZSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgdmFyIF9hcmdzID0gW107XG4gICAgICAgIGZvciAodmFyIF9pID0gMDsgX2kgPCBhcmd1bWVudHMubGVuZ3RoOyBfaSsrKSB7XG4gICAgICAgICAgICBfYXJnc1tfaV0gPSBhcmd1bWVudHNbX2ldO1xuICAgICAgICB9XG4gICAgICAgIC8vIFN1YmNsYXNzZXMgY2FuIG92ZXJyaWRlIHRoaXMgdG8gZG8gYW55IGFkZGl0aW9uYWwgc2V0dXAgd29yayB0aGF0IHdvdWxkIGJlIGNvbnNpZGVyZWQgcGFydCBvZiBhXG4gICAgICAgIC8vIFwiY29uc3RydWN0b3JcIi4gRXNzZW50aWFsbHksIGl0IGlzIGEgaG9vayBpbnRvIHRoZSBwYXJlbnQgY29uc3RydWN0b3IgYmVmb3JlIHRoZSBmb3VuZGF0aW9uIGlzXG4gICAgICAgIC8vIGluaXRpYWxpemVkLiBBbnkgYWRkaXRpb25hbCBhcmd1bWVudHMgYmVzaWRlcyByb290IGFuZCBmb3VuZGF0aW9uIHdpbGwgYmUgcGFzc2VkIGluIGhlcmUuXG4gICAgfTtcbiAgICBNRENDb21wb25lbnQucHJvdG90eXBlLmdldERlZmF1bHRGb3VuZGF0aW9uID0gZnVuY3Rpb24gKCkge1xuICAgICAgICAvLyBTdWJjbGFzc2VzIG11c3Qgb3ZlcnJpZGUgdGhpcyBtZXRob2QgdG8gcmV0dXJuIGEgcHJvcGVybHkgY29uZmlndXJlZCBmb3VuZGF0aW9uIGNsYXNzIGZvciB0aGVcbiAgICAgICAgLy8gY29tcG9uZW50LlxuICAgICAgICB0aHJvdyBuZXcgRXJyb3IoJ1N1YmNsYXNzZXMgbXVzdCBvdmVycmlkZSBnZXREZWZhdWx0Rm91bmRhdGlvbiB0byByZXR1cm4gYSBwcm9wZXJseSBjb25maWd1cmVkICcgK1xuICAgICAgICAgICAgJ2ZvdW5kYXRpb24gY2xhc3MnKTtcbiAgICB9O1xuICAgIE1EQ0NvbXBvbmVudC5wcm90b3R5cGUuaW5pdGlhbFN5bmNXaXRoRE9NID0gZnVuY3Rpb24gKCkge1xuICAgICAgICAvLyBTdWJjbGFzc2VzIHNob3VsZCBvdmVycmlkZSB0aGlzIG1ldGhvZCBpZiB0aGV5IG5lZWQgdG8gcGVyZm9ybSB3b3JrIHRvIHN5bmNocm9uaXplIHdpdGggYSBob3N0IERPTVxuICAgICAgICAvLyBvYmplY3QuIEFuIGV4YW1wbGUgb2YgdGhpcyB3b3VsZCBiZSBhIGZvcm0gY29udHJvbCB3cmFwcGVyIHRoYXQgbmVlZHMgdG8gc3luY2hyb25pemUgaXRzIGludGVybmFsIHN0YXRlXG4gICAgICAgIC8vIHRvIHNvbWUgcHJvcGVydHkgb3IgYXR0cmlidXRlIG9mIHRoZSBob3N0IERPTS4gUGxlYXNlIG5vdGU6IHRoaXMgaXMgKm5vdCogdGhlIHBsYWNlIHRvIHBlcmZvcm0gRE9NXG4gICAgICAgIC8vIHJlYWRzL3dyaXRlcyB0aGF0IHdvdWxkIGNhdXNlIGxheW91dCAvIHBhaW50LCBhcyB0aGlzIGlzIGNhbGxlZCBzeW5jaHJvbm91c2x5IGZyb20gd2l0aGluIHRoZSBjb25zdHJ1Y3Rvci5cbiAgICB9O1xuICAgIE1EQ0NvbXBvbmVudC5wcm90b3R5cGUuZGVzdHJveSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgLy8gU3ViY2xhc3NlcyBtYXkgaW1wbGVtZW50IHRoaXMgbWV0aG9kIHRvIHJlbGVhc2UgYW55IHJlc291cmNlcyAvIGRlcmVnaXN0ZXIgYW55IGxpc3RlbmVycyB0aGV5IGhhdmVcbiAgICAgICAgLy8gYXR0YWNoZWQuIEFuIGV4YW1wbGUgb2YgdGhpcyBtaWdodCBiZSBkZXJlZ2lzdGVyaW5nIGEgcmVzaXplIGV2ZW50IGZyb20gdGhlIHdpbmRvdyBvYmplY3QuXG4gICAgICAgIHRoaXMuZm91bmRhdGlvbi5kZXN0cm95KCk7XG4gICAgfTtcbiAgICBNRENDb21wb25lbnQucHJvdG90eXBlLmxpc3RlbiA9IGZ1bmN0aW9uIChldnRUeXBlLCBoYW5kbGVyLCBvcHRpb25zKSB7XG4gICAgICAgIHRoaXMucm9vdC5hZGRFdmVudExpc3RlbmVyKGV2dFR5cGUsIGhhbmRsZXIsIG9wdGlvbnMpO1xuICAgIH07XG4gICAgTURDQ29tcG9uZW50LnByb3RvdHlwZS51bmxpc3RlbiA9IGZ1bmN0aW9uIChldnRUeXBlLCBoYW5kbGVyLCBvcHRpb25zKSB7XG4gICAgICAgIHRoaXMucm9vdC5yZW1vdmVFdmVudExpc3RlbmVyKGV2dFR5cGUsIGhhbmRsZXIsIG9wdGlvbnMpO1xuICAgIH07XG4gICAgLyoqXG4gICAgICogRmlyZXMgYSBjcm9zcy1icm93c2VyLWNvbXBhdGlibGUgY3VzdG9tIGV2ZW50IGZyb20gdGhlIGNvbXBvbmVudCByb290IG9mIHRoZSBnaXZlbiB0eXBlLCB3aXRoIHRoZSBnaXZlbiBkYXRhLlxuICAgICAqL1xuICAgIE1EQ0NvbXBvbmVudC5wcm90b3R5cGUuZW1pdCA9IGZ1bmN0aW9uIChldnRUeXBlLCBldnREYXRhLCBzaG91bGRCdWJibGUpIHtcbiAgICAgICAgaWYgKHNob3VsZEJ1YmJsZSA9PT0gdm9pZCAwKSB7IHNob3VsZEJ1YmJsZSA9IGZhbHNlOyB9XG4gICAgICAgIHZhciBldnQ7XG4gICAgICAgIGlmICh0eXBlb2YgQ3VzdG9tRXZlbnQgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICAgIGV2dCA9IG5ldyBDdXN0b21FdmVudChldnRUeXBlLCB7XG4gICAgICAgICAgICAgICAgYnViYmxlczogc2hvdWxkQnViYmxlLFxuICAgICAgICAgICAgICAgIGRldGFpbDogZXZ0RGF0YSxcbiAgICAgICAgICAgIH0pO1xuICAgICAgICB9XG4gICAgICAgIGVsc2Uge1xuICAgICAgICAgICAgZXZ0ID0gZG9jdW1lbnQuY3JlYXRlRXZlbnQoJ0N1c3RvbUV2ZW50Jyk7XG4gICAgICAgICAgICBldnQuaW5pdEN1c3RvbUV2ZW50KGV2dFR5cGUsIHNob3VsZEJ1YmJsZSwgZmFsc2UsIGV2dERhdGEpO1xuICAgICAgICB9XG4gICAgICAgIHRoaXMucm9vdC5kaXNwYXRjaEV2ZW50KGV2dCk7XG4gICAgfTtcbiAgICByZXR1cm4gTURDQ29tcG9uZW50O1xufSgpKTtcbmV4cG9ydCB7IE1EQ0NvbXBvbmVudCB9O1xuLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOm5vLWRlZmF1bHQtZXhwb3J0IE5lZWRlZCBmb3IgYmFja3dhcmQgY29tcGF0aWJpbGl0eSB3aXRoIE1EQyBXZWIgdjAuNDQuMCBhbmQgZWFybGllci5cbmV4cG9ydCBkZWZhdWx0IE1EQ0NvbXBvbmVudDtcbi8vIyBzb3VyY2VNYXBwaW5nVVJMPWNvbXBvbmVudC5qcy5tYXAiLCIvKipcbiAqIEBsaWNlbnNlXG4gKiBDb3B5cmlnaHQgMjAxNiBHb29nbGUgSW5jLlxuICpcbiAqIFBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZyBhIGNvcHlcbiAqIG9mIHRoaXMgc29mdHdhcmUgYW5kIGFzc29jaWF0ZWQgZG9jdW1lbnRhdGlvbiBmaWxlcyAodGhlIFwiU29mdHdhcmVcIiksIHRvIGRlYWxcbiAqIGluIHRoZSBTb2Z0d2FyZSB3aXRob3V0IHJlc3RyaWN0aW9uLCBpbmNsdWRpbmcgd2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHNcbiAqIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBtZXJnZSwgcHVibGlzaCwgZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGxcbiAqIGNvcGllcyBvZiB0aGUgU29mdHdhcmUsIGFuZCB0byBwZXJtaXQgcGVyc29ucyB0byB3aG9tIHRoZSBTb2Z0d2FyZSBpc1xuICogZnVybmlzaGVkIHRvIGRvIHNvLCBzdWJqZWN0IHRvIHRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcbiAqXG4gKiBUaGUgYWJvdmUgY29weXJpZ2h0IG5vdGljZSBhbmQgdGhpcyBwZXJtaXNzaW9uIG5vdGljZSBzaGFsbCBiZSBpbmNsdWRlZCBpblxuICogYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuXG4gKlxuICogVEhFIFNPRlRXQVJFIElTIFBST1ZJREVEIFwiQVMgSVNcIiwgV0lUSE9VVCBXQVJSQU5UWSBPRiBBTlkgS0lORCwgRVhQUkVTUyBPUlxuICogSU1QTElFRCwgSU5DTFVESU5HIEJVVCBOT1QgTElNSVRFRCBUTyBUSEUgV0FSUkFOVElFUyBPRiBNRVJDSEFOVEFCSUxJVFksXG4gKiBGSVRORVNTIEZPUiBBIFBBUlRJQ1VMQVIgUFVSUE9TRSBBTkQgTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEVcbiAqIEFVVEhPUlMgT1IgQ09QWVJJR0hUIEhPTERFUlMgQkUgTElBQkxFIEZPUiBBTlkgQ0xBSU0sIERBTUFHRVMgT1IgT1RIRVJcbiAqIExJQUJJTElUWSwgV0hFVEhFUiBJTiBBTiBBQ1RJT04gT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sXG4gKiBPVVQgT0YgT1IgSU4gQ09OTkVDVElPTiBXSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOXG4gKiBUSEUgU09GVFdBUkUuXG4gKi9cbnZhciBNRENGb3VuZGF0aW9uID0gLyoqIEBjbGFzcyAqLyAoZnVuY3Rpb24gKCkge1xuICAgIGZ1bmN0aW9uIE1EQ0ZvdW5kYXRpb24oYWRhcHRlcikge1xuICAgICAgICBpZiAoYWRhcHRlciA9PT0gdm9pZCAwKSB7IGFkYXB0ZXIgPSB7fTsgfVxuICAgICAgICB0aGlzLmFkYXB0ZXIgPSBhZGFwdGVyO1xuICAgIH1cbiAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkoTURDRm91bmRhdGlvbiwgXCJjc3NDbGFzc2VzXCIsIHtcbiAgICAgICAgZ2V0OiBmdW5jdGlvbiAoKSB7XG4gICAgICAgICAgICAvLyBDbGFzc2VzIGV4dGVuZGluZyBNRENGb3VuZGF0aW9uIHNob3VsZCBpbXBsZW1lbnQgdGhpcyBtZXRob2QgdG8gcmV0dXJuIGFuIG9iamVjdCB3aGljaCBleHBvcnRzIGV2ZXJ5XG4gICAgICAgICAgICAvLyBDU1MgY2xhc3MgdGhlIGZvdW5kYXRpb24gY2xhc3MgbmVlZHMgYXMgYSBwcm9wZXJ0eS4gZS5nLiB7QUNUSVZFOiAnbWRjLWNvbXBvbmVudC0tYWN0aXZlJ31cbiAgICAgICAgICAgIHJldHVybiB7fTtcbiAgICAgICAgfSxcbiAgICAgICAgZW51bWVyYWJsZTogdHJ1ZSxcbiAgICAgICAgY29uZmlndXJhYmxlOiB0cnVlXG4gICAgfSk7XG4gICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KE1EQ0ZvdW5kYXRpb24sIFwic3RyaW5nc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgLy8gQ2xhc3NlcyBleHRlbmRpbmcgTURDRm91bmRhdGlvbiBzaG91bGQgaW1wbGVtZW50IHRoaXMgbWV0aG9kIHRvIHJldHVybiBhbiBvYmplY3Qgd2hpY2ggZXhwb3J0cyBhbGxcbiAgICAgICAgICAgIC8vIHNlbWFudGljIHN0cmluZ3MgYXMgY29uc3RhbnRzLiBlLmcuIHtBUklBX1JPTEU6ICd0YWJsaXN0J31cbiAgICAgICAgICAgIHJldHVybiB7fTtcbiAgICAgICAgfSxcbiAgICAgICAgZW51bWVyYWJsZTogdHJ1ZSxcbiAgICAgICAgY29uZmlndXJhYmxlOiB0cnVlXG4gICAgfSk7XG4gICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KE1EQ0ZvdW5kYXRpb24sIFwibnVtYmVyc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgLy8gQ2xhc3NlcyBleHRlbmRpbmcgTURDRm91bmRhdGlvbiBzaG91bGQgaW1wbGVtZW50IHRoaXMgbWV0aG9kIHRvIHJldHVybiBhbiBvYmplY3Qgd2hpY2ggZXhwb3J0cyBhbGxcbiAgICAgICAgICAgIC8vIG9mIGl0cyBzZW1hbnRpYyBudW1iZXJzIGFzIGNvbnN0YW50cy4gZS5nLiB7QU5JTUFUSU9OX0RFTEFZX01TOiAzNTB9XG4gICAgICAgICAgICByZXR1cm4ge307XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENGb3VuZGF0aW9uLCBcImRlZmF1bHRBZGFwdGVyXCIsIHtcbiAgICAgICAgZ2V0OiBmdW5jdGlvbiAoKSB7XG4gICAgICAgICAgICAvLyBDbGFzc2VzIGV4dGVuZGluZyBNRENGb3VuZGF0aW9uIG1heSBjaG9vc2UgdG8gaW1wbGVtZW50IHRoaXMgZ2V0dGVyIGluIG9yZGVyIHRvIHByb3ZpZGUgYSBjb252ZW5pZW50XG4gICAgICAgICAgICAvLyB3YXkgb2Ygdmlld2luZyB0aGUgbmVjZXNzYXJ5IG1ldGhvZHMgb2YgYW4gYWRhcHRlci4gSW4gdGhlIGZ1dHVyZSwgdGhpcyBjb3VsZCBhbHNvIGJlIHVzZWQgZm9yIGFkYXB0ZXJcbiAgICAgICAgICAgIC8vIHZhbGlkYXRpb24uXG4gICAgICAgICAgICByZXR1cm4ge307XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE1EQ0ZvdW5kYXRpb24ucHJvdG90eXBlLmluaXQgPSBmdW5jdGlvbiAoKSB7XG4gICAgICAgIC8vIFN1YmNsYXNzZXMgc2hvdWxkIG92ZXJyaWRlIHRoaXMgbWV0aG9kIHRvIHBlcmZvcm0gaW5pdGlhbGl6YXRpb24gcm91dGluZXMgKHJlZ2lzdGVyaW5nIGV2ZW50cywgZXRjLilcbiAgICB9O1xuICAgIE1EQ0ZvdW5kYXRpb24ucHJvdG90eXBlLmRlc3Ryb3kgPSBmdW5jdGlvbiAoKSB7XG4gICAgICAgIC8vIFN1YmNsYXNzZXMgc2hvdWxkIG92ZXJyaWRlIHRoaXMgbWV0aG9kIHRvIHBlcmZvcm0gZGUtaW5pdGlhbGl6YXRpb24gcm91dGluZXMgKGRlLXJlZ2lzdGVyaW5nIGV2ZW50cywgZXRjLilcbiAgICB9O1xuICAgIHJldHVybiBNRENGb3VuZGF0aW9uO1xufSgpKTtcbmV4cG9ydCB7IE1EQ0ZvdW5kYXRpb24gfTtcbi8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTpuby1kZWZhdWx0LWV4cG9ydCBOZWVkZWQgZm9yIGJhY2t3YXJkIGNvbXBhdGliaWxpdHkgd2l0aCBNREMgV2ViIHYwLjQ0LjAgYW5kIGVhcmxpZXIuXG5leHBvcnQgZGVmYXVsdCBNRENGb3VuZGF0aW9uO1xuLy8jIHNvdXJjZU1hcHBpbmdVUkw9Zm91bmRhdGlvbi5qcy5tYXAiLCIvKipcbiAqIEBsaWNlbnNlXG4gKiBDb3B5cmlnaHQgMjAxOCBHb29nbGUgSW5jLlxuICpcbiAqIFBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZyBhIGNvcHlcbiAqIG9mIHRoaXMgc29mdHdhcmUgYW5kIGFzc29jaWF0ZWQgZG9jdW1lbnRhdGlvbiBmaWxlcyAodGhlIFwiU29mdHdhcmVcIiksIHRvIGRlYWxcbiAqIGluIHRoZSBTb2Z0d2FyZSB3aXRob3V0IHJlc3RyaWN0aW9uLCBpbmNsdWRpbmcgd2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHNcbiAqIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBtZXJnZSwgcHVibGlzaCwgZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGxcbiAqIGNvcGllcyBvZiB0aGUgU29mdHdhcmUsIGFuZCB0byBwZXJtaXQgcGVyc29ucyB0byB3aG9tIHRoZSBTb2Z0d2FyZSBpc1xuICogZnVybmlzaGVkIHRvIGRvIHNvLCBzdWJqZWN0IHRvIHRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcbiAqXG4gKiBUaGUgYWJvdmUgY29weXJpZ2h0IG5vdGljZSBhbmQgdGhpcyBwZXJtaXNzaW9uIG5vdGljZSBzaGFsbCBiZSBpbmNsdWRlZCBpblxuICogYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuXG4gKlxuICogVEhFIFNPRlRXQVJFIElTIFBST1ZJREVEIFwiQVMgSVNcIiwgV0lUSE9VVCBXQVJSQU5UWSBPRiBBTlkgS0lORCwgRVhQUkVTUyBPUlxuICogSU1QTElFRCwgSU5DTFVESU5HIEJVVCBOT1QgTElNSVRFRCBUTyBUSEUgV0FSUkFOVElFUyBPRiBNRVJDSEFOVEFCSUxJVFksXG4gKiBGSVRORVNTIEZPUiBBIFBBUlRJQ1VMQVIgUFVSUE9TRSBBTkQgTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEVcbiAqIEFVVEhPUlMgT1IgQ09QWVJJR0hUIEhPTERFUlMgQkUgTElBQkxFIEZPUiBBTlkgQ0xBSU0sIERBTUFHRVMgT1IgT1RIRVJcbiAqIExJQUJJTElUWSwgV0hFVEhFUiBJTiBBTiBBQ1RJT04gT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sXG4gKiBPVVQgT0YgT1IgSU4gQ09OTkVDVElPTiBXSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOXG4gKiBUSEUgU09GVFdBUkUuXG4gKi9cbi8qKlxuICogQGZpbGVvdmVydmlldyBBIFwicG9ueWZpbGxcIiBpcyBhIHBvbHlmaWxsIHRoYXQgZG9lc24ndCBtb2RpZnkgdGhlIGdsb2JhbCBwcm90b3R5cGUgY2hhaW4uXG4gKiBUaGlzIG1ha2VzIHBvbnlmaWxscyBzYWZlciB0aGFuIHRyYWRpdGlvbmFsIHBvbHlmaWxscywgZXNwZWNpYWxseSBmb3IgbGlicmFyaWVzIGxpa2UgTURDLlxuICovXG5leHBvcnQgZnVuY3Rpb24gY2xvc2VzdChlbGVtZW50LCBzZWxlY3Rvcikge1xuICAgIGlmIChlbGVtZW50LmNsb3Nlc3QpIHtcbiAgICAgICAgcmV0dXJuIGVsZW1lbnQuY2xvc2VzdChzZWxlY3Rvcik7XG4gICAgfVxuICAgIHZhciBlbCA9IGVsZW1lbnQ7XG4gICAgd2hpbGUgKGVsKSB7XG4gICAgICAgIGlmIChtYXRjaGVzKGVsLCBzZWxlY3RvcikpIHtcbiAgICAgICAgICAgIHJldHVybiBlbDtcbiAgICAgICAgfVxuICAgICAgICBlbCA9IGVsLnBhcmVudEVsZW1lbnQ7XG4gICAgfVxuICAgIHJldHVybiBudWxsO1xufVxuZXhwb3J0IGZ1bmN0aW9uIG1hdGNoZXMoZWxlbWVudCwgc2VsZWN0b3IpIHtcbiAgICB2YXIgbmF0aXZlTWF0Y2hlcyA9IGVsZW1lbnQubWF0Y2hlc1xuICAgICAgICB8fCBlbGVtZW50LndlYmtpdE1hdGNoZXNTZWxlY3RvclxuICAgICAgICB8fCBlbGVtZW50Lm1zTWF0Y2hlc1NlbGVjdG9yO1xuICAgIHJldHVybiBuYXRpdmVNYXRjaGVzLmNhbGwoZWxlbWVudCwgc2VsZWN0b3IpO1xufVxuLyoqXG4gKiBVc2VkIHRvIGNvbXB1dGUgdGhlIGVzdGltYXRlZCBzY3JvbGwgd2lkdGggb2YgZWxlbWVudHMuIFdoZW4gYW4gZWxlbWVudCBpc1xuICogaGlkZGVuIGR1ZSB0byBkaXNwbGF5OiBub25lOyBiZWluZyBhcHBsaWVkIHRvIGEgcGFyZW50IGVsZW1lbnQsIHRoZSB3aWR0aCBpc1xuICogcmV0dXJuZWQgYXMgMC4gSG93ZXZlciwgdGhlIGVsZW1lbnQgd2lsbCBoYXZlIGEgdHJ1ZSB3aWR0aCBvbmNlIG5vIGxvbmdlclxuICogaW5zaWRlIGEgZGlzcGxheTogbm9uZSBjb250ZXh0LiBUaGlzIG1ldGhvZCBjb21wdXRlcyBhbiBlc3RpbWF0ZWQgd2lkdGggd2hlblxuICogdGhlIGVsZW1lbnQgaXMgaGlkZGVuIG9yIHJldHVybnMgdGhlIHRydWUgd2lkdGggd2hlbiB0aGUgZWxlbWVudCBpcyB2aXNibGUuXG4gKiBAcGFyYW0ge0VsZW1lbnR9IGVsZW1lbnQgdGhlIGVsZW1lbnQgd2hvc2Ugd2lkdGggdG8gZXN0aW1hdGVcbiAqL1xuZXhwb3J0IGZ1bmN0aW9uIGVzdGltYXRlU2Nyb2xsV2lkdGgoZWxlbWVudCkge1xuICAgIC8vIENoZWNrIHRoZSBvZmZzZXRQYXJlbnQuIElmIHRoZSBlbGVtZW50IGluaGVyaXRzIGRpc3BsYXk6IG5vbmUgZnJvbSBhbnlcbiAgICAvLyBwYXJlbnQsIHRoZSBvZmZzZXRQYXJlbnQgcHJvcGVydHkgd2lsbCBiZSBudWxsIChzZWVcbiAgICAvLyBodHRwczovL2RldmVsb3Blci5tb3ppbGxhLm9yZy9lbi1VUy9kb2NzL1dlYi9BUEkvSFRNTEVsZW1lbnQvb2Zmc2V0UGFyZW50KS5cbiAgICAvLyBUaGlzIGNoZWNrIGVuc3VyZXMgd2Ugb25seSBjbG9uZSB0aGUgbm9kZSB3aGVuIG5lY2Vzc2FyeS5cbiAgICB2YXIgaHRtbEVsID0gZWxlbWVudDtcbiAgICBpZiAoaHRtbEVsLm9mZnNldFBhcmVudCAhPT0gbnVsbCkge1xuICAgICAgICByZXR1cm4gaHRtbEVsLnNjcm9sbFdpZHRoO1xuICAgIH1cbiAgICB2YXIgY2xvbmUgPSBodG1sRWwuY2xvbmVOb2RlKHRydWUpO1xuICAgIGNsb25lLnN0eWxlLnNldFByb3BlcnR5KCdwb3NpdGlvbicsICdhYnNvbHV0ZScpO1xuICAgIGNsb25lLnN0eWxlLnNldFByb3BlcnR5KCd0cmFuc2Zvcm0nLCAndHJhbnNsYXRlKC05OTk5cHgsIC05OTk5cHgpJyk7XG4gICAgZG9jdW1lbnQuZG9jdW1lbnRFbGVtZW50LmFwcGVuZENoaWxkKGNsb25lKTtcbiAgICB2YXIgc2Nyb2xsV2lkdGggPSBjbG9uZS5zY3JvbGxXaWR0aDtcbiAgICBkb2N1bWVudC5kb2N1bWVudEVsZW1lbnQucmVtb3ZlQ2hpbGQoY2xvbmUpO1xuICAgIHJldHVybiBzY3JvbGxXaWR0aDtcbn1cbi8vIyBzb3VyY2VNYXBwaW5nVVJMPXBvbnlmaWxsLmpzLm1hcCIsIi8qKlxuICogQGxpY2Vuc2VcbiAqIENvcHlyaWdodCAyMDE4IEdvb2dsZSBJbmMuXG4gKlxuICogUGVybWlzc2lvbiBpcyBoZXJlYnkgZ3JhbnRlZCwgZnJlZSBvZiBjaGFyZ2UsIHRvIGFueSBwZXJzb24gb2J0YWluaW5nIGEgY29weVxuICogb2YgdGhpcyBzb2Z0d2FyZSBhbmQgYXNzb2NpYXRlZCBkb2N1bWVudGF0aW9uIGZpbGVzICh0aGUgXCJTb2Z0d2FyZVwiKSwgdG8gZGVhbFxuICogaW4gdGhlIFNvZnR3YXJlIHdpdGhvdXQgcmVzdHJpY3Rpb24sIGluY2x1ZGluZyB3aXRob3V0IGxpbWl0YXRpb24gdGhlIHJpZ2h0c1xuICogdG8gdXNlLCBjb3B5LCBtb2RpZnksIG1lcmdlLCBwdWJsaXNoLCBkaXN0cmlidXRlLCBzdWJsaWNlbnNlLCBhbmQvb3Igc2VsbFxuICogY29waWVzIG9mIHRoZSBTb2Z0d2FyZSwgYW5kIHRvIHBlcm1pdCBwZXJzb25zIHRvIHdob20gdGhlIFNvZnR3YXJlIGlzXG4gKiBmdXJuaXNoZWQgdG8gZG8gc28sIHN1YmplY3QgdG8gdGhlIGZvbGxvd2luZyBjb25kaXRpb25zOlxuICpcbiAqIFRoZSBhYm92ZSBjb3B5cmlnaHQgbm90aWNlIGFuZCB0aGlzIHBlcm1pc3Npb24gbm90aWNlIHNoYWxsIGJlIGluY2x1ZGVkIGluXG4gKiBhbGwgY29waWVzIG9yIHN1YnN0YW50aWFsIHBvcnRpb25zIG9mIHRoZSBTb2Z0d2FyZS5cbiAqXG4gKiBUSEUgU09GVFdBUkUgSVMgUFJPVklERUQgXCJBUyBJU1wiLCBXSVRIT1VUIFdBUlJBTlRZIE9GIEFOWSBLSU5ELCBFWFBSRVNTIE9SXG4gKiBJTVBMSUVELCBJTkNMVURJTkcgQlVUIE5PVCBMSU1JVEVEIFRPIFRIRSBXQVJSQU5USUVTIE9GIE1FUkNIQU5UQUJJTElUWSxcbiAqIEZJVE5FU1MgRk9SIEEgUEFSVElDVUxBUiBQVVJQT1NFIEFORCBOT05JTkZSSU5HRU1FTlQuIElOIE5PIEVWRU5UIFNIQUxMIFRIRVxuICogQVVUSE9SUyBPUiBDT1BZUklHSFQgSE9MREVSUyBCRSBMSUFCTEUgRk9SIEFOWSBDTEFJTSwgREFNQUdFUyBPUiBPVEhFUlxuICogTElBQklMSVRZLCBXSEVUSEVSIElOIEFOIEFDVElPTiBPRiBDT05UUkFDVCwgVE9SVCBPUiBPVEhFUldJU0UsIEFSSVNJTkcgRlJPTSxcbiAqIE9VVCBPRiBPUiBJTiBDT05ORUNUSU9OIFdJVEggVEhFIFNPRlRXQVJFIE9SIFRIRSBVU0UgT1IgT1RIRVIgREVBTElOR1MgSU5cbiAqIFRIRSBTT0ZUV0FSRS5cbiAqL1xuaW1wb3J0IHsgX19leHRlbmRzIH0gZnJvbSBcInRzbGliXCI7XG5pbXBvcnQgeyBNRENDb21wb25lbnQgfSBmcm9tICdAbWF0ZXJpYWwvYmFzZS9jb21wb25lbnQnO1xuaW1wb3J0IHsgY2xvc2VzdCB9IGZyb20gJ0BtYXRlcmlhbC9kb20vcG9ueWZpbGwnO1xuaW1wb3J0IHsgc3RyaW5ncyB9IGZyb20gJy4vY29uc3RhbnRzJztcbmltcG9ydCB7IE1EQ1NuYWNrYmFyRm91bmRhdGlvbiB9IGZyb20gJy4vZm91bmRhdGlvbic7XG5pbXBvcnQgKiBhcyB1dGlsIGZyb20gJy4vdXRpbCc7XG52YXIgU1VSRkFDRV9TRUxFQ1RPUiA9IHN0cmluZ3MuU1VSRkFDRV9TRUxFQ1RPUiwgTEFCRUxfU0VMRUNUT1IgPSBzdHJpbmdzLkxBQkVMX1NFTEVDVE9SLCBBQ1RJT05fU0VMRUNUT1IgPSBzdHJpbmdzLkFDVElPTl9TRUxFQ1RPUiwgRElTTUlTU19TRUxFQ1RPUiA9IHN0cmluZ3MuRElTTUlTU19TRUxFQ1RPUiwgT1BFTklOR19FVkVOVCA9IHN0cmluZ3MuT1BFTklOR19FVkVOVCwgT1BFTkVEX0VWRU5UID0gc3RyaW5ncy5PUEVORURfRVZFTlQsIENMT1NJTkdfRVZFTlQgPSBzdHJpbmdzLkNMT1NJTkdfRVZFTlQsIENMT1NFRF9FVkVOVCA9IHN0cmluZ3MuQ0xPU0VEX0VWRU5UO1xudmFyIE1EQ1NuYWNrYmFyID0gLyoqIEBjbGFzcyAqLyAoZnVuY3Rpb24gKF9zdXBlcikge1xuICAgIF9fZXh0ZW5kcyhNRENTbmFja2JhciwgX3N1cGVyKTtcbiAgICBmdW5jdGlvbiBNRENTbmFja2JhcigpIHtcbiAgICAgICAgcmV0dXJuIF9zdXBlciAhPT0gbnVsbCAmJiBfc3VwZXIuYXBwbHkodGhpcywgYXJndW1lbnRzKSB8fCB0aGlzO1xuICAgIH1cbiAgICBNRENTbmFja2Jhci5hdHRhY2hUbyA9IGZ1bmN0aW9uIChyb290KSB7XG4gICAgICAgIHJldHVybiBuZXcgTURDU25hY2tiYXIocm9vdCk7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuaW5pdGlhbGl6ZSA9IGZ1bmN0aW9uIChhbm5vdW5jZXJGYWN0b3J5KSB7XG4gICAgICAgIGlmIChhbm5vdW5jZXJGYWN0b3J5ID09PSB2b2lkIDApIHsgYW5ub3VuY2VyRmFjdG9yeSA9IGZ1bmN0aW9uICgpIHsgcmV0dXJuIHV0aWwuYW5ub3VuY2U7IH07IH1cbiAgICAgICAgdGhpcy5hbm5vdW5jZV8gPSBhbm5vdW5jZXJGYWN0b3J5KCk7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuaW5pdGlhbFN5bmNXaXRoRE9NID0gZnVuY3Rpb24gKCkge1xuICAgICAgICB2YXIgX3RoaXMgPSB0aGlzO1xuICAgICAgICB0aGlzLnN1cmZhY2VFbF8gPSB0aGlzLnJvb3QucXVlcnlTZWxlY3RvcihTVVJGQUNFX1NFTEVDVE9SKTtcbiAgICAgICAgdGhpcy5sYWJlbEVsXyA9IHRoaXMucm9vdC5xdWVyeVNlbGVjdG9yKExBQkVMX1NFTEVDVE9SKTtcbiAgICAgICAgdGhpcy5hY3Rpb25FbF8gPSB0aGlzLnJvb3QucXVlcnlTZWxlY3RvcihBQ1RJT05fU0VMRUNUT1IpO1xuICAgICAgICB0aGlzLmhhbmRsZUtleURvd25fID0gZnVuY3Rpb24gKGV2dCkgeyByZXR1cm4gX3RoaXMuZm91bmRhdGlvbi5oYW5kbGVLZXlEb3duKGV2dCk7IH07XG4gICAgICAgIHRoaXMuaGFuZGxlU3VyZmFjZUNsaWNrXyA9IGZ1bmN0aW9uIChldnQpIHtcbiAgICAgICAgICAgIHZhciB0YXJnZXQgPSBldnQudGFyZ2V0O1xuICAgICAgICAgICAgaWYgKF90aGlzLmlzQWN0aW9uQnV0dG9uXyh0YXJnZXQpKSB7XG4gICAgICAgICAgICAgICAgX3RoaXMuZm91bmRhdGlvbi5oYW5kbGVBY3Rpb25CdXR0b25DbGljayhldnQpO1xuICAgICAgICAgICAgfVxuICAgICAgICAgICAgZWxzZSBpZiAoX3RoaXMuaXNBY3Rpb25JY29uXyh0YXJnZXQpKSB7XG4gICAgICAgICAgICAgICAgX3RoaXMuZm91bmRhdGlvbi5oYW5kbGVBY3Rpb25JY29uQ2xpY2soZXZ0KTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgfTtcbiAgICAgICAgdGhpcy5yZWdpc3RlcktleURvd25IYW5kbGVyXyh0aGlzLmhhbmRsZUtleURvd25fKTtcbiAgICAgICAgdGhpcy5yZWdpc3RlclN1cmZhY2VDbGlja0hhbmRsZXJfKHRoaXMuaGFuZGxlU3VyZmFjZUNsaWNrXyk7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuZGVzdHJveSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgX3N1cGVyLnByb3RvdHlwZS5kZXN0cm95LmNhbGwodGhpcyk7XG4gICAgICAgIHRoaXMuZGVyZWdpc3RlcktleURvd25IYW5kbGVyXyh0aGlzLmhhbmRsZUtleURvd25fKTtcbiAgICAgICAgdGhpcy5kZXJlZ2lzdGVyU3VyZmFjZUNsaWNrSGFuZGxlcl8odGhpcy5oYW5kbGVTdXJmYWNlQ2xpY2tfKTtcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyLnByb3RvdHlwZS5vcGVuID0gZnVuY3Rpb24gKCkge1xuICAgICAgICB0aGlzLmZvdW5kYXRpb24ub3BlbigpO1xuICAgIH07XG4gICAgLyoqXG4gICAgICogQHBhcmFtIHJlYXNvbiBXaHkgdGhlIHNuYWNrYmFyIHdhcyBjbG9zZWQuIFZhbHVlIHdpbGwgYmUgcGFzc2VkIHRvIENMT1NJTkdfRVZFTlQgYW5kIENMT1NFRF9FVkVOVCB2aWEgdGhlXG4gICAgICogICAgIGBldmVudC5kZXRhaWwucmVhc29uYCBwcm9wZXJ0eS4gU3RhbmRhcmQgdmFsdWVzIGFyZSBSRUFTT05fQUNUSU9OIGFuZCBSRUFTT05fRElTTUlTUywgYnV0IGN1c3RvbVxuICAgICAqICAgICBjbGllbnQtc3BlY2lmaWMgdmFsdWVzIG1heSBhbHNvIGJlIHVzZWQgaWYgZGVzaXJlZC5cbiAgICAgKi9cbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuY2xvc2UgPSBmdW5jdGlvbiAocmVhc29uKSB7XG4gICAgICAgIGlmIChyZWFzb24gPT09IHZvaWQgMCkgeyByZWFzb24gPSAnJzsgfVxuICAgICAgICB0aGlzLmZvdW5kYXRpb24uY2xvc2UocmVhc29uKTtcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyLnByb3RvdHlwZS5nZXREZWZhdWx0Rm91bmRhdGlvbiA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgdmFyIF90aGlzID0gdGhpcztcbiAgICAgICAgLy8gRE8gTk9UIElOTElORSB0aGlzIHZhcmlhYmxlLiBGb3IgYmFja3dhcmQgY29tcGF0aWJpbGl0eSwgZm91bmRhdGlvbnMgdGFrZSBhIFBhcnRpYWw8TURDRm9vQWRhcHRlcj4uXG4gICAgICAgIC8vIFRvIGVuc3VyZSB3ZSBkb24ndCBhY2NpZGVudGFsbHkgb21pdCBhbnkgbWV0aG9kcywgd2UgbmVlZCBhIHNlcGFyYXRlLCBzdHJvbmdseSB0eXBlZCBhZGFwdGVyIHZhcmlhYmxlLlxuICAgICAgICB2YXIgYWRhcHRlciA9IHtcbiAgICAgICAgICAgIGFkZENsYXNzOiBmdW5jdGlvbiAoY2xhc3NOYW1lKSB7IHJldHVybiBfdGhpcy5yb290LmNsYXNzTGlzdC5hZGQoY2xhc3NOYW1lKTsgfSxcbiAgICAgICAgICAgIGFubm91bmNlOiBmdW5jdGlvbiAoKSB7IHJldHVybiBfdGhpcy5hbm5vdW5jZV8oX3RoaXMubGFiZWxFbF8pOyB9LFxuICAgICAgICAgICAgbm90aWZ5Q2xvc2VkOiBmdW5jdGlvbiAocmVhc29uKSB7IHJldHVybiBfdGhpcy5lbWl0KENMT1NFRF9FVkVOVCwgcmVhc29uID8geyByZWFzb246IHJlYXNvbiB9IDoge30pOyB9LFxuICAgICAgICAgICAgbm90aWZ5Q2xvc2luZzogZnVuY3Rpb24gKHJlYXNvbikgeyByZXR1cm4gX3RoaXMuZW1pdChDTE9TSU5HX0VWRU5ULCByZWFzb24gPyB7IHJlYXNvbjogcmVhc29uIH0gOiB7fSk7IH0sXG4gICAgICAgICAgICBub3RpZnlPcGVuZWQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIF90aGlzLmVtaXQoT1BFTkVEX0VWRU5ULCB7fSk7IH0sXG4gICAgICAgICAgICBub3RpZnlPcGVuaW5nOiBmdW5jdGlvbiAoKSB7IHJldHVybiBfdGhpcy5lbWl0KE9QRU5JTkdfRVZFTlQsIHt9KTsgfSxcbiAgICAgICAgICAgIHJlbW92ZUNsYXNzOiBmdW5jdGlvbiAoY2xhc3NOYW1lKSB7IHJldHVybiBfdGhpcy5yb290LmNsYXNzTGlzdC5yZW1vdmUoY2xhc3NOYW1lKTsgfSxcbiAgICAgICAgfTtcbiAgICAgICAgcmV0dXJuIG5ldyBNRENTbmFja2JhckZvdW5kYXRpb24oYWRhcHRlcik7XG4gICAgfTtcbiAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkoTURDU25hY2tiYXIucHJvdG90eXBlLCBcInRpbWVvdXRNc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIHRoaXMuZm91bmRhdGlvbi5nZXRUaW1lb3V0TXMoKTtcbiAgICAgICAgfSxcbiAgICAgICAgc2V0OiBmdW5jdGlvbiAodGltZW91dE1zKSB7XG4gICAgICAgICAgICB0aGlzLmZvdW5kYXRpb24uc2V0VGltZW91dE1zKHRpbWVvdXRNcyk7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2Jhci5wcm90b3R5cGUsIFwiY2xvc2VPbkVzY2FwZVwiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIHRoaXMuZm91bmRhdGlvbi5nZXRDbG9zZU9uRXNjYXBlKCk7XG4gICAgICAgIH0sXG4gICAgICAgIHNldDogZnVuY3Rpb24gKGNsb3NlT25Fc2NhcGUpIHtcbiAgICAgICAgICAgIHRoaXMuZm91bmRhdGlvbi5zZXRDbG9zZU9uRXNjYXBlKGNsb3NlT25Fc2NhcGUpO1xuICAgICAgICB9LFxuICAgICAgICBlbnVtZXJhYmxlOiB0cnVlLFxuICAgICAgICBjb25maWd1cmFibGU6IHRydWVcbiAgICB9KTtcbiAgICBPYmplY3QuZGVmaW5lUHJvcGVydHkoTURDU25hY2tiYXIucHJvdG90eXBlLCBcImlzT3BlblwiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIHRoaXMuZm91bmRhdGlvbi5pc09wZW4oKTtcbiAgICAgICAgfSxcbiAgICAgICAgZW51bWVyYWJsZTogdHJ1ZSxcbiAgICAgICAgY29uZmlndXJhYmxlOiB0cnVlXG4gICAgfSk7XG4gICAgT2JqZWN0LmRlZmluZVByb3BlcnR5KE1EQ1NuYWNrYmFyLnByb3RvdHlwZSwgXCJsYWJlbFRleHRcIiwge1xuICAgICAgICBnZXQ6IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgICAgIC8vIFRoaXMgcHJvcGVydHkgb25seSByZXR1cm5zIG51bGwgaWYgdGhlIG5vZGUgaXMgYSBkb2N1bWVudCwgRE9DVFlQRSwgb3Igbm90YXRpb24uXG4gICAgICAgICAgICAvLyBPbiBFbGVtZW50IG5vZGVzLCBpdCBhbHdheXMgcmV0dXJucyBhIHN0cmluZy5cbiAgICAgICAgICAgIHJldHVybiB0aGlzLmxhYmVsRWxfLnRleHRDb250ZW50O1xuICAgICAgICB9LFxuICAgICAgICBzZXQ6IGZ1bmN0aW9uIChsYWJlbFRleHQpIHtcbiAgICAgICAgICAgIHRoaXMubGFiZWxFbF8udGV4dENvbnRlbnQgPSBsYWJlbFRleHQ7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2Jhci5wcm90b3R5cGUsIFwiYWN0aW9uQnV0dG9uVGV4dFwiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIHRoaXMuYWN0aW9uRWxfLnRleHRDb250ZW50O1xuICAgICAgICB9LFxuICAgICAgICBzZXQ6IGZ1bmN0aW9uIChhY3Rpb25CdXR0b25UZXh0KSB7XG4gICAgICAgICAgICB0aGlzLmFjdGlvbkVsXy50ZXh0Q29udGVudCA9IGFjdGlvbkJ1dHRvblRleHQ7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE1EQ1NuYWNrYmFyLnByb3RvdHlwZS5yZWdpc3RlcktleURvd25IYW5kbGVyXyA9IGZ1bmN0aW9uIChoYW5kbGVyKSB7XG4gICAgICAgIHRoaXMubGlzdGVuKCdrZXlkb3duJywgaGFuZGxlcik7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuZGVyZWdpc3RlcktleURvd25IYW5kbGVyXyA9IGZ1bmN0aW9uIChoYW5kbGVyKSB7XG4gICAgICAgIHRoaXMudW5saXN0ZW4oJ2tleWRvd24nLCBoYW5kbGVyKTtcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyLnByb3RvdHlwZS5yZWdpc3RlclN1cmZhY2VDbGlja0hhbmRsZXJfID0gZnVuY3Rpb24gKGhhbmRsZXIpIHtcbiAgICAgICAgdGhpcy5zdXJmYWNlRWxfLmFkZEV2ZW50TGlzdGVuZXIoJ2NsaWNrJywgaGFuZGxlcik7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuZGVyZWdpc3RlclN1cmZhY2VDbGlja0hhbmRsZXJfID0gZnVuY3Rpb24gKGhhbmRsZXIpIHtcbiAgICAgICAgdGhpcy5zdXJmYWNlRWxfLnJlbW92ZUV2ZW50TGlzdGVuZXIoJ2NsaWNrJywgaGFuZGxlcik7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuaXNBY3Rpb25CdXR0b25fID0gZnVuY3Rpb24gKHRhcmdldCkge1xuICAgICAgICByZXR1cm4gQm9vbGVhbihjbG9zZXN0KHRhcmdldCwgQUNUSU9OX1NFTEVDVE9SKSk7XG4gICAgfTtcbiAgICBNRENTbmFja2Jhci5wcm90b3R5cGUuaXNBY3Rpb25JY29uXyA9IGZ1bmN0aW9uICh0YXJnZXQpIHtcbiAgICAgICAgcmV0dXJuIEJvb2xlYW4oY2xvc2VzdCh0YXJnZXQsIERJU01JU1NfU0VMRUNUT1IpKTtcbiAgICB9O1xuICAgIHJldHVybiBNRENTbmFja2Jhcjtcbn0oTURDQ29tcG9uZW50KSk7XG5leHBvcnQgeyBNRENTbmFja2JhciB9O1xuLy8jIHNvdXJjZU1hcHBpbmdVUkw9Y29tcG9uZW50LmpzLm1hcCIsIi8qKlxuICogQGxpY2Vuc2VcbiAqIENvcHlyaWdodCAyMDE4IEdvb2dsZSBJbmMuXG4gKlxuICogUGVybWlzc2lvbiBpcyBoZXJlYnkgZ3JhbnRlZCwgZnJlZSBvZiBjaGFyZ2UsIHRvIGFueSBwZXJzb24gb2J0YWluaW5nIGEgY29weVxuICogb2YgdGhpcyBzb2Z0d2FyZSBhbmQgYXNzb2NpYXRlZCBkb2N1bWVudGF0aW9uIGZpbGVzICh0aGUgXCJTb2Z0d2FyZVwiKSwgdG8gZGVhbFxuICogaW4gdGhlIFNvZnR3YXJlIHdpdGhvdXQgcmVzdHJpY3Rpb24sIGluY2x1ZGluZyB3aXRob3V0IGxpbWl0YXRpb24gdGhlIHJpZ2h0c1xuICogdG8gdXNlLCBjb3B5LCBtb2RpZnksIG1lcmdlLCBwdWJsaXNoLCBkaXN0cmlidXRlLCBzdWJsaWNlbnNlLCBhbmQvb3Igc2VsbFxuICogY29waWVzIG9mIHRoZSBTb2Z0d2FyZSwgYW5kIHRvIHBlcm1pdCBwZXJzb25zIHRvIHdob20gdGhlIFNvZnR3YXJlIGlzXG4gKiBmdXJuaXNoZWQgdG8gZG8gc28sIHN1YmplY3QgdG8gdGhlIGZvbGxvd2luZyBjb25kaXRpb25zOlxuICpcbiAqIFRoZSBhYm92ZSBjb3B5cmlnaHQgbm90aWNlIGFuZCB0aGlzIHBlcm1pc3Npb24gbm90aWNlIHNoYWxsIGJlIGluY2x1ZGVkIGluXG4gKiBhbGwgY29waWVzIG9yIHN1YnN0YW50aWFsIHBvcnRpb25zIG9mIHRoZSBTb2Z0d2FyZS5cbiAqXG4gKiBUSEUgU09GVFdBUkUgSVMgUFJPVklERUQgXCJBUyBJU1wiLCBXSVRIT1VUIFdBUlJBTlRZIE9GIEFOWSBLSU5ELCBFWFBSRVNTIE9SXG4gKiBJTVBMSUVELCBJTkNMVURJTkcgQlVUIE5PVCBMSU1JVEVEIFRPIFRIRSBXQVJSQU5USUVTIE9GIE1FUkNIQU5UQUJJTElUWSxcbiAqIEZJVE5FU1MgRk9SIEEgUEFSVElDVUxBUiBQVVJQT1NFIEFORCBOT05JTkZSSU5HRU1FTlQuIElOIE5PIEVWRU5UIFNIQUxMIFRIRVxuICogQVVUSE9SUyBPUiBDT1BZUklHSFQgSE9MREVSUyBCRSBMSUFCTEUgRk9SIEFOWSBDTEFJTSwgREFNQUdFUyBPUiBPVEhFUlxuICogTElBQklMSVRZLCBXSEVUSEVSIElOIEFOIEFDVElPTiBPRiBDT05UUkFDVCwgVE9SVCBPUiBPVEhFUldJU0UsIEFSSVNJTkcgRlJPTSxcbiAqIE9VVCBPRiBPUiBJTiBDT05ORUNUSU9OIFdJVEggVEhFIFNPRlRXQVJFIE9SIFRIRSBVU0UgT1IgT1RIRVIgREVBTElOR1MgSU5cbiAqIFRIRSBTT0ZUV0FSRS5cbiAqL1xudmFyIGNzc0NsYXNzZXMgPSB7XG4gICAgQ0xPU0lORzogJ21kYy1zbmFja2Jhci0tY2xvc2luZycsXG4gICAgT1BFTjogJ21kYy1zbmFja2Jhci0tb3BlbicsXG4gICAgT1BFTklORzogJ21kYy1zbmFja2Jhci0tb3BlbmluZycsXG59O1xudmFyIHN0cmluZ3MgPSB7XG4gICAgQUNUSU9OX1NFTEVDVE9SOiAnLm1kYy1zbmFja2Jhcl9fYWN0aW9uJyxcbiAgICBBUklBX0xJVkVfTEFCRUxfVEVYVF9BVFRSOiAnZGF0YS1tZGMtc25hY2tiYXItbGFiZWwtdGV4dCcsXG4gICAgQ0xPU0VEX0VWRU5UOiAnTURDU25hY2tiYXI6Y2xvc2VkJyxcbiAgICBDTE9TSU5HX0VWRU5UOiAnTURDU25hY2tiYXI6Y2xvc2luZycsXG4gICAgRElTTUlTU19TRUxFQ1RPUjogJy5tZGMtc25hY2tiYXJfX2Rpc21pc3MnLFxuICAgIExBQkVMX1NFTEVDVE9SOiAnLm1kYy1zbmFja2Jhcl9fbGFiZWwnLFxuICAgIE9QRU5FRF9FVkVOVDogJ01EQ1NuYWNrYmFyOm9wZW5lZCcsXG4gICAgT1BFTklOR19FVkVOVDogJ01EQ1NuYWNrYmFyOm9wZW5pbmcnLFxuICAgIFJFQVNPTl9BQ1RJT046ICdhY3Rpb24nLFxuICAgIFJFQVNPTl9ESVNNSVNTOiAnZGlzbWlzcycsXG4gICAgU1VSRkFDRV9TRUxFQ1RPUjogJy5tZGMtc25hY2tiYXJfX3N1cmZhY2UnLFxufTtcbnZhciBudW1iZXJzID0ge1xuICAgIERFRkFVTFRfQVVUT19ESVNNSVNTX1RJTUVPVVRfTVM6IDUwMDAsXG4gICAgSU5ERVRFUk1JTkFURTogLTEsXG4gICAgTUFYX0FVVE9fRElTTUlTU19USU1FT1VUX01TOiAxMDAwMCxcbiAgICBNSU5fQVVUT19ESVNNSVNTX1RJTUVPVVRfTVM6IDQwMDAsXG4gICAgLy8gVGhlc2UgdmFyaWFibGVzIG5lZWQgdG8gYmUga2VwdCBpbiBzeW5jIHdpdGggdGhlIHZhbHVlcyBpbiBfdmFyaWFibGVzLnNjc3MuXG4gICAgU05BQ0tCQVJfQU5JTUFUSU9OX0NMT1NFX1RJTUVfTVM6IDc1LFxuICAgIFNOQUNLQkFSX0FOSU1BVElPTl9PUEVOX1RJTUVfTVM6IDE1MCxcbiAgICAvKipcbiAgICAgKiBOdW1iZXIgb2YgbWlsbGlzZWNvbmRzIHRvIHdhaXQgYmV0d2VlbiB0ZW1wb3JhcmlseSBjbGVhcmluZyB0aGUgbGFiZWwgdGV4dFxuICAgICAqIGluIHRoZSBET00gYW5kIHN1YnNlcXVlbnRseSByZXN0b3JpbmcgaXQuIFRoaXMgaXMgbmVjZXNzYXJ5IHRvIGZvcmNlIElFIDExXG4gICAgICogdG8gcGljayB1cCB0aGUgYGFyaWEtbGl2ZWAgY29udGVudCBjaGFuZ2UgYW5kIGFubm91bmNlIGl0IHRvIHRoZSB1c2VyLlxuICAgICAqL1xuICAgIEFSSUFfTElWRV9ERUxBWV9NUzogMTAwMCxcbn07XG5leHBvcnQgeyBjc3NDbGFzc2VzLCBzdHJpbmdzLCBudW1iZXJzIH07XG4vLyMgc291cmNlTWFwcGluZ1VSTD1jb25zdGFudHMuanMubWFwIiwiLyoqXG4gKiBAbGljZW5zZVxuICogQ29weXJpZ2h0IDIwMTggR29vZ2xlIEluYy5cbiAqXG4gKiBQZXJtaXNzaW9uIGlzIGhlcmVieSBncmFudGVkLCBmcmVlIG9mIGNoYXJnZSwgdG8gYW55IHBlcnNvbiBvYnRhaW5pbmcgYSBjb3B5XG4gKiBvZiB0aGlzIHNvZnR3YXJlIGFuZCBhc3NvY2lhdGVkIGRvY3VtZW50YXRpb24gZmlsZXMgKHRoZSBcIlNvZnR3YXJlXCIpLCB0byBkZWFsXG4gKiBpbiB0aGUgU29mdHdhcmUgd2l0aG91dCByZXN0cmljdGlvbiwgaW5jbHVkaW5nIHdpdGhvdXQgbGltaXRhdGlvbiB0aGUgcmlnaHRzXG4gKiB0byB1c2UsIGNvcHksIG1vZGlmeSwgbWVyZ2UsIHB1Ymxpc2gsIGRpc3RyaWJ1dGUsIHN1YmxpY2Vuc2UsIGFuZC9vciBzZWxsXG4gKiBjb3BpZXMgb2YgdGhlIFNvZnR3YXJlLCBhbmQgdG8gcGVybWl0IHBlcnNvbnMgdG8gd2hvbSB0aGUgU29mdHdhcmUgaXNcbiAqIGZ1cm5pc2hlZCB0byBkbyBzbywgc3ViamVjdCB0byB0aGUgZm9sbG93aW5nIGNvbmRpdGlvbnM6XG4gKlxuICogVGhlIGFib3ZlIGNvcHlyaWdodCBub3RpY2UgYW5kIHRoaXMgcGVybWlzc2lvbiBub3RpY2Ugc2hhbGwgYmUgaW5jbHVkZWQgaW5cbiAqIGFsbCBjb3BpZXMgb3Igc3Vic3RhbnRpYWwgcG9ydGlvbnMgb2YgdGhlIFNvZnR3YXJlLlxuICpcbiAqIFRIRSBTT0ZUV0FSRSBJUyBQUk9WSURFRCBcIkFTIElTXCIsIFdJVEhPVVQgV0FSUkFOVFkgT0YgQU5ZIEtJTkQsIEVYUFJFU1MgT1JcbiAqIElNUExJRUQsIElOQ0xVRElORyBCVVQgTk9UIExJTUlURUQgVE8gVEhFIFdBUlJBTlRJRVMgT0YgTUVSQ0hBTlRBQklMSVRZLFxuICogRklUTkVTUyBGT1IgQSBQQVJUSUNVTEFSIFBVUlBPU0UgQU5EIE5PTklORlJJTkdFTUVOVC4gSU4gTk8gRVZFTlQgU0hBTEwgVEhFXG4gKiBBVVRIT1JTIE9SIENPUFlSSUdIVCBIT0xERVJTIEJFIExJQUJMRSBGT1IgQU5ZIENMQUlNLCBEQU1BR0VTIE9SIE9USEVSXG4gKiBMSUFCSUxJVFksIFdIRVRIRVIgSU4gQU4gQUNUSU9OIE9GIENPTlRSQUNULCBUT1JUIE9SIE9USEVSV0lTRSwgQVJJU0lORyBGUk9NLFxuICogT1VUIE9GIE9SIElOIENPTk5FQ1RJT04gV0lUSCBUSEUgU09GVFdBUkUgT1IgVEhFIFVTRSBPUiBPVEhFUiBERUFMSU5HUyBJTlxuICogVEhFIFNPRlRXQVJFLlxuICovXG5pbXBvcnQgeyBfX2Fzc2lnbiwgX19leHRlbmRzIH0gZnJvbSBcInRzbGliXCI7XG5pbXBvcnQgeyBNRENGb3VuZGF0aW9uIH0gZnJvbSAnQG1hdGVyaWFsL2Jhc2UvZm91bmRhdGlvbic7XG5pbXBvcnQgeyBjc3NDbGFzc2VzLCBudW1iZXJzLCBzdHJpbmdzIH0gZnJvbSAnLi9jb25zdGFudHMnO1xudmFyIE9QRU5JTkcgPSBjc3NDbGFzc2VzLk9QRU5JTkcsIE9QRU4gPSBjc3NDbGFzc2VzLk9QRU4sIENMT1NJTkcgPSBjc3NDbGFzc2VzLkNMT1NJTkc7XG52YXIgUkVBU09OX0FDVElPTiA9IHN0cmluZ3MuUkVBU09OX0FDVElPTiwgUkVBU09OX0RJU01JU1MgPSBzdHJpbmdzLlJFQVNPTl9ESVNNSVNTO1xudmFyIE1EQ1NuYWNrYmFyRm91bmRhdGlvbiA9IC8qKiBAY2xhc3MgKi8gKGZ1bmN0aW9uIChfc3VwZXIpIHtcbiAgICBfX2V4dGVuZHMoTURDU25hY2tiYXJGb3VuZGF0aW9uLCBfc3VwZXIpO1xuICAgIGZ1bmN0aW9uIE1EQ1NuYWNrYmFyRm91bmRhdGlvbihhZGFwdGVyKSB7XG4gICAgICAgIHZhciBfdGhpcyA9IF9zdXBlci5jYWxsKHRoaXMsIF9fYXNzaWduKF9fYXNzaWduKHt9LCBNRENTbmFja2JhckZvdW5kYXRpb24uZGVmYXVsdEFkYXB0ZXIpLCBhZGFwdGVyKSkgfHwgdGhpcztcbiAgICAgICAgX3RoaXMuaXNPcGVuXyA9IGZhbHNlO1xuICAgICAgICBfdGhpcy5hbmltYXRpb25GcmFtZV8gPSAwO1xuICAgICAgICBfdGhpcy5hbmltYXRpb25UaW1lcl8gPSAwO1xuICAgICAgICBfdGhpcy5hdXRvRGlzbWlzc1RpbWVyXyA9IDA7XG4gICAgICAgIF90aGlzLmF1dG9EaXNtaXNzVGltZW91dE1zXyA9IG51bWJlcnMuREVGQVVMVF9BVVRPX0RJU01JU1NfVElNRU9VVF9NUztcbiAgICAgICAgX3RoaXMuY2xvc2VPbkVzY2FwZV8gPSB0cnVlO1xuICAgICAgICByZXR1cm4gX3RoaXM7XG4gICAgfVxuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2JhckZvdW5kYXRpb24sIFwiY3NzQ2xhc3Nlc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIGNzc0NsYXNzZXM7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2JhckZvdW5kYXRpb24sIFwic3RyaW5nc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIHN0cmluZ3M7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2JhckZvdW5kYXRpb24sIFwibnVtYmVyc1wiLCB7XG4gICAgICAgIGdldDogZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgcmV0dXJuIG51bWJlcnM7XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE9iamVjdC5kZWZpbmVQcm9wZXJ0eShNRENTbmFja2JhckZvdW5kYXRpb24sIFwiZGVmYXVsdEFkYXB0ZXJcIiwge1xuICAgICAgICBnZXQ6IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgICAgIHJldHVybiB7XG4gICAgICAgICAgICAgICAgYWRkQ2xhc3M6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIHVuZGVmaW5lZDsgfSxcbiAgICAgICAgICAgICAgICBhbm5vdW5jZTogZnVuY3Rpb24gKCkgeyByZXR1cm4gdW5kZWZpbmVkOyB9LFxuICAgICAgICAgICAgICAgIG5vdGlmeUNsb3NlZDogZnVuY3Rpb24gKCkgeyByZXR1cm4gdW5kZWZpbmVkOyB9LFxuICAgICAgICAgICAgICAgIG5vdGlmeUNsb3Npbmc6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIHVuZGVmaW5lZDsgfSxcbiAgICAgICAgICAgICAgICBub3RpZnlPcGVuZWQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIHVuZGVmaW5lZDsgfSxcbiAgICAgICAgICAgICAgICBub3RpZnlPcGVuaW5nOiBmdW5jdGlvbiAoKSB7IHJldHVybiB1bmRlZmluZWQ7IH0sXG4gICAgICAgICAgICAgICAgcmVtb3ZlQ2xhc3M6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIHVuZGVmaW5lZDsgfSxcbiAgICAgICAgICAgIH07XG4gICAgICAgIH0sXG4gICAgICAgIGVudW1lcmFibGU6IHRydWUsXG4gICAgICAgIGNvbmZpZ3VyYWJsZTogdHJ1ZVxuICAgIH0pO1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuZGVzdHJveSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgdGhpcy5jbGVhckF1dG9EaXNtaXNzVGltZXJfKCk7XG4gICAgICAgIGNhbmNlbEFuaW1hdGlvbkZyYW1lKHRoaXMuYW5pbWF0aW9uRnJhbWVfKTtcbiAgICAgICAgdGhpcy5hbmltYXRpb25GcmFtZV8gPSAwO1xuICAgICAgICBjbGVhclRpbWVvdXQodGhpcy5hbmltYXRpb25UaW1lcl8pO1xuICAgICAgICB0aGlzLmFuaW1hdGlvblRpbWVyXyA9IDA7XG4gICAgICAgIHRoaXMuYWRhcHRlci5yZW1vdmVDbGFzcyhPUEVOSU5HKTtcbiAgICAgICAgdGhpcy5hZGFwdGVyLnJlbW92ZUNsYXNzKE9QRU4pO1xuICAgICAgICB0aGlzLmFkYXB0ZXIucmVtb3ZlQ2xhc3MoQ0xPU0lORyk7XG4gICAgfTtcbiAgICBNRENTbmFja2JhckZvdW5kYXRpb24ucHJvdG90eXBlLm9wZW4gPSBmdW5jdGlvbiAoKSB7XG4gICAgICAgIHZhciBfdGhpcyA9IHRoaXM7XG4gICAgICAgIHRoaXMuY2xlYXJBdXRvRGlzbWlzc1RpbWVyXygpO1xuICAgICAgICB0aGlzLmlzT3Blbl8gPSB0cnVlO1xuICAgICAgICB0aGlzLmFkYXB0ZXIubm90aWZ5T3BlbmluZygpO1xuICAgICAgICB0aGlzLmFkYXB0ZXIucmVtb3ZlQ2xhc3MoQ0xPU0lORyk7XG4gICAgICAgIHRoaXMuYWRhcHRlci5hZGRDbGFzcyhPUEVOSU5HKTtcbiAgICAgICAgdGhpcy5hZGFwdGVyLmFubm91bmNlKCk7XG4gICAgICAgIC8vIFdhaXQgYSBmcmFtZSBvbmNlIGRpc3BsYXkgaXMgbm8gbG9uZ2VyIFwibm9uZVwiLCB0byBlc3RhYmxpc2ggYmFzaXMgZm9yIGFuaW1hdGlvblxuICAgICAgICB0aGlzLnJ1bk5leHRBbmltYXRpb25GcmFtZV8oZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgX3RoaXMuYWRhcHRlci5hZGRDbGFzcyhPUEVOKTtcbiAgICAgICAgICAgIF90aGlzLmFuaW1hdGlvblRpbWVyXyA9IHNldFRpbWVvdXQoZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgICAgIHZhciB0aW1lb3V0TXMgPSBfdGhpcy5nZXRUaW1lb3V0TXMoKTtcbiAgICAgICAgICAgICAgICBfdGhpcy5oYW5kbGVBbmltYXRpb25UaW1lckVuZF8oKTtcbiAgICAgICAgICAgICAgICBfdGhpcy5hZGFwdGVyLm5vdGlmeU9wZW5lZCgpO1xuICAgICAgICAgICAgICAgIGlmICh0aW1lb3V0TXMgIT09IG51bWJlcnMuSU5ERVRFUk1JTkFURSkge1xuICAgICAgICAgICAgICAgICAgICBfdGhpcy5hdXRvRGlzbWlzc1RpbWVyXyA9IHNldFRpbWVvdXQoZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgICAgICAgICAgICAgX3RoaXMuY2xvc2UoUkVBU09OX0RJU01JU1MpO1xuICAgICAgICAgICAgICAgICAgICB9LCB0aW1lb3V0TXMpO1xuICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgIH0sIG51bWJlcnMuU05BQ0tCQVJfQU5JTUFUSU9OX09QRU5fVElNRV9NUyk7XG4gICAgICAgIH0pO1xuICAgIH07XG4gICAgLyoqXG4gICAgICogQHBhcmFtIHJlYXNvbiBXaHkgdGhlIHNuYWNrYmFyIHdhcyBjbG9zZWQuIFZhbHVlIHdpbGwgYmUgcGFzc2VkIHRvIENMT1NJTkdfRVZFTlQgYW5kIENMT1NFRF9FVkVOVCB2aWEgdGhlXG4gICAgICogICAgIGBldmVudC5kZXRhaWwucmVhc29uYCBwcm9wZXJ0eS4gU3RhbmRhcmQgdmFsdWVzIGFyZSBSRUFTT05fQUNUSU9OIGFuZCBSRUFTT05fRElTTUlTUywgYnV0IGN1c3RvbVxuICAgICAqICAgICBjbGllbnQtc3BlY2lmaWMgdmFsdWVzIG1heSBhbHNvIGJlIHVzZWQgaWYgZGVzaXJlZC5cbiAgICAgKi9cbiAgICBNRENTbmFja2JhckZvdW5kYXRpb24ucHJvdG90eXBlLmNsb3NlID0gZnVuY3Rpb24gKHJlYXNvbikge1xuICAgICAgICB2YXIgX3RoaXMgPSB0aGlzO1xuICAgICAgICBpZiAocmVhc29uID09PSB2b2lkIDApIHsgcmVhc29uID0gJyc7IH1cbiAgICAgICAgaWYgKCF0aGlzLmlzT3Blbl8pIHtcbiAgICAgICAgICAgIC8vIEF2b2lkIHJlZHVuZGFudCBjbG9zZSBjYWxscyAoYW5kIGV2ZW50cyksIGUuZy4gcmVwZWF0ZWQgaW50ZXJhY3Rpb25zIGFzIHRoZSBzbmFja2JhciBpcyBhbmltYXRpbmcgY2xvc2VkXG4gICAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cbiAgICAgICAgY2FuY2VsQW5pbWF0aW9uRnJhbWUodGhpcy5hbmltYXRpb25GcmFtZV8pO1xuICAgICAgICB0aGlzLmFuaW1hdGlvbkZyYW1lXyA9IDA7XG4gICAgICAgIHRoaXMuY2xlYXJBdXRvRGlzbWlzc1RpbWVyXygpO1xuICAgICAgICB0aGlzLmlzT3Blbl8gPSBmYWxzZTtcbiAgICAgICAgdGhpcy5hZGFwdGVyLm5vdGlmeUNsb3NpbmcocmVhc29uKTtcbiAgICAgICAgdGhpcy5hZGFwdGVyLmFkZENsYXNzKGNzc0NsYXNzZXMuQ0xPU0lORyk7XG4gICAgICAgIHRoaXMuYWRhcHRlci5yZW1vdmVDbGFzcyhjc3NDbGFzc2VzLk9QRU4pO1xuICAgICAgICB0aGlzLmFkYXB0ZXIucmVtb3ZlQ2xhc3MoY3NzQ2xhc3Nlcy5PUEVOSU5HKTtcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuYW5pbWF0aW9uVGltZXJfKTtcbiAgICAgICAgdGhpcy5hbmltYXRpb25UaW1lcl8gPSBzZXRUaW1lb3V0KGZ1bmN0aW9uICgpIHtcbiAgICAgICAgICAgIF90aGlzLmhhbmRsZUFuaW1hdGlvblRpbWVyRW5kXygpO1xuICAgICAgICAgICAgX3RoaXMuYWRhcHRlci5ub3RpZnlDbG9zZWQocmVhc29uKTtcbiAgICAgICAgfSwgbnVtYmVycy5TTkFDS0JBUl9BTklNQVRJT05fQ0xPU0VfVElNRV9NUyk7XG4gICAgfTtcbiAgICBNRENTbmFja2JhckZvdW5kYXRpb24ucHJvdG90eXBlLmlzT3BlbiA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgcmV0dXJuIHRoaXMuaXNPcGVuXztcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuZ2V0VGltZW91dE1zID0gZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4gdGhpcy5hdXRvRGlzbWlzc1RpbWVvdXRNc187XG4gICAgfTtcbiAgICBNRENTbmFja2JhckZvdW5kYXRpb24ucHJvdG90eXBlLnNldFRpbWVvdXRNcyA9IGZ1bmN0aW9uICh0aW1lb3V0TXMpIHtcbiAgICAgICAgLy8gVXNlIHNob3J0ZXIgdmFyaWFibGUgbmFtZXMgdG8gbWFrZSB0aGUgY29kZSBtb3JlIHJlYWRhYmxlXG4gICAgICAgIHZhciBtaW5WYWx1ZSA9IG51bWJlcnMuTUlOX0FVVE9fRElTTUlTU19USU1FT1VUX01TO1xuICAgICAgICB2YXIgbWF4VmFsdWUgPSBudW1iZXJzLk1BWF9BVVRPX0RJU01JU1NfVElNRU9VVF9NUztcbiAgICAgICAgdmFyIGluZGV0ZXJtaW5hdGVWYWx1ZSA9IG51bWJlcnMuSU5ERVRFUk1JTkFURTtcbiAgICAgICAgaWYgKHRpbWVvdXRNcyA9PT0gbnVtYmVycy5JTkRFVEVSTUlOQVRFIHx8ICh0aW1lb3V0TXMgPD0gbWF4VmFsdWUgJiYgdGltZW91dE1zID49IG1pblZhbHVlKSkge1xuICAgICAgICAgICAgdGhpcy5hdXRvRGlzbWlzc1RpbWVvdXRNc18gPSB0aW1lb3V0TXM7XG4gICAgICAgIH1cbiAgICAgICAgZWxzZSB7XG4gICAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJcXG4gICAgICAgIHRpbWVvdXRNcyBtdXN0IGJlIGFuIGludGVnZXIgaW4gdGhlIHJhbmdlIFwiICsgbWluVmFsdWUgKyBcIlxcdTIwMTNcIiArIG1heFZhbHVlICsgXCJcXG4gICAgICAgIChvciBcIiArIGluZGV0ZXJtaW5hdGVWYWx1ZSArIFwiIHRvIGRpc2FibGUpLCBidXQgZ290ICdcIiArIHRpbWVvdXRNcyArIFwiJ1wiKTtcbiAgICAgICAgfVxuICAgIH07XG4gICAgTURDU25hY2tiYXJGb3VuZGF0aW9uLnByb3RvdHlwZS5nZXRDbG9zZU9uRXNjYXBlID0gZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4gdGhpcy5jbG9zZU9uRXNjYXBlXztcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuc2V0Q2xvc2VPbkVzY2FwZSA9IGZ1bmN0aW9uIChjbG9zZU9uRXNjYXBlKSB7XG4gICAgICAgIHRoaXMuY2xvc2VPbkVzY2FwZV8gPSBjbG9zZU9uRXNjYXBlO1xuICAgIH07XG4gICAgTURDU25hY2tiYXJGb3VuZGF0aW9uLnByb3RvdHlwZS5oYW5kbGVLZXlEb3duID0gZnVuY3Rpb24gKGV2dCkge1xuICAgICAgICB2YXIgaXNFc2NhcGVLZXkgPSBldnQua2V5ID09PSAnRXNjYXBlJyB8fCBldnQua2V5Q29kZSA9PT0gMjc7XG4gICAgICAgIGlmIChpc0VzY2FwZUtleSAmJiB0aGlzLmdldENsb3NlT25Fc2NhcGUoKSkge1xuICAgICAgICAgICAgdGhpcy5jbG9zZShSRUFTT05fRElTTUlTUyk7XG4gICAgICAgIH1cbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuaGFuZGxlQWN0aW9uQnV0dG9uQ2xpY2sgPSBmdW5jdGlvbiAoX2V2dCkge1xuICAgICAgICB0aGlzLmNsb3NlKFJFQVNPTl9BQ1RJT04pO1xuICAgIH07XG4gICAgTURDU25hY2tiYXJGb3VuZGF0aW9uLnByb3RvdHlwZS5oYW5kbGVBY3Rpb25JY29uQ2xpY2sgPSBmdW5jdGlvbiAoX2V2dCkge1xuICAgICAgICB0aGlzLmNsb3NlKFJFQVNPTl9ESVNNSVNTKTtcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuY2xlYXJBdXRvRGlzbWlzc1RpbWVyXyA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuYXV0b0Rpc21pc3NUaW1lcl8pO1xuICAgICAgICB0aGlzLmF1dG9EaXNtaXNzVGltZXJfID0gMDtcbiAgICB9O1xuICAgIE1EQ1NuYWNrYmFyRm91bmRhdGlvbi5wcm90b3R5cGUuaGFuZGxlQW5pbWF0aW9uVGltZXJFbmRfID0gZnVuY3Rpb24gKCkge1xuICAgICAgICB0aGlzLmFuaW1hdGlvblRpbWVyXyA9IDA7XG4gICAgICAgIHRoaXMuYWRhcHRlci5yZW1vdmVDbGFzcyhjc3NDbGFzc2VzLk9QRU5JTkcpO1xuICAgICAgICB0aGlzLmFkYXB0ZXIucmVtb3ZlQ2xhc3MoY3NzQ2xhc3Nlcy5DTE9TSU5HKTtcbiAgICB9O1xuICAgIC8qKlxuICAgICAqIFJ1bnMgdGhlIGdpdmVuIGxvZ2ljIG9uIHRoZSBuZXh0IGFuaW1hdGlvbiBmcmFtZSwgdXNpbmcgc2V0VGltZW91dCB0byBmYWN0b3IgaW4gRmlyZWZveCByZWZsb3cgYmVoYXZpb3IuXG4gICAgICovXG4gICAgTURDU25hY2tiYXJGb3VuZGF0aW9uLnByb3RvdHlwZS5ydW5OZXh0QW5pbWF0aW9uRnJhbWVfID0gZnVuY3Rpb24gKGNhbGxiYWNrKSB7XG4gICAgICAgIHZhciBfdGhpcyA9IHRoaXM7XG4gICAgICAgIGNhbmNlbEFuaW1hdGlvbkZyYW1lKHRoaXMuYW5pbWF0aW9uRnJhbWVfKTtcbiAgICAgICAgdGhpcy5hbmltYXRpb25GcmFtZV8gPSByZXF1ZXN0QW5pbWF0aW9uRnJhbWUoZnVuY3Rpb24gKCkge1xuICAgICAgICAgICAgX3RoaXMuYW5pbWF0aW9uRnJhbWVfID0gMDtcbiAgICAgICAgICAgIGNsZWFyVGltZW91dChfdGhpcy5hbmltYXRpb25UaW1lcl8pO1xuICAgICAgICAgICAgX3RoaXMuYW5pbWF0aW9uVGltZXJfID0gc2V0VGltZW91dChjYWxsYmFjaywgMCk7XG4gICAgICAgIH0pO1xuICAgIH07XG4gICAgcmV0dXJuIE1EQ1NuYWNrYmFyRm91bmRhdGlvbjtcbn0oTURDRm91bmRhdGlvbikpO1xuZXhwb3J0IHsgTURDU25hY2tiYXJGb3VuZGF0aW9uIH07XG4vLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6bm8tZGVmYXVsdC1leHBvcnQgTmVlZGVkIGZvciBiYWNrd2FyZCBjb21wYXRpYmlsaXR5IHdpdGggTURDIFdlYiB2MC40NC4wIGFuZCBlYXJsaWVyLlxuZXhwb3J0IGRlZmF1bHQgTURDU25hY2tiYXJGb3VuZGF0aW9uO1xuLy8jIHNvdXJjZU1hcHBpbmdVUkw9Zm91bmRhdGlvbi5qcy5tYXAiLCIvKipcbiAqIEBsaWNlbnNlXG4gKiBDb3B5cmlnaHQgMjAxOSBHb29nbGUgSW5jLlxuICpcbiAqIFBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZyBhIGNvcHlcbiAqIG9mIHRoaXMgc29mdHdhcmUgYW5kIGFzc29jaWF0ZWQgZG9jdW1lbnRhdGlvbiBmaWxlcyAodGhlIFwiU29mdHdhcmVcIiksIHRvIGRlYWxcbiAqIGluIHRoZSBTb2Z0d2FyZSB3aXRob3V0IHJlc3RyaWN0aW9uLCBpbmNsdWRpbmcgd2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHNcbiAqIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBtZXJnZSwgcHVibGlzaCwgZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGxcbiAqIGNvcGllcyBvZiB0aGUgU29mdHdhcmUsIGFuZCB0byBwZXJtaXQgcGVyc29ucyB0byB3aG9tIHRoZSBTb2Z0d2FyZSBpc1xuICogZnVybmlzaGVkIHRvIGRvIHNvLCBzdWJqZWN0IHRvIHRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcbiAqXG4gKiBUaGUgYWJvdmUgY29weXJpZ2h0IG5vdGljZSBhbmQgdGhpcyBwZXJtaXNzaW9uIG5vdGljZSBzaGFsbCBiZSBpbmNsdWRlZCBpblxuICogYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuXG4gKlxuICogVEhFIFNPRlRXQVJFIElTIFBST1ZJREVEIFwiQVMgSVNcIiwgV0lUSE9VVCBXQVJSQU5UWSBPRiBBTlkgS0lORCwgRVhQUkVTUyBPUlxuICogSU1QTElFRCwgSU5DTFVESU5HIEJVVCBOT1QgTElNSVRFRCBUTyBUSEUgV0FSUkFOVElFUyBPRiBNRVJDSEFOVEFCSUxJVFksXG4gKiBGSVRORVNTIEZPUiBBIFBBUlRJQ1VMQVIgUFVSUE9TRSBBTkQgTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEVcbiAqIEFVVEhPUlMgT1IgQ09QWVJJR0hUIEhPTERFUlMgQkUgTElBQkxFIEZPUiBBTlkgQ0xBSU0sIERBTUFHRVMgT1IgT1RIRVJcbiAqIExJQUJJTElUWSwgV0hFVEhFUiBJTiBBTiBBQ1RJT04gT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sXG4gKiBPVVQgT0YgT1IgSU4gQ09OTkVDVElPTiBXSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOXG4gKiBUSEUgU09GVFdBUkUuXG4gKi9cbmltcG9ydCAqIGFzIHV0aWwgZnJvbSAnLi91dGlsJztcbmV4cG9ydCB7IHV0aWwgfTtcbmV4cG9ydCAqIGZyb20gJy4vY29tcG9uZW50JztcbmV4cG9ydCAqIGZyb20gJy4vY29uc3RhbnRzJztcbmV4cG9ydCAqIGZyb20gJy4vZm91bmRhdGlvbic7XG4vLyMgc291cmNlTWFwcGluZ1VSTD1pbmRleC5qcy5tYXAiLCIvKipcbiAqIEBsaWNlbnNlXG4gKiBDb3B5cmlnaHQgMjAxOCBHb29nbGUgSW5jLlxuICpcbiAqIFBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZyBhIGNvcHlcbiAqIG9mIHRoaXMgc29mdHdhcmUgYW5kIGFzc29jaWF0ZWQgZG9jdW1lbnRhdGlvbiBmaWxlcyAodGhlIFwiU29mdHdhcmVcIiksIHRvIGRlYWxcbiAqIGluIHRoZSBTb2Z0d2FyZSB3aXRob3V0IHJlc3RyaWN0aW9uLCBpbmNsdWRpbmcgd2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHNcbiAqIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBtZXJnZSwgcHVibGlzaCwgZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGxcbiAqIGNvcGllcyBvZiB0aGUgU29mdHdhcmUsIGFuZCB0byBwZXJtaXQgcGVyc29ucyB0byB3aG9tIHRoZSBTb2Z0d2FyZSBpc1xuICogZnVybmlzaGVkIHRvIGRvIHNvLCBzdWJqZWN0IHRvIHRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcbiAqXG4gKiBUaGUgYWJvdmUgY29weXJpZ2h0IG5vdGljZSBhbmQgdGhpcyBwZXJtaXNzaW9uIG5vdGljZSBzaGFsbCBiZSBpbmNsdWRlZCBpblxuICogYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuXG4gKlxuICogVEhFIFNPRlRXQVJFIElTIFBST1ZJREVEIFwiQVMgSVNcIiwgV0lUSE9VVCBXQVJSQU5UWSBPRiBBTlkgS0lORCwgRVhQUkVTUyBPUlxuICogSU1QTElFRCwgSU5DTFVESU5HIEJVVCBOT1QgTElNSVRFRCBUTyBUSEUgV0FSUkFOVElFUyBPRiBNRVJDSEFOVEFCSUxJVFksXG4gKiBGSVRORVNTIEZPUiBBIFBBUlRJQ1VMQVIgUFVSUE9TRSBBTkQgTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEVcbiAqIEFVVEhPUlMgT1IgQ09QWVJJR0hUIEhPTERFUlMgQkUgTElBQkxFIEZPUiBBTlkgQ0xBSU0sIERBTUFHRVMgT1IgT1RIRVJcbiAqIExJQUJJTElUWSwgV0hFVEhFUiBJTiBBTiBBQ1RJT04gT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sXG4gKiBPVVQgT0YgT1IgSU4gQ09OTkVDVElPTiBXSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOXG4gKiBUSEUgU09GVFdBUkUuXG4gKi9cbmltcG9ydCB7IG51bWJlcnMsIHN0cmluZ3MgfSBmcm9tICcuL2NvbnN0YW50cyc7XG52YXIgQVJJQV9MSVZFX0RFTEFZX01TID0gbnVtYmVycy5BUklBX0xJVkVfREVMQVlfTVM7XG52YXIgQVJJQV9MSVZFX0xBQkVMX1RFWFRfQVRUUiA9IHN0cmluZ3MuQVJJQV9MSVZFX0xBQkVMX1RFWFRfQVRUUjtcbmZ1bmN0aW9uIGFubm91bmNlKGFyaWFFbCwgbGFiZWxFbCkge1xuICAgIGlmIChsYWJlbEVsID09PSB2b2lkIDApIHsgbGFiZWxFbCA9IGFyaWFFbDsgfVxuICAgIHZhciBwcmlvcml0eSA9IGFyaWFFbC5nZXRBdHRyaWJ1dGUoJ2FyaWEtbGl2ZScpO1xuICAgIC8vIFRyaW0gdGV4dCB0byBpZ25vcmUgYCZuYnNwO2AgKHNlZSBiZWxvdykuXG4gICAgLy8gdGV4dENvbnRlbnQgaXMgb25seSBudWxsIGlmIHRoZSBub2RlIGlzIGEgZG9jdW1lbnQsIERPQ1RZUEUsIG9yIG5vdGF0aW9uLlxuICAgIHZhciBsYWJlbFRleHQgPSBsYWJlbEVsLnRleHRDb250ZW50LnRyaW0oKTtcbiAgICBpZiAoIWxhYmVsVGV4dCB8fCAhcHJpb3JpdHkpIHtcbiAgICAgICAgcmV0dXJuO1xuICAgIH1cbiAgICAvLyBUZW1wb3JhcmlseSBkaXNhYmxlIGBhcmlhLWxpdmVgIHRvIHByZXZlbnQgSkFXUytGaXJlZm94IGZyb20gYW5ub3VuY2luZyB0aGUgbWVzc2FnZSB0d2ljZS5cbiAgICBhcmlhRWwuc2V0QXR0cmlidXRlKCdhcmlhLWxpdmUnLCAnb2ZmJyk7XG4gICAgLy8gVGVtcG9yYXJpbHkgY2xlYXIgYHRleHRDb250ZW50YCB0byBmb3JjZSBhIERPTSBtdXRhdGlvbiBldmVudCB0aGF0IHdpbGwgYmUgZGV0ZWN0ZWQgYnkgc2NyZWVuIHJlYWRlcnMuXG4gICAgLy8gYGFyaWEtbGl2ZWAgZWxlbWVudHMgYXJlIG9ubHkgYW5ub3VuY2VkIHdoZW4gdGhlIGVsZW1lbnQncyBgdGV4dENvbnRlbnRgICpjaGFuZ2VzKiwgc28gc25hY2tiYXJzXG4gICAgLy8gc2VudCB0byB0aGUgYnJvd3NlciBpbiB0aGUgaW5pdGlhbCBIVE1MIHJlc3BvbnNlIHdvbid0IGJlIHJlYWQgdW5sZXNzIHdlIGNsZWFyIHRoZSBlbGVtZW50J3MgYHRleHRDb250ZW50YCBmaXJzdC5cbiAgICAvLyBTaW1pbGFybHksIGRpc3BsYXlpbmcgdGhlIHNhbWUgc25hY2tiYXIgbWVzc2FnZSB0d2ljZSBpbiBhIHJvdyBkb2Vzbid0IHRyaWdnZXIgYSBET00gbXV0YXRpb24gZXZlbnQsXG4gICAgLy8gc28gc2NyZWVuIHJlYWRlcnMgd29uJ3QgYW5ub3VuY2UgdGhlIHNlY29uZCBtZXNzYWdlIHVubGVzcyB3ZSBmaXJzdCBjbGVhciBgdGV4dENvbnRlbnRgLlxuICAgIC8vXG4gICAgLy8gV2UgaGF2ZSB0byBjbGVhciB0aGUgbGFiZWwgdGV4dCB0d28gZGlmZmVyZW50IHdheXMgdG8gbWFrZSBpdCB3b3JrIGluIGFsbCBicm93c2VycyBhbmQgc2NyZWVuIHJlYWRlcnM6XG4gICAgLy9cbiAgICAvLyAgIDEuIGB0ZXh0Q29udGVudCA9ICcnYCBpcyByZXF1aXJlZCBmb3IgSUUxMSArIEpBV1NcbiAgICAvLyAgIDIuIGBpbm5lckhUTUwgPSAnJm5ic3A7J2AgaXMgcmVxdWlyZWQgZm9yIENocm9tZSArIEpBV1MgYW5kIE5WREFcbiAgICAvL1xuICAgIC8vIEFsbCBvdGhlciBicm93c2VyL3NjcmVlbiByZWFkZXIgY29tYmluYXRpb25zIHN1cHBvcnQgYm90aCBtZXRob2RzLlxuICAgIC8vXG4gICAgLy8gVGhlIHdyYXBwZXIgYDxzcGFuPmAgdmlzdWFsbHkgaGlkZXMgdGhlIHNwYWNlIGNoYXJhY3RlciBzbyB0aGF0IGl0IGRvZXNuJ3QgY2F1c2UgamFuayB3aGVuIGFkZGVkL3JlbW92ZWQuXG4gICAgLy8gTi5CLjogU2V0dGluZyBgcG9zaXRpb246IGFic29sdXRlYCwgYG9wYWNpdHk6IDBgLCBvciBgaGVpZ2h0OiAwYCBwcmV2ZW50cyBDaHJvbWUgZnJvbSBkZXRlY3RpbmcgdGhlIERPTSBjaGFuZ2UuXG4gICAgLy9cbiAgICAvLyBUaGlzIHRlY2huaXF1ZSBoYXMgYmVlbiB0ZXN0ZWQgaW46XG4gICAgLy9cbiAgICAvLyAgICogSkFXUyAyMDE5OlxuICAgIC8vICAgICAgIC0gQ2hyb21lIDcwXG4gICAgLy8gICAgICAgLSBGaXJlZm94IDYwIChFU1IpXG4gICAgLy8gICAgICAgLSBJRSAxMVxuICAgIC8vICAgKiBOVkRBIDIwMTg6XG4gICAgLy8gICAgICAgLSBDaHJvbWUgNzBcbiAgICAvLyAgICAgICAtIEZpcmVmb3ggNjAgKEVTUilcbiAgICAvLyAgICAgICAtIElFIDExXG4gICAgLy8gICAqIENocm9tZVZveCA1M1xuICAgIGxhYmVsRWwudGV4dENvbnRlbnQgPSAnJztcbiAgICBsYWJlbEVsLmlubmVySFRNTCA9ICc8c3BhbiBzdHlsZT1cImRpc3BsYXk6IGlubGluZS1ibG9jazsgd2lkdGg6IDA7IGhlaWdodDogMXB4O1wiPiZuYnNwOzwvc3Bhbj4nO1xuICAgIC8vIFByZXZlbnQgdmlzdWFsIGphbmsgYnkgdGVtcG9yYXJpbHkgZGlzcGxheWluZyB0aGUgbGFiZWwgdGV4dCBpbiB0aGUgOjpiZWZvcmUgcHNldWRvLWVsZW1lbnQuXG4gICAgLy8gQ1NTIGdlbmVyYXRlZCBjb250ZW50IGlzIG5vcm1hbGx5IGFubm91bmNlZCBieSBzY3JlZW4gcmVhZGVyc1xuICAgIC8vIChleGNlcHQgaW4gSUUgMTE7IHNlZSBodHRwczovL3RpbmsudWsvYWNjZXNzaWJpbGl0eS1zdXBwb3J0LWZvci1jc3MtZ2VuZXJhdGVkLWNvbnRlbnQvKTtcbiAgICAvLyBob3dldmVyLCBgYXJpYS1saXZlYCBpcyB0dXJuZWQgb2ZmLCBzbyB0aGlzIERPTSB1cGRhdGUgd2lsbCBiZSBpZ25vcmVkIGJ5IHNjcmVlbiByZWFkZXJzLlxuICAgIGxhYmVsRWwuc2V0QXR0cmlidXRlKEFSSUFfTElWRV9MQUJFTF9URVhUX0FUVFIsIGxhYmVsVGV4dCk7XG4gICAgc2V0VGltZW91dChmdW5jdGlvbiAoKSB7XG4gICAgICAgIC8vIEFsbG93IHNjcmVlbiByZWFkZXJzIHRvIGFubm91bmNlIGNoYW5nZXMgdG8gdGhlIERPTSBhZ2Fpbi5cbiAgICAgICAgYXJpYUVsLnNldEF0dHJpYnV0ZSgnYXJpYS1saXZlJywgcHJpb3JpdHkpO1xuICAgICAgICAvLyBSZW1vdmUgdGhlIG1lc3NhZ2UgZnJvbSB0aGUgOjpiZWZvcmUgcHNldWRvLWVsZW1lbnQuXG4gICAgICAgIGxhYmVsRWwucmVtb3ZlQXR0cmlidXRlKEFSSUFfTElWRV9MQUJFTF9URVhUX0FUVFIpO1xuICAgICAgICAvLyBSZXN0b3JlIHRoZSBvcmlnaW5hbCBsYWJlbCB0ZXh0LCB3aGljaCB3aWxsIGJlIGFubm91bmNlZCBieSBzY3JlZW4gcmVhZGVycy5cbiAgICAgICAgbGFiZWxFbC50ZXh0Q29udGVudCA9IGxhYmVsVGV4dDtcbiAgICB9LCBBUklBX0xJVkVfREVMQVlfTVMpO1xufVxuZXhwb3J0IHsgYW5ub3VuY2UgfTtcbi8vIyBzb3VyY2VNYXBwaW5nVVJMPXV0aWwuanMubWFwIiwiaW1wb3J0IHsgTURDU25hY2tiYXIgfSBmcm9tIFwiQG1hdGVyaWFsL3NuYWNrYmFyXCI7XHJcbmV4cG9ydCBmdW5jdGlvbiBTaG93TW9kYWxBY3Rpb24oc2hvdzogYm9vbGVhbikge1xyXG4gICAgY29uc3Qgc3R5bGUgPSBzaG93ID8gJ2Jsb2NrJyA6ICdub25lJztcclxuICAgIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdtYWluTW9kYWwnKS5zdHlsZS5kaXNwbGF5ID0gc3R5bGU7XHJcbn1cclxuXHJcbmV4cG9ydCBjb25zdCBDcmVhdGVGb3JtRGF0YSA9IChmb3JtOiBIVE1MRm9ybUVsZW1lbnQpID0+IHtcclxuXHJcbiAgICB2YXIgZm9ybURhdGEgPSBuZXcgRm9ybURhdGEoKTtcclxuICAgIFxyXG4gICAgdmFyIGlucHV0czogTm9kZUxpc3RPZjxIVE1MSW5wdXRFbGVtZW50PiA9IGZvcm0ucXVlcnlTZWxlY3RvckFsbChcImlucHV0XCIpO1xyXG4gICAgaW5wdXRzLmZvckVhY2goaW5wdXQgPT4ge1xyXG5cclxuICAgICAgICBzd2l0Y2ggKGlucHV0LnR5cGUpIHtcclxuICAgICAgICAgICAgY2FzZSBcImNoZWNrYm94XCI6IHtcclxuICAgICAgICAgICAgICAgIGZvcm1EYXRhLmFwcGVuZChpbnB1dC5uYW1lLCBpbnB1dC5jaGVja2VkLnRvU3RyaW5nKCkpO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgZGVmYXVsdDoge1xyXG4gICAgICAgICAgICAgICAgZm9ybURhdGEuYXBwZW5kKGlucHV0Lm5hbWUsIGlucHV0LnZhbHVlKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgICAgICBcclxuXHJcbiAgICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCBpbnB1dC5maWxlcz8ubGVuZ3RoOyBpKyspIHtcclxuICAgICAgICAgICAgZm9ybURhdGEuYXBwZW5kKGlucHV0Lm5hbWUsIGlucHV0LmZpbGVzW2ldLCBpbnB1dC5maWxlc1tpXS5uYW1lKTtcclxuICAgICAgICB9O1xyXG4gICAgfSk7XHJcbiAgICByZXR1cm4gZm9ybURhdGE7XHJcbn1cclxuXHJcblxyXG5leHBvcnQgY29uc3QgU2hvd1NuYWNrQmFyQWN0aW9uID0gKG1lc3NhZ2U6IHN0cmluZywgZXJyb3IgPSBmYWxzZSkgPT4ge1xyXG4gICAgY29uc3Qgc25hY2tiYXIgPSBuZXcgTURDU25hY2tiYXIoZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoJ21haW4tc25hY2snKSk7XHJcbiAgICBjb25zdCBkaXYgPSBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnbWFpbi1zbmFjaycpO1xyXG4gICAgZGl2LmNsYXNzTGlzdC5hZGQoXCJtZGMtc25hY2tiYXItLW9wZW5cIik7XHJcbiAgICBzbmFja2Jhci5sYWJlbFRleHQgPSBtZXNzYWdlO1xyXG4gICAgaWYgKGVycm9yKSB7XHJcbiAgICAgICAgc25hY2tiYXIudGltZW91dE1zID0gMTAwMDA7XHJcbiAgICAgICAgY29uc3Qgc3VyZmFjZTogSFRNTERpdkVsZW1lbnQgPSBkaXYucXVlcnlTZWxlY3RvcihcIi5tZGMtc25hY2tiYXJfX3N1cmZhY2VcIik7XHJcbiAgICAgICAgc3VyZmFjZS5zdHlsZS5iYWNrZ3JvdW5kQ29sb3IgPSBcImRhcmtyZWRcIjtcclxuICAgIH1cclxuICAgIHNuYWNrYmFyLm9wZW4oKTtcclxufSIsImltcG9ydCB7IENyZWF0ZUZvcm1EYXRhLCBTaG93TW9kYWxBY3Rpb24sIFNob3dTbmFja0JhckFjdGlvbiB9IGZyb20gXCIuL2FjdGlvbnNcIjtcclxuaW1wb3J0IHsgVGFnSGFuZGxlcnMgfSBmcm9tIFwiLi90YWdIYW5kbGVyc1wiO1xyXG5pbXBvcnQgeyBkYXRhTXRkQ2xpY2tlckJ5LCBkYXRhTXRkQ2xpY2tlciB9IGZyb20gXCIuL3R5cGVzXCI7XHJcblxyXG5cclxuXHJcbmNvbnN0IHByb3h5Q2xpY2tlcnMgPSBkb2N1bWVudC5xdWVyeVNlbGVjdG9yQWxsKGBbJHtkYXRhTXRkQ2xpY2tlckJ5fV1gKTtcclxuXHJcbnByb3h5Q2xpY2tlcnMuZm9yRWFjaCgoY2xpY2tlcikgPT4ge1xyXG4gICAgY29uc3QgdGFyZ2V0SWQgPSBjbGlja2VyLmdldEF0dHJpYnV0ZShkYXRhTXRkQ2xpY2tlckJ5KTtcclxuICAgIGNsaWNrZXIuYWRkRXZlbnRMaXN0ZW5lcihcImNsaWNrXCIsICgpID0+IHtcclxuICAgICAgICBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCh0YXJnZXRJZCkuY2xpY2soKTtcclxuICAgIH0pXHJcbn0pO1xyXG5cclxuXHJcbmNvbnN0IGNsaWNrZXJzID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvckFsbChgWyR7ZGF0YU10ZENsaWNrZXJ9XWApO1xyXG5cclxuY2xpY2tlcnMuZm9yRWFjaChhc3luYyBjbGlja2VyID0+IHtcclxuXHJcbiAgICBjbGlja2VyLmFkZEV2ZW50TGlzdGVuZXIoXCJjbGlja1wiLCBhc3luYyAoZWxlbW50OiBhbnkpID0+IHtcclxuXHJcbiAgICAgICAgXHJcbiAgICAgICAgY29uc3QgZm9ybTogSFRNTEZvcm1FbGVtZW50ID0gZWxlbW50LmN1cnJlbnRUYXJnZXQuY2xvc2VzdChcImZvcm1cIik7XHJcblxyXG4gICAgICAgIGNvbnN0IHZhbGlkYXRlID0gZm9ybS5yZXBvcnRWYWxpZGl0eSgpO1xyXG4gICAgICAgIGlmICghdmFsaWRhdGUpIHsgcmV0dXJuIGZhbHNlOyB9XHJcblxyXG4gICAgICAgIFNob3dNb2RhbEFjdGlvbih0cnVlKTtcclxuXHJcbiAgICAgICAgdmFyIGZvcm1EYXRhID0gQ3JlYXRlRm9ybURhdGEoZm9ybSk7XHJcbiAgICAgICAgY29uc3QgcmVzcG9uc2UgPSBhd2FpdCBmZXRjaChmb3JtLmFjdGlvbiwgeyBtZXRob2Q6IGZvcm0ubWV0aG9kLCBib2R5OiBmb3JtRGF0YSB9KTtcclxuXHJcblxyXG4gICAgICAgIHN3aXRjaCAocmVzcG9uc2Uuc3RhdHVzKSB7XHJcbiAgICAgICAgICAgIGNhc2UgMjAwOiB7XHJcbiAgICAgICAgICAgICAgICBUYWdIYW5kbGVycy5mb3JFYWNoKGFzeW5jIGhhbmRsZXIgPT4gYXdhaXQgaGFuZGxlcihjbGlja2VyLCByZXNwb25zZSkpO1xyXG4gICAgICAgICAgICAgICAgc2V0VGltZW91dCgoKSA9PiBTaG93TW9kYWxBY3Rpb24oZmFsc2UpLCAxMDAwKTtcclxuICAgICAgICAgICAgICAgIGJyZWFrO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIGNhc2UgNDAwOiB7XHJcbiAgICAgICAgICAgICAgICB2YXIgdmFsdWUgPSBhd2FpdCByZXNwb25zZS50ZXh0KCk7XHJcbiAgICAgICAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHsgU2hvd01vZGFsQWN0aW9uKGZhbHNlKTsgU2hvd1NuYWNrQmFyQWN0aW9uKHZhbHVlLCB0cnVlKTsgfSwgMTAwMCk7XHJcbiAgICAgICAgICAgICAgICBicmVhaztcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBjYXNlIDUwMDoge1xyXG4gICAgICAgICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7IFNob3dNb2RhbEFjdGlvbihmYWxzZSk7IFNob3dTbmFja0JhckFjdGlvbihcIjUwMCBJbnRlcm5hbCBTZXJ2ZXIgRXJyb3JcIiwgdHJ1ZSk7IH0sIDEwMDApO1xyXG4gICAgICAgICAgICAgICAgYnJlYWs7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgZGVmYXVsdDoge1xyXG4gICAgICAgICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7IFNob3dNb2RhbEFjdGlvbihmYWxzZSk7IFNob3dTbmFja0JhckFjdGlvbihgTm8gaGFuZGxlciBmb3Igc3RhdHVzICR7cmVzcG9uc2Uuc3RhdHVzfWAsIHRydWUpOyB9LCAxMDAwKTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgIH1cclxuICAgIH0pO1xyXG59KTtcclxuXHJcblxyXG5cclxuXHJcblxyXG4iLCJpbXBvcnQgeyBUYWdIYW5kbGVyLCBkYXRhUmVzcG9uc2UgfSBmcm9tIFwiLi90eXBlc1wiO1xyXG5cclxuYXN5bmMgZnVuY3Rpb24gUmVzcG9uc2VIYW5kbGVyKGNsaWNrZXI6IEVsZW1lbnQsIHJlc3BvbnNlOiBSZXNwb25zZSkge1xyXG4gICAgdmFyIGlucHV0SWQgPSBjbGlja2VyLmdldEF0dHJpYnV0ZShgWyR7ZGF0YVJlc3BvbnNlfV1gKTtcclxuICAgIGlmICghaW5wdXRJZCkgcmV0dXJuIGZhbHNlO1xyXG4gICAgdmFyIGlucHV0OiBIVE1MSW5wdXRFbGVtZW50ID0gZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoaW5wdXRJZCkgYXMgSFRNTElucHV0RWxlbWVudDtcclxuICAgIGlmICghaW5wdXQpIHJldHVybiBmYWxzZTtcclxuICAgIGlucHV0LnZhbHVlID0gYXdhaXQgcmVzcG9uc2UuanNvbigpO1xyXG59XHJcblxyXG5cclxuXHJcbmV4cG9ydCBjb25zdCBUYWdIYW5kbGVyczogQXJyYXk8VGFnSGFuZGxlcj4gPSBbUmVzcG9uc2VIYW5kbGVyXTtcclxuIiwiZXhwb3J0IGNvbnN0IGRhdGFNdGRDbGlja2VyID0gXCJkYXRhLW10ZC1jbGlja2VyXCI7XHJcbmV4cG9ydCBjb25zdCBkYXRhUmVzcG9uc2UgPSBcImRhdGEtbXRkLXJlc3BvbnNlXCI7XHJcbmV4cG9ydCBjb25zdCBkYXRhTXRkQ2xpY2tlckJ5ID0gXCJkYXRhLW10ZC1jbGlja2VyLWJ5XCI7XHJcblxyXG5leHBvcnQgdHlwZSBUYWdIYW5kbGVyID0gKGNsaWNrZXI6IEVsZW1lbnQsIHJlc3BvbnNlOiBSZXNwb25zZSkgPT4gdm9pZDsiLCIvKiEgKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuQ29weXJpZ2h0IChjKSBNaWNyb3NvZnQgQ29ycG9yYXRpb24uXHJcblxyXG5QZXJtaXNzaW9uIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBhbmQvb3IgZGlzdHJpYnV0ZSB0aGlzIHNvZnR3YXJlIGZvciBhbnlcclxucHVycG9zZSB3aXRoIG9yIHdpdGhvdXQgZmVlIGlzIGhlcmVieSBncmFudGVkLlxyXG5cclxuVEhFIFNPRlRXQVJFIElTIFBST1ZJREVEIFwiQVMgSVNcIiBBTkQgVEhFIEFVVEhPUiBESVNDTEFJTVMgQUxMIFdBUlJBTlRJRVMgV0lUSFxyXG5SRUdBUkQgVE8gVEhJUyBTT0ZUV0FSRSBJTkNMVURJTkcgQUxMIElNUExJRUQgV0FSUkFOVElFUyBPRiBNRVJDSEFOVEFCSUxJVFlcclxuQU5EIEZJVE5FU1MuIElOIE5PIEVWRU5UIFNIQUxMIFRIRSBBVVRIT1IgQkUgTElBQkxFIEZPUiBBTlkgU1BFQ0lBTCwgRElSRUNULFxyXG5JTkRJUkVDVCwgT1IgQ09OU0VRVUVOVElBTCBEQU1BR0VTIE9SIEFOWSBEQU1BR0VTIFdIQVRTT0VWRVIgUkVTVUxUSU5HIEZST01cclxuTE9TUyBPRiBVU0UsIERBVEEgT1IgUFJPRklUUywgV0hFVEhFUiBJTiBBTiBBQ1RJT04gT0YgQ09OVFJBQ1QsIE5FR0xJR0VOQ0UgT1JcclxuT1RIRVIgVE9SVElPVVMgQUNUSU9OLCBBUklTSU5HIE9VVCBPRiBPUiBJTiBDT05ORUNUSU9OIFdJVEggVEhFIFVTRSBPUlxyXG5QRVJGT1JNQU5DRSBPRiBUSElTIFNPRlRXQVJFLlxyXG4qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKiAqL1xyXG4vKiBnbG9iYWwgUmVmbGVjdCwgUHJvbWlzZSAqL1xyXG5cclxudmFyIGV4dGVuZFN0YXRpY3MgPSBmdW5jdGlvbihkLCBiKSB7XHJcbiAgICBleHRlbmRTdGF0aWNzID0gT2JqZWN0LnNldFByb3RvdHlwZU9mIHx8XHJcbiAgICAgICAgKHsgX19wcm90b19fOiBbXSB9IGluc3RhbmNlb2YgQXJyYXkgJiYgZnVuY3Rpb24gKGQsIGIpIHsgZC5fX3Byb3RvX18gPSBiOyB9KSB8fFxyXG4gICAgICAgIGZ1bmN0aW9uIChkLCBiKSB7IGZvciAodmFyIHAgaW4gYikgaWYgKGIuaGFzT3duUHJvcGVydHkocCkpIGRbcF0gPSBiW3BdOyB9O1xyXG4gICAgcmV0dXJuIGV4dGVuZFN0YXRpY3MoZCwgYik7XHJcbn07XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19leHRlbmRzKGQsIGIpIHtcclxuICAgIGV4dGVuZFN0YXRpY3MoZCwgYik7XHJcbiAgICBmdW5jdGlvbiBfXygpIHsgdGhpcy5jb25zdHJ1Y3RvciA9IGQ7IH1cclxuICAgIGQucHJvdG90eXBlID0gYiA9PT0gbnVsbCA/IE9iamVjdC5jcmVhdGUoYikgOiAoX18ucHJvdG90eXBlID0gYi5wcm90b3R5cGUsIG5ldyBfXygpKTtcclxufVxyXG5cclxuZXhwb3J0IHZhciBfX2Fzc2lnbiA9IGZ1bmN0aW9uKCkge1xyXG4gICAgX19hc3NpZ24gPSBPYmplY3QuYXNzaWduIHx8IGZ1bmN0aW9uIF9fYXNzaWduKHQpIHtcclxuICAgICAgICBmb3IgKHZhciBzLCBpID0gMSwgbiA9IGFyZ3VtZW50cy5sZW5ndGg7IGkgPCBuOyBpKyspIHtcclxuICAgICAgICAgICAgcyA9IGFyZ3VtZW50c1tpXTtcclxuICAgICAgICAgICAgZm9yICh2YXIgcCBpbiBzKSBpZiAoT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKHMsIHApKSB0W3BdID0gc1twXTtcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIHQ7XHJcbiAgICB9XHJcbiAgICByZXR1cm4gX19hc3NpZ24uYXBwbHkodGhpcywgYXJndW1lbnRzKTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fcmVzdChzLCBlKSB7XHJcbiAgICB2YXIgdCA9IHt9O1xyXG4gICAgZm9yICh2YXIgcCBpbiBzKSBpZiAoT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKHMsIHApICYmIGUuaW5kZXhPZihwKSA8IDApXHJcbiAgICAgICAgdFtwXSA9IHNbcF07XHJcbiAgICBpZiAocyAhPSBudWxsICYmIHR5cGVvZiBPYmplY3QuZ2V0T3duUHJvcGVydHlTeW1ib2xzID09PSBcImZ1bmN0aW9uXCIpXHJcbiAgICAgICAgZm9yICh2YXIgaSA9IDAsIHAgPSBPYmplY3QuZ2V0T3duUHJvcGVydHlTeW1ib2xzKHMpOyBpIDwgcC5sZW5ndGg7IGkrKykge1xyXG4gICAgICAgICAgICBpZiAoZS5pbmRleE9mKHBbaV0pIDwgMCAmJiBPYmplY3QucHJvdG90eXBlLnByb3BlcnR5SXNFbnVtZXJhYmxlLmNhbGwocywgcFtpXSkpXHJcbiAgICAgICAgICAgICAgICB0W3BbaV1dID0gc1twW2ldXTtcclxuICAgICAgICB9XHJcbiAgICByZXR1cm4gdDtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fZGVjb3JhdGUoZGVjb3JhdG9ycywgdGFyZ2V0LCBrZXksIGRlc2MpIHtcclxuICAgIHZhciBjID0gYXJndW1lbnRzLmxlbmd0aCwgciA9IGMgPCAzID8gdGFyZ2V0IDogZGVzYyA9PT0gbnVsbCA/IGRlc2MgPSBPYmplY3QuZ2V0T3duUHJvcGVydHlEZXNjcmlwdG9yKHRhcmdldCwga2V5KSA6IGRlc2MsIGQ7XHJcbiAgICBpZiAodHlwZW9mIFJlZmxlY3QgPT09IFwib2JqZWN0XCIgJiYgdHlwZW9mIFJlZmxlY3QuZGVjb3JhdGUgPT09IFwiZnVuY3Rpb25cIikgciA9IFJlZmxlY3QuZGVjb3JhdGUoZGVjb3JhdG9ycywgdGFyZ2V0LCBrZXksIGRlc2MpO1xyXG4gICAgZWxzZSBmb3IgKHZhciBpID0gZGVjb3JhdG9ycy5sZW5ndGggLSAxOyBpID49IDA7IGktLSkgaWYgKGQgPSBkZWNvcmF0b3JzW2ldKSByID0gKGMgPCAzID8gZChyKSA6IGMgPiAzID8gZCh0YXJnZXQsIGtleSwgcikgOiBkKHRhcmdldCwga2V5KSkgfHwgcjtcclxuICAgIHJldHVybiBjID4gMyAmJiByICYmIE9iamVjdC5kZWZpbmVQcm9wZXJ0eSh0YXJnZXQsIGtleSwgciksIHI7XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBfX3BhcmFtKHBhcmFtSW5kZXgsIGRlY29yYXRvcikge1xyXG4gICAgcmV0dXJuIGZ1bmN0aW9uICh0YXJnZXQsIGtleSkgeyBkZWNvcmF0b3IodGFyZ2V0LCBrZXksIHBhcmFtSW5kZXgpOyB9XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBfX21ldGFkYXRhKG1ldGFkYXRhS2V5LCBtZXRhZGF0YVZhbHVlKSB7XHJcbiAgICBpZiAodHlwZW9mIFJlZmxlY3QgPT09IFwib2JqZWN0XCIgJiYgdHlwZW9mIFJlZmxlY3QubWV0YWRhdGEgPT09IFwiZnVuY3Rpb25cIikgcmV0dXJuIFJlZmxlY3QubWV0YWRhdGEobWV0YWRhdGFLZXksIG1ldGFkYXRhVmFsdWUpO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19hd2FpdGVyKHRoaXNBcmcsIF9hcmd1bWVudHMsIFAsIGdlbmVyYXRvcikge1xyXG4gICAgZnVuY3Rpb24gYWRvcHQodmFsdWUpIHsgcmV0dXJuIHZhbHVlIGluc3RhbmNlb2YgUCA/IHZhbHVlIDogbmV3IFAoZnVuY3Rpb24gKHJlc29sdmUpIHsgcmVzb2x2ZSh2YWx1ZSk7IH0pOyB9XHJcbiAgICByZXR1cm4gbmV3IChQIHx8IChQID0gUHJvbWlzZSkpKGZ1bmN0aW9uIChyZXNvbHZlLCByZWplY3QpIHtcclxuICAgICAgICBmdW5jdGlvbiBmdWxmaWxsZWQodmFsdWUpIHsgdHJ5IHsgc3RlcChnZW5lcmF0b3IubmV4dCh2YWx1ZSkpOyB9IGNhdGNoIChlKSB7IHJlamVjdChlKTsgfSB9XHJcbiAgICAgICAgZnVuY3Rpb24gcmVqZWN0ZWQodmFsdWUpIHsgdHJ5IHsgc3RlcChnZW5lcmF0b3JbXCJ0aHJvd1wiXSh2YWx1ZSkpOyB9IGNhdGNoIChlKSB7IHJlamVjdChlKTsgfSB9XHJcbiAgICAgICAgZnVuY3Rpb24gc3RlcChyZXN1bHQpIHsgcmVzdWx0LmRvbmUgPyByZXNvbHZlKHJlc3VsdC52YWx1ZSkgOiBhZG9wdChyZXN1bHQudmFsdWUpLnRoZW4oZnVsZmlsbGVkLCByZWplY3RlZCk7IH1cclxuICAgICAgICBzdGVwKChnZW5lcmF0b3IgPSBnZW5lcmF0b3IuYXBwbHkodGhpc0FyZywgX2FyZ3VtZW50cyB8fCBbXSkpLm5leHQoKSk7XHJcbiAgICB9KTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fZ2VuZXJhdG9yKHRoaXNBcmcsIGJvZHkpIHtcclxuICAgIHZhciBfID0geyBsYWJlbDogMCwgc2VudDogZnVuY3Rpb24oKSB7IGlmICh0WzBdICYgMSkgdGhyb3cgdFsxXTsgcmV0dXJuIHRbMV07IH0sIHRyeXM6IFtdLCBvcHM6IFtdIH0sIGYsIHksIHQsIGc7XHJcbiAgICByZXR1cm4gZyA9IHsgbmV4dDogdmVyYigwKSwgXCJ0aHJvd1wiOiB2ZXJiKDEpLCBcInJldHVyblwiOiB2ZXJiKDIpIH0sIHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiAoZ1tTeW1ib2wuaXRlcmF0b3JdID0gZnVuY3Rpb24oKSB7IHJldHVybiB0aGlzOyB9KSwgZztcclxuICAgIGZ1bmN0aW9uIHZlcmIobikgeyByZXR1cm4gZnVuY3Rpb24gKHYpIHsgcmV0dXJuIHN0ZXAoW24sIHZdKTsgfTsgfVxyXG4gICAgZnVuY3Rpb24gc3RlcChvcCkge1xyXG4gICAgICAgIGlmIChmKSB0aHJvdyBuZXcgVHlwZUVycm9yKFwiR2VuZXJhdG9yIGlzIGFscmVhZHkgZXhlY3V0aW5nLlwiKTtcclxuICAgICAgICB3aGlsZSAoXykgdHJ5IHtcclxuICAgICAgICAgICAgaWYgKGYgPSAxLCB5ICYmICh0ID0gb3BbMF0gJiAyID8geVtcInJldHVyblwiXSA6IG9wWzBdID8geVtcInRocm93XCJdIHx8ICgodCA9IHlbXCJyZXR1cm5cIl0pICYmIHQuY2FsbCh5KSwgMCkgOiB5Lm5leHQpICYmICEodCA9IHQuY2FsbCh5LCBvcFsxXSkpLmRvbmUpIHJldHVybiB0O1xyXG4gICAgICAgICAgICBpZiAoeSA9IDAsIHQpIG9wID0gW29wWzBdICYgMiwgdC52YWx1ZV07XHJcbiAgICAgICAgICAgIHN3aXRjaCAob3BbMF0pIHtcclxuICAgICAgICAgICAgICAgIGNhc2UgMDogY2FzZSAxOiB0ID0gb3A7IGJyZWFrO1xyXG4gICAgICAgICAgICAgICAgY2FzZSA0OiBfLmxhYmVsKys7IHJldHVybiB7IHZhbHVlOiBvcFsxXSwgZG9uZTogZmFsc2UgfTtcclxuICAgICAgICAgICAgICAgIGNhc2UgNTogXy5sYWJlbCsrOyB5ID0gb3BbMV07IG9wID0gWzBdOyBjb250aW51ZTtcclxuICAgICAgICAgICAgICAgIGNhc2UgNzogb3AgPSBfLm9wcy5wb3AoKTsgXy50cnlzLnBvcCgpOyBjb250aW51ZTtcclxuICAgICAgICAgICAgICAgIGRlZmF1bHQ6XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKCEodCA9IF8udHJ5cywgdCA9IHQubGVuZ3RoID4gMCAmJiB0W3QubGVuZ3RoIC0gMV0pICYmIChvcFswXSA9PT0gNiB8fCBvcFswXSA9PT0gMikpIHsgXyA9IDA7IGNvbnRpbnVlOyB9XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKG9wWzBdID09PSAzICYmICghdCB8fCAob3BbMV0gPiB0WzBdICYmIG9wWzFdIDwgdFszXSkpKSB7IF8ubGFiZWwgPSBvcFsxXTsgYnJlYWs7IH1cclxuICAgICAgICAgICAgICAgICAgICBpZiAob3BbMF0gPT09IDYgJiYgXy5sYWJlbCA8IHRbMV0pIHsgXy5sYWJlbCA9IHRbMV07IHQgPSBvcDsgYnJlYWs7IH1cclxuICAgICAgICAgICAgICAgICAgICBpZiAodCAmJiBfLmxhYmVsIDwgdFsyXSkgeyBfLmxhYmVsID0gdFsyXTsgXy5vcHMucHVzaChvcCk7IGJyZWFrOyB9XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKHRbMl0pIF8ub3BzLnBvcCgpO1xyXG4gICAgICAgICAgICAgICAgICAgIF8udHJ5cy5wb3AoKTsgY29udGludWU7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgb3AgPSBib2R5LmNhbGwodGhpc0FyZywgXyk7XHJcbiAgICAgICAgfSBjYXRjaCAoZSkgeyBvcCA9IFs2LCBlXTsgeSA9IDA7IH0gZmluYWxseSB7IGYgPSB0ID0gMDsgfVxyXG4gICAgICAgIGlmIChvcFswXSAmIDUpIHRocm93IG9wWzFdOyByZXR1cm4geyB2YWx1ZTogb3BbMF0gPyBvcFsxXSA6IHZvaWQgMCwgZG9uZTogdHJ1ZSB9O1xyXG4gICAgfVxyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19jcmVhdGVCaW5kaW5nKG8sIG0sIGssIGsyKSB7XHJcbiAgICBpZiAoazIgPT09IHVuZGVmaW5lZCkgazIgPSBrO1xyXG4gICAgb1trMl0gPSBtW2tdO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19leHBvcnRTdGFyKG0sIGV4cG9ydHMpIHtcclxuICAgIGZvciAodmFyIHAgaW4gbSkgaWYgKHAgIT09IFwiZGVmYXVsdFwiICYmICFleHBvcnRzLmhhc093blByb3BlcnR5KHApKSBleHBvcnRzW3BdID0gbVtwXTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fdmFsdWVzKG8pIHtcclxuICAgIHZhciBzID0gdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIFN5bWJvbC5pdGVyYXRvciwgbSA9IHMgJiYgb1tzXSwgaSA9IDA7XHJcbiAgICBpZiAobSkgcmV0dXJuIG0uY2FsbChvKTtcclxuICAgIGlmIChvICYmIHR5cGVvZiBvLmxlbmd0aCA9PT0gXCJudW1iZXJcIikgcmV0dXJuIHtcclxuICAgICAgICBuZXh0OiBmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIGlmIChvICYmIGkgPj0gby5sZW5ndGgpIG8gPSB2b2lkIDA7XHJcbiAgICAgICAgICAgIHJldHVybiB7IHZhbHVlOiBvICYmIG9baSsrXSwgZG9uZTogIW8gfTtcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG4gICAgdGhyb3cgbmV3IFR5cGVFcnJvcihzID8gXCJPYmplY3QgaXMgbm90IGl0ZXJhYmxlLlwiIDogXCJTeW1ib2wuaXRlcmF0b3IgaXMgbm90IGRlZmluZWQuXCIpO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19yZWFkKG8sIG4pIHtcclxuICAgIHZhciBtID0gdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIG9bU3ltYm9sLml0ZXJhdG9yXTtcclxuICAgIGlmICghbSkgcmV0dXJuIG87XHJcbiAgICB2YXIgaSA9IG0uY2FsbChvKSwgciwgYXIgPSBbXSwgZTtcclxuICAgIHRyeSB7XHJcbiAgICAgICAgd2hpbGUgKChuID09PSB2b2lkIDAgfHwgbi0tID4gMCkgJiYgIShyID0gaS5uZXh0KCkpLmRvbmUpIGFyLnB1c2goci52YWx1ZSk7XHJcbiAgICB9XHJcbiAgICBjYXRjaCAoZXJyb3IpIHsgZSA9IHsgZXJyb3I6IGVycm9yIH07IH1cclxuICAgIGZpbmFsbHkge1xyXG4gICAgICAgIHRyeSB7XHJcbiAgICAgICAgICAgIGlmIChyICYmICFyLmRvbmUgJiYgKG0gPSBpW1wicmV0dXJuXCJdKSkgbS5jYWxsKGkpO1xyXG4gICAgICAgIH1cclxuICAgICAgICBmaW5hbGx5IHsgaWYgKGUpIHRocm93IGUuZXJyb3I7IH1cclxuICAgIH1cclxuICAgIHJldHVybiBhcjtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fc3ByZWFkKCkge1xyXG4gICAgZm9yICh2YXIgYXIgPSBbXSwgaSA9IDA7IGkgPCBhcmd1bWVudHMubGVuZ3RoOyBpKyspXHJcbiAgICAgICAgYXIgPSBhci5jb25jYXQoX19yZWFkKGFyZ3VtZW50c1tpXSkpO1xyXG4gICAgcmV0dXJuIGFyO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19zcHJlYWRBcnJheXMoKSB7XHJcbiAgICBmb3IgKHZhciBzID0gMCwgaSA9IDAsIGlsID0gYXJndW1lbnRzLmxlbmd0aDsgaSA8IGlsOyBpKyspIHMgKz0gYXJndW1lbnRzW2ldLmxlbmd0aDtcclxuICAgIGZvciAodmFyIHIgPSBBcnJheShzKSwgayA9IDAsIGkgPSAwOyBpIDwgaWw7IGkrKylcclxuICAgICAgICBmb3IgKHZhciBhID0gYXJndW1lbnRzW2ldLCBqID0gMCwgamwgPSBhLmxlbmd0aDsgaiA8IGpsOyBqKyssIGsrKylcclxuICAgICAgICAgICAgcltrXSA9IGFbal07XHJcbiAgICByZXR1cm4gcjtcclxufTtcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBfX2F3YWl0KHYpIHtcclxuICAgIHJldHVybiB0aGlzIGluc3RhbmNlb2YgX19hd2FpdCA/ICh0aGlzLnYgPSB2LCB0aGlzKSA6IG5ldyBfX2F3YWl0KHYpO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19hc3luY0dlbmVyYXRvcih0aGlzQXJnLCBfYXJndW1lbnRzLCBnZW5lcmF0b3IpIHtcclxuICAgIGlmICghU3ltYm9sLmFzeW5jSXRlcmF0b3IpIHRocm93IG5ldyBUeXBlRXJyb3IoXCJTeW1ib2wuYXN5bmNJdGVyYXRvciBpcyBub3QgZGVmaW5lZC5cIik7XHJcbiAgICB2YXIgZyA9IGdlbmVyYXRvci5hcHBseSh0aGlzQXJnLCBfYXJndW1lbnRzIHx8IFtdKSwgaSwgcSA9IFtdO1xyXG4gICAgcmV0dXJuIGkgPSB7fSwgdmVyYihcIm5leHRcIiksIHZlcmIoXCJ0aHJvd1wiKSwgdmVyYihcInJldHVyblwiKSwgaVtTeW1ib2wuYXN5bmNJdGVyYXRvcl0gPSBmdW5jdGlvbiAoKSB7IHJldHVybiB0aGlzOyB9LCBpO1xyXG4gICAgZnVuY3Rpb24gdmVyYihuKSB7IGlmIChnW25dKSBpW25dID0gZnVuY3Rpb24gKHYpIHsgcmV0dXJuIG5ldyBQcm9taXNlKGZ1bmN0aW9uIChhLCBiKSB7IHEucHVzaChbbiwgdiwgYSwgYl0pID4gMSB8fCByZXN1bWUobiwgdik7IH0pOyB9OyB9XHJcbiAgICBmdW5jdGlvbiByZXN1bWUobiwgdikgeyB0cnkgeyBzdGVwKGdbbl0odikpOyB9IGNhdGNoIChlKSB7IHNldHRsZShxWzBdWzNdLCBlKTsgfSB9XHJcbiAgICBmdW5jdGlvbiBzdGVwKHIpIHsgci52YWx1ZSBpbnN0YW5jZW9mIF9fYXdhaXQgPyBQcm9taXNlLnJlc29sdmUoci52YWx1ZS52KS50aGVuKGZ1bGZpbGwsIHJlamVjdCkgOiBzZXR0bGUocVswXVsyXSwgcik7IH1cclxuICAgIGZ1bmN0aW9uIGZ1bGZpbGwodmFsdWUpIHsgcmVzdW1lKFwibmV4dFwiLCB2YWx1ZSk7IH1cclxuICAgIGZ1bmN0aW9uIHJlamVjdCh2YWx1ZSkgeyByZXN1bWUoXCJ0aHJvd1wiLCB2YWx1ZSk7IH1cclxuICAgIGZ1bmN0aW9uIHNldHRsZShmLCB2KSB7IGlmIChmKHYpLCBxLnNoaWZ0KCksIHEubGVuZ3RoKSByZXN1bWUocVswXVswXSwgcVswXVsxXSk7IH1cclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fYXN5bmNEZWxlZ2F0b3Iobykge1xyXG4gICAgdmFyIGksIHA7XHJcbiAgICByZXR1cm4gaSA9IHt9LCB2ZXJiKFwibmV4dFwiKSwgdmVyYihcInRocm93XCIsIGZ1bmN0aW9uIChlKSB7IHRocm93IGU7IH0pLCB2ZXJiKFwicmV0dXJuXCIpLCBpW1N5bWJvbC5pdGVyYXRvcl0gPSBmdW5jdGlvbiAoKSB7IHJldHVybiB0aGlzOyB9LCBpO1xyXG4gICAgZnVuY3Rpb24gdmVyYihuLCBmKSB7IGlbbl0gPSBvW25dID8gZnVuY3Rpb24gKHYpIHsgcmV0dXJuIChwID0gIXApID8geyB2YWx1ZTogX19hd2FpdChvW25dKHYpKSwgZG9uZTogbiA9PT0gXCJyZXR1cm5cIiB9IDogZiA/IGYodikgOiB2OyB9IDogZjsgfVxyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gX19hc3luY1ZhbHVlcyhvKSB7XHJcbiAgICBpZiAoIVN5bWJvbC5hc3luY0l0ZXJhdG9yKSB0aHJvdyBuZXcgVHlwZUVycm9yKFwiU3ltYm9sLmFzeW5jSXRlcmF0b3IgaXMgbm90IGRlZmluZWQuXCIpO1xyXG4gICAgdmFyIG0gPSBvW1N5bWJvbC5hc3luY0l0ZXJhdG9yXSwgaTtcclxuICAgIHJldHVybiBtID8gbS5jYWxsKG8pIDogKG8gPSB0eXBlb2YgX192YWx1ZXMgPT09IFwiZnVuY3Rpb25cIiA/IF9fdmFsdWVzKG8pIDogb1tTeW1ib2wuaXRlcmF0b3JdKCksIGkgPSB7fSwgdmVyYihcIm5leHRcIiksIHZlcmIoXCJ0aHJvd1wiKSwgdmVyYihcInJldHVyblwiKSwgaVtTeW1ib2wuYXN5bmNJdGVyYXRvcl0gPSBmdW5jdGlvbiAoKSB7IHJldHVybiB0aGlzOyB9LCBpKTtcclxuICAgIGZ1bmN0aW9uIHZlcmIobikgeyBpW25dID0gb1tuXSAmJiBmdW5jdGlvbiAodikgeyByZXR1cm4gbmV3IFByb21pc2UoZnVuY3Rpb24gKHJlc29sdmUsIHJlamVjdCkgeyB2ID0gb1tuXSh2KSwgc2V0dGxlKHJlc29sdmUsIHJlamVjdCwgdi5kb25lLCB2LnZhbHVlKTsgfSk7IH07IH1cclxuICAgIGZ1bmN0aW9uIHNldHRsZShyZXNvbHZlLCByZWplY3QsIGQsIHYpIHsgUHJvbWlzZS5yZXNvbHZlKHYpLnRoZW4oZnVuY3Rpb24odikgeyByZXNvbHZlKHsgdmFsdWU6IHYsIGRvbmU6IGQgfSk7IH0sIHJlamVjdCk7IH1cclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fbWFrZVRlbXBsYXRlT2JqZWN0KGNvb2tlZCwgcmF3KSB7XHJcbiAgICBpZiAoT2JqZWN0LmRlZmluZVByb3BlcnR5KSB7IE9iamVjdC5kZWZpbmVQcm9wZXJ0eShjb29rZWQsIFwicmF3XCIsIHsgdmFsdWU6IHJhdyB9KTsgfSBlbHNlIHsgY29va2VkLnJhdyA9IHJhdzsgfVxyXG4gICAgcmV0dXJuIGNvb2tlZDtcclxufTtcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBfX2ltcG9ydFN0YXIobW9kKSB7XHJcbiAgICBpZiAobW9kICYmIG1vZC5fX2VzTW9kdWxlKSByZXR1cm4gbW9kO1xyXG4gICAgdmFyIHJlc3VsdCA9IHt9O1xyXG4gICAgaWYgKG1vZCAhPSBudWxsKSBmb3IgKHZhciBrIGluIG1vZCkgaWYgKE9iamVjdC5oYXNPd25Qcm9wZXJ0eS5jYWxsKG1vZCwgaykpIHJlc3VsdFtrXSA9IG1vZFtrXTtcclxuICAgIHJlc3VsdC5kZWZhdWx0ID0gbW9kO1xyXG4gICAgcmV0dXJuIHJlc3VsdDtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9faW1wb3J0RGVmYXVsdChtb2QpIHtcclxuICAgIHJldHVybiAobW9kICYmIG1vZC5fX2VzTW9kdWxlKSA/IG1vZCA6IHsgZGVmYXVsdDogbW9kIH07XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBfX2NsYXNzUHJpdmF0ZUZpZWxkR2V0KHJlY2VpdmVyLCBwcml2YXRlTWFwKSB7XHJcbiAgICBpZiAoIXByaXZhdGVNYXAuaGFzKHJlY2VpdmVyKSkge1xyXG4gICAgICAgIHRocm93IG5ldyBUeXBlRXJyb3IoXCJhdHRlbXB0ZWQgdG8gZ2V0IHByaXZhdGUgZmllbGQgb24gbm9uLWluc3RhbmNlXCIpO1xyXG4gICAgfVxyXG4gICAgcmV0dXJuIHByaXZhdGVNYXAuZ2V0KHJlY2VpdmVyKTtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIF9fY2xhc3NQcml2YXRlRmllbGRTZXQocmVjZWl2ZXIsIHByaXZhdGVNYXAsIHZhbHVlKSB7XHJcbiAgICBpZiAoIXByaXZhdGVNYXAuaGFzKHJlY2VpdmVyKSkge1xyXG4gICAgICAgIHRocm93IG5ldyBUeXBlRXJyb3IoXCJhdHRlbXB0ZWQgdG8gc2V0IHByaXZhdGUgZmllbGQgb24gbm9uLWluc3RhbmNlXCIpO1xyXG4gICAgfVxyXG4gICAgcHJpdmF0ZU1hcC5zZXQocmVjZWl2ZXIsIHZhbHVlKTtcclxuICAgIHJldHVybiB2YWx1ZTtcclxufVxyXG4iLCIvLyBUaGUgbW9kdWxlIGNhY2hlXG52YXIgX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fID0ge307XG5cbi8vIFRoZSByZXF1aXJlIGZ1bmN0aW9uXG5mdW5jdGlvbiBfX3dlYnBhY2tfcmVxdWlyZV9fKG1vZHVsZUlkKSB7XG5cdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuXHR2YXIgY2FjaGVkTW9kdWxlID0gX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fW21vZHVsZUlkXTtcblx0aWYgKGNhY2hlZE1vZHVsZSAhPT0gdW5kZWZpbmVkKSB7XG5cdFx0cmV0dXJuIGNhY2hlZE1vZHVsZS5leHBvcnRzO1xuXHR9XG5cdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG5cdHZhciBtb2R1bGUgPSBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX19bbW9kdWxlSWRdID0ge1xuXHRcdC8vIG5vIG1vZHVsZS5pZCBuZWVkZWRcblx0XHQvLyBubyBtb2R1bGUubG9hZGVkIG5lZWRlZFxuXHRcdGV4cG9ydHM6IHt9XG5cdH07XG5cblx0Ly8gRXhlY3V0ZSB0aGUgbW9kdWxlIGZ1bmN0aW9uXG5cdF9fd2VicGFja19tb2R1bGVzX19bbW9kdWxlSWRdLmNhbGwobW9kdWxlLmV4cG9ydHMsIG1vZHVsZSwgbW9kdWxlLmV4cG9ydHMsIF9fd2VicGFja19yZXF1aXJlX18pO1xuXG5cdC8vIFJldHVybiB0aGUgZXhwb3J0cyBvZiB0aGUgbW9kdWxlXG5cdHJldHVybiBtb2R1bGUuZXhwb3J0cztcbn1cblxuIiwiLy8gZGVmaW5lIGdldHRlciBmdW5jdGlvbnMgZm9yIGhhcm1vbnkgZXhwb3J0c1xuX193ZWJwYWNrX3JlcXVpcmVfXy5kID0gKGV4cG9ydHMsIGRlZmluaXRpb24pID0+IHtcblx0Zm9yKHZhciBrZXkgaW4gZGVmaW5pdGlvbikge1xuXHRcdGlmKF9fd2VicGFja19yZXF1aXJlX18ubyhkZWZpbml0aW9uLCBrZXkpICYmICFfX3dlYnBhY2tfcmVxdWlyZV9fLm8oZXhwb3J0cywga2V5KSkge1xuXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIGtleSwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGRlZmluaXRpb25ba2V5XSB9KTtcblx0XHR9XG5cdH1cbn07IiwiX193ZWJwYWNrX3JlcXVpcmVfXy5vID0gKG9iaiwgcHJvcCkgPT4gKE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChvYmosIHByb3ApKSIsIi8vIGRlZmluZSBfX2VzTW9kdWxlIG9uIGV4cG9ydHNcbl9fd2VicGFja19yZXF1aXJlX18uciA9IChleHBvcnRzKSA9PiB7XG5cdGlmKHR5cGVvZiBTeW1ib2wgIT09ICd1bmRlZmluZWQnICYmIFN5bWJvbC50b1N0cmluZ1RhZykge1xuXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBTeW1ib2wudG9TdHJpbmdUYWcsIHsgdmFsdWU6ICdNb2R1bGUnIH0pO1xuXHR9XG5cdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCAnX19lc01vZHVsZScsIHsgdmFsdWU6IHRydWUgfSk7XG59OyIsIiIsIi8vIHN0YXJ0dXBcbi8vIExvYWQgZW50cnkgbW9kdWxlIGFuZCByZXR1cm4gZXhwb3J0c1xuLy8gVGhpcyBlbnRyeSBtb2R1bGUgaXMgcmVmZXJlbmNlZCBieSBvdGhlciBtb2R1bGVzIHNvIGl0IGNhbid0IGJlIGlubGluZWRcbnZhciBfX3dlYnBhY2tfZXhwb3J0c19fID0gX193ZWJwYWNrX3JlcXVpcmVfXyhcIi4vc3JjL2ZldGNoRGF0YS9pbmRleC50c1wiKTtcbiIsIiJdLCJuYW1lcyI6W10sInNvdXJjZVJvb3QiOiIifQ==