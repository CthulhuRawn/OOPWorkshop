!(function () {
    var myApp = angular.module('myApp');
    myApp.controller('SearchCustomersCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.types = DomainDecodes.regionDecode;
        $scope.results;

        $scope.propertyName = 'Id';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.customers,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו לקוחות העונים על הדרישה');
                }
            });
        }
    }]);

    myApp.controller('CustomersCtrl', ['$scope', '$http', '$routeParams', '$location', 'DomainDecodes', 'ServerRoutes', 'toaster', 'LoginService',
    function ($scope, $http, $routeParams, $location, DomainDecodes, ServerRoutes, toaster, LoginService) {
        var init = function () {

            $scope.now = moment(new Date());
            $scope.dateToCompare = moment(new Date() - 30);

            $scope.regionTypes = DomainDecodes.regionDecode;
            $scope.complaintTypes = DomainDecodes.complaintType;

            $scope.model = {};
            $scope.model.customer = {};
            $scope.model.review = {};
            $scope.model.complaint = {};
            $scope.model.passwordChanging = {};
            $scope.originalCustomer = {};
            $scope.submitted = false;

            $http({
                url: ServerRoutes.customers,
                method: "GET",
                params: { id: $routeParams.Id },
            }).then(function searchCompleted(response) {
                $scope.model.customer = angular.copy(response.data);
                $scope.originalCustomer = angular.copy($scope.model.customer);

                $scope.dateToCompare = moment($scope.model.customer.FreezeDate,'DD/MM/YYYY');
            });

            $scope.getReviews($scope);
        }

        $scope.submitCustomer = function (isValid) {
            $scope.submitted = true;

            if (!isValid)
                return;


            if ($scope.model.passwordChanging.new != null && $scope.model.passwordChanging.new != "") {
                $scope.model.customer.Password = $scope.model.passwordChanging.new;
                $scope.model.verifiedPassword = $scope.model.customer.Password;
            }

            $http({
                url: ServerRoutes.customers,
                method: "PUT",
                params: { id: $scope.model.customer.Id },
                data: $scope.model.customer,
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    $scope.originalCustomer = angular.copy($scope.model.customer);
                    toaster.success('נתוני הלקוח נשמרו בהצלחה');
                    $scope.submitted = false;

                    if (response.data.AuthenticationKey != null) {
                        LoginService.updatePassword(response.data.AuthenticationKey);
                    }
                }
            });
        };

        $scope.saveReview = function () {
            $scope.model.review.Date = moment(new Date()).format("DD/MM/YYYY");
            $scope.model.review.ReviewedCustomer = {};
            $scope.model.review.ReviewedCustomer.Id = $scope.model.customer.Id;
            $http({
                url: ServerRoutes.reviews.base,
                method: "POST",
                data: $scope.model.review,
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    toaster.success('פרטי חוות הדעת נשמרו בהצלחה');
                    $scope.model.review = {};
                }
            });
        };

        $scope.saveComplaint = function () {

            $scope.model.complaint.Date = moment(new Date()).format("DD/MM/YYYY");
            $scope.model.complaint.OffendingCustomer = {};
            $scope.model.complaint.OffendingCustomer.Id = $scope.model.customer.Id;

            $http({
                url: ServerRoutes.complaints.base,
                method: "POST",
                data: $scope.model.complaint,
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    toaster.success('פרטי התלונה נשמרו בהצלחה');
                    $scope.model.complaint = {};
                }
            });
        };

        $scope.freezeCustomer = function () {
            var freezeDate = new Date();
            freezeDate.setDate(freezeDate.getDate() + 30);
            $scope.dateToCompare = moment(new Date() + 30);

            $scope.model.customer.FreezeDate = moment(freezeDate).format("DD/MM/YYYY");
            $scope.submitCustomer(true);
        }

        $scope.cancelChanges = function () {
            $scope.model.customer = angular.copy($scope.originalCustomer);
        };

        $scope.getComplaints = function (scope) {
            scope.customerComplaints = {};
            $http({
                url: ServerRoutes.complaints.search,
                method: "GET",
                params: { customerId: $routeParams.Id },
            }).then(function searchCompleted(response) {
                scope.customerComplaints = angular.copy(response.data)
            });
        };

        $scope.getReviews = function (scope) {
            scope.customerReviews = {};
            $http({
                url: ServerRoutes.reviews.search,
                method: "GET",
                params: { customerId: $routeParams.Id },
            }).then(function searchCompleted(response) {
                scope.customerReviews = angular.copy(response.data)
            });
        };

        init();
    }]);

    myApp.controller('CustomerModalCtrl', function ($uibModal, $log) {
        var $ctrl = this;

        $ctrl.open = function (func, template) {

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: template,
                controller: 'CustomerModalInstanceCtrl',
                controllerAs: '$ctrl',
                size: 'lg',
                resolve: {
                    action: function () {
                        return func;
                    }
                }
            });

        };
    });

    myApp.controller('CustomerModalInstanceCtrl', function ($uibModalInstance, action, $http, $routeParams, $scope, ServerRoutes) {
        action($scope);

        $scope.close = function () {
            $uibModalInstance.close();
        };
    });
})();
