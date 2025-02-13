using Newtonsoft.Json;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;
using VXI_New_Hire_Survey_Tool.ViewModel;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Configuration;
using VXI_Trainer_Survey.Repository;
using System.Net.Http;
using System.Web.Configuration;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using MoreLinq;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            //GetReportsByGroupAndCourse();
            var admin = (UserManage)Session["admin"];
            var UserAccount = (Respondents)Session["Account"] ?? null;
            if (admin != null)
            {
                if (admin == null)
                {
                    return Redirect("~/Login/Index");
                }

                else if (admin.Roles == "ADMIN")
                {
                    return View();
                }
                else if (admin.Roles == "REPORT")
                {
                    return View();
                }
                else
                {

                    return View();
                }
            }
            else if (UserAccount != null && UserAccount.Team == "INNOVATION AND TECHNOLOGY")
            {
                return View();
            }
            else
            {
                return Redirect("~/Login/Index");
            }

        }

        public ActionResult Dashboard()
        {
            //GetReportsByGroupAndCourse();
            var admin = (UserManage)Session["admin"];
            var UserAccount = (Respondents)Session["Account"] ?? null;
            if (admin != null)
            {
                if (admin == null)
                {
                    return Redirect("~/Login/Index");
                }

                else if (admin.Roles == "ADMIN")
                {
                    return View();
                }
                else if (admin.Roles == "REPORT")
                {
                    return View();
                }
                else
                {

                    return View();
                }
            }
            else if (UserAccount != null && UserAccount.Team == "INNOVATION AND TECHNOLOGY")
            {
                return View();
            }
            else
            {
                return Redirect("~/Login/Index");
            }

        }


        public JsonResult GetPercentageDashboard(FilterVM _model)
        {

            //DateTime? DATEOFRESPONSE = null;
            //DateTime? HiredDate = null;
            DateTime? StartDate = null;
            DateTime? EndmDate = null;

            DateTime? HireDateFrom = null;
            DateTime? HireDateTo = null;


            DateTime? DATEOFRESPONSEFROM = null;
            DateTime? DATEOFRESPONSETO = null;

            string DATEOFRESPONSES = null;
            string StartDateString = null;

            string StartHireDateFrom = null;
            string StartHireDateto = null;

            string StartDATEOFRESPONSEFROM = null;
            string StartDATEOFRESPONSETO = null;

            string ENDDateString = null;




            if (_model.Sitelocations != null && _model.Sitelocations.Contains("ALL"))
            {
                _model.Sitelocations = null;
            }
            if (_model.Lob != null && _model.Lob.Contains("ALL"))
            {
                _model.Lob = null;
            }


            if (_model.Wave != null && _model.Wave.Contains("ALL"))
            {
                _model.Wave = null; // Set to null if "ALL"
            }




            if (!string.IsNullOrEmpty(_model.HireDateFrom))
            {
                HireDateFrom = DateTime.Parse(_model.HireDateFrom);
                StartHireDateFrom = HireDateFrom.Value.ToString("MM/dd/yyyy");
            }

            if (!string.IsNullOrEmpty(_model.HireDateTo))
            {
                HireDateTo = DateTime.Parse(_model.HireDateTo);
                StartHireDateto = HireDateTo.Value.ToString("MM/dd/yyyy");
            }



            if (!string.IsNullOrEmpty(_model.DATEOFRESPONSEFROM))
            {
                DATEOFRESPONSEFROM = DateTime.Parse(_model.DATEOFRESPONSEFROM);
                StartDATEOFRESPONSEFROM = DATEOFRESPONSEFROM.Value.ToString("MM/dd/yyyy");
            }

            if (!string.IsNullOrEmpty(_model.DATEOFRESPONSETO))
            {
                DATEOFRESPONSETO = DateTime.Parse(_model.DATEOFRESPONSETO);
                StartDATEOFRESPONSETO = DATEOFRESPONSETO.Value.ToString("MM/dd/yyyy");
            }




            //if (!string.IsNullOrEmpty(_model.DATEOFRESPONSE))
            //{

            //    DATEOFRESPONSE = DateTime.Parse(_model.DATEOFRESPONSE);
            //    DATEOFRESPONSES = DATEOFRESPONSE.Value.ToString("MM/dd/yyyy");
            //}


            if (!string.IsNullOrEmpty(_model.FromDate))
            {
                StartDate = DateTime.Parse(_model.FromDate);
                StartDateString = StartDate.Value.ToString("MM/dd/yyyy");
            }



            if (!string.IsNullOrEmpty(_model.ToDate))
            {
                EndmDate = DateTime.Parse(_model.ToDate);
                ENDDateString = EndmDate.Value.ToString("MM/dd/yyyy");
            }

            try
            {
                var combinedResults = new CombinedResults
                {
                    Dashboard1Results = FetchSurveyResults("GetPercentageDashboard", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard2Results = FetchSurveyResults("GetPercentageDashboard2", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard3Results = FetchSurveyResults("GetPercentageDashboard3", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard4Results = FetchSurveyResults("GetPercentageDashboard4", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard5Results = FetchSurveyResults("GetPercentageDashboard5", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard6Results = FetchSurveyResults("GetPercentageDashboard6", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Dashboard7Results = FetchSurveyResults("GetPercentageDashboard7", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, DATEOFRESPONSES, _model.Sitelocations, _model.Lob, _model.Wave),
                    Comments = FetchComments("GetComment", ENDDateString, StartDateString, StartDATEOFRESPONSEFROM, StartDATEOFRESPONSETO, StartHireDateFrom, StartHireDateto, _model.Sitelocations, _model.Lob, _model.Wave)

                };

                return Json(combinedResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception (using a logging framework like NLog, Serilog, etc.)
                Console.WriteLine($"Error occurred: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while fetching the data." }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<SurveyResultVM> FetchSurveyResults(
         string storedProcedureName,
         string Todate,
         string FromDate,
         string StartDATEOFRESPONSEFROM,
         string StartDATEOFRESPONSETO,
         string StartHireDateFrom,
         string StartHireDateto,
         string DATEOFRESPONSES,
         string[] Site,
         string[] LOB,
         string[] wave  )
        {
            var results = new List<SurveyResultVM>();

            // Join the Site array into a comma-separated string
            string siteList = Site != null && Site.Length > 0 ? string.Join(",", Site) : string.Empty;
            string LobList = LOB != null && LOB.Length > 0 ? string.Join(",", LOB) : string.Empty;
            string Wavelist = wave != null && wave.Length > 0 ? string.Join(",", wave) : string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["ReportDbs"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(FromDate) ? (object)DBNull.Value : FromDate);
                    command.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(Todate) ? (object)DBNull.Value : Todate);
                    command.Parameters.AddWithValue("@HireDateFrom", string.IsNullOrEmpty(StartHireDateFrom) ? (object)DBNull.Value : StartHireDateFrom);
                    command.Parameters.AddWithValue("@HireDateTo", string.IsNullOrEmpty(StartHireDateto) ? (object)DBNull.Value : StartHireDateto);
                    command.Parameters.AddWithValue("@DateRespondentFrom", string.IsNullOrEmpty(StartDATEOFRESPONSEFROM) ? (object)DBNull.Value : StartDATEOFRESPONSEFROM);
                    command.Parameters.AddWithValue("@DateRespondentTo", string.IsNullOrEmpty(StartDATEOFRESPONSETO) ? (object)DBNull.Value : StartDATEOFRESPONSETO);

                    // Pass the Site array as a comma-separated string
                    command.Parameters.AddWithValue("@Site", string.IsNullOrEmpty(siteList) ? (object)DBNull.Value : siteList);
                    command.Parameters.AddWithValue("@Lob", string.IsNullOrEmpty(LobList) ? (object)DBNull.Value : LobList);
                    command.Parameters.AddWithValue("@Wave", string.IsNullOrEmpty(Wavelist) ? (object)DBNull.Value : Wavelist);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new SurveyResultVM
                            {
                                QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId")),
                                QuestionTitle = reader.GetString(reader.GetOrdinal("QuestionTitle")).Trim(),
                                StronglyDisagree = reader.IsDBNull(reader.GetOrdinal("StronglyDisagree")) ? 0 : reader.GetInt32(reader.GetOrdinal("StronglyDisagree")),
                                Disagree = reader.IsDBNull(reader.GetOrdinal("Disagree")) ? 0 : reader.GetInt32(reader.GetOrdinal("Disagree")),
                                Neutral = reader.IsDBNull(reader.GetOrdinal("Neutral")) ? 0 : reader.GetInt32(reader.GetOrdinal("Neutral")),
                                Agree = reader.IsDBNull(reader.GetOrdinal("Agree")) ? 0 : reader.GetInt32(reader.GetOrdinal("Agree")),
                                StronglyAgree = reader.IsDBNull(reader.GetOrdinal("StronglyAgree")) ? 0 : reader.GetInt32(reader.GetOrdinal("StronglyAgree"))
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }


        private List<CommentVM> FetchComments(
            string storedProcedureName,
            string toDate,
            string fromDate,
            string StartDATEOFRESPONSEFROM,
            string StartDATEOFRESPONSETO,
            string StartHireDateFrom,
            string StartHireDateto,
            string[] site,
            string[] lob,
            string[] wave)
        {
            var results = new List<CommentVM>();

            // Join the site array into a comma-separated string
            string siteList = site != null && site.Length > 0 ? string.Join(",", site) : string.Empty;
            string LobList = lob != null && lob.Length > 0 ? string.Join(",", lob) : string.Empty;
            string Wavelist = wave != null && wave.Length > 0 ? string.Join(",", wave) : string.Empty;


            string connectionString = ConfigurationManager.ConnectionStrings["ReportDbs"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@FromDate", string.IsNullOrEmpty(fromDate) ? (object)DBNull.Value : fromDate);
                    command.Parameters.AddWithValue("@ToDate", string.IsNullOrEmpty(toDate) ? (object)DBNull.Value : toDate);
                    command.Parameters.AddWithValue("@HireDateFrom", string.IsNullOrEmpty(StartHireDateFrom) ? (object)DBNull.Value : StartHireDateFrom);
                    command.Parameters.AddWithValue("@HireDateTo", string.IsNullOrEmpty(StartHireDateto) ? (object)DBNull.Value : StartHireDateto);
                    command.Parameters.AddWithValue("@DateRespondentFrom", string.IsNullOrEmpty(StartDATEOFRESPONSEFROM) ? (object)DBNull.Value : StartDATEOFRESPONSEFROM);
                    command.Parameters.AddWithValue("@DateRespondentTo", string.IsNullOrEmpty(StartDATEOFRESPONSETO) ? (object)DBNull.Value : StartDATEOFRESPONSETO);

                    // Pass the sites as a comma-separated list
                    command.Parameters.AddWithValue("@Site", string.IsNullOrEmpty(siteList) ? (object)DBNull.Value : siteList);
                    command.Parameters.AddWithValue("@Lob", string.IsNullOrEmpty(LobList) ? (object)DBNull.Value : LobList);
                    command.Parameters.AddWithValue("@Wave", string.IsNullOrEmpty(Wavelist) ? (object)DBNull.Value : Wavelist);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new CommentVM
                            {
                                Comment = reader.GetString(reader.GetOrdinal("Comment")).Trim()
                            };

                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }


        public class CombinedResults
        {
            public List<SurveyResultVM> Dashboard1Results { get; set; }
            public List<SurveyResultVM> Dashboard2Results { get; set; }
            public List<SurveyResultVM> Dashboard3Results { get; set; }
            public List<SurveyResultVM> Dashboard4Results { get; set; }
            public List<SurveyResultVM> Dashboard5Results { get; set; }
            public List<SurveyResultVM> Dashboard6Results { get; set; }
            public List<SurveyResultVM> Dashboard7Results { get; set; }
            public List<CommentVM> Comments { get; set; }
        }



        [HttpPost]
        public JsonResult GetAllSurveyInfo(FilterVM _model)
        {
            List<SurveyInfoVM> surveyInfoList = new List<SurveyInfoVM>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ReportDbs"].ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    DateTime? StartDate = null;
                    DateTime? EndmDate = null;
                    string StartDateString = null;
                    string ENDDateString = null;

                    // Handle 'ALL' values and convert them to null
                    if (_model.Sitelocations != null && _model.Sitelocations.Contains("ALL"))
                    {
                        _model.Sitelocations = null; // Set to null if "ALL"
                    }

                    if (_model.Lob != null && _model.Lob.Contains("ALL"))
                    {
                        _model.Lob = null; // Set to null if "ALL"
                    }

                    if (_model.Wave != null && _model.Wave.Contains("ALL"))
                    {
                        _model.Wave = null; // Set to null if "ALL"
                    }

                    if (_model.Months == "ALL")
                    {
                        _model.Months = null; // Set to null if "ALL"
                    }

                    if (_model.Year == "ALL")
                    {
                        _model.Year = null; // Set to null if "ALL"
                    }

                    // Parse the date values
                    if (!string.IsNullOrEmpty(_model.FromDate))
                    {
                        StartDate = DateTime.Parse(_model.FromDate);
                        StartDateString = StartDate.Value.ToString("MM/dd/yyyy");
                    }

                    if (!string.IsNullOrEmpty(_model.ToDate))
                    {
                        EndmDate = DateTime.Parse(_model.ToDate);
                        ENDDateString = EndmDate.Value.ToString("MM/dd/yyyy");
                    }

                    // Convert SiteLocations array to comma-separated string
                    string siteLocationsList = _model.Sitelocations != null && _model.Sitelocations.Length > 0
                        ? string.Join(",", _model.Sitelocations)
                        : null;

                    string Loblist = _model.Lob != null && _model.Lob.Length > 0
                        ? string.Join(",", _model.Lob)
                        : null;



                    string Wavelist = _model.Wave != null && _model.Wave.Length > 0
                        ? string.Join(",", _model.Wave)
                        : null;

                    using (SqlCommand command = new SqlCommand("GetAllSurveyInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters, handle null values
                        command.Parameters.AddWithValue("@FromDate", (object)(StartDate.HasValue ? StartDate.Value : (object)DBNull.Value));
                        command.Parameters.AddWithValue("@ToDate", (object)(EndmDate.HasValue ? EndmDate.Value : (object)DBNull.Value));
                        //command.Parameters.AddWithValue("@FromDate", (object)(StartDate.HasValue ? StartDate.Value : DBNull.Value));
                        //command.Parameters.AddWithValue("@ToDate", (object)(EndmDate.HasValue ? EndmDate.Value : DBNull.Value));
                        command.Parameters.AddWithValue("@SiteLocations", string.IsNullOrEmpty(siteLocationsList) ? DBNull.Value : (object)siteLocationsList);
                        command.Parameters.AddWithValue("@Month", string.IsNullOrEmpty(_model.Months) ? DBNull.Value : (object)_model.Months);
                        command.Parameters.AddWithValue("@Year", string.IsNullOrEmpty(_model.Year) ? DBNull.Value : (object)_model.Year);
                        command.Parameters.AddWithValue("@Lob", string.IsNullOrEmpty(Loblist) ? DBNull.Value : (object)Loblist);
                        command.Parameters.AddWithValue("@Wave", string.IsNullOrEmpty(Wavelist) ? DBNull.Value : (object)Wavelist);
                        command.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(_model.Status) ? DBNull.Value : (object)_model.Status);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate your result type from the reader
                                SurveyInfoVM surveyInfo = new SurveyInfoVM
                                {
                                    SurveyResponseId = reader["SurveyResponseId"].ToString(),
                                    HRID = reader["HRID"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    LOB = reader["LOB"].ToString(),
                                    Site = reader["Site"].ToString(),
                                    TLHRID = reader["TLHRID"].ToString(),
                                    DateCreated = reader["DateCreated"].ToString(),
                                    Hiredate = reader["Hiredate"].ToString(),
                                    Status = reader["Status"].ToString()
                                };

                                surveyInfoList.Add(surveyInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine("Error: " + ex.Message);
            }

            return Json(surveyInfoList, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetSite()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var site = (from s in Obj.Locations
                            where s.IsActive == true
                            select new { s.LocationId, s.Site, s.Location }).ToList();

                return Json(site, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetHireDate()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var GetHireDate = (from s in Obj.SurveyAnswer_info
                                   orderby s.HireDate descending
                                   select s.HireDate).Distinct().ToList();

                return Json(GetHireDate, JsonRequestBehavior.AllowGet);





            }
        }

        public JsonResult GetDATEOFRESPONSE()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var GetDATEOFRESPONSE = Obj.SurveyAnswer_info
                    .ToList() // Execute the query first to work with in-memory data
                    .GroupBy(s => s.DateCreated)
                    .Select(g => g.OrderByDescending(s => s.DateCreated).FirstOrDefault())
                    .Select(s => new
                    {
                        DateCreated = DateTime.TryParse(s.DateCreated, out DateTime dateCreated)
                                                ? dateCreated.ToString("MM/dd/yyyy")
                                                : s.DateCreated // Retain original if parsing fails


                    })
                    .OrderByDescending(s => s.DateCreated).Distinct().ToList();

                return Json(GetDATEOFRESPONSE, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetYear()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var GetDATEOFRESPONSE = Obj.SurveyAnswer_info
                    .ToList() // Execute the query first to work with in-memory data
                    .GroupBy(s => s.DateCreated)
                    .Select(g => g.OrderByDescending(s => s.DateCreated).FirstOrDefault())
                    .Select(s => new
                    {
                        Year = DateTime.TryParse(s.DateCreated, out DateTime dateCreated)
                                                ? dateCreated.ToString("yyyy")
                                                : s.DateCreated // Retain original if parsing fails
                    })
                    .OrderByDescending(s => s.Year)
                    .Distinct()
                    .ToList();

                return Json(GetDATEOFRESPONSE, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult GetLob(int filterR)
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var Lob = (from s in Obj.Lobs
                           where (s.LocationId == 1 || filterR == 1 || s.LocationId == filterR) && s.IsActive == true
                           select new { s.LocationId, s.Name })
                          .ToList();


                return Json(Lob, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetLobsDrowpdown()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var GetLobs = Obj.Respondents
                      .ToList() // Execute the query first to work with in-memory data
                      .GroupBy(s => s.Project)
                      .Select(g => g.OrderByDescending(s => s.Project).FirstOrDefault())
                      .Select(s => new
                      {
                          Department = s.Project,
                          Team = s.Team
                      })
                      .OrderByDescending(s => s.Department).Distinct().ToList();



                return Json(GetLobs, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetPrograms(string[] site)
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                // Return null if wave array is empty or null
                if (site == null || site.Length == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                // Fetch distinct team names that contain "Wave"
                var teams = (from s in Obj.Respondents
                             where site.Contains(s.Site) // Filter where site matches
                             select s.Project)           // Select the Project field
                              .Distinct()                  // Ensure distinct Project values
                              .OrderByDescending(s => s)   // Order by Project in descending order
                              .Select(s => new {           // Create the final result with Department property
                                Department = s
                              })
                              .ToList();

                // Execute query and get the results

                // Return null if no matching results are found
                if (teams == null || teams.Count == 0)
                {

                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                // Return the team names directly as an array of strings
                return Json(teams, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult Getwaves(string[] wave)
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                // Return null if wave array is empty or null
                if (wave == null || wave.Length == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

                // Fetch distinct team names that contain "Wave"
                var waves = (from s in Obj.Respondents
                             where wave.Contains(s.Project) // Check if s.Project exists in the wave array
                             select s.Team)                  // Select only the Team field
                            .Where(team => team.Contains("Wave")) // Filter by "Wave"
                            .Distinct()                       // Ensure distinct values
                            .ToList();                        // Execute query and get the results

                // Return null if no matching results are found
                if (waves == null || waves.Count == 0)
                {

                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                // Return the team names directly as an array of strings
                return Json(waves, JsonRequestBehavior.AllowGet);
            }
        }





        [HttpGet]
        public JsonResult SitesDropdown()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var subsites = Obj.SurveyAnswer_info
                 .GroupBy(s => s.Site)
                 .Select(g => g.OrderByDescending(s => s.Site).FirstOrDefault().Site)
                 .OrderByDescending(site => site)
                 .ToList();

                var siteLocations = Obj.Locations
                .Where(loc => subsites.Contains(loc.Location))
                .GroupBy(loc => loc.Location)
                .Select(g => new
                {
                    Location = g.Key,
                    Site = g.OrderByDescending(loc => loc.Site).FirstOrDefault().Site
                })
               .OrderByDescending(x => x.Site)
               .Distinct()
               .ToList();
                return Json(siteLocations, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpGet]
        public JsonResult WaveDropdown()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                // Filter, select distinct wave names, and order them
                var waveList = Obj.SurveyAnswer_info
                    .Where(s => s.Team.Contains("Wave")) // Filter by "Wave"
                    .Select(s => s.Team.Trim()) // Ensure no leading/trailing spaces
                    .Distinct() // Remove duplicates
                    //.OrderByDescending(team => team) // Order by descending
                    .ToList();

                return Json(waveList, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult LOB()
        {
            DepartmentsViewModel LOB = new DepartmentsViewModel();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    MediaTypeWithQualityHeaderValue contentType =
                        new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    HttpResponseMessage response = client
                        .GetAsync($"{WebConfigurationManager.AppSettings["GlobalServiceList"]}GlobalHR/Departments")
                        .Result;
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    stringData = Regex.Unescape(stringData);
                    stringData = Regex.Unescape(stringData);
                    stringData = @stringData.TrimStart('"');
                    stringData = @stringData.TrimEnd('"');
                    if (stringData != "\"Source sequence doesn't contain any elements.\"")
                    {
                        stringData = stringData.StartsWith("[") && stringData.EndsWith("]")
                            ? $"{{\"data\":{stringData}}}"
                            : stringData;
                    }
                    LOB = JsonConvert.DeserializeObject<DepartmentsViewModel>(stringData);
                    return Json(stringData, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [DeleteFile]
        public async Task<FileResult> GetReport(FilterVM model)
        {

            //string toDate = Convert.ToDateTime(model.FromDate).ToString("yyyy-MM-dd");
            //string fromDate = Convert.ToDateTime(model.ToDate).ToString("yyyy-MM-dd");

            var today = DateTime.Now.AddDays(1);
            var time = today.TimeOfDay;
            //command.Parameters.AddWithValue("@FromDate", (object)(StartDate.HasValue ? StartDate.Value : (object)DBNull.Value));
            if (!string.IsNullOrEmpty(model.FromDate))
            {
                model.FromDate = DateTime.Parse(model.FromDate).ToShortDateString();
            }
            else
            {
                model.FromDate = null;
            }

            if (!string.IsNullOrEmpty(model.ToDate))
            {
                model.ToDate = DateTime.Parse(model.ToDate).ToShortDateString();
            }
            else
            {
                model.ToDate = null;
            }
            //model.FromDate = DateTime.Parse(model.FromDate).ToShortDateString();
            //model.ToDate = DateTime.Parse(model.ToDate).ToShortDateString();
            var tmpFile = $"{model.Report.ToUpper()}_Report-{today:MM.dd.yyyy}.{time.Hours}.{time.Minutes}.{time.Seconds}.{time.Milliseconds}.xlsx";
            return await CreateResult(tmpFile, new SqlParameter($"{model.Report.ToUpper()} Report", GlobalDefaultValues.SqlQuery.GetRawDataReport(model)));
        }

        private async Task<FileResult> CreateResult(string fileName, params SqlParameter[] parameters)
        {
            var FileName = await ExcelHelper.ToExcelAsync(ConfigurationManager.ConnectionStrings["ReportDbs"].ToString(), parameters);
            var ContentType = MimeMapping.GetMimeMapping(FileName);
            Response.Clear();
            Response.ContentType = ContentType;
            Response.AddHeader("content-disposition", $"attachment;filename= {fileName}");
            Response.BufferOutput = false;
            return File(FileName, ContentType);
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            /// <inheritdoc />
            /// <summary>Called by the ASP.NET MVC framework after the action result executes.</summary>
            /// <param name="filterContext">The filter context.</param>
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                try
                {
                    filterContext.HttpContext.Response.Flush();

                    if (filterContext.Result is FilePathResult filePathResult)
                        System.IO.File.Delete(filePathResult.FileName);
                }
                catch
                {
                    //IGNORE
                }
            }
        }


        public static class GlobalDefaultValues
        {
            public static class SqlQuery
            {
                public static string GetRawDataReport(FilterVM _model)
                {
                    if (_model.Sitelocations == null || _model.Sitelocations.Contains("ALL"))
                    {
                        _model.Sitelocations = null;
                    }
                    if (_model.Lob == null || _model.Lob.Contains("ALL"))
                    {
                        _model.Lob = null;
                    }

                    if (_model.Wave != null && _model.Wave.Contains("ALL"))
                    {
                        _model.Wave = null; // Set to null if "ALL"
                    }

                    if (_model.Months == null)
                    {
                        _model.Months = null;
                    }
                    if (_model.Year == null)
                    {
                        _model.Year = null;
                    }


                    DateTime? Startdate = null;
                    DateTime? Enddate = null;

                    if (!string.IsNullOrEmpty(_model.FromDate) && DateTime.TryParse(_model.FromDate, out DateTime fromDate))
                    {
                        Startdate = fromDate;
                    }

                    if (!string.IsNullOrEmpty(_model.ToDate) && DateTime.TryParse(_model.ToDate, out DateTime toDate))
                    {
                        Enddate = toDate;
                    }

                    string siteLocationsList = _model.Sitelocations != null && _model.Sitelocations.Length > 0
                   ? string.Join(",", _model.Sitelocations)
                   : null;

                    string Loblist = _model.Lob != null && _model.Lob.Length > 0
                        ? string.Join(",", _model.Lob)
                        : null;


                    string Wavelist = _model.Wave != null && _model.Wave.Length > 0
                        ? string.Join(",", _model.Wave)
                        : null;

                    string StartDateString = Startdate?.ToString("MM/dd/yyyy");
                    string EndDateString = Enddate?.ToString("MM/dd/yyyy");

                    // Construct the SQL query string using string interpolation
                    // Replace empty strings with NULL
                    string site = string.IsNullOrEmpty(siteLocationsList) ? "NULL" : $"N'{siteLocationsList}'";
                    string Status = string.IsNullOrEmpty(_model.Status) ? "NULL" : $"N'{_model.Status}'";
                    string lob = string.IsNullOrEmpty(Loblist) ? "NULL" : $"N'{Loblist}'";
                    string Wave = string.IsNullOrEmpty(Wavelist) ? "NULL" : $"N'{Wavelist}'";
                    string startDate = string.IsNullOrEmpty(StartDateString) ? "NULL" : $"N'{StartDateString}'";
                    string endDate = string.IsNullOrEmpty(EndDateString) ? "NULL" : $"N'{EndDateString}'";
                    string Month = string.IsNullOrEmpty(_model.Months) ? "NULL" : $"N'{_model.Months}'";
                    string Year = string.IsNullOrEmpty(_model.Year) ? "NULL" : $"'{_model.Year}'";

                    string query = $"EXEC dbo.GetSurveyAnswers @Site = {site}, @Lob = {lob},  @Wave = {Wave} , @Status = {Status}, @Month = {Month}, @Year = {Year},  @SDate = {startDate}, @EDate = {endDate}";

                    // Log the query for debugging purposes
                    Console.WriteLine($"Generated Query: {query}");

                    return query;
                }

            }
        }



    }




}
