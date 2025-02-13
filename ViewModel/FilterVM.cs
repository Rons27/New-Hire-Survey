using System.Collections.Generic;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class FilterVM
    {
        public string ToDate { get; set; }
        public string FromDate { get; set; }
        public string Months { get; set; }
        public string Year { get; set; }
        public string[] Sitelocations { get; set; }
        public string HireDate { get; set; }
        public string DATEOFRESPONSE { get; set; }
        public string Report { get; set; }
        public string Status { get; set; }
        public string[] Lob { get; set; }
        public string[] Wave { get; set; }


        public string HireDateFrom { get; set; }
        public string HireDateTo { get; set; }

        public string DATEOFRESPONSEFROM { get; set; }
        public string DATEOFRESPONSETO { get; set; }

    }
}