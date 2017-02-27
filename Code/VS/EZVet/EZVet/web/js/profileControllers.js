!(function() {
    var myApp = angular.module('myApp');
    myApp.controller('profileCtrl',
    [
        '$scope', '$http', 'ServerRoutes', 'toaster',
        function($scope, $http, ServerRoutes, toaster) {
            $scope.submitted = false;
            $scope.model = {};

            $http({
                url: ServerRoutes.profile.get,
                method: "GET"
            }).success(function searchCompleted(response) {
                $scope.model = response;
            });

            $scope.submitUser = function (isValid) {
                $scope.submitted = true;

                if (!isValid)
                    return;

                $http({
                    url: ServerRoutes.profile.update,
                    method: "POST",
                    data: $scope.model
                }).success(function searchCompleted() {
                    toaster.success("Profile updated!");
                });
            };
        }
    ]);
})();