﻿!(function() {
    var myApp = angular.module('myApp');
    myApp.controller('vetCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', '$location', function ($scope, $http, ServerRoutes, toaster, $location) {
            $scope.model = {};
            $scope.vets = [];
            $scope.pageTitle = "Vet Search";

            $scope.propertyName = 'Id';
            $scope.reverse = false;

            $scope.petId = $location.search()["pet"];
            
            if ($scope.petId) {
                $http({
                    url: ServerRoutes.animals.patient,
                    method: "GET",
                    params: { Id: $scope.petId },
                }).then(function searchCompleted(response) {
                    $scope.pageTitle = $scope.pageTitle + " for " + response.data.Name;
                });
            } else {
                $scope.petId = -1;
            }

            $scope.sortBy = function (propertyName) {
                // reverse current or false for new property
                $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
                $scope.propertyName = propertyName;
            };

            $scope.setVetForPet = function(vetId) {
                $http({
                    url: ServerRoutes.vets.assign,
                    method: "GET",
                    params: {vetId: vetId, petId: $scope.petId},
                }).then(function setCompleted(response) {
                    if (response.status === 204)
                        toaster.info('Success!');
                });
            };

            $scope.submitSearch = function() {
                $http({
                    url: ServerRoutes.vets.all,
                    method: "GET",
                    params: $scope.model
                }).then(function searchCompleted(response) {
                    $scope.vets = response.data;

                    if (angular.equals($scope.vets, [])) {
                        toaster.info('No doctors found');
                    }
                });
            };

            
        }
    ]);

    myApp.controller('vetPageCtrl', ['$rootScope', '$scope', 'toaster', '$http', 'ServerRoutes', '$location', 'DomainDecodes', function ($rootScope, $scope,
            toaster,
            $http,
            ServerRoutes,
            $location,
            DomainDecodes) {
        
        $scope.animalTypes = DomainDecodes.animalTypes;
        
        $scope.toggleSelection = function (type) {
                var idx = $scope.vet.Types.indexOf(type);

                if (idx > -1) {
                    $scope.vet.Types.splice(idx, 1);
                }
                
                else {
                    $scope.vet.Types.push(type);
                }
            }


            $scope.vet = {};
            $scope.vetId = $location.search()["id"];
        if ($rootScope.sharedVariables.role === "Doctor") {
            $scope.vetId = $rootScope.sharedVariables.userId;
        }
            $http({
                url: ServerRoutes.vets.getVet,
                method: "GET",
                params: { vetId: $scope.vetId }
            }).then(function searchCompleted(response) {
                $scope.vet = response.data;
            });

            $scope.save = function() {
                $http({
                    url: ServerRoutes.vets.saveVet,
                    method: "POST",
                    data: angular.copy($scope.vet)
                }).then(function searchCompleted(response) {
                    if (response.status === 200) {
                        $scope.vet = response.data;
                        toaster.success('Saved!');
                    } else {
                        toaster.error('Error!');
                    }
                });
            };
        }
    ]);
})();