!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('LoginCtrl', ['$rootScope', '$scope', '$http', '$location', 'LoginService', 'ServerRoutes', function ($rootScope, $scope, $http, $location, LoginService, ServerRoutes) {
        var init = function () {
            $scope.model = {};
            $scope.noUser = false;
            $scope.userFrozen = false;

            if (LoginService.hasPreviousLogin()) {
                LoginService.navigateToHomepage();
            }
        };

        $scope.submitUser = function () {
            $scope.noUser = false;
            $scope.userFrozen = false;

            $http({
                method: 'POST',
                url: ServerRoutes.login.login,
                data: $scope.model
            }).then(function (response) {
                if (response.data.Role == "None") {
                    $scope.noUser = true;
                }
                else if (response.data.IsUserFrozen) {
                    $scope.userFrozen = true;
                }
                else {
                    LoginService.saveLogin(response.data);
                    LoginService.navigateToHomepage();
                }
            });
        };

        init();
    }]);

    myApp.controller('RegistrationFormCtrl', ['$scope', '$http', '$routeParams', '$location', 'LoginService', 'DomainDecodes', 'ServerRoutes', 'toaster',
        function ($scope, $http, $routeParams, $location, LoginService, DomainDecodes, ServerRoutes, toaster) {
            $scope.regionTypes = DomainDecodes.regionDecode;
            $scope.submitted = false;
            $scope.model = {};

            $scope.submitCustomer = function (isValid) {
                $scope.submitted = true;

                if (!isValid)
                    return;

                $http({
                    url: ServerRoutes.login.registration,
                    method: "POST",
                    data: $scope.model,
                }).success(function searchCompleted(response) {
                    if (response.AlreadyExists) {
                        toaster.error("אופס!", "שם משתמש כבר קיים באתר!", 5000);
                    }
                    else {
                        //$location.path("/login");
                       
                        $scope.loginModel = {}
                        $scope.loginModel.username = $scope.model.Username;
                        $scope.loginModel.password = $scope.model.Password;

                        $http({
                            method: 'POST',
                            url: ServerRoutes.login.login,
                            data: $scope.loginModel
                        }).then(function (response) {
                                toaster.success("תודה שהצטרפת!", "כעת אתה לקוח מן המניין", 5000);
                                LoginService.saveLogin(response.data);
                                LoginService.navigateToHomepage();
                        })
                    }
                });
            };
        }]);
    })
();
