!(function(){
	var app = angular.module("myApp", []);

	// app.config(['$routeProvider', function($routeProvider) {
		// // $routeProvider.
		  // // when('/gilad', {
			// // templateUrl: 'html/amitPartial.html',
			// // controller: 'first'
		  // // }).
		  // // when('/amit', {
			// // templateUrl: 'html/giladPartial.html',
			// // controller: 'first'
		  // // }).
		  // // otherwise({
			// // redirectTo: '/gilad'
		  // // });
	// }]);

	// configure our routes
    app.config(function($routeProvider) {
        $routeProvider

            // route for the home page
            .when('/', {
                templateUrl : 'pages/home.html',
                controller  : 'mainController'
            })

            // route for the about page
            .when('/about', {
                templateUrl : 'pages/about.html',
                controller  : 'aboutController'
            })

            // route for the contact page
            .when('/contact', {
                templateUrl : 'pages/contact.html',
                controller  : 'contactController'
            });
    });	
	
	app.controller('first', ['$scope', function($scope){
		$scope.name = 'amit';
	}]);
})();