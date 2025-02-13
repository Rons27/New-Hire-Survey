using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;
using VXI_New_Hire_Survey_Tool.ViewModel;
using VXI_New_Hire_Survey_Tool.VM;
using VXI_Trainer_Survey.Repository;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class BroadcastController : Controller
    {
        // GET: Boardcast
        public ActionResult Index()
        {


            var admin = (UserManage)Session["admin"];
            var UserAccount = (Respondents)Session["Account"];
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
        [HttpGet]
        public string GetSurveyResponses()
        {
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var Respondents = (from s in Obj.Respondents select s).ToList();


                string jsonData = JsonConvert.SerializeObject(Respondents);

                //return Json(Respondents, JsonRequestBehavior.AllowGet);
                return jsonData;

            }
        }

        [HttpPost]
        public string deleteResponses(int id)
        {
            string message = "";
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var result = (from s in Obj.Respondents where s.Id == id select s).FirstOrDefault();
                Obj.Respondents.Remove(result);
                Obj.SaveChanges();

                message = "Record deleted";
                return message;

            }
        }

        [HttpPost]
        public JsonResult getOneuser(int Id)
        {

            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var result = (from s in Obj.Respondents where s.Id == Id select s).FirstOrDefault();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public String UpdateUsers(Respondents _params)
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

                var result = (from s in Obj.Respondents where s.Id == _params.Id select s).FirstOrDefault();
                if (result != null)
                {
                    result.Email = _params.Email;
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
        public string SentEmail(int id, string URL)
        {
            RootObject superData = new RootObject();
            string message = "";
            //var ccEmail = "Ronnie.Alfonso@vxi.com";
            List<string> cc = new List<string>();
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                if (ConfigurationManager.AppSettings["EmailNotification"] == "ON")
                {
                    var currentDate = DateTime.Now;
                    var RespondentID = id.ToString();

                    var Respondents = (from s in Obj.Respondents where s.Id == id select s).FirstOrDefault();
                    var SurveyAnswer2 = (from sa in Obj.SurveyAnswer where sa.ResponseId == RespondentID && sa.HRID == Respondents.Hrid select sa).FirstOrDefault();


                    var hireDate = DateTime.Parse(Respondents.HireDate);
                    var differenceInDays = (currentDate.Date - hireDate.Date).TotalDays;

                    var ccuser = (from c in Obj.UserManage where c.isSentEmail == true && (c.EmailAddress != null && c.EmailAddress != "") select c).ToList();
                    foreach (var ccs in ccuser)
                    {
                        if (ccs != null)
                        {

                            cc.Add(ccs.EmailAddress);
                        }
                        else
                        {
                            //NO Email 
                        }
                    }

                    //superData = getFromApi(ccs.HRID, "PH");
                    if (differenceInDays >= 30 && differenceInDays <= 90  && (Respondents.Status != "Expired"))
                    {
                        List<string> to = new List<string>();
                        to.Add(Respondents.Email);
                        //cc.Add(ccEmail);
                        //cc.Add(Respondents.SupervisorID);
                        //cc.Add(ccEmail);

                        System.Text.StringBuilder header = new StringBuilder();
                        StringBuilder body = new StringBuilder();

                        body.AppendLine("<p>Welcome to the VXI FAMILY! </p>");
                        body.AppendLine("<p>As a new hire, we’re excited to have you on board. </p>");
                        body.AppendLine("<p> As part of our commitment to creating an exceptional employee experience," + "<br> </p>");
                        body.AppendLine("<p> we invite you to participate in our  <b> New Hire Survey. </b>" + "<br> </p>");
                        body.AppendLine("<p> Your feedback is valuable and will help us improve our processes. Thank you! 😊 </p>");
                        body.AppendLine("<p>You may access the survey on this link: <b>" + "" + URL + "</b>.</p>");

                        StringBuilder footer = new StringBuilder();
                        AutoMailerUtil mailer = new AutoMailerUtil(new EmailCredentialModels() { Host = "vximailboxmk01.vxi.com.ph", Username = "welcometovxi", Password = "W3lcom3VX1", Domain = "vxiphp" });

                        mailer.Send(new EmailModels()
                        {
                            Subject = " New Hire Survey",
                            Address = "WelcometoVXI@vxi.com",
                            To = to,
                            Cc = cc,
                            Header = header,
                            Body = body,
                            Footer = footer,
                        });
                        message = "success";
                    }
                    else if (SurveyAnswer2 != null)
                    {
                        message = "Arlradytake";
                    }
                    else
                    {
                        message = "notAllowed";
                    }

                }
                else
                {
                    message = "Off";
                }



                return message;

            }
        }

        [HttpPost]
        public string SentAllEmail(string URL)
        {
            RootObject superData = new RootObject();
            string message = "";
            //var ccEmail = "Ronnie.Alfonso@vxi.com";
            List<string> cc = new List<string>();
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                if (ConfigurationManager.AppSettings["EmailNotification"] == "ON")
                {
                    var Respondents = (from s in Obj.Respondents select s).ToList();
                    var ccuser = (from c in Obj.UserManage where c.isSentEmail == true && (c.EmailAddress != null && c.EmailAddress != "") select c).ToList();

                    var currentDate = DateTime.Now;
                    //superData = getFromApi(Respondents.SupervisorID, Respondents.Region);
                    if (Respondents.Count != 0)
                    {

                        foreach (var ccs in ccuser)
                        {
                            if (ccs != null)
                            {

                                cc.Add(ccs.EmailAddress);
                            }
                            else
                            {
                                //NO Email 
                            }
                        }

                        foreach (var SurveyRespondent in Respondents)
                        {
                            var RespondentID = SurveyRespondent.Id.ToString();
                            var SurveyAnswer2 = (from sa in Obj.SurveyAnswer where sa.ResponseId == RespondentID && sa.HRID == SurveyRespondent.Hrid select sa).FirstOrDefault();

                            var hireDate = DateTime.Parse(SurveyRespondent.HireDate);
                            var differenceInDays = (currentDate.Date - hireDate.Date).TotalDays;
                                 //29
                            if (differenceInDays >= 30  && differenceInDays <= 90 && (SurveyRespondent.Status != "Expired"))
                            {
                                List<string> to = new List<string>();
                               
                                to.Add(SurveyRespondent.Email);
                                //cc.Add(ccEmail);
                                //cc.Add(Respondents.SupervisorID);

                                System.Text.StringBuilder header = new StringBuilder();
                                StringBuilder body = new StringBuilder();

                                body.AppendLine("<p>Welcome to the VXI FAMILY! </p>");
                                body.AppendLine("<p>As a new hire, we’re excited to have you on board. </p>");
                                body.AppendLine("<p> As part of our commitment to creating an exceptional employee experience," + "<br> </p>");
                                body.AppendLine("<p> we invite you to participate in our  <b> New Hire Survey. </b>" + "<br> </p>");
                                body.AppendLine("<p> Your feedback is valuable and will help us improve our processes. Thank you! 😊 </p>");
                                body.AppendLine("<p>You may access the survey on this link: <b>" + "" + URL + "</b></p>");


                                StringBuilder footer = new StringBuilder();
                                AutoMailerUtil mailer = new AutoMailerUtil(new EmailCredentialModels() { Host = "vximailboxmk01.vxi.com.ph", Username = "welcometovxi", Password = "W3lcom3VX1", Domain = "vxiphp" });

                                mailer.Send(new EmailModels()
                                {
                                    Subject = " New Hire Survey",
                                    Address = "WelcometoVXI@vxi.com",
                                    To = to,
                                    Cc = cc,
                                    Header = header,
                                    Body = body,
                                    Footer = footer,
                                });

                                message = "success";

                            }
                            else if (SurveyAnswer2 == null)
                            {
                                //message = "The Respondent is not Allowed to take the survey";
                                message = "NoRespondent";
                            }
                            else
                            {
                                message = "notAllowed";
                            }

                        }

                    }

                }
                else
                {
                    message = "Off";
                }

                return message;

            }
        }



        public RootObject getFromApi(string HRID, string Region)
        {
            var Domain = "";
            if (Region == "PH")
            {
                Domain = "VXIPHP";
            }
            else if (Region == "IN")
            {
                Domain = "VXIINDIA";
            }
            else
            {
                Domain = "MCM";
            }

            RootObject users = new RootObject();
            string initialData = "";
            using (HttpClient client = new HttpClient())
            {
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                //HttpResponseMessage response = client.GetAsync($"GlobalHR/Employees/FindEEByWinIDDomain/{HRID}/VXIPHP").Result;
                HttpResponseMessage response = client.GetAsync($"{ConfigurationManager.AppSettings["iTech_Api_Domain"]}{HRID}/{Domain}").Result;

                initialData = response.Content.ReadAsStringAsync().Result;
                initialData = JsonConvert.DeserializeObject<string>(initialData);

                users = JsonConvert.DeserializeObject<RootObject>(initialData);

                return users;
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




  

    }
}