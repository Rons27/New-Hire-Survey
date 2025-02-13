using System.Collections.Generic;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class DepartmentsViewModel
    {
        public List<DepartmentViewModel> Table { get; set; }
    }
    public class DepartmentViewModel
    {
        public string Region { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
    }

}