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
        $routeProvider.when('/RegistrationForm', {
            templateUrl: 'web/partials/RegistrationForm.html',
            controller: 'RegistrationFormCtrl'
        });
        $routeProvider.when('/AnimalsList', {
            templateUrl: 'web/partials/AnimalsList.html',
            controller: 'animalsCtrl',
            activetab: 'AnimalsList'
        });
        $routeProvider.when('/PatientPage/:id?', {
            templateUrl: 'web/partials/PatientPage.html',
            controller: 'patientCtrl',
            activetab: 'PatientPage'
        });
        $routeProvider.when('/VetSearch/:pet?', {
            templateUrl: 'web/partials/VetSearch.html',
            controller: 'vetCtrl',
            activetab: 'VetSearch'
        });
        $routeProvider.when('/VetPage/:id?', {
            templateUrl: 'web/partials/VetPage.html',
            controller: 'vetPageCtrl',
            activetab: 'VetPage'
        });
        $routeProvider.when('/TreatmentPage/:id?', {
            templateUrl: 'web/partials/TreatmentPage.html',
            controller: 'treatmentPageCtrl',
            activetab: 'TreatmentPage'
        });
        $routeProvider.when('/Profile', {
            templateUrl: 'web/partials/Profile.html',
            controller: 'profileCtrl',
            activetab: 'Profile'
        });
        $routeProvider.when('/reportIncomeItem', {
            templateUrl: 'web/partials/ReportPerItem.html',
            controller: 'reportIncomeItemCtrl',
            activetab: 'reportIncomeItem'
        });
        $routeProvider.when('/reportIncomeType', {
            templateUrl: 'web/partials/ReportPerType.html',
            controller: 'reportIncomeTypeCtrl',
            activetab: 'reportIncomeType'
        });
        $routeProvider.when('/reportOutcome', {
            templateUrl: 'web/partials/ReportFinance.html',
            controller: 'reportFinanceCtrl',
            activetab: 'reportOutcome'
        });
        $routeProvider.when('/reportNextVisits', {
            templateUrl: 'web/partials/ReportVisits.html',
            controller: 'reportVisitsCtrl',
            activetab: 'reportNextVisits'
        });
        $routeProvider.when('/reportIncome', {
            templateUrl: 'web/partials/ReportFinance.html',
            controller: 'reportFinanceCtrl',
            activetab: 'reportIncome'
        });
        $routeProvider.when('/reportOutcomePerAnimal', {
            templateUrl: 'web/partials/ReportFinancePerAnimal.html',
            controller: 'reportOutcomePerAnimalCtrl',
            activetab: 'reportOutcomePerAnimal'
        });

        $routeProvider.otherwise({
            redirectTo: '/login'
        });
    }]);

    ezVetApp.run(function ($rootScope, $location, $route, LoginService) {
        var nonAuthenticanUrls = ["/", "/login", "/RegistrationForm"];

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

 
