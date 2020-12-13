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

/*
 * Returns true if the reference parameter is either null or undefined 
 * @param	{object/function}	reference	
 */
function isUndefinedOrNull(reference) {
    return (isUndefined(reference) || isNull(reference));
}
isNullOrUndefined = isUndefinedOrNull; // alias for people who may confuse !