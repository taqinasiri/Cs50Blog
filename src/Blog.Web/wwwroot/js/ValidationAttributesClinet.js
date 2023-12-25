//bool valiation
jQuery.validator.addMethod('boolValidation', function (value, element, param) {
    return $(element).data('val-validstatus') == $(element)[0].checked;
});
jQuery.validator.unobtrusive.adapters.addBool('boolValidation');

//fileRequired
jQuery.validator.addMethod("fileRequired", function (value, element, param) {
    if (element.files[0] != null) return element.files[0].size > 0;
    return false;
});
jQuery.validator.unobtrusive.adapters.addBool("fileRequired");

//fileNotEmpty
jQuery.validator.addMethod("fileNotEmpty", function (value, element, param) {
    if (element.files[0] != null) return element.files[0].size > 0;
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool("fileNotEmpty");

//allowExtensions
jQuery.validator.addMethod('allowExtensions', function (value, element, param) {
    if (element.files[0] != null) {
        var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        return whiteListExtensions.includes(element.files[0].type);
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool('allowExtensions');

//isImage
jQuery.validator.addMethod('isImage', function (value, element, param) {
    if (element.files[0] != null) {
        var whiteListExtensions = $(element).data('val-whitelistextensions').split(',');
        return whiteListExtensions.includes(element.files[0].type);
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool('isImage');

//maxFileSize
jQuery.validator.addMethod('maxFileSize', function (value, element, param) {
    if (element.files[0] != null) {
        var maxFileSize = $(element).data('val-maxsize');
        var selectedFileSize = element.files[0].size;
        return maxFileSize >= selectedFileSize;
    }
    return true;
});
jQuery.validator.unobtrusive.adapters.addBool('maxFileSize');