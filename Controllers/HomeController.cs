using System.Linq;
using System.Web.Mvc;
using VXI_New_Hire_Survey_Tool.Models;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var UserAccount = (Respondents)Session["Account"];
            var admin = (UserManage)Session["admin"];

            if (UserAccount != null || admin != null)
            {
                if (admin != null)
                {
                    if (admin.Roles == "REPORT")
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
                    else if (admin.Roles == "ADMIN")
                    {
                        return View();
                    }

                    else if (admin.Roles != "ADMIN")
                    {
                        return View();
                    }
                    else
                    {
                        return Redirect("~/Login/Index");
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }
}