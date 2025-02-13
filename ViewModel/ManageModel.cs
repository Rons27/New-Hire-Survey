using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VXI_New_Hire_Survey_Tool.Models
{
    public class AddAdmin 
    {
        public IEnumerable<adminInfo> Table;
    }

    public class adminInfo
    {
        public string Domain { get; set; }
        public string WindowsID { get; set; }
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PositionLevel { get; set; }
        public string HireDate { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Team { get; set; }
        public string Location { get; set; }
        public string LocationDesc { get; set; }
        public string TitleName { get; set; }
        public string SupervisorID { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string ProjectID { get; set; }
        public string Project { get; set; }
        public string JobLevel { get; set; }
        public string Ranking { get; set; }
        public string IsActive { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Hrid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Team { get; set; }
        public string LocationDesc { get; set; }
        public string Country { get; set; }
        public string Deparatment { get; set; }
        public string EmailAddress { get; internal set; }
        public string Role { get; internal set; }
        public string Position { get; internal set; }
        public string Lob { get; internal set; }
        public string DateOfBirth { get; internal set; }
        public string DateHired { get; internal set; }
        public string Address { get; internal set; }
        public string City { get; internal set; }
        public string ContactNumber { get; internal set; }
        public string TinNumber { get; internal set; }
        public string SSSNumber { get; internal set; }
        public string PagibigNumber { get; internal set; }
        public string CivilStatus { get; internal set; }
        public object Table { get; internal set; }
        public object PresentAddress { get; internal set; }
    }

}