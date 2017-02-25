!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('animalsCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', '$rootScope', function ($scope, $http, ServerRoutes, DomainDecodes, toaster, $rootScope) {
        $scope.pets = [];

        $scope.getGender = function(id) {
            return DomainDecodes.genders.filter(y => y.id == id)[0].name;
        }

        $scope.getType = function (id) {
            return DomainDecodes.animalTypes.filter(y => y.id == id)[0].name;
        }

        $scope.calcAge = function (dateOfBirth) {
            var dob = moment(dateOfBirth, 'DD/MM/YYYY');
            return Math.round(moment().diff(dob, 'years', true) * 100) / 100;
        }

        var url = ServerRoutes.animals.owner;
        if ($rootScope.sharedVariables.role === "Doctor")
            url = ServerRoutes.animals.doctor;

        $http({
            url: url,
            method: "GET",
            params: { Id: $rootScope.sharedVariables.userId }
        }).then(function searchCompleted(response) {
            $scope.pets = angular.copy(response.data);
        });
    }]);

    myApp.controller('patientCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', '$rootScope', '$routeParams',
        function ($scope, $http, ServerRoutes, DomainDecodes, toaster, $rootScope, $routeParams) {
            $scope.pet = { Id: -1 };
            $scope.animalTypes = DomainDecodes.animalTypes;

            var id = $routeParams.id;
            if (id) {
                $http({
                    url: ServerRoutes.animals.patient,
                    method: "GET",
                    params: { Id: id},
                }).then(function searchCompleted(response) {
                    $scope.pet = angular.copy(response.data);
                });
            }

            $scope.savePet = function () {
                $http({
                    url: ServerRoutes.animals.patient,
                    method: "POST",
                    data: angular.copy($scope.pet)
                }).then(function searchCompleted(response) {
                    if (response.status == 200) {
                        $scope.pet = angular.copy(response.data);
                        toaster.success('Pet Saved!');
                    }
                });
            }
        }
    ]);
    })();
