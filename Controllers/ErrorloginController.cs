using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class ErrorloginController : Controller
    {
        // GET: Errrorlogin
        public ActionResult Index()
        {
            Session["admin"] = null;
            Session["Account"] = null;
            return View();

        }
    }
}