(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$jobsService', JobsServiceFactory);

    JobsServiceFactory.$inject = ['$baseService', '$bringpro'];

    function JobsServiceFactory($baseService, $bringpro) {
        var aBringProServiceObject = bringpro.services.jobs;

        var newService = $baseService.merge(true, {}, aBringProServiceObject, $baseService);

        console.log("jobs service", aBringproServiceObject);

        return newService;
    }
})();
