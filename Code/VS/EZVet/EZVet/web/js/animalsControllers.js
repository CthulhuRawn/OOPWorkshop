﻿!(function () {
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
            return Math.round(moment().diff(dateOfBirth, 'years', true) * 100) / 100;


        }

        $http({
            url: ServerRoutes.animals.owner,
            method: "GET",
            params: { Id: $rootScope.sharedVariables.userId }
        }).then(function searchCompleted(response) {
            $scope.pets = angular.copy(response.data);
        });
    }]);

    myApp.controller('patientCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', 'toaster', '$rootScope', '$location',
        function ($scope, $http, ServerRoutes, DomainDecodes, toaster, $rootScope, $location) {
            $scope.pet = { Id: -1 };
            $scope.animalTypes = DomainDecodes.animalTypes;

            var id = $location.search()["id"];
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
