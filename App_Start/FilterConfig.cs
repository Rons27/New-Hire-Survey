using System.Web;
using System.Web.Mvc;

namespace VXI_New_Hire_Survey_Tool
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
