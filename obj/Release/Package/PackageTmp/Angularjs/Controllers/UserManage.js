App.controller("ManageController",
    function ($scope, $http, Loader) {
        Loader.Close();
        $scope.addAdmin = function () {
            $scope.searcNewhHrid_input = "";
            $(".addAdminModal").modal("toggle");
        }
        //$scope.addAdminModal2 = function () {
        //    $scope.searcNewhHrid_input = "";
        //    $(".addAdminModal2").modal("toggle");
        //}


        $scope.save = {
            Hrid: '',
            LastName: '',
            FirstName: '',
            LOB: '',
            Position: '',
            Site: '',
            PositionLevel: '',
            NtAccount: '',

        }
        $scope.details = {
            Id: '',
            Hrid: '',
            LastName: '',
            FirstName: '',
            LOB: '',
            Position: '',
            Site: '',
            PositionLevel: '',
        }
        $scope.clear = function () {
            $scope.save = {
                Hrid: '',
                LastName: '',
                FirstName: '',
                LOB: '',
                Position: '',
                Site: '',
                PositionLevel: '',
                HireDate: '',
                Role: ''
            }
        }

        $scope.saveIsdisable = true;
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
                $scope.UserFilter = result.data;
                if (result.data) {
                    $scope.save.Hrid = $scope.UserFilter.data[0].hrid;
                    $scope.save.FirstName = $scope.UserFilter.data[0].first_name;
                    $scope.save.LastName = $scope.UserFilter.data[0].last_name;
                    $scope.save.LOB = $scope.UserFilter.data[0].department;
                    $scope.save.Position = $scope.UserFilter.data[0].business_title;
                    $scope.save.PositionLevel = $scope.UserFilter.data[0].position_level;
                    $scope.save.Site = $scope.UserFilter.data[0].site;
                    $scope.save.HireDate = $scope.UserFilter.data[0].hire_date;
                    $scope.save.Ntaccount = $scope.UserFilter.data[0].hrid;
                    $scope.save.EmailAddress = $scope.UserFilter.data[0].email_address;

                    $scope.saveIsdisable = false;
                }
                else {
                    $scope.clear();
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



        $scope.Users = function () {
            Loader.Open();
            $http({
                method: "GET",
                url: url + 'UserManage/GetUsers',
            }).then(function successCallback(response) {
                $scope.UsersList = response.data;
                $scope.totalPages = Math.ceil($scope.UsersList.length / $scope.pageSize);
                if ($('#userTable').length) {  // Check if element exists
                    $('#userTable').DataTable({
                        "paging": true,
                        "pageLength": $scope.pageSize
                    });
                }
                Loader.Close();

                $scope.newData = [];
                var ctr = response.data.length;
                for (var i = response.data.length - 1; i >= 0; i--) {

                    $scope.newData.push([
                        ctr,
                        response.data[i].HRID,
                        response.data[i].Last_Name,
                        response.data[i].First_Name,
                        response.data[i].LOB,
                        response.data[i].Position,
                        response.data[i].Site,
                        response.data[i].Position_Level,
                        //response.data[i].Createdby,
                        //response.data[i].UpdatedBy,
                        response.data[i].DateCreated,
                        response.data[i].DateUpdated,
                        response.data[i].HireDate,
                        response.data[i].Roles,
                        response.data[i].IsActive,
                        '<div style="display:flex; gap:10px;">' +
                        '<button onclick="angular.element(this).scope().getUserById(' + response.data[i].Id + ')" class="sendremarks_button btn btn-primary">Update</button>' +
                        //'<button onclick="angular.element(this).scope().delete(' + response.data[i].Id + ')" class="sendremarks_button btn btn-danger">Delete</button>' +
                        '</div>'
                    ]);

                    ctr--;
                }
                if ($.fn.DataTable.isDataTable('#dataTable'))
                    $('#dataTable').DataTable().destroy();

                $('#dataTable').DataTable({
                    dom: 'Bfrtip',
                    data: $scope.newData,
                    columns: [
                        { 'title': '<span class="custom-header">USER ID</span>' },
                        { 'title': '<span class="custom-header">HRID</span>' },
                        { 'title': '<span class="custom-header">Last Name</span>' },
                        { 'title': '<span class="custom-header">First Name</span>' },
                        { 'title': '<span class="custom-header">Dept / Lob / Account</span>' },
                        { 'title': '<span class="custom-header">Position</span>' },
                        { 'title': '<span class="custom-header">Site</span>' },
                        { 'title': '<span class="custom-header">Position Level</span>' },
                        //{ 'title': '<span class="custom-header">Createdby</span>' },
                        //{ 'title': '<span class="custom-header">UpdatedBy</span>' },
                        { 'title': '<span class="custom-header">DateCreated</span>' },
                        { 'title': '<span class=custom-header">DateUpdated</span>' },
                        { 'title': '<span class=custom-header">HireDate</span>' },
                        { 'title': '<span class="custom-header">Roles</span>' },
                        { 'title': '<span class=custom-header">IsActive</span>' },
                        { 'title': '<span class="custom-header">Action</span>' }
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






        $scope.saveUser = function () {
            $scope.save.Hrid = $scope.searcNewhHrid_input;
            saveIsdisable = true;
            if ($scope.searcNewhHrid_input === "" || $scope.searcNewhHrid_input === undefined ||
                $scope.save.FirstName === "" || $scope.save.FirstName === undefined ||
                $scope.save.LastName === "" || $scope.save.LastName === undefined ||
                $scope.save.Roles === "" || $scope.save.Roles === undefined) {

                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Please complete all the fields!',
                    showConfirmButton: false,
                    timer: 4000
                });
            } else {

                Loader.Open();
                saveIsdisable = true;
                $http({
                    method: "POST",
                    url: url + 'UserManage/saveUser',
                    data: JSON.stringify($scope.save)
                }).then(function successCallback(result) {

                    if (result.data == "Exist") {
                        saveIsdisable = false;
                        Loader.Close();
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'Record already exist!',
                            showConfirmButton: false,
                            timer: 4000
                        })
                        $scope.clear();
                    } else {
                        Loader.Close();
                        saveIsdisable = false;
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Successfully saved record!',
                            showConfirmButton: false,
                            timer: 4000
                        })
                        document.getElementById("btnClose").click();
                        $scope.clear();
                        saveIsdisable = false;
                        $scope.Users();
                    }
                    $scope.hideModalLoader();
                    saveIsdisable = false;
                },
                    function errorCallback(result) {
                        console.log("Error2:", "You are now allowed to access this website!");
                        $scope.hideModalLoader();
                    });

            }
        }
        $scope.getUserById = function (id) {
            $scope.searcNewhHrid_input2 = "";
            $(".addAdminModal2").modal("toggle");
            $http({
                method: "POST",
                url: url + 'UserManage/getOneuser',
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
                url: url + 'UserManage/UpdateUsers',
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
        
                $scope.Users();
                document.getElementById("UpdateUser2").click();
            },
                function errorCallback(result) {
                });
        }

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
                    /*  $(".page-loader").removeClass("d-none");*/
                    $http({
                        method: "POST",
                        url: url + 'UserManage/deleteUser',
                        data: {
                            id: id
                        }
                    }).then(function successCallback(response) {
                        $scope.Users();
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
                            $scope.Users();
                        }
                    },
                        function errorCallback(data) {
                        });
                }
            });
        }

        $scope.Users();
    });

