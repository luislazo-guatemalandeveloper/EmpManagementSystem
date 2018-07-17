
appEIS.factory('employeeMgmtService', function ($http) {
    empMgmtObj = {};

    empMgmtObj.getAll = function () {
        var Emps;

        Emps = $http({ method: 'Get', url: 'http://localhost:61158/api/Employee' }).
        then(function (response) {
            return response.data;

        });

        return Emps;
    };

    empMgmtObj.createEmployee = function (emp) {
        var Emp;

        Emp = $http({ method: 'Post', url: 'http://localhost:61158/api/Employee', data: emp }).
        then(function (response) {

            return response.data;

        }, function (error) {
            return error.data;
        });

        return Emp;
    };

    empMgmtObj.createMultiEmployee = function (fileName) {
        var Emp;

        Emp = $http({ method: 'Post', url: 'http://localhost:61158/api/Employee/CreateMultiEmployee', params: { fileName: fileName } }).
        then(function (response) {

            return response;

        }, function (error) {
            return error;
        });

        return Emp;
    };

    empMgmtObj.deleteEmployeeById = function (eid) {
        var Emps;

        Emps = $http({ method: 'Delete', url: 'http://localhost:61158/api/Employee', params: { id: eid } }).
        then(function (response) {
            return response.data;

        });

        return Emps;
    };

    empMgmtObj.remindEmployeeById = function (eid, msg) {
        var Emps;

        Emps = $http({ method: 'Get', url: 'http://localhost:61158/api/Employee/Remind', params: { id: eid, msgStr: msg } }).
        then(function (response) {
            return response;

        });

        return Emps;
    };

    return empMgmtObj;
});



appEIS.controller('employeeMgmtController', function ($scope, employeeMgmtService, utilityService, $window) {
    $scope.msg = "Welcome To Employee Mgmt"

    employeeMgmtService.getAll().then(function (result) {
        $scope.Emps = result;
    });


    $scope.Sort = function (col) {
        $scope.key = col;
        $scope.AscOrDesc = !$scope.AscOrDesc;
    };

    $scope.CreateEmployee = function (Emp, IsValid) {
        if (IsValid) {
            //Emp.Password = Math.random().toString(36).substr(2, 5);
           // Emp.Password = utilityService.randomPassword();
            employeeMgmtService.createEmployee(Emp).then(function (result) {
                if (result.ModelState == null) {
                    $scope.Msg = " You have successfully created " + result.EmployeeId;
                    $scope.Flg = true;
                    employeeMgmtService.getAll().then(function (result) {
                        $scope.Emps = result;
                    });
                    utilityService.myAlert();
                }
                else {
                    $scope.serverErrorMsgs = result.ModelState;
                }
            });
        };
    };

    $scope.CreateMultiEmployee = function () {
        var file = $scope.myFile;
        var uploadUrl = 'http://localhost:61158/api/Upload/';
        utilityService.uploadFile(file, uploadUrl, $scope.eid).then(function (fileName) {

            employeeMgmtService.createMultiEmployee(fileName).then(function (result) {
                if (result.status == 200) {
                    $scope.Msg = " You have successfully Created " + result.data + " record(s) ";
                    $scope.Flg = true;
                    utilityService.myAlert();

                    employeeMgmtService.getAll().then(function (result) {
                        $scope.Emps = result;
                    });
                }
                else {
                    $scope.serverErrorMsgs = result.data.ModelState;
                }
            });
        });
    };

    $scope.DeleteEmployeeById = function (Emp) {
        if ($window.confirm("Do you want to delete Employee with Id:" + Emp.EmployeeId + "?")) {
            employeeMgmtService.deleteEmployeeById(Emp.EmployeeId).then(function (result) {
                if (result.ModelState == null) {
                    $scope.Msg = " You have successfully deleted " + result.EmployeeId;
                    $scope.Flg = true;
                    utilityService.myAlert();

                    employeeMgmtService.getAll().then(function (result) {
                        $scope.Emps = result;
                    });
                }
                else {
                    $scope.serverErrorMsgs = result.ModelState;
                }
            });
        }
    };

    $scope.RemindEmployeeById = function (Emp) {
        var msg = $window.prompt("Please enter your message", "Need your info! its urgent");
        employeeMgmtService.remindEmployeeById(Emp.EmployeeId, msg).then(function (result) {
            if (result.status == 200) {
                $scope.Msg = " You have successfully reminded! ";
                $scope.Flg = true;
                utilityService.myAlert();
            }
            else {
                $scope.serverErrorMsgs = result.data.ModelState;
            }
        });

    };

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("Employees.xlsx",{headers:true}) FROM ?', [$scope.Emps]);
    };
});