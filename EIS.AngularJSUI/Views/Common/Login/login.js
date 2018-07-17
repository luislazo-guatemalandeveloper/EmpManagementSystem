appEIS.factory('loginService', function ($http) {
    loginObj = {};

    loginObj.getByEmp = function (employee) {
        var Emp;

        Emp = $http({
            method: 'POST', url: 'http://localhost:61158/api/Login', data: employee
        }).
        then(function (response) {
            console.log(response);
            return response;
        }, function (error) {
            console.log(error);
            return error;
        });

        return Emp;
    };

    return loginObj;
});

appEIS.controller('loginController', function ($scope, loginService, $cookies, $rootScope, $location) {
    $scope.Login = function (emp, IsValid) {
        if (IsValid) {
            loginService.getByEmp(emp).then(function (result) {

                //if (result.status == 500)
                //{
                //    $scope.serverErrorMsgs = [{"0":result.data.ExceptionMessage}];
                //}
                //else
                    if (result.status == 200) {

                    $scope.Emp = result.data;
                    $scope.errorMsgs = "";

                    $cookies.put("Auth", "true");
                    $rootScope.Auth = $cookies.get("Auth");
                                          

                    $cookies.put("EmpSignIn", JSON.stringify($scope.Emp));
                    $rootScope.EmpSignIn = JSON.parse($cookies.get("EmpSignIn"));

                    console.log($rootScope.EmpSignIn);

                    $location.path('/');
                }
                else {
                    $scope.serverErrorMsgs = result.data.ModelState;
                }
            }, function (error) {
                console.log(error);
            });
        }
    }
});