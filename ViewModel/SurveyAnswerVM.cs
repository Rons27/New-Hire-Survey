using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VXI_New_Hire_Survey_Tool.ViewModel
{
    public class SurveyAnswerVM
    {
        public int Id { get; set; }
        public string ResponseId { get; set; }
        public string HRID { get; set; }
        public string answer { get; set; }
        public string questionId { get; set; }
        public string DateCreated { get; set; }
    }
}