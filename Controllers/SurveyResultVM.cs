namespace VXI_New_Hire_Survey_Tool.Controllers
{
    public class SurveyResultVM
    {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionBody { get; set; }
        public int StronglyDisagree { get; set; }
        public int Disagree { get; set; }
        public int Neutral { get; set; }
        public int Agree { get; set; }
        public int StronglyAgree { get; set; }
        public string Comment { get; set; }
        //public decimal StronglyDisagreePct { get; set; }
        //public decimal DisagreePct { get; set; }
        //public decimal NeutralPct { get; set; }
        //public decimal AgreePct { get; set; }
        //public decimal StronglyAgreePct { get; set; }

    }
}