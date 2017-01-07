!(function () {
    var myApp = angular.module('myApp');
    myApp.controller('SearchEmployeesCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', function ($scope, $http, ServerRoutes) {
        $scope.model = {};
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
                url: ServerRoutes.employees,
                method: "GET",
                params: $scope.model,
            }).then(function searchCompleted(response) {
                $scope.results = response.data;

                if (angular.equals($scope.results, [])) {
                    toaster.info('לא נמצאו עובדים העונים על הדרישה');
                }
            });

        }
    }]);

    myApp.controller('EmployeesCtrl', ['$scope', '$http', '$routeParams', '$location', 'ServerRoutes', 'toaster', function ($scope, $http, $routeParams, $location, ServerRoutes, toaster) {

        var init = function () {

            $scope.submitted = false;
            $scope.model = {};
            $scope.model.employee = {};
            $scope.model.passwordChanging = {};
            $scope.model.passwordChanging.new = null;
            $scope.model.passwordChanging.newVerify = null;

            $scope.originalModel = {};

            if ($routeParams.Id) {
                $scope.isNew = false;

                $http({
                    url: ServerRoutes.employees,
                    method: "GET",
                    params: { id: $routeParams.Id },
                }).then(function searchCompleted(response) {
                    $scope.model.employee = angular.copy(response.data)
                    $scope.originalModel = angular.copy($scope.model.employee);
                    $scope.model.verifiedPassword = $scope.model.employee.Password;
                });
            }
            else {
                $scope.isNew = true;
            }
        }

        $scope.submitEmployee = function (isValid) {
            $scope.submitted = true;

            if (!isValid)
                return;

            if ($scope.model.passwordChanging.new != null && $scope.model.passwordChanging.new != "") {
                $scope.model.employee.Password = $scope.model.passwordChanging.new;
                $scope.model.verifiedPassword = $scope.model.employee.Password;
            }

            if ($scope.isNew) {
                $http({
                    url: ServerRoutes.employees,
                    method: "POST",
                    data: $scope.model.employee,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        if (response.data.AlreadyExists) {
                            toaster.error("אופס!", "שם משתמש כבר קיים באתר!", 5000);
                            return;
                        }

                        // TODO: is this line needed?
                        $scope.model.verifiedPassword = $scope.model.employee.Password;
                        toaster.success('העובד נשמר בהצלחה');
                        $location.path('/editEmployee/' + response.data.Employee.Id);
                    }
                });
            }
            else {
                $http({
                    url: ServerRoutes.employees,
                    method: "PUT",
                    params: { id: $scope.model.employee.Id },
                    data: $scope.model.employee,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $scope.originalModel = angular.copy($scope.model.employee);
                        toaster.success('פרטי העובד עודכנו בהצלחה');
                    }
                });
            }
        };

        $scope.cancelChanges = function () {
            $scope.model.employee = angular.copy($scope.originalModel);
        };

        $scope.delete = function () {
            $http({
                url: ServerRoutes.employees,
                method: "DELETE",
                params: { id: $scope.model.employee.Id }
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    $location.path('/editEmployee');
                    toaster.success('העובד נמחק בהצלחה');
                }
            });
        };

        init();
    }]);
})();
