!(function () {
    // Declare app level module which depends on filters, and services
    var ezVetApp = angular.module('myApp', ['ngRoute', 'ui.bootstrap', 'moment-picker', 'angularSpinner', 'ngAnimate', 'toaster']);

    ezVetApp.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {
        $httpProvider.interceptors.push('httpErrorHandler');

        $routeProvider.when('/', {
            templateUrl: 'web/partials/Login.html',
            controller: 'LoginCtrl'
        });

        $routeProvider.when('/login', {
            templateUrl: 'web/partials/Login.html',
            controller: 'LoginCtrl'
        }); 
        $routeProvider.when('/registrationForm', {
            templateUrl: 'web/partials/registrationForm.html',
            controller: 'RegistrationFormCtrl'
        });
        $routeProvider.when('/AnimalsListCustomer', {
            templateUrl: 'web/partials/AnimalsListCustomer.html',
            controller: 'animalsCtrl',
            activetab: 'AnimalsListCustomer'
        });
        $routeProvider.when('/AnimalsListVet', {
            templateUrl: 'web/partials/AnimalsListVet.html',
            controller: 'animalsCtrl',
            activetab: 'AnimalsListVet'
        });
        $routeProvider.when('/PatientPage/:Id?', {
            templateUrl: 'web/partials/PatientPage.html',
            controller: 'patientCtrl',
            activetab: 'PatientPage'
        });
        $routeProvider.when('/VetSearch', {
            templateUrl: 'web/partials/VetSearch.html',
            controller: 'vetCtrl',
            activetab: 'VetSearch'
        });
        $routeProvider.when('/VetPage', {
            templateUrl: 'web/partials/VetPage.html',
            controller: 'vetPageCtrl',
            activetab: 'VetPage'
        });

        $routeProvider.otherwise({
            redirectTo: '/login'
        });
    }]);

    ezVetApp.run(function ($rootScope, $location, $route, LoginService) {
        var nonAuthenticanUrls = ["/", "/login", "/registrationForm"];

        $rootScope.sharedVariables = {
            isLogin: true
        };

        $rootScope.logout = function () {
            LoginService.deleteLogin();
        };

        $rootScope.$on('$routeChangeStart', function (event) {
            var currPath = $location.path();
            
            if (nonAuthenticanUrls.indexOf(currPath) != -1)
                return;

            // user not logged in and tried to navigate to a secured page
            if ($rootScope.sharedVariables.isLogin) {
                event.preventDefault();
                $location.path('/login');
            }
        });

        if (LoginService.hasPreviousLogin()) {
            LoginService.navigateToHomepage($location.path());
        }

        $rootScope.$route = $route;
        $rootScope.spinnerActive = false;

        $rootScope.$on('us-spinner:spin', function (event, key) {
            $rootScope.spinnerActive = true;
        });

        $rootScope.$on('us-spinner:stop', function (event, key) {
            $rootScope.spinnerActive = false;
        });
    });
})();

 
