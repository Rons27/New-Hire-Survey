using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;
using VXI_New_Hire_Survey_Tool.ViewModel;
using VXI_New_Hire_Survey_Tool.VM;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class UserManageController : Controller
    {
        // GET: UserManage
        public ActionResult Index()
        {

            var admin = (UserManage)Session["admin"];
            var UserAccount = (Respondents)Session["Account"] ?? null;
            if (admin != null)
            {
                if (admin != null)
                {
                    if (admin.Roles == "REPORT")
                    {
                        if (admin.LOB == "INNOVATION AND TECHNOLOGY")
                        {
                            return View();
                        }
                        else
                        {
                            Session["Access"] = admin.Roles;
                            return Redirect("~/REPORT/Index");
                        }
                    }
                    else if (admin.Roles != "ADMIN")
                    {
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        return View();
                    }
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

        //[HttpPost]
        //public JsonResult searchNewAdminByHrid(string hrid, string Domain)
        //{
        //    Domain = "VXIPHP";

        //    RootObject users = new RootObject();
        //    string initialData = "";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
        //        client.DefaultRequestHeaders.Accept.Add(contentType);
        //        HttpResponseMessage response = client.GetAsync($"{ConfigurationManager.AppSettings["iTech_Api_Domain"]}{hrid}/{Domain}").Result;

        //        initialData = response.Content.ReadAsStringAsync().Result;
        //        initialData = JsonConvert.DeserializeObject<string>(initialData);
        //        users = JsonConvert.DeserializeObject<RootObject>(initialData);
        //    }
        //    return Json(users, JsonRequestBehavior.AllowGet);

        //}

        //[HttpPost]
        //public JsonResult SearchNewAdminByHrid(string hrid)
        //{
        //    try
        //    {
        //        string authApiUrl = "https://qa-employee-api.vxi.com/api/token/";
        //        string userManageUrl = "https://qa-employee-api.vxi.com/api/employee";
        //        string username = "itech";
        //        string password = "4zcl*OSiT8CpT@0`";

        //        using (HttpClient client = new HttpClient())
        //        {
        //            // Authenticate and get token
        //            var authData = new { username = username, password = password };
        //            var authResponse = client.PostAsJsonAsync(authApiUrl, authData).Result;

        //            if (!authResponse.IsSuccessStatusCode)
        //            {
        //                return Json(new { error = "Authentication failed" }, JsonRequestBehavior.AllowGet);
        //            }

        //            var authContent = authResponse.Content.ReadAsStringAsync().Result;
        //            dynamic authResult = JsonConvert.DeserializeObject(authContent);
        //            string token = authResult.access;

        //            // Fetch user details
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //            HttpResponseMessage response = client.GetAsync($"{userManageUrl}/{hrid}/").Result;

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                return Json(new { error = "User not found" }, JsonRequestBehavior.AllowGet);
        //            }

        //            var userData = response.Content.ReadAsStringAsync().Result;
        //            var user = JsonConvert.DeserializeObject<RootObject2>(userData);

        //            return Json(user., JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}



        [HttpPost]
        public JsonResult SearchNewAdminByHrid(string hrid)
        {
            RootObject users = new RootObject();

            using (HttpClient client = new HttpClient())
            {
                // Set request headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Call the API using ConfigurationManager
                HttpResponseMessage response = client.GetAsync($"{ConfigurationManager.AppSettings["Jira_API"]}{hrid}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string initialData = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<RootObject2>(initialData);
                    return Json(user, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "error", message = "Failed to fetch data" }, JsonRequestBehavior.AllowGet);
                }
            }
        }



        [HttpGet]
        public JsonResult GetUsers()
        {

            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var result = (from s in Obj.UserManage select s).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public string saveUser(UserManageVM _params)
        {
            var hiredate = _params.HireDate;
            var message = "";
            var hrid = "";
            var first_name = "";
            var last_name = "";
            var UserAccount = (Respondents)Session["Account"] ?? null;
            var admin = (UserManage)Session["admin"] ?? null;
            if (UserAccount != null)
            {
                hrid = UserAccount.Hrid;
                first_name = UserAccount.FirstName;
                last_name = UserAccount.LastName;
            }
            else
            {
                hrid = admin.HRID;
                first_name = admin.First_Name;
                last_name = admin.Last_Name;
            }



            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var result = (from s in Obj.UserManage where s.HRID == _params.HRID select s).FirstOrDefault();
                if (result != null)
                {
                    message = "Exist";
                }
                else
                {
                   

                    Obj.UserManage.Add(new UserManage()
                    {
                        HRID = _params.HRID,
                        NTAccount = _params.NTAccount,
                        First_Name = _params.FirstName,
                        Last_Name = _params.LastName,
                        LOB = _params.LOB,
                        Position = _params.Position,
                        Position_Level = _params.PositionLevel,
                        Site = _params.Site,
                        Roles = _params.Roles,
                        HireDate = _params.HireDate,
                        DateCreated = DateTime.Now.ToString(),
                        DateUpdated = DateTime.Now.ToString(),
                        Createdby = first_name + " " + last_name,
                        UpdatedBy = first_name + " " + last_name,
                        EmailAddress = _params.EmailAddress,
                        IsActive = true,
                    });
                    Obj.SaveChanges();
                    message = "Success";
                }

                return message;
            }

        }

        [HttpPost]
        public JsonResult getOneuser(int Id)
        {

            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var result = (from s in Obj.UserManage where s.Id == Id select s).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public String UpdateUsers(UserManageVM _params)
        {
            var hrid = "";
            var first_name = "";
            var last_name = "";
            var UserAccount = (Respondents)Session["Account"] ?? null;
            var admin = (UserManage)Session["admin"] ?? null;
            if (UserAccount != null)
            {
                hrid = UserAccount.Hrid;
                first_name = UserAccount.FirstName;
                last_name = UserAccount.LastName;
            }
            else
            {
                hrid = admin.HRID;
                first_name = admin.First_Name;
                last_name = admin.Last_Name;
            }

            var msg = "";
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var result = (from s in Obj.UserManage where s.Id == _params.Id select s).FirstOrDefault();
                if (result != null)
                {

                    result.Roles = _params.Roles;
                    result.IsActive = _params.IsActive;
                    result.isSentEmail = _params.isSentEmail;
                    result.DateUpdated = DateTime.Now.ToString();
                    result.UpdatedBy = first_name + " " + last_name;
                    Obj.SaveChanges();
                    msg = "Successfully updated";

                }
                else
                {
                    msg = "Update Failed";
                }
                return msg;
            }

        }

        [HttpPost]
        public string deleteUser(int id)
        {
            string message = "";
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var result = (from s in Obj.UserManage where s.Id == id select s).FirstOrDefault();
                Obj.UserManage.Remove(result);
                Obj.SaveChanges();

                message = "Record deleted";
                return message;

            }
        }




    }
}