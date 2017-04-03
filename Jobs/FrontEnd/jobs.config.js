
(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $routeProvider.when('/',  //This will direct to create job
            {
                templateUrl: '/Assets/Themes/bringpro/js/features/jobs/templates/requestDelivery.html',
                controller: 'jobRequestController',
                controllerAs: 'job'
            }).when('/:jobId',    //this will redirect to update job
            {
                templateUrl: '/Assets/Themes/bringpro/js/features/jobs/templates/jobFlowForm.html',
                controller: 'jobController',
                controllerAs: 'job'
            })
            .otherwise("/");

            $locationProvider.html5Mode(false).hashPrefix('');

        }])

})();