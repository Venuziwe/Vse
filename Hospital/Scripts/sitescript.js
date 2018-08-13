var AJAXapp = angular.module('AJAXapp', []);

AJAXapp.controller('AJAXController', function AJAXController($scope, $http) {

    $scope.doctors = [];
    $scope.visits = [];
    $scope.DepartmentID = "0";



    $scope.getDoctors = function () {


        $http({ method: 'GET', url: '/Home/GetDoctors', params: { DepartmentID: $scope.DepartmentID } }).then(
            function success(response) {
                $scope.doctors = response.data.doctors;
            },
            function error(response) {

            });
    };

    $scope.getFIO = function () {


        $http({ method: 'GET', url: '/Home/GetFIO', params: { LastName: $scope.LastName, FirstName: $scope.LastName, MiddleName: $scope.MiddleName  } }).then(
            function success(response) {
                $scope.fio = response.data.fio;
            },
            function error(response) {

            });
    };  
});
