!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('LoginCtrl', ['$rootScope', '$scope', '$http', '$location', 'LoginService', 'ServerRoutes', function ($rootScope, $scope, $http, $location, LoginService, ServerRoutes) {
        var init = function () {
            $scope.model = {};
            $scope.noUser = false;

            if (LoginService.hasPreviousLogin()) {
                LoginService.navigateToHomepage();
            }
        };

        $scope.submitUser = function () {
            $scope.noUser = false;
            $http({
                method: 'POST',
                url: ServerRoutes.login.login,
                data: $scope.model
            }).then(function (response) {
                if (!response.data.Role || response.data.Role == "None") {
                    $scope.noUser = true;
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
                        toaster.error("Oops!", "User already exists!", 5000);
                    }
                    else {
                        
                       
                        $scope.loginModel = {}
                        $scope.loginModel.username = $scope.model.Username;
                        $scope.loginModel.password = $scope.model.Password;

                        $http({
                                method: 'POST',
                                url: ServerRoutes.login.login,
                                data: $scope.loginModel
                            })
                            .then(function() {
                                toaster.success("Thanks!", "You are now one of us!", 5000);
                                //LoginService.saveLogin(response.data);
                                //LoginService.navigateToHomepage();
                                $location.path("/login");
                            });
                    }
                });
            };
        }]);
    })
();
