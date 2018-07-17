appEIS.factory('employeeUpdateService', function ($http) {

    empUpdatetObj = {};

    empUpdatetObj.getByEid = function (eid) {
        var Emp;

        Emp = $http({ method: 'Get', url: 'http://localhost:61158/api/Employee', params: { id: eid } }).
        then(function (response) {
            return response.data;
        });

        return Emp;
    };

    empUpdatetObj.updateEmployee = function (emp) {
        console.log(emp);
        var Emp;

        Emp = $http({ method: 'Put', url: 'http://localhost:61158/api/Employee', data: emp }).
        then(function (response) {
            return response.data;
        }, function (error) {
            return error.data;
        });

        return Emp;
    };

    return empUpdatetObj;
});

appEIS.controller('employeeUpdateController', function ($scope, $routeParams, employeeUpdateService, utilityService, $rootScope) {

    $('#profilePanel a').click(function (e) {
        e.preventDefault();
    });

    //$scope.eid = $routeParams.EmployeeId;

    $scope.eid = $routeParams.EmployeeId != null ? $routeParams.EmployeeId : $rootScope.EmpSignIn.EmployeeId;


    employeeUpdateService.getByEid($scope.eid).then(function (result) {
        $scope.Emp = result;
        $scope.Emp.DOJ = new Date($scope.Emp.DOJ);
    });

    $scope.UpdateEmployee = function (Emp, IsValid) {

        if (IsValid) {
            employeeUpdateService.updateEmployee(Emp).then(function (result) {
                console.log(result);
                if (result.ModelState == null) {
                    $scope.Msg = " You have successfully updated " + result.EmployeeId;
                    $scope.Flg = true;
                    $scope.serverErrorMsgs = "";
                    utilityService.myAlert();
                }
                else {
                    $scope.serverErrorMsgs = result.ModelState;
                }
            });
        }
    };

    $scope.UploadFile = function () {
        var file = $scope.myFile;
        var uploadUrl = "http://localhost:61158/api/Upload/";
        utilityService.uploadFile(file, uploadUrl, $scope.eid).then(function (result) {
            $scope.image = result;
        });
    };

    utilityService.getFile("http://localhost:61158/api/Upload", $scope.eid).then(function (result) {
            $scope.image = result;
        });

});


