using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;
using VXI_New_Hire_Survey_Tool.ViewModel;
using VXI_New_Hire_Survey_Tool.VM;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public async Task<ActionResult> Index()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var currentDate = DateTime.Now;
                var getuser = await Obj.Respondents.ToListAsync();

                var SystemupdateResposdent = await Obj.DateSystemUpdate.FirstOrDefaultAsync();

                var DateUpdatedSystem = DateTime.Parse(SystemupdateResposdent.DateUpdatedSystem);
                var counts = (currentDate.Date - DateUpdatedSystem.Date).TotalDays;

                if (counts >= 1)
                {
                    SystemupdateResposdent.DateUpdatedSystem = currentDate.ToString();
                    await Obj.SaveChangesAsync();

                    foreach (var UpdateRespondents in getuser)
                    {
                        var hireDate = DateTime.Parse(UpdateRespondents.HireDate); // Assuming HireDate is in a valid DateTime format
                        var differenceInDays = (currentDate.Date - hireDate).TotalDays;
                        bool isUserAllowed = differenceInDays >= 91;

                        if (isUserAllowed && (UpdateRespondents.Status != "Completed" && UpdateRespondents.Status != "Expired"))
                        {
                            UpdateRespondents.Status = "Expired";
                            await Obj.SaveChangesAsync();
                        }
                    }
                }
            }

            return View();
        }

        public ActionResult Logouts()
        {
            Session["admin"] = null;
            Session["Account"] = null;
            return Redirect("~/Login/Index");
        }

        public JsonResult LoginUser(LoginVM Login)
        {
            var hiredate = Login.Hiredate;
            string formatted_hiredate = DateTime.ParseExact(hiredate, "MMddyyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
            //string formatted_hiredate = DateTime.ParseExact(hiredate, "MMddyyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

            var message = "";
            RootObject users = new RootObject();
            LocationsViewModel Sites = new LocationsViewModel();
            string sam = Request.LogonUserIdentity.Name;
            string[] samtest = sam.Split('\\');
            //var domain = samtest[0];
            //var domain = "VXIPHP";
            var ntAccount = samtest[1];
            //ntAccount = "adivina";
            string sam2 = Request.LogonUserIdentity.Name;
            string[] parts = sam2.Split('\\');
            string username = parts[1];

            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                try
                {
                    var getuser = (from s in Obj.Respondents
                                   where s.Hrid == Login.Hrid && s.HireDate == formatted_hiredate
                                   select s).FirstOrDefault();

                    var admin = (from s in Obj.UserManage where s.HRID == Login.Hrid && s.HireDate == formatted_hiredate && s.IsActive == true select s).FirstOrDefault();

                    if (getuser != null)
                    {
                        var currentDate = DateTime.Now;
                        var hireDate = DateTime.Parse(getuser.HireDate); // Assuming HireDate is in a valid DateTime format
                        var differenceInDays = (currentDate.Date - hireDate).TotalDays;

                        bool isUserAllowed = differenceInDays >= 30 && differenceInDays <= 90;

                        if (isUserAllowed == false && getuser.Status != "Completed")
                        {
                            getuser.Status = "Expired";
                        }
                        else
                        {
                            getuser.Status = "Pending";
                        }
                        bool isAdmin = admin?.Roles == "ADMIN";
                        bool isReport = admin?.Roles == "REPORT";
                        bool isInnovationTech = getuser?.Project == "INNOVATION AND TECHNOLOGY" || getuser?.Project == "HUMAN RESOURCES";

                        if (isUserAllowed || isAdmin || isReport || isInnovationTech)
                        {
                            Session["Hrid"] = getuser.Hrid;
                            Session["FirstName"] = getuser.FirstName;
                            Session["LastName"] = getuser.LastName;
                            Session["MiddleName"] = getuser.MiddleName;
                            Session["HireDate"] = getuser.HireDate;
                            Session["Email"] = getuser.Email;
                            Session["Team"] = getuser.Team ?? "";
                            Session["Department"] = getuser.Department;
                            Session["Region"] = getuser.Region;
                            Session["Site"] = getuser.Site.Replace(",", "");
                            Session["Supervisorid"] = getuser.SupervisorID;
                            Session["PositionLevel"] = getuser.PositionLevel;
                            Session["Account"] = getuser;
                            Session["admin"] = admin;

                            var result = (from s in Obj.UserManage where s.HRID == getuser.Hrid && s.IsActive == true select s).FirstOrDefault();
                            if (result == null)
                            {
                                if (Session["Team"].ToString() == "INNOVATION AND TECHNOLOGY" || Session["Team"].ToString() == "HUMAN RESOURCES")
                                {
                                    Session["Access"] = "ADMIN";
                                    message = "Success";
                                }
                                else
                                {
                                    Session["Access"] = "USER";
                                    message = "Success";
                                }
                            }
                            else
                            {
                                var access = result.Roles;
                                if (access == "ADMIN")
                                {
                                    Session["Access"] = access;
                                    message = "Success";
                                }
                                else if (access == "REPORT")
                                {
                                    Session["Access"] = access;
                                    message = "REPORT";
                                }
                                else
                                {
                                    message = "USER";
                                }
                            }
                        }
                        else
                        {
                            message = "Nopermission";
                            Session["admin"] = null;
                            Session["Account"] = null;
                        }
                    }
                    else if (admin != null)
                    {
                        var HRID_ADMIN = "";
                        HRID_ADMIN = admin.HRID;
                        Session["Hrid"] = HRID_ADMIN;
                        Session["FirstName"] = admin.First_Name;
                        Session["LastName"] = admin.Last_Name;
                        Session["Team"] = admin.LOB == null ? "" : admin.LOB;
                        Session["Site"] = admin.Site.Replace(",", "");
                        Session["admin"] = admin;
                        var result = (from s in Obj.UserManage where s.HRID == HRID_ADMIN select s).FirstOrDefault();

                        if (result.LOB == "INNOVATION AND TECHNOLOGY" || result.LOB == "HUMAN RESOURCES")
                        {

                            string initialData = "";
                            var domain = "VXIPHP";
                            using (HttpClient client = new HttpClient())
                            {
                                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                                client.DefaultRequestHeaders.Accept.Add(contentType);
                                HttpResponseMessage response = client.GetAsync($"{ConfigurationManager.AppSettings["iTech_Api_Domain"]}{HRID_ADMIN}/{domain}").Result;

                                initialData = response.Content.ReadAsStringAsync().Result;
                                initialData = JsonConvert.DeserializeObject<string>(initialData);
                                users = JsonConvert.DeserializeObject<RootObject>(initialData);

                                Obj.LogVisit.Add(new LogVisit()
                                {
                                    Hrid = Login.Hrid,
                                    CountryCode = users.Table[0].Country,
                                    SiteCode = users.Table[0].LocationDesc,
                                    //ProgramCode = users.Table[0].Team,
                                    FullName = users.Table[0].FirstName + " " + users.Table[0].LastName,
                                    DateCreated = DateTime.Now
                                });

                                Obj.SaveChanges();


                                Obj.Respondents.Add(new Respondents()
                                {
                                    Hrid = users.Table[0].ID,
                                    FirstName = users.Table[0].FirstName,
                                    LastName = users.Table[0].LastName,
                                    MiddleName = users.Table[0].MiddleName,
                                    HireDate = users.Table[0].HireDate,
                                    Department = users.Table[0].Project,
                                    Email = users.Table[0].Email,
                                    Team = users.Table[0].Team,
                                    Project = users.Table[0].Project,
                                    Region = users.Table[0].Country,
                                    Site = users.Table[0].Location,
                                    SupervisorID = users.Table[0].SupervisorID,
                                    PositionLevel = users.Table[0].TitleName,
                                    NtAccount = users.Table[0].WindowsID,
                                    Status = "Pending",
                                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd")
                                });
                                Obj.SaveChanges();


                                var getuser2 = (from s in Obj.Respondents
                                                where s.Hrid == Login.Hrid && s.HireDate == formatted_hiredate
                                                select s).FirstOrDefault();

                                Session["Hrid"] = getuser2.Hrid;
                                Session["FirstName"] = getuser2.FirstName;
                                Session["LastName"] = getuser2.LastName;
                                Session["MiddleName"] = getuser2.MiddleName;
                                Session["HireDate"] = getuser2.HireDate;
                                Session["Email"] = getuser2.Email;
                                Session["Team"] = getuser2.Team ?? "";
                                Session["Department"] = getuser2.Department;
                                Session["Region"] = getuser2.Region;
                                Session["Site"] = getuser2.Site.Replace(",", "");
                                Session["Supervisorid"] = getuser2.SupervisorID;
                                Session["PositionLevel"] = getuser2.PositionLevel;
                                Session["Account"] = getuser2;
                                Session["admin"] = admin;

                            }

                        }

                        if (result == null)
                        {
                            if (Session["Team"].ToString() == "INNOVATION AND TECHNOLOGY")
                            {
                                Session["Access"] = "ADMIN";
                                message = "Success";
                                message = "REPORT";
                            }
                            else
                            {
                                Session["Access"] = "REPORT";
                            }
                        }
                        else if (Session["Team"].ToString() == "INNOVATION AND TECHNOLOGY")
                        {
                            Session["Access"] = "ADMIN";
                            message = "Success";

                        }
                        else
                        {
                            var access = result.Roles;
                            if (access == "ADMIN")
                            {
                                Session["Access"] = result.Roles;
                                message = "Success";
                            }
                            else if (access == "REPORT")
                            {
                                Session["Access"] = result.Roles;
                                message = "REPORT";
                            }
                            else
                            {
                                message = "NOAccess";
                                Session["admin"] = null;
                                Session["Account"] = null;
                            }
                        }

                    }
                    else
                    {
                        message = "NOAccess";
                        Session["admin"] = null;
                        Session["Account"] = null;
                    }

                }
                catch (Exception)
                {
                    message = "NOAccess";
                    Session["admin"] = null;
                    Session["Account"] = null;
                    throw;
                }

                string initialData2 = "";
                var domain2 = "VXIPHP";
                using (HttpClient client = new HttpClient())
                {
                    MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    HttpResponseMessage response = client.GetAsync($"{ConfigurationManager.AppSettings["iTech_Api_Domain"]}{Login.Hrid}/{domain2}").Result;

                    initialData2 = response.Content.ReadAsStringAsync().Result;
                    initialData2 = JsonConvert.DeserializeObject<string>(initialData2);
                    users = JsonConvert.DeserializeObject<RootObject>(initialData2);
               
                    Obj.LogVisit.Add(new LogVisit()
                    {
                        Hrid = Login.Hrid,
                        CountryCode = users.Table[0].Country,
                        SiteCode = users.Table[0].LocationDesc,
                        //ProgramCode = users.Table[0].Team,
                        FullName = users.Table[0].FirstName + " " + users.Table[0].LastName,
                        DateCreated = DateTime.Now
                    });

                    Obj.SaveChanges();
            
                }

                    Locations();
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GETEMAIL(string HRID, string EmailAddress)
        {
            var apiBaseUrl = ConfigurationManager.AppSettings["PRISM"];
            var apiUrl = $"{apiBaseUrl}{HRID}";

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);

                // Log the API URL
                System.Diagnostics.Debug.WriteLine("API URL: " + apiUrl);

                try
                {
                    var response = httpClient.GetAsync(apiUrl).Result;

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    var stringData = response.Content.ReadAsStringAsync().Result;

                    // Log the JSON response for debugging
                    System.Diagnostics.Debug.WriteLine("API Response: " + stringData);

                    // Try to deserialize the JSON response
                    UsersVM user = null;
                    try
                    {
                        user = JsonConvert.DeserializeObject<UsersVM>(stringData);
                    }
                    catch (JsonException ex)
                    {
                        // Log the JSON deserialization error
                        System.Diagnostics.Debug.WriteLine("Deserialization Error: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine("Raw JSON: " + stringData);
                        return Json(new { error = "Invalid JSON format." }, JsonRequestBehavior.AllowGet);
                    }

                    // Ensure the user object is not null before accessing its properties
                    if (user != null)
                    {
                        EmailAddress = user.EmailAddress1.Value ?? "_";
                    }
                    else
                    {
                        return Json(new { error = "User not found." }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Log the HTTP request error
                    System.Diagnostics.Debug.WriteLine("Request Error: " + ex.Message);
                    return Json(new { error = "Request error." }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Log any other errors
                    System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    return Json(new { error = "An error occurred." }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(EmailAddress, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Locations()
        {

            try
            {
                using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
                {


                    LocationsViewModel Sites = new LocationsViewModel();
                    using (HttpClient client = new HttpClient())
                    {

                        MediaTypeWithQualityHeaderValue contentType =
                            new MediaTypeWithQualityHeaderValue("application/json");
                        client.DefaultRequestHeaders.Accept.Add(contentType);
                        HttpResponseMessage response = client
                            .GetAsync($"{WebConfigurationManager.AppSettings["GlobalServiceList"]}GlobalHR/Locations")
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
                        Sites = JsonConvert.DeserializeObject<LocationsViewModel>(stringData);
                        var Regions = Sites.Table;
                        var locationList = Obj.Locations.ToList();
                        var regionList = Regions.Where(r => r.Region.ToString() == "PH").ToList();
                        var existingSites = locationList.Select(s => s.Site).ToHashSet();
                        foreach (var region in regionList)
                        {
                            if (!existingSites.Contains(region.LocationDesc))
                            {
                                Obj.Locations.Add(new Locations()
                                {
                                    Region = region.Region,
                                    Location = region.Location,
                                    Site = region.LocationDesc,
                                    DateCreated = DateTime.Now,
                                    CreatedBy = "SYSTEM",
                                    UpdatedBy = "SYSTEM",
                                    IsActive = true
                                });

                            }
                        }
                        Obj.SaveChanges();
                        return Json(stringData, JsonRequestBehavior.AllowGet);




                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}