
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('jobController', JobController).filter('phoneNumber', filter);;


    JobController.$inject = [
        '$scope', '$baseController', "$jobsService", "$jobServiceItemOptions", "$uibModal",
        "localStorageService", "$filter", "$jobScheduleService", "$log", "$location", "$billingService",
        "$dashboardService", "$window", "$utilityService", "$userCreditsService"
    ];

    function JobController(
        $scope,
        $baseController,
        $jobsService,
        $jobServiceItemOptions,
        $uibModal,
        $localStorageService,
        $filter,
        $jobScheduleService,
        $log,
        $location,
        $billingService,
        $dashboardService,
        $window,
        $utilityService,
        $userCreditsService) {

        //  controllerAs with vm syntax: https://github.com/johnpapa/angular-styleguide#style-y032
        var vm = this;

        $baseController.merge(vm, $baseController);

        vm.$jobsService = $jobsService;
        vm.$jobServiceItemOptions = $jobServiceItemOptions;
        vm.$billingService = $billingService;
        vm.$dashboardService = $dashboardService;
        vm.$scope = $scope;
        vm.$uibModal = $uibModal;
        vm.$location = $location;
        vm.$window = $window;
        vm.$localStorageService = $localStorageService;
        vm.$jobScheduleService = $jobScheduleService;
        vm.$filter = $filter;
        vm.$utilityService = $utilityService;
        vm.$userCreditsService = $userCreditsService;
        vm.activeInfo = null;
        vm.jobId = null;
        vm.jobItems = null;
        vm.userCreditOptions = {};
        vm.asapJobButtons = [
            { name: 'Store Pickup', value: 0, id: 0 },
            { name: 'Home Pickup', value: 1, id: 1 },
            { name: 'Labor Only', value: 2, id: 2 }
        ];

        vm.scheJobButtons = [
            { name: 'Store Pickup', value: 3, id: 3 },
            { name: 'Home Pickup', value: 4, id: 4 },
            { name: 'Labor Only', value: 5, id: 5 }
        ];

        vm.jobTimeButtons = [
            { name: '9AM to 11AM', value: 0, id: 6 },
            { name: '11AM to 1PM', value: 1, id: 7 },
            { name: '1PM to 3PM', value: 2, id: 8 },
            { name: '3PM to 5PM', value: 3, id: 9 }
        ];

        vm.wayPointServices = [
            { name: 'Pick Up', value: 0, class: 'wayPointPickup' },
            { name: 'Drop Off', value: 1, class: 'wayPointDropOff' },
            { name: 'Assembly/Installation', value: 2, class: 'wayPointAssemble' },
            { name: 'Removal of Trash', value: 3, class: 'wayPointTrash' },
            { name: 'Other', value: 4, class: 'wayPointOther' }
        ];

        vm.itemsPickup = [];
        vm.itemsDeliver = [];

        vm.accordionPanels = {
            isCustomHeaderOpen: false,
            isFirstOpen: true,
            open: true
        };

        vm.addressPanel = {
            open: true
        }

        vm.timeAccordionPanels = {
            open: true,
            isOpen: false
        }
        vm.wayPointAccordionPanels = {
            open: true,
            isOpen: false
        }

        vm.paymentPanels = {
            open: true,
            isOpen: false
        }

        vm.guestPaymentPanels = {
            open: true,
            isOpen: false
        }

        vm.jobOverviewPanels = {
            open: true,
            isOpen: false
        }

        vm.oneAtATime = true;

        vm.cardNonce = "";
        vm.cardSelectedNonce = null;//DF might not need this anymore
        vm.validCard = false;
        vm.cardSelect = function (nonce) {

            vm.cardSelectedNonce = nonce;
            console.log("Card Nonce", vm.cardSelectedNonce);
        }

        vm.otherOptions = false;
        vm.deliveryOptionText = 'Choose Delivery Option';
        vm.schedulingOptionText = 'Scheduled: Choose Date & Time Slot';
        vm.asapOptionText = 'ASAP: Choose Time Slot';
        vm.updatedSchedulingOptionText = null;
        vm.updatedAsapOptionText = null;
        vm.defaultDeliveryOption = false;
        vm.updatedDeliveryOption = false;
        vm.updatedDeliveryOptionText = null;
        vm.timeSlotOptionText = null;
        vm.todayAvailability = {};
        vm.activeJob = null;
        vm.activeTime = null;
        vm.currentDate = null;
        vm.wayPoints = [];
        vm.itemQuantity = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        vm.currentJobScheduleId = 0;
        vm.asapJobLabel = null;
        vm.scheJobLabel = null;
        vm.showDateTimePicker = false;
        vm.hideAsapButton = false;
        vm.showScheduledSubmitButton = false;
        vm.hideScheduleButton = false;
        vm.timeScheduleSubmit = true;
        vm.showScheduleTimeSelect = true;
        vm.dateToday = _convertDateToZeroTime(new Date());
        vm.availableTimeSlotsByToday = {};
        vm.jobSchedule = {};
        vm.timeSlotInfo = {
            teamId: null
        };
        vm.availableTimeSlotsByDate = {};
        vm.datePicker = false;
        vm.dateFormat = {
            formatYear: 'yy',
            minDate: _addDays(new Date(), 1),
            showWeeks: false

        };
        vm.submitText = 'Save & Add Pickup Location';
        vm.showPickupItems = false;
        vm.showDropOffItems = false;
        vm.hideTimeScheduler = true;
        vm.addressStreetNumber = null;
        vm.addressStreetName = null;
        vm.addressStreet = null;
        vm.addressCity = null;
        vm.addressState = null;
        vm.addressZipcode = null;
        vm.addressLat = null;
        vm.addressLong = null;
        vm.showTimeSlotPicker = true;
        vm.asapUnavailable = true; //For ng-hide/show on ASAP job text
        vm.customerInfo = null; //Daniel 
        vm.paymentInfo = {};
        vm.submitPayment = _submitPayment;
        vm.customerCards = {};
        vm.getCurrentUserCCInfo = _getCurrentUserCCInfo;
        vm.openModal = _openModal;
        vm.updateSuccess = _updateSucess;
        vm.guestPaymentNonce = null;
        vm.websiteId = $("#websiteId").val(); //website ID from the websiteViewModel - now the website ID does NOT have to be hardcoded
        //Values for the guest checkout below
        vm.firstName = null;
        vm.lastName = null;
        vm.phone = null;
        vm.address = null;
        vm.state = null;
        vm.zipCode = null;
        vm.city = null;
        vm.externalCustomerId = null;
        vm.mediaId = null;
        vm.currentItem = null;
        vm.currentPickUpItem = null;
        vm.currentDropOffItem = null;
        vm.pickupItemId = null;
        vm.dropOffItemId = null;


        vm.dzOptions = {
            url: "/api/media/insert",
            clickable: ".fileinput-button",
            addRemoveLinks: true,
            acceptedFiles: ".png,.jpg,.gif,.jpeg",
            autoProcessQueue: true,
            parallelUploads: 1,
            init: function () {
                var upload = this;

                upload.on("sending",
                    function (file, xhr, formData) {
                        formData.append("mediaTypeId", 2);
                        console.log("HERE ", formData, $location);
                    });


                upload.on("success",
                    function (data, json) {
                        console.log('image data', data);
                        console.log('json data', json.item);

                        vm.mediaId = json.item;
                        vm.currentItem.mediaId = vm.mediaId;

                    })
                upload.on("error",
                    function (file, errorMessage, xhr) {
                        console.log(errorMessage);
                    });
            }

        }

        vm.dzCallbacks = {
            'addedfile': function (file) {
                console.log(file);
                $scope.newFile = file;
            },
            'success': function (file, xhr) {
                console.log(file, xhr);
            }
        }

        //  bindable members (functions) always go up top while the "meat" of the functions go below: https://github.com/johnpapa/angular-styleguide#style-y033
        vm.submitCoupon = _submitCoupon;
        vm.asapJob = _asapJob;
        vm.scheJob = _scheJob;
        vm.jobTimeButton = _jobTimeButton;
        vm.addAddress = _addAddress;
        vm.openDatePicker = _openDatePicker;
        vm.submitJob = _submitJob;
        vm.addItem = _addItem;
        vm.togglePickupItems = _togglePickupItems;
        vm.toggleDropoffItems = _toggleDropoffItems;
        vm.onDateChange = _onDateChange;
        vm.deletePickupItem = _deletePickupItem;
        vm.deleteSuccess = _deleteSuccess;
        vm.wayPointSuccess = _wayPointSuccess;
        vm.jobDropZone = _jobDropZone;
        vm.submitTimeSelection = _submitTimeSelection;
        vm.setCurrentItem = _setCurrentItem;
        vm.getJobByIdSuccess = _getJobByIdSuccess;
        vm.clientToken = $("#clientCCToken").val();
        vm.insertOrUpdate = _insertOrUpdate;
        vm.submitPayment = _submitPayment;
        vm.submitCard = _submitCard;
        vm.insertBringgJobSuccess = _insertBringgJobSuccess;
        vm.insertBringgJobFail = _insertBringgJobFail;
        vm.getJob = _getJob;
        vm.toggleItemForDropoff = _toggleItemForDropoff;
        vm.finalSubmit = _finalSubmit;





        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$jobsService.getNotifier($scope);


        render();

        function render() {
            vm.$jobServiceItemOptions.getAll(_getJobItemsSuccess, _commonErrorHandler);
            _getCustomerByUserId();
            vm.getCurrentUserCCInfo();

            braintree.client.create({
                authorization: vm.clientToken
            }, _clientInstance);

            _insertOrUpdate();
        }

        function _openModal() {
            var modalInstance = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Assets/Themes/bringpro/js/features/jobs/templates/paymentModal.html',
                controller: 'modalController as mc'
            });
            modalInstance.result.then(function () {
                //vm.modalSelected = selectedItem;
                _getCurrentUserCCInfo();
            },
                function () {
                    console.log('Modal dismissed at: ' + new Date());
                });


        }

        function _finalSubmit() {

            // _submitJob();
            vm.$jobsService.InsertBringgJob(vm.jobId, vm.insertBringgJobSuccess, vm.insertBringgJobFail);

            var jobPayload = {
                'PaymentNonce': vm.customerCards.externalCardIdNonce

            }


            vm.$jobsService.updatePaymentNonce(vm.jobId, jobPayload, vm.updateSuccess, vm.commonErrorHandler);

        }

        function _isDropoffItemSelected(parentId) {
            //  loop thru outstannding items array and check each id against parentId
            //  if it is NOT found in the array, then that checkbox is UNchecked
            //  if it IS found in the array, the checkbox should be checked
        }

        function _toggleItemForDropoff(parentItemId, waypointPickupArray) {
            console.log('paretnItemId: ', parentItemId);
            console.log('waypointPickupArray: ', waypointPickupArray);
            console.log('vm.itemsPickup: ', vm.itemsPickup);
            for (var i = 0; i < vm.itemsPickup.length; i++) {

                if (vm.itemsPickup[i].id == parentItemId) {

                    var itemsDropOffArray = {
                        jobId: vm.itemsPickup[i].jobId,
                        waypointId: vm.itemsPickup[i].waypointId,
                        itemTypeId: vm.itemsPickup[i].itemTypeId,
                        quantity: vm.itemsPickup[i].quantity,
                        mediaId: vm.itemsPickup[i].mediaId,
                        operation: 2,
                        parentItemId: parentItemId,
                    };
                    waypointPickupArray.push(itemsDropOffArray);
                    _InsertOrUpdateScheduleSlot();

                }
            }
            //  loop through vm.itemsPickup
            //  inside the loop: find the pickup item with id = parentItemId passed in here
            //  create a new payload object with all the same fields from the parent item MINUS the id, 
            //  and add the parentId and operation = 2 field
            //  push the new payload object into waypointPickupArray
            //  call _submitJob() to save in db
        }

        // $("#credit-card-number").attr('required', 'required');


        function _insertOrUpdate() {
            if (vm.$routeParams.jobId) {
                //  console.log('We are in edit mode. This is the jobId: ', vm.$routeParams.jobId);
                vm.jobId = vm.$routeParams.jobId;
                _getJob(vm.jobId);
                vm.accordionPanels.isFirstOpen = !vm.accordionPanels.isFirstOpen;
            } else {
                _addAddress();
            }

        }

        function _getJob(jobId) {
            vm.$jobsService.getById(jobId, _getJobByIdSuccess, _commonErrorHandler);
        }

        function _openDatePicker() {
            vm.datePicker = true;
        }

        function _asapJob() {
            vm.$alertService.success("ASAP Job Selected!");
            //vm.disableScheduleButton = true;
            vm.hideScheduleButton = false;
            vm.hideAsapButton = true;
            vm.showAsapSelectedButton = true;

            //Grab ASAP Time Slot Id for Schedule Table
            vm.activeTime = vm.todayAvailability[0].id;
            console.log("Current Time Slot ASAP: ", vm.activeTime);

            //Hide Schdule stuff
            vm.showScheduledSelectedButton = false;
            vm.hideScheduleButton = false;
            vm.showScheduledSubmitButton = false;
            vm.showDateTimePicker = false;

            //Inserting Schedule After ASAP Submit
            vm.jobScheduleDate = vm.dateToday;
            _InsertOrUpdateScheduleSlot();
        }

        function _scheJob() {
            //Used for the ASAP time slot only
            //vm.asapUnavailable = true;
            vm.showScheduledSubmitButton = true;
            vm.hideScheduleButton = true;
            vm.hideAsapButton = false;

            vm.activeTime = null;

            //vm.showDatePicker = true;
            vm.showDateTimePicker = true;

            //Hide ASAP Stuff
            vm.showAsapSelectedButton = false;
            vm.hideAsapButton = false;

        }

        function _InsertOrUpdateScheduleSlot() {
            //jobid, scheduleid, date, jobScheduleId?
            //Waypoint Stuff First
            for (var i = 0; i < vm.wayPoints.length; i++) {

                var wayPoint = vm.wayPoints[i];

                var addressComponents = wayPoint.address.address_components;

                var itemsPickUp = wayPoint.jobWaypointItemsPickup;
                var itemsDropOff = wayPoint.jobWaypointItemsDropOff;

                var wayPointItems = wayPoint.items;

                wayPoint.address.externalPlaceId = wayPoint.address.id;
                wayPoint.jobId = vm.jobId;

                //wayPoint.address.latitude = wayPoint.address.geometry.viewport.f.f
                //wayPoint.address.longitude = wayPoint.address.geometry.viewport.b.f

                console.log('this is addressComponents ', addressComponents);
                for (var j = 0; j < addressComponents.length; j++) {
                    var types = addressComponents[j]
                    var componentTypes = types.types

                    if (componentTypes) {
                        if (componentTypes == "street_number") {
                            var addressNumber = types.long_name;
                        }
                        if (componentTypes == "route") {
                            var addressName = types.long_name;

                            wayPoint.address.line1 = addressNumber + " " + addressName;
                        }
                        if (componentTypes[0] == "locality") {
                            wayPoint.address.city = types.long_name;
                        }
                        if (componentTypes[0] == "administrative_area_level_1") {
                            wayPoint.address.state = types.long_name;
                        }
                        if (componentTypes == "postal_code") {
                            wayPoint.address.zipCode = types.short_name;
                        }

                    }
                }
            }

            //Add waypoints AND add the Time Slot Info.
            var payload = {
                waypoints: vm.wayPoints
                , JobScheduleId: vm.currentJobScheduleId
                , JobId: vm.$routeParams.jobId
                , ScheduleId: vm.activeTime
                , Date: vm.jobScheduleDate
            }
            console.log("Job Schedule Payload: ", payload);
            vm.$jobsService.insertJobWaypoint(payload, _InsertOrUpdateScheduleSlotSuccess, _InsertOrUpdateScheduleSlotError);

        }

        function _InsertOrUpdateScheduleSlotSuccess(data) {
            vm.notify(function () {
                vm.currentJobScheduleId = data.jobScheduleId;
                console.log("Current Job Schedule Id Result: ", vm.currentJobScheduleId);

            });
        }

        function _InsertOrUpdateScheduleSlotError(jqXhr, error) {
            console.error(error);
        }

        function _submitJobSuccess(data) {
            console.log('data', data.item);
            vm.jobId = data.item;
            vm.$localStorageService.set('jobId', vm.jobId);
        }

        function _commonErrorHandler(data) {
            console.log('error: data is: ', data);

        }

        function _togglePickupItems() {
            vm.showPickupItems = !vm.showPickupItems;
        }

        function _toggleDropoffItems() {
            vm.showDropOffItems = !vm.showDropOffItems;
        }

        function _addAddress() {

            if (!vm.wayPoints) {
                vm.wayPoints = [];
            }
            vm.wayPoints.push({ jobWaypointItemsPickup: [{}], jobWaypointItemsDropOff: [{}] });
            vm.accordionPanels.open = false;


        }

        function _setCurrentItem(currentItem) {
            vm.currentItem = currentItem;

            console.log("set current item", vm.currentItem)

            //call this function using ng-click
            //dropzone should open
            //currentItem needs to be attached to vm.
            //on success take mediaId and attach it to this vm.

        }

        function _submitJob() {


            //for (var p = 0; p < currentPickupItem.length; p++) {
            //    var currentPickupItemArray = currentPickupItem[p];
            //}

            //vm.currentPickUpItem = currentPickupItem;
            //console.log('currentpickupitem', currentPickupItem);


            for (var i = 0; i < vm.wayPoints.length; i++) {

                var wayPoint = vm.wayPoints[i];

                var addressComponents = wayPoint.address.address_components;

                var itemsPickUp = wayPoint.jobWaypointItemsPickup;
                var itemsDropOff = wayPoint.jobWaypointItemsDropOff;

                var wayPointItems = wayPoint.items;

                wayPoint.address.externalPlaceId = wayPoint.address.id;
                wayPoint.jobId = vm.jobId;

                //wayPoint.address.latitude = wayPoint.address.geometry.viewport.f.f
                //wayPoint.address.longitude = wayPoint.address.geometry.viewport.b.f

                console.log('this is addressComponents ', addressComponents);
                for (var j = 0; j < addressComponents.length; j++) {
                    var types = addressComponents[j]
                    var componentTypes = types.types

                    if (componentTypes) {
                        if (componentTypes == "street_number") {
                            var addressNumber = types.long_name;
                        }
                        if (componentTypes == "route") {
                            var addressName = types.long_name;

                            wayPoint.address.line1 = addressNumber + " " + addressName;
                        }
                        if (componentTypes[0] == "locality") {
                            wayPoint.address.city = types.long_name;
                        }
                        if (componentTypes[0] == "administrative_area_level_1") {
                            wayPoint.address.state = types.long_name;
                        }
                        if (componentTypes == "postal_code") {
                            wayPoint.address.zipCode = types.short_name;
                        }

                        if (wayPoint.address.geometry) {
                            wayPoint.address.latitude = wayPoint.address.geometry.location.lat();
                            wayPoint.address.longitude = wayPoint.address.geometry.location.lng();
                        }
                    }
                }

                for (var k = 0; k < itemsPickUp.length; k++) {
                    var itemsPickUpArray = itemsPickUp[k];

                    itemsPickUpArray.operation = 1;


                }
                for (var j = 0; j < itemsDropOff.length; j++) {
                    var itemsDropOffArray = itemsDropOff[j];

                    // itemsDropOffArray.parentItemId = itemsDropOff[j].id;
                    //itemsDropOffArray.parentItemId = vm.itemsPickup[j].id;

                    itemsDropOffArray.operation = 2;
                    // console.log('itemsDropOffArray at the end of submitJob', itemsDropOffArray);


                }


            }

            //this section loops through the waypoint IDs and appends the ID to the correct waypoint.
            function _wayPointSuccess(data) {
                console.log('wp success data: ', data);
                //_addAddress();

                _insertOrUpdate();
                console.log('waypoint successfully sent to server', data);
            }


            //THIS SECTION IS FOR GETTING JOBBYID AND SETTING THE ID TO EACH ITEMTYPE - DF
            function _getJobByIdSuccess(data) {
                console.log('getJobByIdSuccess', data);

                if (data.item) {
                    //vm.notify(function () {
                    vm.job = data.item

                    vm.wayPoints = data.item.jobWaypoints;
                    console.log('this is vm.wayPoints', vm.wayPoints);

                    vm.itemsPickup = vm.job.itemsToBeDroppedOff;
                    vm.wayPoints = data.item.jobWaypoints;

                    vm.timeSlotInfo.teamId = data.item.teamId;
                    console.log("Team Id Is: ", vm.timeSlotInfo.teamId);
                    //})
                }

                //we loop through every wayPoint and format the address via a utility function so it is exactly like geocomplete's address
                if (vm.wayPoints) {
                    for (var i = 0; i < vm.wayPoints.length; i++) {
                        // console.log('vm.wayPoints[i].address before going into utility ', vm.wayPoints[i].address);
                        if (vm.wayPoints[i].address) {
                            vm.wayPoints[i].address = vm.$utilityService.formatAddress(vm.wayPoints[i].address);
                        }
                    }
                    _getTimeSlotsForTeamId();
                } else { // if there are no wayPoints, we want to call _addAddress() which will create an initial accordion
                    _addAddress();
                }
            }

            function _jobDropZone() {
                new Dropzone('.job-dropzone', { url: "/api/media/insert" });
                console.log('dropzonefiring');
            }


            function _addItem(pickupItemsInSpecificWaypoint) {
                pickupItemsInSpecificWaypoint.push({});
                console.log('item added...this is items pickup for this waypoint: ', pickupItemsInSpecificWaypoint);
            }


            function _deletePickupItem(pickUpItemsInSpecificWaypoint, index) {
                console.log("list of items in one waypoint", pickUpItemsInSpecificWaypoint);
                console.log("index of item in pickupItem list", index);

                if (pickUpItemsInSpecificWaypoint[index].id) {
                    console.log('this item has an id:', pickUpItemsInSpecificWaypoint[index].id);
                    _deleteItem(pickUpItemsInSpecificWaypoint[index].id);
                }
                pickUpItemsInSpecificWaypoint.splice(index, 1);
            }

            function _deleteItem(id) {
                vm.$jobsService.deleteJobItems(id, _onDeleteItemSuccess, _onDeleteItemError);
            }

            function _onDeleteItemSuccess() {
                console.log("Delete Item Success");
            }

            function _onDeleteItemError() {
                console.log("Delte Item Error");
            }

            function _deleteSuccess(data) {
                console.log('delete error', data);
            }

            function _getJobItemsSuccess(data) {
                vm.jobItems = data.items;
                console.log('get jobitems success', vm.jobItems);

            }

            function _onDateChange() {
                //Gets Date on Date Select
                console.log("Current Chosen Date Datepicker Format: ", vm.currentDate);
                vm.currentSubmitDate = _convertDateToZeroTime(vm.currentDate);
                vm.timeSlotInfo.queryDate = vm.currentSubmitDate;
                vm.timeSlotInfo.queryDay = _getDayOfWeekFromDate(new Date(vm.currentDate));
                console.log("Current Chosen Date Zero format: ", vm.timeSlotInfo.queryDate);
                _getAvailableByDate();

                //Then Do a Get with Date & Website Id
            }

            //Get Today Time Slots on Render
            //function _getTodayTimeSlots() {
            //    console.log("Today Format: ", vm.dateToday);
            //    vm.timeSlotInfo.queryDate = vm.dateToday;
            //    vm.timeSlotInfo.queryDay = _getDayOfWeekFromDate(new Date(vm.dateToday));
            //    _getAvailableByToday();
            //}

            function _convertDateToZeroTime(date) {
                date = vm.$filter('date')(date, "yyyy/MM/dd");
                //console.log("Zero Format Date: ", date);
                return date;
            }

            function _getDayOfWeekFromDate(date) {
                var dayOfWeek = vm.$filter('date')(date, "EEEE");
                return dayOfWeek;
            }

            function _addDays(date, days) {
                var result = new Date(date);
                result.setDate(result.getDate() + days);
                return result;
            }

            // Getting time slots based on location
            function _getTimeSlotsForTeamId() {
                if (vm.timeSlotInfo.teamId === 0) {
                    //Later need to change this to modal with more info about contacting Jamie
                    vm.$alertService.warning("We are currently unable to deliver to your location as it is out of our delivery area.  Please contact Jamie at bringpro@bringpro.com");
                    //Disable Both Schedule Sections
                    vm.hideTimeScheduler = true;
                }
                if (vm.timeSlotInfo.teamId > 0) {
                    _grabTodayAvailability();
                    //Un-hide the Scheduler section because Team EXISTS For the Zipcode entered.
                    vm.hideTimeScheduler = false;

                }
            }

            function _getAvailableByDate() {
                //Hard Coded for now
                //vm.timeSlotInfo.websiteId = vm.websites.id;
                console.log("Data object w/ zero date And Website Id: ", vm.timeSlotInfo);
                vm.$jobScheduleService.getAvailableByDate(vm.timeSlotInfo, _getAvailableByDateSuccess, _getAvailableByDateError)
            }

            function _getAvailableByDateSuccess(data) {
                vm.notify(function () {
                    vm.availableTimeSlotsByDate = data.items;
                    console.log("Available Time Slots For Selected Date: ", vm.availableTimeSlotsByDate);
                    for (var i = 0; i < vm.availableTimeSlotsByDate.length; i++) {
                        if ((vm.availableTimeSlotsByDate[i].capacity - vm.availableTimeSlotsByDate[i].usedCapacity) <= 0) {
                            vm.availableTimeSlotsByDate[i].disabled = true;
                        }
                    }

                    if (vm.availableTimeSlotsByDate.length === 0) {
                        vm.$alertService.warning("We currently do not have any slots for this day!")
                    }

                    console.log("Check Disabled: ", vm.availableTimeSlotsByDate);
                });
            }

            function _getAvailableByDateError(jqXhr, error) {
                console.error(error);
            }

            //Getting Today's ASAP Time Slot
            function _grabTodayAvailability() {
                vm.timeSlotInfo.scheduleType = false;
                vm.timeSlotInfo.queryDate = vm.dateToday;
                vm.timeSlotInfo.queryDay = _getDayOfWeekFromDate(new Date(vm.dateToday));
                //Later will need Team Id As well as Website Id to enter into the ASAP TIME SLOT if none exists.
                console.log("ASAP GET TIME SLOT Payload: ", vm.timeSlotInfo);
                vm.$jobScheduleService.getTimeSlotByDate(vm.timeSlotInfo, _grabTodayAvailabilitySuccess, _grabTodayAvailabilityError);
            }

            function _grabTodayAvailabilitySuccess(data) {
                vm.notify(function () {
                    vm.todayAvailability = data.items;
                    console.log("ASAP Time Slot Info: ", vm.todayAvailability);
                    for (var i = 0; i < vm.todayAvailability.length; i++) {
                        if (vm.todayAvailability[i].capacity === 0) {
                            vm.asapUnavailable = true;
                        } else if (vm.todayAvailability[i].capacity === 1) {
                            vm.asapUnavailable = false;
                        }
                    }

                });
            }

            function _grabTodayAvailabilityError(jqXhr, error) {
                console.error(error);
            }

            //Need to replace this area with NEW ASAP logic
            //function _getAvailableByToday() {
            //    //Hard Coded for now
            //    //vm.timeSlotInfo.websiteId = vm.websites.id;
            //    console.log("Data object w/ zero date And Website Id: ", vm.timeSlotInfo);
            //    vm.$jobScheduleService.getAvailableByDate(vm.timeSlotInfo, _getAvailableByTodaySuccess, _getAvailableByTodayError)
            //}

            //function _getAvailableByTodaySuccess(data) {
            //    vm.notify(function () {
            //        vm.availableTimeSlotsByToday = data.items;
            //        console.log("Available Time Slots For TODAY: ", vm.availableTimeSlotsByToday);
            //        for (var i = 0; i < vm.availableTimeSlotsByToday.length; i++) {
            //            if (vm.availableTimeSlotsByToday[i].capacity === 0) {
            //                vm.availableTimeSlotsByToday[i].disabled = true;
            //            }
            //        }
            //    });
            //}

            //function _getAvailableByTodayError(jqXhr, error) {
            //    console.error(error);
            //}

            //Old Time Button Function to activate Accordion text, grab scheduleid, and show active button
            //function _asapTimeButton(id) {
            //    console.log("Active Time ASAP id", id);
            //    vm.activeTime = id;
            //    for (var i = 0; i < vm.availableTimeSlotsByToday.length; i++) {
            //        if (vm.availableTimeSlotsByToday[i].id == vm.activeTime) {
            //            vm.timeSlotOptionText = (" between " +
            //                vm.availableTimeSlotsByToday[i].timeStart + " to " + vm.availableTimeSlotsByToday[i].timeEnd);
            //        }
            //    }
            //    vm.timeScheduleSubmit = false;

            //}

            //CLICK HANDLER FOR TIME SLOT BUTTONS ON SCHEDULED
            function _jobTimeButton(id) {
                vm.activeTime = id;
                console.log("Active Time Scheduled Id: ", vm.activeTime);
                for (var i = 0; i < vm.availableTimeSlotsByDate.length; i++) {
                    if (vm.availableTimeSlotsByDate[i].id == vm.activeTime) {
                        vm.timeSlotOptionText = (vm.availableTimeSlotsByDate[i].timeStart + " to " + vm.availableTimeSlotsByDate[i].timeEnd);
                    }
                }
                vm.timeScheduleSubmit = false;

            }

            function _submitTimeSelection() {
                vm.$alertService.success("Scheduled Job Selected!");
                console.log("Current Submit info Schedule Id: ", vm.activeTime);
                console.log("Date Current: ", vm.dateToday);
                console.log("Date Selected: ", vm.currentSubmitDate);
                console.log("Job Id: ", vm.jobId);
                console.log("Current Time Slot Selected: ", vm.activeTime);

                vm.showScheduledSubmitButton = false;
                vm.showScheduledSelectedButton = true;

                //Hide Date/Selected Time Slot
                //vm.showDatePicker = false;
                vm.showDateTimePicker = false;

                //Inserting Schedule After Scheduled Submit
                vm.jobScheduleDate = vm.currentSubmitDate;
                _InsertOrUpdateScheduleSlot();

            }


            //Payment and coupon
            function _getCustomerByUserId() {

                vm.$billingService.getCustomerByUserId(_getCustomerByUserIdSuccess, _getCustomerByUserIdError);

            }

            function _getCustomerByUserIdError(jqXhr, error) {
                console.log("Error getting CC info", error);
            }

            function _getCustomerByUserIdSuccess(data) {
                vm.notify(function () {
                    vm.customerInfo = data.item;
                    console.log("Braintree", vm.customerInfo);
                });
            }

            function _getCurrentUserCCInfo() {
                vm.$dashboardService.getCurrentUserCCInfo(_getCurrentUserCCInfoSuccess, _getCurrentUserCCInfoError);
            }

            function _getCurrentUserCCInfoSuccess(data) {
                vm.customerCards = data.items;
                console.log("Customer Card List:", vm.customerCards);
            }

            function _getCurrentUserCCInfoError(jqXhr, error) {

                console.log("Error getting CC info", error);
            }

            function _updateSucess(data) {
                console.log(data);
            }


            //Brain Tree
            function _clientInstance(err, clientInstance) {
                if (err) {
                    console.error(err);
                    return;
                }
                braintree.hostedFields.create({
                    client: clientInstance,
                    styles: {
                        'input': {
                            'font-size': '12px',
                            'font-family': 'helvetica, tahoma, calibri, sans-serif',
                            'color': '#3a3a3a'
                        },
                        ':focus': {
                            'color': 'black'
                        },
                        'input.invalid': {
                            'color': 'red'
                        },
                        'input.valid': {
                            'color': 'green'
                        }
                    },
                    fields: {
                        number: {
                            selector: '#card-number',
                            placeholder: '1234 5678 9000 1234'
                        },
                        cvv: {
                            selector: '#cvv',
                            placeholder: '123'
                        },
                        expirationMonth: {
                            selector: '#expiration-month',
                            placeholder: 'MM'
                        },
                        expirationYear: {
                            selector: '#expiration-year',
                            placeholder: 'YY'
                        },
                        postalCode: {
                            selector: '#postal-code',
                            placeholder: '12345'
                        }
                    }
                },
                    _hostedFieldsInstance);

            }

            function _hostedFieldsInstance(err, hostedFieldsInstance) {
                if (err) {
                    console.error(err);
                    return;
                }
                //New Validation for EACH Field
                hostedFieldsInstance.on('validityChange', _onValidityChange);
                hostedFieldsInstance.on('cardTypeChange', _onCardTypeChange);

                vm.hostedFieldsInstance = hostedFieldsInstance;

            }



            function _onCardTypeChange(event) {
                // Handle a field's change, such as a change in validity or credit card type
                if (event.cards.length === 1) {
                    $('#card-type').text(event.cards[0].niceType);
                } else {
                    $('#card-type').text('Card');
                }
            }


            function _onValidityChange(event) {
                var field = event.fields[event.emittedBy];


                vm.notify(function () {
                    var formValid = Object.keys(event.fields).every(function (key) {
                        return event.fields[key].isValid;
                    });

                    if (formValid) {
                        vm.validCard = true;
                        console.log("goodCard", vm.validCard);

                    } else {
                        vm.validCard = false;
                        console.log("bad card", vm.validCard);
                    };
                });

                if (field.isValid) {


                    if (event.emittedBy === 'expirationMonth') {
                        $('#expiration-month').next('small').text('');
                    } else if (event.emittedBy === 'expirationYear') {
                        $('#expiration-year').next('small').text('');
                    } else if (event.emittedBy === 'number') {
                        $('#card-number').next('small').text('');
                    } else if (event.emittedBy === 'postalCode') {
                        $('#postal-code').next('small').text('');
                    } else if (event.emittedBy === 'cvv') {
                        $('#cvv').next('small').text('');
                    }

                    // Apply styling for a valid field
                    $(field.container).parents('.form-group').addClass('has-success');
                    //$('#cvv').next('span').text('');
                } else if (field.isPotentiallyValid) {


                    // Remove styling  from potentially valid fields
                    $(field.container).parents('.form-group').removeClass('has-warning');
                    $(field.container).parents('.form-group').removeClass('has-success');
                    if (event.emittedBy === 'number') {
                        $('#card-number').next('small').text('');
                    }
                    if (event.emittedBy === 'postalCode') {
                        $('#postal-code').next('small').text('');
                    }
                    if (event.emittedBy === 'cvv') {
                        $('#cvv').next('small').text('');
                    }
                    if (event.emittedBy === 'expirationMonth') {
                        $('#expiration-month').next('small').text('');
                    }
                    if (event.emittedBy === 'expirationYear') {
                        $('#expiration-year').next('small').text('');
                    }
                } else {
                    // Add styling to invalid fields
                    $(field.container).parents('.form-group').addClass('has-warning');

                    if (event.emittedBy === 'postalCode') {
                        $('#postal-code').next('small').text('Check Postal Code');
                    }
                    if (event.emittedBy === 'cvv') {
                        $('#cvv').next('small').text('Check CVV');
                    }
                    if (event.emittedBy === 'expirationMonth') {
                        $('#expiration-month').next('small').text('Check Month');
                    }
                    if (event.emittedBy === 'expirationYear') {
                        $('#expiration-year').next('small').text('Check Year');
                    }
                    // Add helper text for an invalid card number
                    if (event.emittedBy === 'number') {
                        $('#card-number').next('small').text('Check Card Number');
                    }
                }

            }


            function _tokenize(err, payload) {
                if (err) {
                    console.log(err);
                    return;
                }

                //in here for checkout


                //            var payload = {
                //                'externalCardIdNonce': payload.nonce
                //            }
                //            vm.firstName = null;
                //            vm.lastName = null;
                //            vm.phone = null;
                //            vm.address = null;
                //            vm.state = null;
                //            vm.zipCode = null;
                //            vm.city = null;

                var payload =
                {
                    email: "test",
                    externalCardIdNonce: payload.nonce,
                    firstName: vm.firstName,
                    jobId: vm.jobId,
                    lastName: vm.lastName,
                    phone: vm.phone
                }


                vm.$jobsService.guestCheckout(payload, _guestPaymentSuccess, vm.commonErrorHandler);

                //            vm.$billingService.insertGuestPayment(payload, _guestPaymentSuccess, vm.commonErrorHandler);

            }

            function _guestPaymentSuccess(data) {
                console.log("Nonce", vm.jobId, data);

                //            var jobPayload = {
                //                'PaymentNonce': data.item
                //            }
                //
                //            vm.$jobsService.updatePaymentNonce(vm.jobId, jobPayload, vm.updateSuccess, vm.commonErrorHandler);



            }

            $scope.onlyNumbers = /^\d+$/;
            //now we need to send the payload.job

            console.log("current User", vm.currentUser);

            vm.registeredUser = function () {
                if (vm.currentUser) {
                    return true;
                }
            }


            //This is for the Guest CheckOut - will need to update the job table with the Phone number
            function _submitCard(isValid) {

                if (isValid) {
                    vm.hostedFieldsInstance.tokenize(_tokenize);


                    console.log('job id is in Guest Check Out', vm.jobId);
                    vm.$jobsService.InsertBringgJob(vm.jobId, vm.insertBringgJobSuccess, vm.insertBringgJobFail);
                } else {
                    console.log("Not valid")
                }


            }

            function _insertBringgJobSuccess(data) {
                //  bringpro/dashboard/#/jobs/2481/invoice
                var url = "/" + vm.currentWebsite.slug + "/dashboard/#/jobs/" + data.item + "/invoice";

                vm.$alertService.success("Your job has been submitted.")

                window.location.href = url;
            }

            function _insertBringgJobFail(jqXhr, error) {
                console.log("Insert Bringg Job fail", jqXhr);
            }


            //This is the function for the registered user CheckOut
            function _submitPayment() {

                vm.$jobsService.InsertBringgJob(vm.jobId, vm.insertBringgJobSuccess, vm.insertBringgJobFail);


                //update service in here to update the job nonce
                var jobPayload = {
                    'PaymentNonce': vm.customerCards.externalCardIdNonce

                }

                vm.$jobsService.updatePaymentNonce(vm.jobId, jobPayload, vm.updateSuccess, vm.commonErrorHandler);

            }

            //Get for the Credits Part of Job Overview
            function _GetCreditsForJobPage() {
                vm.$userCreditsService.GetCreditsForJobPage(_GetCreditsForJobPageSuccess, _GetCreditsForJobPageError);
            }

            function _GetCreditsForJobPageSuccess(data) {
                vm.notify(function () {
                    vm.userCreditOptions = data.items;
                    console.log("User Credit Options Object", vm.userCreditOptions);
                });
            }

            function _GetCreditsForJobPageError(jqXhr, error) {
                console.error(error);
            }

            function _submitCoupon() {
                vm.$alertService.warning("This Feature is currently being implemented!")
            }

        }
    }
})();
