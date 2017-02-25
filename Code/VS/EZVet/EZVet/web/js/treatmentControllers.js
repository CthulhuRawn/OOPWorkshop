!(function() {
    var myApp = angular.module('myApp');
    myApp.controller('treatmentPageCtrl', ['$scope', '$http', 'ServerRoutes', 'toaster', '$routeParams', function ($scope, $http, ServerRoutes, toaster, $routeParams) {
            $scope.treatment = { vaccines: [], treatments: [], medications: [] };
            $scope.newTreatment = {};
            $scope.newMedication = {};
            $scope.newVaccine = {};

            $scope.treatment.PetId = $routeParams.id;
            
            if ($scope.treatment.PetId) {
                $http({
                    url: ServerRoutes.animals.patient,
                    method: "GET",
                    params: { Id: $scope.treatment.PetId }
                }).then(function searchCompleted(response) {
                    $scope.treatment.Animal = response.data;
                });
            } else {
                $scope.treatment.PetId = undefined;
            }

            $scope.addTreatment = function() {
                $scope.treatment.treatments.push({ Name: $scope.newTreatment.Name, Price: $scope.newTreatment.Price });
                $scope.newTreatment.Name = "";
                $scope.newTreatment.Price = "";
            };

            $scope.addVaccine = function () {
                $scope.treatment.vaccines.push({ Name: $scope.newVaccine.Name, Price: $scope.newVaccine.Price });
                $scope.newVaccine.Name = "";
                $scope.newVaccine.Price = "";
            };

            $scope.addMedication= function () {
                $scope.treatment.medications.push({ Name: $scope.newMedication.Name, Price: $scope.newMedication.Price, Dose: $scope.newMedication.Dose });
                $scope.newMedication.Name = "";
                $scope.newMedication.Price = "";
                $scope.newMedication.Dose = "";
            };
            $scope.calcAge = function (dateOfBirth) {
                var dob = moment(dateOfBirth, 'DD/MM/YYYY');
                return Math.round(moment().diff(dob, 'years', true) * 100) / 100;
            }

            $scope.saveTreatment = function() {
                $http({
                    url: ServerRoutes.treatments.save,
                    method: "POST",
                    data: angular.copy($scope.treatment)
                }).then(function searchCompleted(response) {
                    if (response.status === 200) {
                        $scope.treatment = angular.copy(response.data);
                        toaster.success('Treatment Saved!');
                    }
                });
            };
        }
    ]);
})();