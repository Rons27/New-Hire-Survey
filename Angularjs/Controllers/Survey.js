App.controller("SurveyController",
    function ($scope, $http, Loader) {
        Loader.Close();


        $scope.answeredQuestionsCount = 0;
        $scope.onQuestionAnswered = function () {
            $scope.answeredQuestionsCount = Object.values($scope.SelectAnswers).filter(answer => answer !== "").length;
            // Calculate answered questions based on non-empty answers
        };
        //$scope.isSubmitEnabled = function () {
        //    return $scope.answeredQuestionsCount === $scope.Question.length && $scope.comment && $scope.TLHRID;
        //};

        $scope.Answer_info = {
            TL_HRID: '',
            comment: ''
        };
        $scope.TLHRID = ''; // Initialize TLHRID
        $scope.validateTLHRID = function () {
            if ($scope.TLHRID && isNaN($scope.TLHRID)) {
                $scope.TLHRID = $scope.TLHRID.replace(/\D/g, ''); // Remove non-numeric characters
            }
        };

        $scope.status = false;
        $scope.GetStatus = function () {
            Loader.Open();
            $http({
                method: "GET",
                url: url + 'Takesurvey/GetStatus'
            }).then(function successCallback(result) {
                if (result.data == "Alreadytake") {
                    $scope.status = true; // Show the button
                } else {
                    $scope.status = false; // Hide the button
                }
                Loader.Close();
            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        };

        //$scope.GetSurvey = function () {
        //    Loader.Open();
        //    $http({
        //        method: "GET",
        //        url: url + 'Takesurvey/GetSurvey'
        //    }).then(function successCallback(result) {
        //        $scope.Question = result.data.questionList;
        //        $scope.Answer = result.data.SurveyChoices;


        //        Loader.Close();
        //    }, function errorCallback(res) {

        //    });
        //}
        $scope.GetSurvey = function () {
            Loader.Open();
            $http({
                method: "GET",
                url: url + 'Takesurvey/GetSurvey'
            }).then(function successCallback(result) {
                $scope.Question = result.data.questionList;
                $scope.Answer = result.data.SurveyChoices;

                // Initialize the counter
                $scope.globalCounter = 1;

                // Group questions by category and adjust category names if needed
                $scope.groupedQuestions = $scope.Question.reduce(function (acc, q) {
                    // Check and adjust category name
                    if (q.Category === "Work_Conditions") {
                        q.Category = "Work Conditions";
                    }
                    if (q.Category === "Work_Responsibility") {
                        q.Category = "Work Responsibility";
                    }
                    if (q.Category === "Pay_Benefits") {
                        q.Category = "Pay Benefits";
                    } 
                    if (q.Category === "Company_Policies_Administration") {
                        q.Category = "Company Policies Administration";
                    }
                    if (q.Category === "Career_Advancement") {
                        q.Category = "Career Advancement";
                    }

                    // Add to grouped categories
                    if (!acc[q.Category]) {
                        acc[q.Category] = [];
                    }
                    acc[q.Category].push(q);
                    return acc;
                }, {});

                Loader.Close();
            }, function errorCallback(res) {
                // Handle error
            });
        };

        // Function to get the number for each question
        $scope.getNumber = function (index) {
            return $scope.globalCounter + index;
        };




        $scope.SelectAnswers = {};
        // Function to save answers
        $scope.SaveAnswers = function () {
            if (Number($scope.answeredQuestionsCount) !== Number($scope.Question.length) || !$scope.comment) {
                Swal.fire({
                    icon: "error",
                    title: "Please complete the survey.",
                    showConfirmButton: true,
                });
                return; // Exit the function if validation fails
            }
            else {


                Swal.fire({
                    icon: "question",
                    title: "Are you sure you want to submit your answers?",
                    showCancelButton: true,
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'No',
                }).then(function (result) {
                    if (result.isConfirmed) {
                        // Proceed with the submission
                        Loader.Open();

                        var Respondents = [];
                        for (var questionId in $scope.SelectAnswers) {
                            if ($scope.SelectAnswers.hasOwnProperty(questionId)) {
                                Respondents.push({
                                    questionId: questionId,
                                    answer: $scope.SelectAnswers[questionId]
                                });
                            }
                        }

                        $scope.Answer_info = {
                            TrainorID: $scope.searcNewhHrid_input,
                            comment: $scope.comment,
                            Fullname: $scope.Fullname,
                        };

                        $http({
                            method: "POST",
                            url: url + 'Takesurvey/SaveAnwers',
                            data: {
                                answers: Respondents,
                                Answer_info: $scope.Answer_info
                            }
                        }).then(function successCallback(result) {
                            if (result.data == "Success") {
                                Swal.fire({
                                    icon: "success",
                                    title: "Survey has been completed",
                                    showConfirmButton: false,
                                });
                                $scope.SelectAnswers = {};
                                $scope.comment = "";
                                $scope.TLHRID = "";

                                setTimeout(function () {
                                    window.location.replace(url + "Home/Index");
                                },
                                    2000);

                            }
                            else if (result.data == "NEED TO REGISTER") {
                                Swal.fire({
                                    icon: "error",
                                    title: "Please Kindly Register into Survey Respondent",
                                    showConfirmButton: false,
                                });
                            }
                            else {
                                window.location.replace(url + "login/Index");
                            }
                            $scope.GetStatus();
                            Loader.Close();
                        }, function errorCallback(res) {
                            Loader.Close();
                            // Handle the error if needed
                        });
                    }
                });
            }
        };


        $scope.Update_Survey_Respondent = function () {
            Swal.fire({
                icon: "question",
                title: "Are you sure you want to Update Respondent?",
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
            }).then(function (result) {
                if (result.isConfirmed) {
                    // Proceed with the submission
                    Loader.Open();
                    $http({
                        method: "GET",
                        url: url + 'Broadcast/UpdateSurveyRespondent',

                    }).then(function successCallback(result) {
                        Loader.Close();
                        if (result.data == "Success") {
                            Swal.fire({
                                icon: "success",
                                title: "Successfully Updated",
                                showConfirmButton: false,
                            });

                        }

                        else {
                            Swal.fire({
                                icon: "error",
                                title: "Error",
                                showConfirmButton: false,
                            });
                        }
                        $scope.GetSurveyResponses();
                        Loader.Close();
                    }, function errorCallback(res) {
                        Loader.Close();
                        // Handle the error if needed
                    });
                    $scope.GetSurveyResponses();
                }
            });

        };


        $scope.GetSurveyResponses = function () {
            Loader.Open();
            $http({
                method: "GET",
                url: url + 'Broadcast/GetSurveyResponses',
            }).then(function successCallback(response) {
                $scope.UsersList = response.data;
                Loader.Close();

                $scope.newData = [];
                var ctr = response.data.length;
                for (var i = response.data.length - 1; i >= 0; i--) {
                    let buttonsHtml = '<div style="display:flex; gap:10px;">';

                    if (response.data[i].Status != "Completed") {
                        buttonsHtml += '<button onclick="angular.element(this).scope().SentEmail(' + response.data[i].Id + ')" class="sendremarks_button btn btn-warning">Notify</button>';
                    }

                    buttonsHtml +=
                        '<button onclick="angular.element(this).scope().getUserById(' + response.data[i].Id + ')" class="sendremarks_button btn btn-primary">Update</button>' +
                        //'<button onclick="angular.element(this).scope().delete(' + response.data[i].Id + ')" class="sendremarks_button btn btn-danger">Delete</button>' +
                        '</div>';

                    $scope.newData.push([
                        ctr,
                        response.data[i].Hrid,
                        response.data[i].FirstName,
                        response.data[i].LastName,
                        response.data[i].HireDate,
                        response.data[i].Department,
                        response.data[i].Email,
                        response.data[i].Project,
                        response.data[i].Region,
                        response.data[i].Site,
                        response.data[i].SupervisorID,
                        response.data[i].PositionLevel,
                        response.data[i].Status,
                        response.data[i].DateCreated,
                        buttonsHtml
                    ]);

                    ctr--;
                }

                if ($.fn.DataTable.isDataTable('#dataTable'))
                    $('#dataTable').DataTable().destroy();

                $('#dataTable').DataTable({
                    dom: 'Bfrtip',
                    data: $scope.newData,
                    columns: [
                        { 'title': '<span class="custom-header">ResponseId</span>' },
                        { 'title': '<span class="custom-header">HRID</span>' },
                        { 'title': '<span class="custom-header">First Name</span>' },
                        { 'title': '<span class="custom-header">Last Name</span>' },
                        { 'title': '<span class="custom-header">HireDate' },
                        { 'title': '<span class="custom-header">Department</span>' },
                        { 'title': '<span class="custom-header">Email</span>' },
                        { 'title': '<span class="custom-header">LOB</span>' },
                        { 'title': '<span class="custom-header">Region</span>' },
                        { 'title': '<span class="custom-header">Site</span>' },
                        { 'title': '<span class=custom-header">SupervisorID</span>' },
                        { 'title': '<span class="custom-header">PositionLevel</span>' },
                        { 'title': '<span class="custom-header">Status</span>' },
                        { 'title': '<span class=custom-header">DateCreated</span>' },
                        { 'title': '<span class="custom-header"></span>' }
                        //{ 'title': '<span class="custom-header">Action</span>' }
                    ],
                    scrollX: true,
                    deferRender: true,
                    scroller: true,
                    "autoWidth": true,
                    "order": [[0, "decs"]],
                    "pageLength": 5,  // This sets the number of items per page to 5

                });


            },
                function errorCallback(response) {
                    // Handle errors (optional)
                });
        };


        $scope.delete = function (id) {
            Swal.fire({
                title: "Question:",
                text: "Are you sure you want to delete?",
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                confirmButtonClass: 'custom-green-button', // Add your custom CSS class for the                                                     confirm button
                cancelButtonClass: 'custom-cancel-button'
            }).then((res) => {

                if (res.isConfirmed) {
                    Loader.Open();
                    $http({
                        method: "POST",
                        url: url + 'Broadcast/deleteResponses',
                        data: {
                            id: id
                        }
                    }).then(function successCallback(response) {
                        $scope.GetSurveyResponses();
                        Loader.Close();
                        if (response.data.result == "Fail") {
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Failed Deleted!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                        }
                        else {
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Successfully Deleted!',
                                showConfirmButton: false,
                                timer: 3000
                            })
                            $scope.GetSurveyResponses();
                        }
                    },
                        function errorCallback(data) {
                        });
                }

                Loader.Close();
            });
        }


        $scope.getUserById = function (id) {
            $scope.searcNewhHrid_input2 = "";
            $(".addAdminModal2").modal("toggle");
            $http({
                method: "POST",
                url: url + 'Broadcast/getOneuser',
                data: {
                    Id: id
                }
            }).then(function successCallback(result) {
                $scope.details = result.data;


            },
                function errorCallback(result) {

                });
        }

        $scope.UpdateUser = function () {
            Loader.Open();
            $http({
                method: "POST",
                url: url + 'Broadcast/UpdateUsers',
                data: JSON.stringify($scope.details)
            }).then(function successCallback(result) {
                Loader.Close();
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Successfully saved record!',
                    showConfirmButton: false,
                    timer: 4000
                })

                $scope.GetSurveyResponses();
                document.getElementById("UpdateUser2").click();
            },
                function errorCallback(result) {
                });
        }

        $scope.SentEmail = function (id) {
            Swal.fire({
                icon: "question",
                title: "Are you sure you want to send an Email?", // Corrected grammar
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
            }).then((res) => {
                if (res.isConfirmed) {
                    $scope.currentUrl = window.location.href;
                    $scope.baseUrl = $scope.currentUrl.substring(0, $scope.currentUrl.lastIndexOf('/'));
                    Loader.Open();
                    $http({
                        method: "POST",
                        url: url + 'Broadcast/SentEmail',
                        data: {
                            Id: id,
                            URL: $scope.baseUrl
                        }
                    }).then(function successCallback(result) {
                        if (result.data == "success") {
                            Loader.Close();
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Successfully Sent Email!',
                                showConfirmButton: false,
                                timer: 4000
                            });
                        }
                        else if (result.data == "Arlradytake") {
                            Loader.Close();
                            Swal.fire({
                                position: 'center',
                                icon: 'warning',
                                title: 'This User already finished the Survey!',
                                showConfirmButton: false,
                                timer: 4000
                            });
                        }
                        else if (result.data == "Off") {
                            Loader.Close();
                            Swal.fire({
                                position: 'center',
                                icon: 'warning',
                                title: 'The Emailer is Off!',
                                showConfirmButton: false,
                                timer: 4000
                            });
                        }
                        else {
                            Loader.Close();
                            Swal.fire({
                                position: 'center',
                                icon: 'warning',
                                title: 'The respondent is not allowed to take the survey.',
                                showConfirmButton: false,
                                timer: 4000
                            });
                        }

                        $scope.GetSurveyResponses();
                        document.getElementById("UpdateUser2").click();
                    },
                        function errorCallback(result) {
                            // Handle error
                        });
                }
            });
        };



        $scope.SentAllEmail = function (id) {
            Swal.fire({
                icon: "question",
                title: "Are you sure you want to send an Email?", // Corrected grammar
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
            }).then((res) => {
                if (res.isConfirmed) {
                    $scope.currentUrl = window.location.href;
                    $scope.baseUrl = $scope.currentUrl.substring(0, $scope.currentUrl.lastIndexOf('/'));
                    Loader.Open();
                    $http({
                        method: "POST",
                        url: url + 'Broadcast/SentAllEmail',
                        data: {
                            Id: id,
                            URL: $scope.baseUrl
                        }
                    }).then(function successCallback(result) {
                        Loader.Close();
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Successfully Sent Email!',
                            showConfirmButton: false,
                            timer: 4000
                        });

                        $scope.GetSurveyResponses();
                        document.getElementById("UpdateUser2").click();
                    },
                        function errorCallback(result) {
                            // Handle error
                        });
                }
            });
        };

        $scope.searcNewhHrid_input = "";
        $scope.searchUser = function () {
            Loader.Open();
            if (!/^\d+$/.test($scope.searcNewhHrid_input)) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Please enter a valid numeric value for Hrid!',
                    showConfirmButton: false,
                    timer: 4000
                });
                Loader.Close();
                return;

            }

            $http({
                method: "POST",
                url: url + 'UserManage/searchNewAdminByHrid',
                data: {
                    hrid: $scope.searcNewhHrid_input,
                    Domain: $scope.Domain
                }
            }).then(function successCallback(result) {
                Loader.Close();

                if (result.data.Table.length != 0) {

                    $scope.UserFilter = result.data.Table;
                    $scope.Fullname = $scope.UserFilter[0].FirstName + " " + $scope.UserFilter[0].LastName;

                    $scope.saveIsdisable = false;
                }
                else {
                    Loader.Close();
                    $scope.saveIsdisable = false;
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'No Hrid Found!',
                        showConfirmButton: false,
                        timer: 4000
                    })

                }

                $scope.hideModalLoader();
            },
                function errorCallback(result) {
                    Loader.Close();
                    $scope.clear();
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'No Hrid Found!',
                        showConfirmButton: false,
                        timer: 4000
                    })


                });
        }


        $scope.showNotification = false; // Initially hide notification
        $scope.showTakeSurveyNotification = function () {
            $scope.showNotification = true;
        };
        $scope.redirectToSurvey = function () {

            //$http({
            //    method: "POST",
            //    url: '/Takesurvey/Acknowledge'
            //})
            //    .then(function (response) {
            //        console.log(response.data);
            //    }, function (error) {
            //        console.error('Error:', error);
            //    });

            window.location.replace(url + "TakeSurvey/Takesurvey");

        };

    })