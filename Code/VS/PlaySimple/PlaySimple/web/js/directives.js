!(function () {
    var myApp = angular.module('myApp');
    myApp.directive('equals', function() {
        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function(scope, elem, attrs, ngModel) {
                if(!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function() {
                    validate();
                });

                // observe the other value and re-validate on change
                attrs.$observe('equals', function (val) {
                    validate();
                });

                var validate = function() {
                    // values
                    var val1 = ngModel.$viewValue;
                    var val2 = attrs.equals;

                    // set validity
                    ngModel.$setValidity('equals', val1 === val2 || (!val1 && !val2));
                };
            }
        }
    });

    myApp.directive('equalsornull', function () {
        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, elem, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function () {
                    validate();
                });

                // observe the other value and re-validate on change
                attrs.$observe('equals', function (val) {
                    validate();
                });

                var validate = function () {
                    // values
                    var val1 = ngModel.$viewValue;
                    var val2 = attrs.equalsornull;

                    // set validity
                    ngModel.$setValidity('equalsornull', val1 == null || val1 === val2);
                };
            }
        }
    });

    myApp.directive('psdate', function () {
        var today = new Date();
        today.setHours(0, 0, 0, 0);

        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, elem, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function () {
                    validate();
                });

                var validate = function () {
                    var val = ngModel.$viewValue;
                    
                    if (!val) {
                        ngModel.$setValidity('psdate', true);
                    }
                    else {
                        var valDate = moment(val, "DD-MM-YYYY");
                        ngModel.$setValidity('psdate', valDate.isValid());
                    }
                };
            }
        }
    });

    myApp.directive('emptyToNull', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, ctrl) {
                ctrl.$parsers.push(function (viewValue) {
                    if (viewValue === "") {
                        return null;
                    }
                    return viewValue;
                });
            }
        };
    });

    myApp.directive('past', function () {
        var today = new Date();
        today.setHours(0, 0, 0, 0);

        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, elem, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function () {
                    validate();
                });

                // observe the other value and re-validate on change
                attrs.$observe('equals', function (val) {
                    validate();
                });

                var validate = function () {
                    var val = ngModel.$viewValue;
                    var valDate = moment(val, "DD-MM-YYYY").toDate()

                    // set validity
                    ngModel.$setValidity('past', valDate <= today);
                };
            }
        }
    });

    myApp.directive('future', function () {
        var today = new Date();
        today.setHours(0, 0, 0, 0);

        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, elem, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // watch own value and re-validate on change
                scope.$watch(attrs.ngModel, function () {
                    validate();
                });

                // observe the other value and re-validate on change
                attrs.$observe('equals', function (val) {
                    validate();
                });

                var validate = function () {
                    var val = ngModel.$viewValue;
                    var valDate = moment(val, "DD-MM-YYYY").toDate()

                    // set validity
                    ngModel.$setValidity('future', valDate >= today);
                };
            }
        }
    });
})();
