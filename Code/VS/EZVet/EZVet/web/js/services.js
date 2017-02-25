!(function () {
    myApp = angular.module('myApp');

    myApp.factory('httpErrorHandler', ['usSpinnerService', 'toaster', '$location', function (usSpinnerService, toaster, $location, $q) {
        var errorHandler = {
            request: function (config) {
                if (!config.url.endsWith(".html"))
                    usSpinnerService.spin('spinner');

                return config;
            },
            response: function (response) {
                if (!response.config.url.endsWith(".html"))
                    usSpinnerService.stop('spinner');

                return response;
            },
            responseError: function (response) {
                if (!response.config.url.endsWith(".html"))
                    usSpinnerService.stop('spinner');

                // unauthorized
                if (response.status == 401) {
                    toaster.warning('You are not autorized to view this page');
                    $location.path("/login");
                }

                // unexpected error
                else if (response.status == 500 || response.status == 400) {
                    toaster.error("Oops!", "We seem to have a problem...", 5000);
                    return $q.reject(rejection);
                }

                return response;
            }
        };

        return errorHandler;
    }]);

    myApp.factory("LoginService", ['$rootScope', '$location', '$http', function ($rootScope, $location, $http) {
        return {
            hasPreviousLogin: function hasPreviousLogin() {
                return localStorage.getItem("currLogin") != null;
            },

            saveLogin: function saveLogin(login) {
                localStorage.setItem("currLogin", JSON.stringify(login))
            },

            updatePassword: function updatePassword(authenticationKey) {
                var loginData = JSON.parse(localStorage.getItem("currLogin"));
                loginData.AuthorizationKey = authenticationKey;
                $http.defaults.headers.common['Authorization'] = loginData.AuthorizationKey;
                this.saveLogin(loginData);
            },

            deleteLogin: function deleteLogin() {
                $rootScope.sharedVariables.isLogin = true;
                localStorage.removeItem("currLogin");
                $location.path('/login');
            },

            navigateToHomepage: function navigateToHomepage(path) {

                var loginData = JSON.parse(localStorage.getItem("currLogin"));

                $http.defaults.headers.common['Authorization'] = loginData.AuthorizationKey;
                $rootScope.sharedVariables.role = loginData.Role;
                $rootScope.sharedVariables.userId = loginData.UserId;
                $rootScope.sharedVariables.isLogin = false;

                if (!path) {
                    if (loginData.Role == "Admin") {
                        path = '/searchCustomers';
                    }
                    else if (loginData.Role == "Doctor") { // 
                        path = '/VetPage';
                    }
                    else {
                        path = '/AnimalsListCustomer';
                    }
                }

                $location.path(path);
            }
        };
    }]);

    myApp.factory("ServerRoutes", function () {
        var urlBase = "http://localhost:59233/api/";

        return {
            fields: urlBase + "fields",
            employees: urlBase + "employees",
            customers: urlBase + "customers",
            participants: urlBase + "participants",
            login: {
                registration: urlBase + "login/registration",
                login: urlBase + "login/login"
            },
            animals: {
                owner: urlBase + "animals/myAnimals",
                doctor: urlBase + "animals/myPatients",
                patient: urlBase + "animals/animal"
            },
            vets: {
                all: urlBase + "vets/all",
                getVet: urlBase + "vets/get",
                saveVet: urlBase + "vets/save",
                assign: urlBase + "vets/assign"
            },
            treatments: {
                save: urlBase + "treatments/save"
            }
        }
    });

    myApp.factory("DomainDecodes", function(){
        return {
            genders: [
                {
                    id: 1,
                    name: 'Male'
                },
                {
                    id: 2,
                    name: 'Female'
                }
            ],
            animalTypes:[
                {
                    id: 1,
                    name: 'Cat'
                },
            {
                id: 2,
                name: 'Dog'
            },
            {
                id: 3,
                name: 'Fish'
            },
            {
                id: 4,
                name: 'Bird'
            }
            ]};
    });
})();

