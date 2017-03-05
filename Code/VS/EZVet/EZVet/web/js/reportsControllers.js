!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('reportIncomeItemCtrl',
    [
        '$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster',
        function($scope, $http, ServerRoutes, DomainDecodes, toaster) {
            $scope.model = {};
            $scope.dateParts = DomainDecodes.dateParts;
            var today = new Date();
            $scope.model.EndDate = moment(today).format("DD/MM/YYYY");

            $scope.propertyName = 'Date';
            $scope.reverse = false;

            $scope.sortBy = function(propertyName) {
                // reverse current or false for new property
                $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
                $scope.propertyName = propertyName;
            };
            $scope.print = function() {
                window.print();
            }

            $scope.submitSearch = function() {
                $http({
                    url: ServerRoutes.reports.perItem,
                    method: "GET",
                    params: $scope.model
                }).then(function searchCompleted(response) {
                    $scope.rows = response.data;

                    if (angular.equals($scope.rows, [])) {
                        toaster.info("No rows match your search");
                    }
                });
            }
        }
    ]);

    myApp.controller('reportIncomeTypeCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.dateParts = DomainDecodes.dateParts;
        $scope.animalTypes = DomainDecodes.animalTypes;
        var today = new Date();
        $scope.model.EndDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Date';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.print = function () {
            window.print();
        }

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.perType,
                method: "GET",
                params: $scope.model
            }).then(function searchCompleted(response) {
                $scope.rows = response.data;

                if (angular.equals($scope.rows, [])) {
                    toaster.info("No rows match your search");
                }
            });
        }
    }]);

    myApp.controller('reportVisitsCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', 'DomainDecodes', '$rootScope', function ($scope, $http, ServerRoutes, toaster, DomainDecodes, $rootScope) {
        $scope.model = {};
        $scope.visitsTimes = DomainDecodes.visitsTimes;
        $scope.model.Time = 1;
        if ($rootScope.sharedVariables.role === "Doctor") {
            $scope.entity = "Owner";
        } else {
            $scope.entity = "Doctor";
        }
        $scope.print = function () {
            window.print();
        }

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.visits,
                method: "GET",
                params: $scope.model
            }).then(function searchCompleted(response) {
                $scope.rows = response.data;
                if (angular.equals($scope.rows, [])) {
                    toaster.info('No ' + DomainDecodes.visitsTimes[$scope.model.Time-1].name + ' Visits!');
                }
            });
        }
    }]);

myApp.controller('reportFinanceCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.dateParts = DomainDecodes.dateParts;
        var today = new Date();
        $scope.model.EndDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Date';
        $scope.reverse = false;

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.print = function() {
            window.print();
        }

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.finance,
                method: "GET",
                params: $scope.model
            }).then(function searchCompleted(response) {
                $scope.rows = response.data;

                if (angular.equals($scope.rows, [])) {
                    toaster.info("No rows match your search");
                }
            });
        }
}]);

myApp.controller('reportOutcomePerAnimalCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', function ($scope, $http, ServerRoutes, DomainDecodes, toaster) {
        $scope.model = {};
        $scope.dateParts = DomainDecodes.dateParts;
        
        var today = new Date();
        $scope.model.EndDate = moment(today).format("DD/MM/YYYY");

        $scope.propertyName = 'Date';
        $scope.reverse = false;

        $http({
            url: ServerRoutes.animals.mine,
            method: "GET"
        }).then(function searchCompleted(response) {
            $scope.myAnimals = angular.copy(response.data);
        });

        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.print = function() {
            window.print();
        }

        $scope.submitSearch = function () {
            $http({
                url: ServerRoutes.reports.perAnimal,
                method: "GET",
                params: $scope.model
            }).then(function searchCompleted(response) {
                $scope.rows = response.data;

                if (angular.equals($scope.rows, [])) {
                    toaster.info("No rows match your search");
                }
            });
        }
    }]);
})();
