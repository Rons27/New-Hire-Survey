using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;
using VXI_New_Hire_Survey_Tool.ViewModel;
using VXI_New_Hire_Survey_Tool.VM;
using VXI_Trainer_Survey.Repository;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class TakesurveyController : Controller
    {
        // GET: Takesurvey
        public ActionResult Index()
        {
            var UserAccount = (Respondents)Session["Account"] ?? null;
            var admin = (UserManage)Session["admin"];

            if (UserAccount != null || admin != null)
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
                    else if (admin.Roles == "ADMIN")
                    {
                        return View();
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
            else
            {
                return Redirect("~/Login/Index");
            }
        }

        public ActionResult TakeSurvey()
        {
            var UserAccount = (Respondents)Session["Account"] ?? null;
            var admin = (UserManage)Session["admin"];

            if (UserAccount != null || admin != null)
            {
                return View();
            }
            else
            {
                return Redirect("~/Login/Index");
            }
        }   

        [HttpGet]
        public JsonResult GetSurvey()
        {
            using (VXI_NEW_HIRE_SURVEYEntities obj = new VXI_NEW_HIRE_SURVEYEntities())
            {

                var questionList = (from s in obj.SurveyQuestion where s.IsActive == true select s).ToList().OrderBy(i => i.Id);
                var SurveyChoices = (from sc in obj.SurveyChoices select sc).ToList().OrderBy(i => i.Id);

                return Json(new { questionList, SurveyChoices }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult GetStatus()
        {
            var message = "";
            using (VXI_NEW_HIRE_SURVEYEntities obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var hrid = "";
                var UserAccount = (Respondents)Session["Account"] ?? null;
                var admin = (UserManage)Session["admin"] ?? null;
                if (UserAccount != null)
                {
                    hrid = UserAccount.Hrid;
                    var ResponseId = UserAccount.Id.ToString();
                }
                else
                {
                    hrid = admin.HRID;
                    var ResponseId = admin.Id.ToString();
                }



                var userStatus = (from s in obj.SurveyAnswer where s.HRID == hrid select s).FirstOrDefault();
                var SurveyAnswer_info_list = (from s in obj.SurveyAnswer_info where s.HRID == hrid select s).FirstOrDefault();

                if (userStatus == null && SurveyAnswer_info_list == null)
                {
                    message = "Untake";
                }
                else
                {
                    message = "Alreadytake";
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveAnwers(List<SurveyAnswerVM> answers, Answer_info Answer_info)
        {
            using (VXI_NEW_HIRE_SURVEYEntities obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                var message = "";
                var hrid = "";
                var ResponseId = "";
                var Respondent_Email = "";
                var Site = "";
                var Lob = "";
                var UserAccount = (Respondents)Session["Account"] ?? null;
                var admin = (UserManage)Session["admin"] ?? null;
                var Teams = "";
                var Departments = "";
                var Project = "";
                var FirstName = "";
                var Lastname = "";
                string HireDate = "";



                var SupervisorId = "";
                if (UserAccount != null)
                {
                    DateTime DateHire = DateTime.Parse(UserAccount.HireDate);
                    HireDate = DateHire.ToString("MM/dd/yyyy");

                    FirstName = UserAccount.FirstName;
                    Lastname = UserAccount.LastName;
                    Site = UserAccount.Site;
                    Lob = UserAccount.Team;
                    hrid = UserAccount.Hrid;
                    SupervisorId = UserAccount.SupervisorID;
                    ResponseId = UserAccount.Id.ToString();
                    Respondent_Email = UserAccount.Email;
                    Teams = UserAccount.Team;
                    Departments = UserAccount.Department;
                    Project = UserAccount.Project;

                }
                else
                {
                    DateTime DateHire = DateTime.Parse(admin.HireDate);
                     HireDate = DateHire.ToString("MM/dd/yyyy");


                    FirstName = admin.First_Name;
                    Lastname = admin.Last_Name;
                    Site = admin.Site;
                    Lob = UserAccount.Team;
                    hrid = admin.HRID;
                    ResponseId = admin.Id.ToString();
                    Respondent_Email = admin.EmailAddress;
                 
                }

                //var Respondents = (from sc in obj.Respondents select sc).ToList().OrderBy(i => i.Id);
                var SurveyRespondent = (from SR in obj.Respondents where SR.Hrid == hrid select SR).FirstOrDefault();

                var SurveyAnswerlist = (from s in obj.SurveyAnswer where s.HRID == hrid select s).FirstOrDefault();
                var SurveyAnswer_info_list = (from s in obj.SurveyAnswer_info where s.HRID == hrid select s).FirstOrDefault();

                var Locationlist = (from s in obj.Locations where s.Location == Site select s).FirstOrDefault();

                //this part is auto update  12   LOB  
                if (Locationlist != null)
                {
                    var locationID = Locationlist.LocationId;
                    var Loblist = (from l in obj.Lobs where l.Name == Lob && l.LocationId == locationID select l).FirstOrDefault();

                    if (Loblist == null)
                    {
                        obj.Lobs.Add(new Lobs()
                        {
                            LocationId = Locationlist.LocationId,
                            Name = Lob,
                            Description = Lob,
                            CreatedBy = "SYSTEM",
                            UpdatedBy = "SYSTEM",
                            DateUpdated = DateTime.Now.ToString("yyyy-MM-dd"),
                            IsActive = true,
                            DateCreated = DateTime.Now.ToString("yyyy-MM-dd")

                        });
                        obj.SaveChanges();

                    }
                }
                if (SurveyRespondent != null)
                {

                    if (SurveyAnswerlist == null && SurveyAnswer_info_list == null)
                    {
                        foreach (var userAnswers in answers)
                        {
                            obj.SurveyAnswer.Add(new SurveyAnswer()
                            {
                                ResponseId = ResponseId,
                                HRID = hrid,
                                Answers = userAnswers.answer,
                                QuestionId = userAnswers.questionId,
                                DateCreated = DateTime.Now.ToString("MM/dd/yyyy")
                            });
                        }

                        obj.SurveyAnswer_info.Add(new SurveyAnswer_info()
                        {
                            ResponseId = ResponseId,
                            HRID = hrid,
                            Department = Project,
                            Team = Teams,
                            //TL_HRID = Answer_info.TrainorID,
                            Comment = Answer_info.comment,
                            //Trainor_Fullname = Answer_info.Fullname,
                            SupervisorId = SupervisorId,
                            Site = Site,
                            HireDate = HireDate,
                            DateCreated = DateTime.Now.ToString()

                        }); ;

                        SurveyRespondent.Status = "Completed";

                        obj.SaveChanges();

                        message = "Success";
                    }

                    else
                    {
                        message = "already_take";
                    }
                }
                else
                {
                    message = "NEED TO REGISTER";
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        private void SentEmail()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public string SentEmail(string Respondent_Email, string SupervisorId, string FirstName, string Lastname)
        {
            RootObject superData = new RootObject();
            string message = "";
            //var ccEmail = "mark.luna@vxi.com";
            //var ccEmail = "ronnie.b.alfonso.jr@gmail.com";
            List<string> cc = new List<string>();
            using (VXI_NEW_HIRE_SURVEYEntities Obj = new VXI_NEW_HIRE_SURVEYEntities())
            {
                if (ConfigurationManager.AppSettings["EmailNotification"] == "ON")
                {
                    var currentDate = DateTime.Now;

                    superData = getFromApi(SupervisorId, "PH");
                    var TLEmail = superData.Table[0].Email;

                    List<string> to = new List<string>();
                    to.Add(Respondent_Email);
                    //to.Add(ccEmail);
                    to.Add(TLEmail);

                    var ccuser = (from c in Obj.UserManage where c.isSentEmail == true && (c.EmailAddress != null && c.EmailAddress != "") select c).ToList();
                    foreach (var ccs in ccuser)
                    {
                        if (ccs != null)
                        {
                            cc.Add(ccs.EmailAddress);
                            //cc.Add(ccEmail);
                        }
                        else
                        {
                            //NO Email 
                        }
                    }

                    System.Text.StringBuilder header = new StringBuilder();
                    StringBuilder body = new StringBuilder();
                    body.AppendLine("<p>Dear: <b>" + "" + FirstName + " " + Lastname + "</b>,</p>");
                    body.AppendLine("<p>We truly appreciate the time and effort you took to complete our survey.</p>");
                    body.AppendLine("<p>Your opinion truly matters to us and will help to make things even better.</p>");
                    body.AppendLine("<p>Once again, thank you for your time and contribution.</p>");
                    body.AppendLine("<p>We look forward to your continued support and engagement.</p>");

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
    }
}