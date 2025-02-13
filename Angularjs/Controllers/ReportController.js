App.controller("Reportctrl",
    function ($scope, $http, FileSaver, Loader) {
        Loader.Open();
        $scope.test = () => {
            alert("Hello world");
        }
        $scope.Clear = function () {

            $scope.ReportFilter = {
                Site: [], // or ""
                Sitelocations: null, // or ""
                Report: 'VXI NEW HIRE SURVEY',
                ToDate: '',
                FromDate: '',
                Months: '',
                Year: '',
                FromDate: '',
                Lobs: [],
                Wave: [],

            }
            location.reload();

        }


        //$scope.getStyleForContainer = function () {
        //    return {
        //        height: '10vh',
        //        overflow: 'auto'
        //    };
        //};

        //$(document).ready(function () {

        //    var multipleCancelButton = new Choices('#choices-multiple-remove-button', {
        //        removeItemButton: true

        //    });


        //});

        //$(document).ready(function () {

        //    var multipleCancelButton = new Choices('#choices-multiple-remove-button', {
        //        removeItemButton: true,
        //        maxItemCount: 5,
        //        searchResultLimit: 5,
        //        renderChoiceLimit: 5
        //    });


        //});


        //$timeout(function () {
        //    var multipleCancelButton = new Choices('#choices-multiple-remove-button', {
        //        removeItemButton: true,
        //        searchEnabled: true,
        //        placeholder: true,
        //        placeholderValue: 'Select Site...',
        //    });
        //}, 0);


       

        //$timeout(function () {
        //    var multipleCancelButton = new Choices('#choices-multiple-remove-button', {
        //        removeItemButton: true
        //    });
        //}, 0);

        $scope.activeTab = 'tab1';
        $scope.setActiveTab = function (tab) {
            $scope.activeTab = tab;
            $scope.Clear();
        };
        $scope.handleSiteChange = function () {
            console.log('Selected Sites:', $scope.ReportFilter.Site);
        };
        $scope.isActiveTab = function (tab) {
            return $scope.activeTab === tab;
            $scope.Clear();
        };

        $scope.ReportFilter = {
            Site: [], // or ""
            Sitelocations: null, // or ""
            Report: 'VXI NEW HIRE SURVEY',
            ToDate: '',
            FromDate: '',
            Lobs: [],
            Wave: [],
            Report_type: 'Raw Report',
        }


        $scope.Getsite = function () {
            $http({
                method: "GET",
                url: url + 'Report/SitesDropdown'
            }).then(function successCallback(result) {
                $scope.Sites = result.data;  // Directly bind the data
              

                /*console.log($scope.Sites);*/
            }, function errorCallback(res) {
                console.error('Error fetching sites:', res);
                Loader.Close();
                // Handle the error as needed
            });
        };


        $scope.Getwave = function () {
            $http({
                method: "GET",
                url: url + 'Report/WaveDropdown'
            }).then(function successCallback(result) {
                $scope.Wavelist = result.data;  // Directly bind the data


                /*console.log($scope.Sites);*/
            }, function errorCallback(res) {
                console.error('Error fetching sites:', res);
                Loader.Close();
                // Handle the error as needed
            });
        };



        $scope.showDropdown = false;
        $scope.selectedSites = [];

        $scope.toggleDropdown = function () {
            $scope.showDropdown = !$scope.showDropdown;
        };

        $scope.toggleOption = function (item) {
            item.selected = !item.selected;
            if (item.selected) {
                $scope.selectedSites.push(item);
            } else {
                $scope.selectedSites.splice($scope.selectedSites.indexOf(item), 1);
            }
        };


        $scope.GetHireDate = function () {
            $http({
                method: "GET",
                url: url + 'Report/GetHireDate'
            }).then(function successCallback(result) {
                $scope.HireDate = result.data;
                $scope.GetDATEOFRESPONSE();
            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        };


        $scope.GetDATEOFRESPONSE = function () {
            $http({
                method: "GET",
                url: url + 'Report/GetDATEOFRESPONSE'
            }).then(function successCallback(result) {
                $scope.GetDATEOFRESPONSE = result.data;

            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        };

        $scope.GetYear = function () {
            $http({
                method: "GET",
                url: url + 'Report/GetYear'
            }).then(function successCallback(result) {
                $scope.Yearlist = result.data;

            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        };


        $scope.GetTeam = function () {
            $http({
                method: "GET",
                url: url + 'Report/GetTeam'
            }).then(function successCallback(result) {
                $scope.GetTeam = result.data;

            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        };

        $scope.GetLob = function (locationID) {
            $http({
                method: "GET",
                url: url + 'Report/GetLobsDrowpdown',
                params: { filterR: locationID }  // Use params instead of data
            }).then(function successCallback(result) {
                $scope.Lobslist = result.data;
            }, function errorCallback(res) {
                Loader.Close();
                // Handle the error as needed
            });
        }

        $scope.teamlist = function () {
            $scope.site = $scope.ReportFilter.Site;

            $http({
                method: "GET",
                url: url + 'Report/GetPrograms',
                params: { site: $scope.site }
            }).then(function successCallback(result) {
                $scope.Lobslist = result.data;
                if ($scope.Lobslist === "") {
                    $scope.GetLob();
                }
                else if ($scope.Lobslist === 0) {
                    $scope.Sites = "";
                }
            }, function errorCallback(res) {
                Loader.Close();

            });
        };


        $scope.Wavedisable = function () {
            $scope.waves = $scope.ReportFilter.Lobs;

            $http({
                method: "GET",
                url: url + 'Report/Getwaves',
                params: { wave: $scope.waves }  
            }).then(function successCallback(result) {
                $scope.Wavelist = result.data;
                if ($scope.Wavelist === "") {
                    $scope.Getwave();
                }
                else if ($scope.Wavelist === 0) {
                    $scope.Wavelist = "";
                }
            }, function errorCallback(res) {
                Loader.Close();
 
            });
        };



        $scope.GetReports = function () {


            if ($scope.ReportFilter.Wave == null) {
                $scope.ReportFilter.Wave = "";
            } else {
                $scope.ReportFilter.Wave = $scope.ReportFilter.Wave;
            }

            if ($scope.ReportFilter.Site == null) {
                $scope.ReportFilter.Sitelocations = "";
            }
            else {
                $scope.ReportFilter.Sitelocations = $scope.ReportFilter.Site;
            }

            if ($scope.ReportFilter.Lobs == null) {
                $scope.ReportFilter.Lob = "";
            }
            else {

                $scope.ReportFilter.Lob = $scope.ReportFilter.Lobs;
            }

            Loader.Open();
            $http({
                method: "POST",
                url: url + 'Report/GetReport',
                data: JSON.stringify($scope.ReportFilter),
                headers: {
                    accept: 'application/vnd.ms-excel'
                },
                responseType: 'arraybuffer',
                cache: false
            }).then(function successCallback(response) {
                Loader.Close();
                var zip = null;
                if (response.data) {
                    zip = new Blob([response.data], {
                        type: 'application/vnd.ms-excel'
                    });
                }
                var fileName = response.headers('content-disposition').split(";")[1].trim().split("=")[1] || 'Report.xlsx';

                FileSaver.saveAs(zip, fileName);
            },
                function errorCallback(res) { closeModal(); });
        };


        $scope.GetinfoSurvey = function () {
            Loader.Open();

            if ($scope.ReportFilter.Wave == null) {
                $scope.ReportFilter.Wave = "";
            } else {
                $scope.ReportFilter.Wave = $scope.ReportFilter.Wave;
            }

            if ($scope.ReportFilter.Site == null) {
                $scope.ReportFilter.Sitelocations = "";
            } else {
                $scope.ReportFilter.Sitelocations = $scope.ReportFilter.Site;
            }

            if ($scope.ReportFilter.Lobs == null) {
                $scope.ReportFilter.Lob = "";
            } else {
                $scope.ReportFilter.Lob = $scope.ReportFilter.Lobs;
            }

            $http({
                method: "POST",
                url: url + 'Report/GetAllSurveyInfo',
                data: JSON.stringify($scope.ReportFilter),
            }).then(function successCallback(response) {
                $scope.UsersList = response.data;
                $scope.totalPages = Math.ceil($scope.UsersList.length / $scope.pageSize);
                $scope.programSummary = {};
                $scope.grandTotal = {
                    Enrolled: 0,
                    Completed: 0,
                    Expired: 0,
                    Total: 0
                };
                response.data.forEach(function (user) {
                    if (!$scope.programSummary[user.LOB]) {
                        $scope.programSummary[user.LOB] = {
                            Enrolled: 0,
                            Completed: 0,
                            Expired: 0
                        };
                    }
                    if (user.Status === 'Completed') $scope.programSummary[user.LOB].Completed++;
                    if (user.Status === 'Expired') $scope.programSummary[user.LOB].Expired++;
                    if (user.Status === 'Pending') $scope.programSummary[user.LOB].Enrolled++;
                    $scope.grandTotal.Total++;
                });

                // After all data is processed, calculate the Enrolled and percentages
                Object.keys($scope.programSummary).forEach(function (lob) {
                    var summary = $scope.programSummary[lob];

                    // Calculate Enrolled as the sum of Completed and Expired
                    summary.Enrolled = summary.Completed + summary.Expired;

                    // Add to the grand totals
                    $scope.grandTotal.Enrolled += summary.Enrolled;
                    $scope.grandTotal.Completed += summary.Completed;
                    $scope.grandTotal.Expired += summary.Expired;

                    var total = summary.Completed + summary.Expired;
                    summary.CompletedPercentage = (total > 0) ? ((summary.Completed / total) * 100).toFixed(2) : 0;
                    summary.ExpiredPercentage = (total > 0) ? ((summary.Expired / total) * 100).toFixed(2) : 0;
                });
                $scope.grandTotalAllLOb = $scope.grandTotal.Completed + $scope.grandTotal.Expired;


                $scope.grandTotal.CompletedPercentage = ($scope.grandTotal.Total > 0) ? (($scope.grandTotal.Completed / $scope.grandTotal.Total) * 100).toFixed(2) : 0;
                $scope.grandTotal.ExpiredPercentage = ($scope.grandTotal.Total > 0) ? (($scope.grandTotal.Expired / $scope.grandTotal.Total) * 100).toFixed(2) : 0;


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
                        response.data[i].FirstName,
                        response.data[i].LastName,
                        response.data[i].LOB,
                        response.data[i].Site,
                        response.data[i].TLHRID,
                        //response.data[i].TrainerHRID,
                        //response.data[i].TrainerName,
                        response.data[i].Hiredate,
                        response.data[i].Status,
                        response.data[i].DateCreated,
                    ]);

                    ctr--;
                }
                if ($.fn.DataTable.isDataTable('#dataTable'))
                    $('#dataTable').DataTable().destroy();

                $('#dataTable').DataTable({
                    dom: 'Bfrtip',
                    data: $scope.newData,
                    columns: [
                        { 'title': '<span class="custom-header">ResponseID</span>' },
                        { 'title': '<span class="custom-header">HRID</span>' },
                        { 'title': '<span class="custom-header">First Name</span>' },
                        { 'title': '<span class="custom-header">Last Name</span>' },
                        { 'title': '<span class="custom-header">LOB</span>' },
                        { 'title': '<span class="custom-header">Site</span>' },
                        { 'title': '<span class="custom-header">TL HRID</span>' },
                        //{ 'title': '<span class="custom-header">Trainer HRID</span>' },
                        //{ 'title': '<span class="custom-header">Trainer Name</span>' },
                        { 'title': '<span class="custom-header">HireDate</span>' },
                        { 'title': '<span class="custom-header">Status</span>' },
                        { 'title': '<span class="custom-header">Date Completed</span>' }
                    ],
                    scrollX: true,
                    deferRender: true,
                    scroller: true,
                    "autoWidth": true,
                    "order": [[0, "decs"]],
                    "pageLength": 5,  // This sets the number of items per page to 10
                    buttons: [],
                });

            }, function errorCallback(response) {
                // Handle errors (optional)
                Loader.Close();
            });
        };


        function transformData(data) {
            // Example transformation logic
            return data.map(item => ({
                category: item.QuestionTitle,
                stronglyDisagree: item.StronglyDisagree,
                disagree: item.Disagree,
                neutral: item.Neutral,
                agree: item.Agree,
                stronglyAgree: item.StronglyAgree
            }));
        } 

        //function transformData2(data) {
        //    // Example transformation logic
        //    return data.map(item => ({
        //        category: item.QuestionTitle,
        //        stronglyDisagree: item.StronglyDisagree,
        //        disagree: item.Disagree,
        //        neutral: item.Neutral,
        //        agree: item.Agree,
        //        stronglyAgree: item.StronglyAgree
        //    }));
        //}

        $scope.GetPercentageDashboard = function () {
            try {
                Loader.Open();

                if ($scope.ReportFilter.Site == null) {
                    $scope.ReportFilter.Sitelocations = "";
                } else {
                    $scope.ReportFilter.Sitelocations = $scope.ReportFilter.Site;
                }

                if ($scope.ReportFilter.Lobs == null) {
                    $scope.ReportFilter.Lob = "";
                } else {
                    $scope.ReportFilter.Lob = $scope.ReportFilter.Lobs;
                }

                if ($scope.ReportFilter.Wave == null) {
                    $scope.ReportFilter.Wave = "";
                } else {
                    $scope.ReportFilter.Wave = $scope.ReportFilter.Wave;
                }

                $http({
                    method: "POST",
                    url: url + 'Report/GetPercentageDashboard',
                    data: JSON.stringify($scope.ReportFilter),
                }).then(function successCallback(response) {
                    try {
                        $scope.Summary = response.data.Dashboard1Results;
                        $scope.Summary2 = response.data.Dashboard2Results;
                        $scope.Summary3 = response.data.Dashboard3Results;
                        $scope.Summary4 = response.data.Dashboard4Results;
                        $scope.Summary5 = response.data.Dashboard5Results;
                        $scope.Summary6 = response.data.Dashboard6Results;
                        $scope.Summary7 = response.data.Dashboard7Results;
                        $scope.Comments = response.data.Comments;

                        var chartData = transformData($scope.Summary);
                        var chartData2 = transformData($scope.Summary2);
                        var chartData3 = transformData($scope.Summary3);
                        var chartData4 = transformData($scope.Summary4);
                        var chartData5 = transformData($scope.Summary5);
                        var chartData6 = transformData($scope.Summary6);
                        var chartData7 = transformData($scope.Summary7);

                        // Render the chart
                        renderChart(chartData);
                        renderChart2(chartData2);
                        renderChart3(chartData3);
                        renderChart4(chartData4);
                        renderChart5(chartData5);
                        renderChart6(chartData6);
                        renderChart7(chartData7);
                    } catch (error) {
                        console.error("Error processing response data:", error);
                        Loader.Close();
                    }
                }, function errorCallback(response) {
                    console.error("HTTP request failed:", response);
                }).finally(function () {
                    Loader.Close();
                });
            } catch (error) {
                console.error("Unexpected error in GetPercentageDashboard:", error);
                Loader.Close();
            }
        };

        var chart; // Declare chart variable for first chart
        var chart2; // Declare chart2 variable for second chart

        var chart3; // Declare chart2 variable for second chart
        var chart4; // Declare chart2 variable for second chart

        var chart5; // Declare chart2 variable for second chart
        var chart6; // Declare chart2 variable for second chart
        var chart7; // Declare chart2 variable for second chart

        function renderChart(chartData) {
            if (chart) {
                chart.dispose();
            }

            var chartdiv = document.getElementById("chartdiv");

            if (chartData.length === 0) {
                chartdiv.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv.innerHTML = "";
            }

            var categoryCount = chartData.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart = am4core.create("chartdiv", am4charts.XYChart);
                chart.data = chartData;

                var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 500;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14;
                categoryAxis.renderer.labels.template.horizontalCenter = "left";
                categoryAxis.renderer.labels.template.textAlign = "left";
                /*categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart.legend = new am4charts.Legend();
                chart.exporting.menu = new am4core.ExportMenu();
            });
        }

    

        function renderChart2(chartData2) {
            if (chart2) {
                chart2.dispose();
            }

            var chartdiv2 = document.getElementById("chartdiv2");

            if (chartData2.length === 0) {
                chartdiv2.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv2.innerHTML = "";
            }

            var categoryCount = chartData2.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv2.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart2 = am4core.create("chartdiv2", am4charts.XYChart);
                chart2.data = chartData2;

                var categoryAxis = chart2.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 500;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14;
                /*categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart2.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart2.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart2.legend = new am4charts.Legend();
                chart2.exporting.menu = new am4core.ExportMenu();
            });
        }
        function renderChart3(chartData3) {
            if (chart3) {
                chart3.dispose();
            }

            var chartdiv3 = document.getElementById("chartdiv3");

            if (chartData3.length === 0) {
                chartdiv3.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv3.innerHTML = "";
            }

            var categoryCount = chartData3.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv3.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart3 = am4core.create("chartdiv3", am4charts.XYChart);
                chart3.data = chartData3;

                var categoryAxis = chart3.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 480;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14.5;
/*                categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart3.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart3.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart3.legend = new am4charts.Legend();
                chart3.exporting.menu = new am4core.ExportMenu();
            });
        }
        function renderChart4(chartData4) {
            if (chart4) {
                chart4.dispose();
            }

            var chartdiv4 = document.getElementById("chartdiv4");

            if (chartData4.length === 0) {
                chartdiv4.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv4.innerHTML = "";
            }

            var categoryCount = chartData4.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv4.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart4 = am4core.create("chartdiv4", am4charts.XYChart);
                chart4.data = chartData4;

                var categoryAxis = chart4.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 480;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 13;
        /*        categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart4.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart4.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart4.legend = new am4charts.Legend();
                chart4.exporting.menu = new am4core.ExportMenu();
            });
        }
        function renderChart5(chartData5) {
            if (chart5) {
                chart5.dispose();
            }

            var chartdiv5 = document.getElementById("chartdiv5");

            if (chartData5.length === 0) {
                chartdiv5.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv5.innerHTML = "";
            }

            var categoryCount = chartData5.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv5.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart5 = am4core.create("chartdiv5", am4charts.XYChart);
                chart5.data = chartData5;

                var categoryAxis = chart5.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 500;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14.2;
      /*          categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart5.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart5.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart5.legend = new am4charts.Legend();
                chart5.exporting.menu = new am4core.ExportMenu();
            });
        }
        function renderChart6(chartData6) {
            if (chart6) {
                chart6.dispose();
            }

            var chartdiv6 = document.getElementById("chartdiv6");

            if (chartData6.length === 0) {
                chartdiv6.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv6.innerHTML = "";
            }

            var categoryCount = chartData6.length;
            var calculatedHeight = (categoryCount * 65);
            chartdiv6.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart6 = am4core.create("chartdiv6", am4charts.XYChart);
                chart6.data = chartData6;

                var categoryAxis = chart6.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 480;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14;
               /* categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart6.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart6.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart6.legend = new am4charts.Legend();
                chart6.exporting.menu = new am4core.ExportMenu();
            });
        }
        function renderChart7(chartData7) {
            if (chart7) {
                chart7.dispose();
            }

            var chartdiv7 = document.getElementById("chartdiv7");

            if (chartData7.length === 0) {
                chartdiv7.innerHTML = "<div style='text-align: center; font-size: 16px; color: #666;'>No data found</div>";
                return;
            } else {
                chartdiv7.innerHTML = "";
            }

            var categoryCount = chartData7.length;
            var calculatedHeight = (categoryCount * 60);
            chartdiv7.style.height = (calculatedHeight > 800 ? 900 : calculatedHeight) + "px";

            am4core.ready(function () {
                chart7 = am4core.create("chartdiv7", am4charts.XYChart);
                chart7.data = chartData7;

                var categoryAxis = chart7.yAxes.push(new am4charts.CategoryAxis());
                categoryAxis.dataFields.category = "category";
                categoryAxis.renderer.inversed = true;
                categoryAxis.renderer.grid.template.location = 0;
                categoryAxis.renderer.minGridDistance = 12.5;
                categoryAxis.renderer.labels.template.maxWidth = 480;
                categoryAxis.renderer.labels.template.wrap = true;
                categoryAxis.renderer.labels.template.truncate = false;
                categoryAxis.renderer.labels.template.fontSize = 14;
   /*             categoryAxis.renderer.labels.template.fontWeight = "bold";*/

                var valueAxis = chart7.xAxes.push(new am4charts.ValueAxis());
                valueAxis.renderer.opposite = true;
                valueAxis.min = 0; // starting value of the axis
                valueAxis.maxPrecision = 0; // ensures no decimals are shown
                valueAxis.renderer.minGridDistance = 50; // adjust this to control the grid distance
                valueAxis.strictMinMax = true; // enforce the min and max values
                valueAxis.renderer.labels.template.adapter.add("text", function (text) {
                    return Math.round(text); // ensure labels are rounded to whole numbers
                });

                function createSeries(field, name, color) {
                    var series = chart7.series.push(new am4charts.ColumnSeries());
                    series.dataFields.valueX = field;
                    series.dataFields.categoryY = "category";
                    series.name = name;
                    series.columns.template.tooltipText = "{name}: [bold]{valueX}[/]";
                    series.columns.template.fill = am4core.color(color);
                    series.columns.template.stroke = am4core.color(color);
                    series.stacked = true;
                    series.columns.template.minHeight = 20;
                    series.columns.template.maxHeight = 50;
                }

                createSeries("stronglyDisagree", "Strongly Disagree", "#ff0000");
                createSeries("disagree", "Disagree", "#f4b183");
                createSeries("neutral", "Neutral", "#ffd966");
                createSeries("agree", "Agree", "#92d050");
                createSeries("stronglyAgree", "Strongly Agree", "#00b050");

                chart7.legend = new am4charts.Legend();
                chart7.exporting.menu = new am4core.ExportMenu();
            });
            Loader.Close();
        }
  

        $scope.dateFormatter = function (_datePosted) {
            const re = /-?\d+/;
            const m = re.exec(_datePosted);
            const res = parseInt(m[0], 10);

            const formattedDate = new Date(res);

            const month = (formattedDate.getMonth() + 1).toString().padStart(2, '0');
            const day = formattedDate.getDate().toString().padStart(2, '0');
            const year = formattedDate.getFullYear();

            return month + '/' + day + '/' + year;
        };

        $scope.months = [
            { id: 1, name: "January" },
            { id: 2, name: "February" },
            { id: 3, name: "March" },
            { id: 4, name: "April" },
            { id: 5, name: "May" },
            { id: 6, name: "June" },
            { id: 7, name: "July" },
            { id: 8, name: "August" },
            { id: 9, name: "September" },
            { id: 10, name: "October" },
            { id: 11, name: "November" },
            { id: 12, name: "December" }
        ];


        $scope.dateFormatter = function (_datePosted) {
            // Assuming _datePosted is in the format 'yyyy-mm-dd'
            const parts = _datePosted.split('-');

            if (parts.length === 3) {
                const year = parseInt(parts[0], 10);
                const month = parseInt(parts[1], 10);
                const day = parseInt(parts[2], 10);

                if (!isNaN(year) && !isNaN(month) && !isNaN(day)) {
                    const formattedDate = new Date(year, month - 1, day);

                    const monthStr = (formattedDate.getMonth() + 1).toString().padStart(2, '0');
                    const dayStr = formattedDate.getDate().toString().padStart(2, '0');
                    const yearStr = formattedDate.getFullYear();

                    return monthStr + '/' + dayStr + '/' + yearStr;
                }
            }

            // Return the original date if parsing fails
            return _datePosted;
        };

        $scope.FilterFromDate = function () {
            $scope.FilteredMainCourselist = [];
            var dateTo = new Date($scope.ReportFilter.ToDate);
            var dateFrom = new Date($scope.ReportFilter.FromDate);

            dateFrom.setHours(23);
            dateFrom.setMinutes(59);
            for (var i = 0; i < $scope.MainCourselist.length; i++) {
                var date = $scope.dateFormatter($scope.MainCourselist[i].datecreated);

                if (date >= dateTo && date <= dateFrom) {
                    $scope.FilteredMainCourselist.push($scope.MainCourselist[i]);
                    console.log($scope.MainCourselist[i])
                }
            }

            $scope.category = true;
            $scope.course = false;
        }

        $scope.FilterToDate = function () {
            $scope.toDate = true;
            $scope.category = false;
            $scope.course = false;
        }

        $scope.FilterCategory = function () {
            $scope.FilteredCoursesList = [];
            for (var i = 0; i < $scope.CoursesList.length; i++) {

                if ($scope.ReportFilter.category == $scope.CoursesList[i].Cat_id)
                    $scope.FilteredCoursesList.push($scope.CoursesList[i]);
            }

            $scope.course = true;
        }

         
        //$scope.exportCharts = function () {
        //    exportChartsToExcel();
        //};
        $scope.downloadExcelWithCharts = function () {
            Loader.Open();
            const tableElement = document.getElementById('test'); // Select the table

            html2canvas(tableElement, { scale: 2 }).then((canvas) => {
                const imgData = canvas.toDataURL('image/png'); // Convert canvas to image

                // Create a new workbook
                const workbook = new ExcelJS.Workbook();
                const worksheet = workbook.addWorksheet('Table with Charts');

                // Set row heights and column widths to fit the image
                worksheet.getRow(1).height = 80; // Adjust as needed
                worksheet.getColumn(1).width = 100; // Adjust as needed

                // Add the image to the worksheet
                const imageId = workbook.addImage({
                    base64: imgData,
                    extension: 'png',
                });

                worksheet.addImage(imageId, {
                    tl: { col: 0, row: 0 }, // Top-left corner
                    ext: { width: 900, height: 1400 }, // Image dimensions
                });
                Loader.Close();
                workbook.xlsx.writeBuffer().then((buffer) => {
                    const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
                    const link = document.createElement('a');
                    link.href = URL.createObjectURL(blob);
                    link.download = 'DASHBOARD Charts.xlsx';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                });
            });
        };


        $scope.downloadPdf = function () {
            Loader.Open();
            const tableElement = document.getElementById('test'); // Select the table element

            html2canvas(tableElement, { scale: 2 }).then((canvas) => {
                const imgData = canvas.toDataURL('image/png'); // Convert canvas to image
                const pdf = new jspdf.jsPDF('p', 'mm', 'a4');  // Initialize jsPDF
                const pageWidth = pdf.internal.pageSize.getWidth();
                const pageHeight = pdf.internal.pageSize.getHeight();

                const imgWidth = pageWidth - 20; // Add margins
                const imgHeight = (canvas.height * imgWidth) / canvas.width;

                let positionY = 10; // Start position for content

                if (imgHeight > pageHeight) {
                    // Multi-page logic
                    pdf.addImage(imgData, 'PNG', 10, positionY, imgWidth, imgHeight);
                    let remainingHeight = imgHeight;
                    while (remainingHeight > pageHeight - 20) {
                        pdf.addPage();
                        remainingHeight -= pageHeight - 20;
                        pdf.addImage(imgData, 'PNG', 10, -remainingHeight + 10, imgWidth, imgHeight);
                    }
                } else {
                    // Single page
                    pdf.addImage(imgData, 'PNG', 10, positionY, imgWidth, imgHeight);
                }
                Loader.Close();
                pdf.save('DASHBOARD Charts.pdf'); // Save the PDF
            });
        };

        $scope.exportCharts = function () {
            Loader.Open();
            let charts = [chart, chart2, chart3, chart4, chart5, chart6, chart7]; // Your chart instances
            let workbook = XLSX.utils.book_new();

            // Array of category names
            let categoryNames = [
                "Work Conditions",
                "Work & Responsibility",
                "Training",
                "Supervision",
                "Pay & Benefits",
                "Company Policies & Administration",
                "Career Advancement"
            ];

            let worksheetData = [];

            // Loop through each chart and add data to the worksheet
            charts.forEach((chart, index) => {
                let chartData = chart.data; // Get the chart data
                let categoryTitle = categoryNames[index]; // Use the appropriate category name

                // Add the category title (for clarity)
                worksheetData.push([categoryTitle]);

                // Add the header row
                let headers = ["Questions", "Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"];
                worksheetData.push(headers);

                // Add the data for the category
                chartData.forEach((row) => {
                    worksheetData.push([row.category, row.stronglyDisagree, row.disagree, row.neutral, row.agree, row.stronglyAgree]);
                });

                // Add a blank row after each category's data for spacing
                worksheetData.push([]);  // This will create a blank row between categories
            });

            // Create a worksheet for the chart data
            let worksheet = XLSX.utils.aoa_to_sheet(worksheetData);
            XLSX.utils.book_append_sheet(workbook, worksheet, "Survey Data");
            Loader.Close();
            // Export the workbook to Excel
            XLSX.writeFile(workbook, "SurveyDataCharts.xlsx");
        }


        //$scope.exportCharts = function () {
        //    let charts = [chart, chart2, chart3, chart4, chart5, chart6, chart7]; // Chart instances
        //    let categoryNames = [
        //        "Work Conditions", "Work & Responsibility", "Training", "Supervision", "Pay & Benefits",
        //        "Company Policies & Administration", "Career Advancement"
        //    ];

        //    // Prepare data for export
        //    let worksheetData = [];
        //    charts.forEach((chart, index) => {
        //        let chartData = chart.data;
        //        let categoryTitle = categoryNames[index];

        //        let categoryData = [];
        //        categoryData.push([categoryTitle]); // Category title
        //        categoryData.push(["Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"]);

        //        chartData.forEach((row) => {
        //            categoryData.push([
        //                row.category,
        //                row.stronglyDisagree || 0,
        //                row.disagree || 0,
        //                row.neutral || 0,
        //                row.agree || 0,
        //                row.stronglyAgree || 0
        //            ]);
        //        });

        //        worksheetData.push(categoryData); // Push the formatted data for this category
        //    });

        //    // Log worksheetData to verify the structure before sending it
        //    console.log("Worksheet Data to Export:", JSON.stringify(worksheetData));

        //    // Send the data to the server for Excel generation
        //    $http({
        //        method: "POST",
        //        url: url + 'ExpoertChart/GenerateExcelWithCharts',
        //        data: JSON.stringify({ worksheetData: worksheetData }),
        //        headers: { 'Content-Type': 'application/json' }
        //    }).then(function (response) {
        //        var blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

        //        // Create an anchor tag to simulate the download
        //        var link = document.createElement('a');
        //        link.href = URL.createObjectURL(blob);
        //        link.download = 'SurveyDataCharts.xlsx'; // Set the file name for download
        //        link.click(); // Trigger the download

        //    }, function (error) {
        //        console.error('Error exporting data to Excel:', error);
        //    });
        //};

          //function exportChartsToExcel() {
        //    let charts = [chart, chart2, chart3, chart4, chart5, chart6, chart7]; // Your chart instances
        //    let workbook = XLSX.utils.book_new();

        //    // Array of category names
        //    let categoryNames = [
        //        "Work Conditions",
        //        "Work & Responsibility",
        //        "Training",
        //        "Supervision",
        //        "Pay & Benefits",
        //        "Company Policies & Administration",
        //        "Career Advancement"
        //    ];

        //    let worksheetData = [];

        //    // Loop through each chart and add data to the worksheet
        //    charts.forEach((chart, index) => {
        //        let chartData = chart.data; // Get the chart data
        //        let categoryTitle = categoryNames[index]; // Use the appropriate category name

        //        // Add the category title (for clarity)
        //        worksheetData.push([categoryTitle]);

        //        // Add the header row
        //        let headers = ["Category", "Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"];
        //        worksheetData.push(headers);

        //        // Add the data for the category
        //        chartData.forEach((row) => {
        //            worksheetData.push([row.category, row.stronglyDisagree, row.disagree, row.neutral, row.agree, row.stronglyAgree]);
        //        });

        //        // Add a blank row after each category's data for spacing
        //        worksheetData.push([]);  // This will create a blank row between categories
        //    });

        //    // Create a worksheet for the chart data
        //    let worksheet = XLSX.utils.aoa_to_sheet(worksheetData);
        //    XLSX.utils.book_append_sheet(workbook, worksheet, "Survey Data");

        //    // Export the workbook to Excel
        //    XLSX.writeFile(workbook, "SurveyDataCharts.xlsx");
        //}

        //function exportChartsToExcel() {
        //    let charts = [chart, chart2, chart3, chart4, chart5, chart6, chart7]; // Your chart instances
        //    let workbook = new ExcelJS.Workbook();
        //    let worksheet = workbook.addWorksheet("Survey Data");

        //    // Array of category names
        //    let categoryNames = [
        //        "Work Conditions",
        //        "Work & Responsibility",
        //        "Training",
        //        "Supervision",
        //        "Pay & Benefits",
        //        "Company Policies & Administration",
        //        "Career Advancement"
        //    ];

        //    let worksheetData = [];
        //    let rowIndex = 1; // To track where we are in the worksheet

        //    // Loop through each chart and add data to the worksheet
        //    charts.forEach((chart, index) => {
        //        let chartData = chart.data; // Get the chart data
        //        let categoryTitle = categoryNames[index]; // Use the appropriate category name

        //        // Add the category title (for clarity)
        //        worksheetData.push([categoryTitle]);

        //        // Add the header row
        //        let headers = ["Questions", "Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"];
        //        worksheetData.push(headers);

        //        // Add the data for the category
        //        chartData.forEach((row) => {
        //            worksheetData.push([row.category, row.stronglyDisagree, row.disagree, row.neutral, row.agree, row.stronglyAgree]);
        //        });

        //        // Add a blank row after each category's data for spacing
        //        worksheetData.push([]);  // This will create a blank row between categories

        //        // Write the data to the worksheet
        //        worksheet.addRows(worksheetData);

        //        // Reset the worksheetData array for next category
        //        worksheetData = [];
        //    });

        //    // Add a bar chart to the worksheet
        //    const chart = worksheet.addChart({
        //        chartType: "bar", // Bar chart type
        //        range: "B2:F15", // The data range where the chart will be created
        //        title: "Survey Results",
        //    });

        //    // Adjust the range dynamically depending on how much data you have
        //    // You can calculate this dynamically based on the `rowIndex` value
        //    chart.setPosition(10, 10); // Set the chart position on the worksheet

        //    // Write the Excel file to disk
        //    workbook.xlsx.writeBuffer().then((buffer) => {
        //        saveAs(new Blob([buffer]), "SurveyDataCharts.xlsx");
        //    });
        //}



      




    });