using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VXI_New_Hire_Survey_Tool.ViewModel
{
    public class SurveyAnswerViewModel
    {
        public int SurveyResponseId { get; set; }
        public string Hrid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string HireDate { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
        public string Project { get; set; }
        public string Region { get; set; }
        public string Site { get; set; }
        public string SupervisorID { get; set; }
        public string PositionLevel { get; set; }
        public string NtAccount { get; set; }
        public string DateCreated { get; set; }
        public string TL_HRID { get; set; }
        public string Comment { get; set; }
        public string AnswersID { get; set; }
        public string QuestionID { get; set; }
        public List<string> Answers { get; set; }  // List of answers
        public List<QuestionAnswerPair> QuestionAnswers { get; set; }
    }

    public class QuestionAnswerPair
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
    }
}