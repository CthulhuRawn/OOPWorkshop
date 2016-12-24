!(function () {
    myApp = angular.module('myApp');

    myApp.factory('httpErrorHandler', ['usSpinnerService', 'toaster', '$location', function (usSpinnerService, toaster, $location) {
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
                    toaster.warning('אין לך הרשאות למסך זה');
                    $location.path("/login");
                }

                // unexpected error
                else if (response.status == 500 || response.status == 400) {
                    toaster.error("אופס!", "אירעה שגיאה בשרת...", 5000);
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
                    else if (loginData.Role == "Employee") { // 
                        path = '/searchOrders';
                    }
                    else {
                        path = '/ownedOrders';
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

            complaints: {
                base: urlBase + "complaints",
                search: urlBase + "complaints/search"
            },
            reviews:{
                base: urlBase + "reviews",
                search: urlBase + "reviews/search"
            },
            orders: {
                base: urlBase + "orders",
                availables: urlBase + "orders/availables",
                searchmyorders: urlBase + "orders/searchownedorders",
                search: urlBase + "orders/search",
                optionals: urlBase + "orders/optionals",
                updatepraticipant: urlBase + "orders/updatepraticipant",
                availablestojoin: urlBase + "orders/availablestojoin",
                jointoorder: urlBase + "orders/jointoorder"
            },
            reports: {
                fields: urlBase + "reports/fields",
                customers: urlBase + "reports/customers",
                complaints: urlBase + "reports/complaints"
            }
        }
    });

    myApp.factory("DomainDecodes", function(){
        return {
            orderStatus: [
            {
                id: 1,
                name: 'ממתין לאישור'
            },
            {
                id: 2,
                name: 'התקבל'
            },
            {
                id: 3,
                name: 'נדחה'
            },
            {
                id: 4,
                name: 'בוטל'
            }
            ],
            complaintType: [
                {
                    id: 1,
                    name: 'אי תשלום'
                },
                {
                    id: 2,
                    name: 'אי הגעה'
                },
                {
                    id: 3,
                    name: 'חוסר ספורטיביות'
                }
            ],
            fieldSize: [
                {
                    id: 1,
                    name: 'קטן'
                },
                {
                    id: 2,
                    name: 'בינוני'
                },
                {
                    id: 3,
                    name: 'גדול'
                }
            ],
            fieldType: [
                {
                    id: 1,
                    name: 'כדורגל'
                },
                {
                    id: 2,
                    name: 'כדורסל'
                },
                {
                    id: 3,
                    name: 'טניס'
                },
                {
                id: 4,
                name: 'כדורעף'
                }
            ],
            invitationStatus: [
                {
                    id: 1,
                    name: 'נשלח'
                },
                {
                    id: 2,
                    name: 'התקבל'
                },
                {
                    id: 3,
                    name: 'נדחה'
                }
            ],
            regionDecode: [
                {
                    id: 1,
                    name: 'דן'
                },
                {
                    id: 2,
                    name: 'נגב'
                },
                {
                    id: 3,
                    name: 'חיפה'
                },
                {
                    id: 4,
                    name: 'ירושלים'
                }
         ]};
    });
})();

