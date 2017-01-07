!(function () {
    var myApp = angular.module('myApp');
    
    myApp.controller('FieldsCtrl', ['$scope', '$http', '$routeParams', '$location', 'DomainDecodes', 'ServerRoutes', 'toaster', function ($scope, $http, $routeParams, $location, DomainDecodes, ServerRoutes, toaster) {
        var init = function () {
            $scope.sizes = DomainDecodes.fieldSize;
            $scope.types = DomainDecodes.fieldType;

            $scope.submitted = false;
            $scope.model = {};
            $scope.originalModel = {};

            if ($routeParams.Id) {
                $scope.isNew = false;

                $http({
                    url: ServerRoutes.fields,
                    method: "GET",
                    params: { id: $routeParams.Id },
                }).then(function searchCompleted(response) {
                    $scope.model = angular.copy(response.data)
                    $scope.originalModel = angular.copy($scope.model);
                });
            }
            else {
                $scope.isNew = true;
            }
        }

        $scope.submitField = function (isValid) {
            $scope.submitted = true;

            if (!isValid)
                return;

            if ($scope.isNew) {
                $http({
                    url: ServerRoutes.fields,
                    method: "POST",
                    data: $scope.model,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $location.path('/editField/' + response.data.Id);
                        toaster.success('המגרש נשמר בהצלחה');
                    }
                });
            }
            else {
                $http({
                    url: ServerRoutes.fields,
                    method: "PUT",
                    params: { id: $scope.model.Id },
                    data: $scope.model,
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $scope.originalModel = angular.copy($scope.model);
                        toaster.success('נתוני המגרש עודכנו בהצלחה');
                    }
                });
            }
        };

        $scope.cancelChanges = function () {
            $scope.model = angular.copy($scope.originalModel);
        };

        $scope.delete = function () {
            $http({
                url: ServerRoutes.fields,
                method: "DELETE",
                params: { id: $scope.model.Id }
            }).then(function searchCompleted(response) {
                if (response.status == 200) {
                    $location.path('/reportFields');
                    toaster.success('המגרש נמחק בהצלחה');
                }
            });
        };

        init();
    }]);
})();
