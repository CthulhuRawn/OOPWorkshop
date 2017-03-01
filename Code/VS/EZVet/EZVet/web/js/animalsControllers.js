!(function () {
    var myApp = angular.module('myApp');

    myApp.controller('animalsCtrl', ['$scope', '$http', 'ServerRoutes', 'DomainDecodes', function ($scope, $http, ServerRoutes, DomainDecodes) {
        $scope.propertyName = 'Id';
        $scope.reverse = false;
        
        $scope.sortBy = function (propertyName) {
            // reverse current or false for new property
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.comparator = function (v1, v2) {
           
            switch ($scope.propertyName) {
                case 'DateOfBirth':
                    return $scope.calcAge(v1.value) > $scope.calcAge(v2.value) ? 1 : -1;
                    break;
                case 'Type':
                    return $scope.getType(v1.value) > $scope.getType(v2.value) ? 1 : -1;
                    break;
                case 'Gender':
                    return $scope.getGender(v1.value) > $scope.getGender(v2.value) ? 1 : -1;
                    break;
                default:
                    if (v1.type !== 'string' || v2.type !== 'string') {
                        return (v1.index < v2.index) ? -1 : 1;
                    }
                    return v1.value.localeCompare(v2.value);
                    break;
            }
            
            return v1.value.localeCompare(v2.value);
        };

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

        $http({
            url: ServerRoutes.animals.mine,
            method: "GET"
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
