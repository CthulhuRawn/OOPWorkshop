!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('SearchAvailableOrdersCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.types = DomainDecodes.fieldType;
        $scope.submitted = false;
        $scope.results;

        var today = new Date();
        $scope.model.date = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Field.Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function (isValid) {
            $scope.submitted = true;

            if (!isValid)
                return;

            $http({
                url: ServerRoutes.orders.availables,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = angular.copy(response.data);
                $scope.submitted = false;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו הזמנות העונות על הדרישה');
                }
            });
        }
    }]);

    myApp.controller('ownedOrdersCrtl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.types = DomainDecodes.orderStatus;
        $scope.results;
        var today = new Date();
        $scope.model.fromDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.orders.searchmyorders,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו הזמנות העונים על הדרישה');
                }
            });
        }
    }]);

    myApp.controller('OrdersCtrl', ['$rootScope', '$scope', '$http', '$routeParams', '$location', 'DomainDecodes', 'ServerRoutes', 'toaster', function ($rootScope, $scope, $http, $routeParams, $location, DomainDecodes, ServerRoutes, toaster) {

        $scope.getOptionals = function () {

            var optinalOrdersParams = {};
            optinalOrdersParams.orderId = $scope.model.Id;
            optinalOrdersParams.fieldId = $scope.model.Field.Id;
            optinalOrdersParams.fieldType = $scope.model.Field.Type;
            optinalOrdersParams.date = $scope.datePicker;
            $http({
                url: ServerRoutes.orders.optionals,
                method: "GET",
                params: optinalOrdersParams,
            }).then(function searchCompleted(response) {
                $scope.optinalOrders = angular.copy(response.data);

                if (angular.equals($scope.optinalOrders, [])) {
                    toaster.info('לא נמצאו מועדים פנויים');
                }
            });
        }

        $scope.getOptionalField = function () {
            var optinalFieldsParams = {};
            optinalFieldsParams.fieldId = null;
            optinalFieldsParams.fieldName = null;
            optinalFieldsParams.type = $scope.model.Field.Type;

            $http({
                url: ServerRoutes.fields,
                method: "GET",
                params: optinalFieldsParams,
            }).then(function searchCompleted(response) {
                $scope.optinalFields = angular.copy(response.data);
            });
        }

        $scope.fieldTypeChanged = function () {
            var optinalFieldsParams = {};
            optinalFieldsParams.fieldId = null;
            optinalFieldsParams.fieldName = null;
            optinalFieldsParams.type = $scope.model.Field.Type;

            $http({
                url: ServerRoutes.fields,
                method: "GET",
                params: optinalFieldsParams,
            }).then(function searchCompleted(response) {
                $scope.optinalFields = angular.copy(response.data);

                $scope.model.Field.Id = $scope.optinalFields[0].Id;
                $scope.getOptionals();
            });

        }

        $scope.acceptRejectParticipant = function (participant, status) {
            participant.Status = status;

            $http({
                url: ServerRoutes.orders.updatepraticipant,
                method: "PUT",
                params: { id: participant.Id },
                data: participant,
            }).then(function searchCompleted(response) {
                $location.path('/editOrder/' + response.data.Id);
                $scope.model.Participants = angular.copy(response.data.Participants);
                $scope.originalModel.Participants = angular.copy($scope.model.Participants);
            });
        }

        var init = function () {
            $scope.fieldType = DomainDecodes.fieldType;

            $scope.model = {};
            $scope.originalModel = {};
            $scope.optinalOrders = {};

            $scope.lessThen24Hours = false;

            if ($routeParams.Id) {
                $scope.isNew = false;

                $http({
                    url: ServerRoutes.orders.base,
                    method: "GET",
                    params: { id: $routeParams.Id },
                }).then(function searchCompleted(response) {
                    $scope.model = angular.copy(response.data);
                    $scope.originalModel = angular.copy($scope.model);
                    $scope.datePicker = moment(new Date($scope.model.StartDate)).format("DD/MM/YYYY");
                    $scope.getOptionals();
                    $scope.getOptionalField();


                    var tempDate = new Date($scope.model.StartDate);
                    tempDate.setDate(tempDate.getDate() - 1);

                    $scope.lessThen24Hours = new Date() > tempDate;
                });
            }
            else {
                $scope.isNew = true;
                $scope.model = angular.fromJson($routeParams.order);
                $scope.model.Owner = { Id: $rootScope.sharedVariables.userId };
                $scope.datePicker = moment(new Date($scope.model.StartDate)).format("DD/MM/YYYY");
                $scope.getOptionals();
                $scope.getOptionalField();
            }
        }

        $scope.submitOrder = function (isValid, status) {
            $scope.submitted = true;

            if (!isValid)
                return;

            //$scope.model.Participants = null;
            $scope.model.Status = status;
            var modalToSend = angular.copy($scope.model);
            modalToSend.StartDate = new Date(modalToSend.StartDate).getTime();
            if ($scope.isNew) {
                $http({
                    url: ServerRoutes.orders.base,
                    method: "POST",
                    data: modalToSend,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $scope.model = angular.copy(response.data);
                        $scope.originalModel = angular.copy($scope.model);
                        $location.path('#/editOrder/' + $scope.model.Id);
                        toaster.success('ההזמנה נשמרה בהצלחה');     
                    }
                });
            }
            else {
                $http({
                    url: ServerRoutes.orders.base,
                    method: "PUT",
                    params: { id: $scope.model.Id },
                    data: modalToSend,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $scope.originalModel = angular.copy($scope.model);
                        toaster.success('ההזמנה עודכנה בהצלחה');
                    }
                });
            }
        };

        $scope.cancelOrder = function () {

            $scope.originalModel.Status = 4;
            var modalToSend = angular.copy($scope.originalModel);
            //modalToSend.StartDate = new Date(modalToSend.StartDate).getTime();

            $http({
                url: ServerRoutes.orders.base,
                method: "PUT",
                params: { id: $scope.originalModel.Id },
                data: modalToSend,
            }).then(function searchCompleted(response) {
                //$scope.originalModel = angular.copy($scope.model);
                if (response.status == 200) {
                    toaster.success('ההזמנה בוטלה בהצלחה');
                    $location.path('/ownedOrders');
                }
            });
        }

        $scope.cancelChanges = function () {
            $scope.model = angular.copy($scope.originalModel);
            $scope.fieldTypeChanged();
        };


        $scope.acceptRejectOrder = function (newStatus) {
            $scope.model.Status = newStatus;

            $http({
                url: ServerRoutes.orders.base,
                method: "PUT",
                params: { id: $scope.model.Id },
                data: $scope.model,
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    toaster.success('הפעולה בוצעה בהצלחה');
                    $location.path('/searchOrders');
                }
            });
        }

        init();
    }]);

    myApp.controller('SearchAvailableOrdersToJoinCrtl', ['$scope', '$route', '$http', 'DomainDecodes', 'ServerRoutes', 'toaster', function ($scope, $route, $http, DomainDecodes, ServerRoutes, toaster) {
        $scope.model = {};
        $scope.orderStatuses = DomainDecodes.orderStatus;
        $scope.results;

        var today = new Date();
        $scope.model.fromDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function (isValid) {
            $scope.submitted = true;

            if (!isValid)
                return;

            $http({
                url: ServerRoutes.orders.availablestojoin,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו הזמנות העונות על הדרישה');
                }
            });
        }

        $scope.joinToOrder = function (order, index) {
            $http({
                url: ServerRoutes.orders.jointoorder,
                method: "GET",
                params: { orderId: order.Id },
            }).then(function searchCompleted(response) {
                //$location.path('/searchAvailableOrdersToJoin');
                // $route.reload();
                if (response.status == 200) {
                    toaster.success('בקשת ההצטרפות נשמרה בהצלחה');
                    $scope.results.splice(index, 1);
                }
            });
        }
    }]);

    myApp.controller('SearchOrdersCrtl', ['$scope', '$route', '$http', 'DomainDecodes', 'ServerRoutes', 'toaster',
    function ($scope, $route, $http, DomainDecodes, ServerRoutes, toaster) {
        $scope.model = {};
        $scope.orderStatuses = DomainDecodes.orderStatus;
        $scope.results;

        var today = new Date();
        $scope.model.fromDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {

            $http({
                url: ServerRoutes.orders.search,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו הזמנות העונות על הדרישה');
                }
            });
        }

        $scope.submitSearch();

        $scope.acceptRejectOrder = function (order, newStatus) {
            order.Status = newStatus;

            $http({
                url: ServerRoutes.orders.base,
                method: "PUT",
                params: { id: order.Id },
                data: order,
            }).then(function searchCompleted(response) {
                order = response.data;

                toaster.success('הפעולה בוצעה בהצלחה');
            });
        }
    }]);

    myApp.controller('PendingOrdersToJoinCrtl', ['$scope', '$route', '$http', 'DomainDecodes', 'ServerRoutes', 'toaster', function ($scope, $route, $http, DomainDecodes, ServerRoutes, toaster) {
        $scope.model = {};
        $scope.orderStatuses = DomainDecodes.orderStatus;
        $scope.invitationStatuses = DomainDecodes.invitationStatus;
        $scope.results;

        var today = new Date();
        $scope.model.fromDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Order.Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.participants,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו הזמנות העונות על הדרישה');
                }
            });
        }

        //$scope.submitSearch();

        $scope.cancelJoining = function (participantId, index) {

            $http({
                url: ServerRoutes.participants,
                method: "DELETE",
                params: { id: participantId }
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    $scope.results.splice(index, 1);
                    toaster.success('הפעולה בוצעה בהצלחה');
                }
            });
        }
    }]);
})();
