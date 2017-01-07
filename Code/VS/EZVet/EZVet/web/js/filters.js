!(function () {
    var myApp = angular.module('myApp');
      myApp.filter('interpolate', ['version', function (version) {
          return function (text) {
              return String(text).replace(/\%VERSION\%/mg, version);
          }
      }]);
      myApp.filter('ageFilter', function () {
          function calculateAge(birthday) { // birthday is a date
              var birthday = moment(birthday, "DD/MM/YYYY");
              var today = moment(new Date());
              var age = moment.duration(today.diff(birthday)).asYears();
              var age = Math.floor(age);
              return age;
          }

          return function (birthdate) {
              return calculateAge(birthdate);
          };
      });

      myApp.filter('dateOrderFilter', function ($filter) {

          function dateString(dateFromServer) {

              if (!dateFromServer) {
                  return "";
              }
              var twoHoursLater = new Date();
              twoHoursLater.setTime(dateFromServer);
              twoHoursLater.setHours(twoHoursLater.getHours() + 2);
              return $filter('date')(dateFromServer, 'dd/MM/yy HH:mm', 'GMT') + $filter('date')(twoHoursLater, '-HH:mm', 'GMT');
              //return pad(convertedDate.getHours(), 2) + ":00-" + pad((convertedDate.getHours() + 2), 2) + ":00 " + pad(convertedDate.getDay(), 2) + "/" + pad(convertedDate.getMonth(), 2) + "/" + convertedDate.getFullYear();

          }
          
          return function (dateFromServer) {
              return dateString(dateFromServer);
          };
      });
})();

