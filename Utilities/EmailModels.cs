using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace VXI_Trainer_Survey.Repository
{
    public class EmailModels
    {
        public EmailModels()
        {
            this.To = new List<string>();
            this.Cc = new List<string>();
            Header = new StringBuilder();
            Body = new StringBuilder();
            Footer = new StringBuilder();
        }

        public string Address { get; set; }
        public string Subject { get; set; }
        public string QrCodeFileName { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public StringBuilder Header { get; set; }
        public StringBuilder Body { get; set; }
        public StringBuilder Footer { get; set; }
    }
}