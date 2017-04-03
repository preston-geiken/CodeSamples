
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('jobRequestController', jobRequestController);

    jobRequestController.$inject = [
        '$scope', '$baseController', "$jobsService", "$location"
    ];

    function jobRequestController(
        $scope,
        $baseController,
        $jobsService,
        $location
       ) {

        //  controllerAs with vm syntax: https://github.com/johnpapa/angular-styleguide#style-y032
        var vm = this; //this points to a new {}
        vm.$scope = $scope;
        vm.$jobsService = $jobsService;
        vm.$location = $location;
         
        vm.jobId = null;
        vm.asapJobButtons = [
            { name: 'Store Pickup', value: 0, id: 0 },
            { name: 'Home Pickup', value: 1, id: 1 },
            { name: 'Labor Only', value: 2, id: 2 }
        ];
        vm.asapSubmitDisabled = true;
        vm.submitText = 'Save & Add Pickup Location';

        //  bindable members (functions) always go up top while the "meat" of the functions go below: https://github.com/johnpapa/angular-styleguide#style-y033
        vm.asapJobSelect = _asapJobSelect;
        vm.submitAsapJob = _submitAsapJob;
        
        //-- this line to simulate inheritance
        $baseController.merge(vm, $baseController);

        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$jobsService.getNotifier($scope);

        render();

        function render() {
        }

        vm.asapJobLabel = 'Choose job type';


        function _asapJobSelect(job) {
            vm.activeJob = job;

            if (job.id == 0) {
                vm.asapJobLabel = 'Pick up from a retail store';
                vm.updatedDeliveryOptionText = 'ASAP: Retail pick up for Today';
            } else if (job.id == 1) {
                vm.asapJobLabel = 'Pick up from home or private residence';
                vm.updatedDeliveryOptionText = 'ASAP: Residence pick up for Today';
            } else {
                vm.asapJobLabel = 'Labor within a single home';
                vm.updatedDeliveryOptionText = 'ASAP: Labor for Today';
            }
            if (job.id == 2 || job.id == 5) {
                vm.submitText = 'Save & Add Location';
            } else {
                vm.submitText = 'Save & Add Pickup Location';
            }
            vm.asapSubmitDisabled = false;
        }

        function _submitAsapJob(data) {
            console.log('asapData', vm.activeJob.id);
            var jobPayload = {
                'jobType': vm.activeJob.id,
                'websiteId': 55 //WebsiteId is hardcoded for now
            };
            _insertJob(jobPayload);
        }

        function _insertJob(jobPayload) {
            vm.$jobsService.insert(jobPayload, _insertJobSuccess, _insertJobError);
        }

        function _insertJobSuccess(data) {  
            vm.notify(function () {
                console.log('data', data.item);
                vm.jobId = data.item;
                vm.$location.url(vm.jobId);
            });
        }

        function _insertJobError(jqXhr, error) {
            console.log('insertJobError', jqXhr);
        }
    }
})();

