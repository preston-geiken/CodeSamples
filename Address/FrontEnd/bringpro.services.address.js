
if (!bringpro.services.address) {
    bringpro.services.address = {};
}

bringpro.services.address.insert = function (data, onSuccess, onError) {

    var url =  "/api/address/";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };

    $.ajax(url, settings);
}

bringpro.services.address.update = function (id, data, onSuccess, onError) {

    var url =  "/api/address/" + id;

    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);

}

bringpro.services.address.getById = function (id, onSuccess, onError) {

    var url =  "/api/address/" + id;

    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);

}

bringpro.services.address.getAll = function (onAjaxSuccess, onAjaxError) {
    var url =  "/api/address/";

    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

}

bringpro.services.address.delete = function (id, onSuccess, onError) {


    var url = "/api/address/delete/" + id;

    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };

    $.ajax(url, settings);

}
