!(function () {
    // Declare app level module which depends on filters, and services
    var playSimpleApp = angular.module('myApp', ['ngRoute', 'ui.bootstrap', 'moment-picker', 'angularSpinner', 'ngAnimate', 'toaster']);

    playSimpleApp.config(['$routeProvider', '$httpProvider', function ($routeProvider, $httpProvider) {
        $httpProvider.interceptors.push('httpErrorHandler');

        $routeProvider.when('/', {
            templateUrl: 'web/partials/loginForm.html',
            controller: 'LoginCtrl'
        });

        $routeProvider.when('/login', {
            templateUrl: 'web/partials/loginForm.html',
            controller: 'LoginCtrl'
        }); 
        $routeProvider.when('/registrationForm', {
            templateUrl: 'web/partials/registrationForm.html',
            controller: 'RegistrationFormCtrl'
        });

        $routeProvider.when('/ownedPendingInvitations', {
            templateUrl: 'web/partials/ownedPendingInvitations.html',
            controller: 'LoginCtrl',
            activetab: ''
        });

        $routeProvider.when('/editField/:Id?', {
            templateUrl: 'web/partials/editField.html',
            controller: 'FieldsCtrl',
            activetab: 'editField'
        });
        $routeProvider.when('/editMyCustomer/:Id?', {
            templateUrl: 'web/partials/editCustomer.html',
            controller: 'CustomersCtrl',
            activetab: 'editMyCustomer'
        });
        $routeProvider.when('/editCustomer/:Id?', {
            templateUrl: 'web/partials/editCustomer.html',
            controller: 'CustomersCtrl',
            activetab: ''
        });
        $routeProvider.when('/editEmployee/:Id?', {
            templateUrl: 'web/partials/editEmployee.html',
            controller: 'EmployeesCtrl',
            activetab: 'editEmployee'
        });
        $routeProvider.when('/editEmployee/:Id?', {
            templateUrl: 'web/partials/editEmployee.html',
            controller: 'EmployeesCtrl',
            activetab: 'editEmployee'
        });
        $routeProvider.when('/searchEmployees', {
            templateUrl: 'web/partials/searchEmployee.html',
            controller: 'SearchEmployeesCtrl',
            activetab: 'searchEmployees'
        });
        $routeProvider.when('/reportCustomer', {
            templateUrl: 'web/partials/reportCustomer.html',
            controller: 'ReportCustomerCtrl',
            activetab: 'reportCustomer'
        });
        $routeProvider.when('/reportComplaint', {
            templateUrl: 'web/partials/reportComplaint.html',
            controller: 'ReportComplaintCtrl',
            activetab: 'reportComplaint'
        }); 
        $routeProvider.when('/reportFields', {
            templateUrl: 'web/partials/reportFields.html',
            controller: 'ReportFieldsCtrl',
            activetab: 'reportFields'
        });
        $routeProvider.when('/searchCustomers', {
            templateUrl: 'web/partials/searchCustomers.html',
            controller: 'SearchCustomersCtrl',
            activetab: 'searchCustomers'
        }); 
        $routeProvider.when('/searchAvailableOrders', {
            templateUrl: 'web/partials/searchAvailableOrders.html',
            controller: 'SearchAvailableOrdersCtrl',
            activetab: 'searchAvailableOrders'
        });
        $routeProvider.when('/ownedOrders', {
            templateUrl: 'web/partials/ownedOrders.html',
            controller: 'ownedOrdersCrtl',
            activetab: 'ownedOrders'
        });
        $routeProvider.when('/editOrder/:Id?', {
            templateUrl: 'web/partials/editOrder.html',
            controller: 'OrdersCtrl',
            activetab: ''
        });
        $routeProvider.when('/editOrder', {
            templateUrl: 'web/partials/editOrder.html',
            controller: 'OrdersCtrl',
            activetab: ''
        }); 
        $routeProvider.when('/searchAvailableOrdersToJoin', {
            templateUrl: 'web/partials/searchAvailableOrdersToJoin.html',
            controller: 'SearchAvailableOrdersToJoinCrtl',
            activetab: 'searchAvailableOrdersToJoin'
        });
        $routeProvider.when('/searchOrders', {
            templateUrl: 'web/partials/searchOrders.html',
            controller: 'SearchOrdersCrtl',
            activetab: 'searchOrders'
        });
        $routeProvider.when('/pendingOrdersToJoin', {
            templateUrl: 'web/partials/pendingOrdersToJoin.html',
            controller: 'PendingOrdersToJoinCrtl',
            activetab: 'pendingOrdersToJoin'
        });
        $routeProvider.otherwise({
            redirectTo: '/login'
        });
    }]);

    playSimpleApp.run(function ($rootScope, $location, $route, LoginService) {
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

 
