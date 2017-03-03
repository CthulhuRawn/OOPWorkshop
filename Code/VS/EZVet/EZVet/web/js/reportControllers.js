!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('ReportFieldsCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', function ($scope, $http, ServerRoutes, toaster) {
        $scope.model = {};
        $scope.results;

        $scope.propertyName = 'FieldId';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.fields,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results,[])) {
                    toaster.info('לא נמצאו מגרשים העונים על הדרישה');
                }
            });
        }
    }]);

    myApp.controller('ReportCustomerCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', function ($scope, $http, ServerRoutes, toaster) {
        $scope.model = {};
        $scope.results;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.customers,
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

    myApp.controller('ReportComplaintCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.types = DomainDecodes.complaintType;
        $scope.results;
        $scope.submitted = false;
        var today = new Date();
        $scope.model.untilDate = moment(today).format("DD/MM/YYYY");

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
                url: ServerRoutes.reports.complaints,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו נתונים העונים על הדרישה');
                }

                $scope.submitted = false;
            });
        }
    }]);
})();
