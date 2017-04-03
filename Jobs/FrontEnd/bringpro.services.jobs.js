bringpro.services.jobs = bringpro.services.jobs || {};

bringpro.services.jobs.insert = function (data, onSuccess, onError) {
    var url = "/api/jobs/create";
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

bringpro.services.jobs.update = function (id, data, onSuccess, onError) {
    var url = "/api/jobs/" + id;
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

bringpro.services.jobs.updatePaymentNonce = function (id, data, onSuccess, onError) {
    var url = "/api/jobs/update/" + id;
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

bringpro.services.jobs.get = function (onAjaxSuccess, onAjaxError) {
    var url = "/api/jobs/"
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


bringpro.services.jobs.getById = function (id, onSuccess, onError) {
    var url = "/api/jobs/" + id;
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

bringpro.services.jobs.delete = function (id, onSuccess, onError) {
    var url = "/api/jobs/" + id;
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

bringpro.services.jobs.getItemsById = function (id, onSuccess, onError) {
    var url = "/api/items/" + id;
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



bringpro.services.jobs.updateJobsItems = function (id, data, onSuccess, onError) {
    var url = "/api/jobs/items/" + id;
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


bringpro.services.jobs.deleteJobItems = function (id, onSuccess, onError) {
    var url = "/api/jobs/items/" + id;
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

bringpro.services.jobs.getServiceById = function (id, onSuccess, onError) {
    var url = "/api/service/" + id;
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



bringpro.services.jobs.updateJobsService = function (id, data, onSuccess, onError) {
    var url = "/api/jobs/service/" + id;
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

bringpro.services.jobs.jobsAddressInsert = function (data, onSuccess, onError) {
    var url = "/api/jobs/job-address";
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


bringpro.services.jobs.insertJobWaypoint = function (data, onSuccess, onError) {
    var url = "/api/jobs/waypoints";
    var settings = {
        cache: false
        , contentType: "application/json"
        , data: JSON.stringify(data)
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };

    $.ajax(url, settings);

}


bringpro.services.jobs.getPaginationWithFilter = function (data, onAjaxSuccess, onAjaxError) {
    var url = "/api/jobs"
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , data: data
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

}

bringpro.services.jobs.getActivityByJobId = function (id, onSuccess, onError) {
    var url = "/api/jobs/activity/" + id;
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: id
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "GET"
    };

    $.ajax(url, settings);
}

bringpro.services.jobs.InsertBringgJob = function (id, onSuccess, onError) {
    var url = "/api/jobs/submit/" + id;
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onSuccess
            , error: onError
            , type: "POST"
    };

    $.ajax(url, settings);
}
