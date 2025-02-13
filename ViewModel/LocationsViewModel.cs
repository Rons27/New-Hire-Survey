using System.Collections.Generic;

namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class LocationsViewModel
    {
        public List<LocationViewModel> Table { get; set; }
    }
    public class LocationViewModel
    {
        public string Region { get; set; }
        public string Location { get; set; }
        public string LocationDesc { get; set; }
    }

}