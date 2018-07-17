appEIS.factory('recoverPasswordService', function ($http,$rootScope) {
    recoverPasswordObj = {};

    recoverPasswordObj.getByEmp = function (employee) {
        var Emp;

        Emp = $http({
            method: 'GET', url: 'http://localhost:61158/api/Login/RecoverPassword', params: { empStr: JSON.stringify(employee) }
        }).
        then(function (response) {
            return response;
        }, function (error) {
            return error;
        });

        return Emp;
    };

    return recoverPasswordObj;
});

appEIS.controller('recoverPasswordController', function ($scope, recoverPasswordService, utilityService) {
    $scope.RecoverPassword = function (emp, IsValid) {
        console.log(emp);
        if (IsValid) {
            recoverPasswordService.getByEmp(emp).then(function (result) {
                if (result.status == 200) {
                    $scope.Msg = " You login credentials has been emailed. Kindly check you email.";
                    $scope.Flg = true;
                    $scope.serverErrorMsgs = "";
                    utilityService.myAlert();
                }
                else {
                    $scope.serverErrorMsgs = result.data.ModelState;
                }
            });
        }
    }
});