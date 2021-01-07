// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getElementOfArrayByPropertyValue(array, property, value) {

    var elementFounded = [];

    for (element in array) {
        if (isDefined(array[element][property]) && areEqual(array[element][property], value)) {
            elementFounded.push(array[element]);
            break;
        }
    }

    return elementFounded;
};

function translate(val) {
    return val;
}

function areEqual(o1, o2) {
    var eq = false;

    if ((typeof (o1) == 'string') && (typeof (o2) == 'string')) {
        // compare string values
        eq = (o1.toLowerCase() == o2.toLowerCase());
    } else {
        // compare object references
        eq = (o1 == o2);
    }
    return eq;
}

function getTicks() {
    var date = new Date();
    return date.getTime();
};

function isDefined(reference) {
    return !isUndefinedOrNull(reference);
};

function isUndefined(parameterValue) {
    return (typeof (parameterValue) == 'undefined');
};

function isNull(reference) {
    return (reference == null);
};


function CloneObject(o) {
    if (!o || typeof o != "object" || typeof (o) == 'function') {
        return o;
    }
    if (o.nodeType && "cloneNode" in o) {
        return o.cloneNode(true);
    }
    if (o instanceof Date) {
        return new Date(o.getTime());
    }
    var r, i, l, s, name;
    if ($.isArray(o)) {
        r = [];
        for (i = 0, l = o.length; i < l; ++i) {
            if (i in o) {
                r.push(CloneObject(o[i]));
            }
        }
    } else {
        // serialize the object to a JSON string
        var jsonString = JSON.stringify(o);

        // deserialize JSON string to a new object
        r = jQuery.parseJSON(jsonString);
    }

    return r;
};

/*
 * Returns true if the reference parameter is either null or undefined 
 * @param	{object/function}	reference	
 */
function isUndefinedOrNull(reference) {
    return (isUndefined(reference) || isNull(reference));
}
isNullOrUndefined = isUndefinedOrNull; // alias for people who may confuse !