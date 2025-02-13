using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VXI_New_Hire_Survey_Tool.ViewModel
{
    public class UserManageVM
    {
        public string HRID { get; set; }
        public string NTAccount { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string HireDate { get; set; }
        public string LOB { get; set; }
        public string Position { get; set; }
        public string Site { get; set; }
        public string PositionLevel { get; set; }
        public string Roles { get; set; }
        public int Id { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<bool> isSentEmail { get; set; }
    }
}