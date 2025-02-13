App.controller("loginController",
    function ($scope, $http, Loader) {
        $scope.loginInfo = {
            Hrid: '',
            Hiredate: ''
        };
        Loader.Close();
        $scope.test = function () {
            alert('Hello');
        }

        $scope.showHridError = false;


        $scope.resetHridValidation = function () {
            document.getElementById("req_hrid").style.display = "none";
        };
        $scope.resetbdayValidation = function () {
            document.getElementById("req_bday").style.display = "none";
        };



        $scope.Login = function () {
            Loader.Open();
            $http({
                method: "POST",
                url: url + 'Login/LoginUser',
                data: { Login: $scope.loginInfo }
            }).then(function successCallback(response) {
                if (response.data == "Success") {
                    window.location.replace(url + "Home/Index");
                    $scope.Close();
                }
                else if (response.data == "Nopermission") {
                    window.location.replace(url + "Errorlogin/Index");
                    $scope.Close();
                }
                else if (response.data == "REPORT") {
                    window.location.replace(url + "Report/Index");
                    $scope.Close();
                }
                else {
                    Swal.fire({
                        icon: "error",
                        title: "Invalid Credentials",
                        showConfirmButton: false,
                    });
                    Loader.Close();

                    //Swal.fire({
                    //    icon: "error",
                    //    title: "Oops...",
                    //    text: "An error occurred. Please contact the support team for assistance.",
                    //}).then(() => {
                    //    window.location.reload();
                    //});
                }

            },
                function errorCallback(result) {
                    Loader.Close();
                    Swal.fire({
                        icon: "error",
                        title: "Invalid Credentials",
                        showConfirmButton: false,
                    });
                });

        }
    })