using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace VXI_Trainer_Survey.Repository
{
    public class AutoMailerUtil
    {
        private readonly EmailCredentialModels _credential;
        private const string Style = @"<style> div, table{ font-family: Tahoma; font-size: 14px; } table{ border: 1px solid gray; border-collapse: collapse; } td{ border: 1px solid silver; padding: 2px 8px; } span{ font-size: 12px; font-style: italic; } </style>";

        public AutoMailerUtil(EmailCredentialModels credential)
        {
            _credential = new EmailCredentialModels();
            _credential = credential;
        }

        public StatusModels Send(EmailModels email)
        {
            try
            {
                var mail = new MailMessage
                {
                    //From = new MailAddress("iTRACKAutoReply@vxi.com")
                    From = new MailAddress(email.Address)
                };

                //mail.To is your recipient(s) ex. rudnan.chavez@vxi.com
                foreach (string t in email.To)
                {
                    mail.To.Add(t);
                }

                //mail.Cc is your carbon copy recipient(s) ex. ramon.santos@vxi.com
                foreach (string t in email.Cc)
                {
                    mail.CC.Add(t);
                }

                //Put your own email subject here
                //mail.Subject = "iTRACK Online Work Request";
                mail.Subject = email.Subject;

                //Put body or message of email here
                mail.Body = Style + email.Header.ToString() + email.Body.ToString();
                mail.IsBodyHtml = true;


                //AlternateView imgView = AlternateView.CreateAlternateViewFromString(mail.Body + "<br/><img src=cid:imgpath height=200 width=200>", null, "text/html");
                //LinkedResource lr = new LinkedResource(email.QrCodeFileName, MediaTypeNames.Image.Jpeg);
                //lr.ContentId = "imgpath";
                //imgView.LinkedResources.Add(lr);
                //mail.AlternateViews.Add(imgView);
                //mail.Body = lr.ContentId + email.Footer.ToString();


                var client = new SmtpClient
                {
                    Host = _credential.Host,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(_credential.Username, _credential.Password, _credential.Domain)
                };
                client.Send(mail);

                return new StatusModels()
                {
                    Flag = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                //Implement your own error handling

                return new StatusModels()
                {
                    Flag = false,
                    Message = "Error: " + ex.Message
                };
            }
        }

        public StatusModels SendMessage(EmailModels email)
        {
            try
            {
                var mail = new MailMessage
                {
                    //From = new MailAddress("iTRACKAutoReply@vxi.com")
                    From = new MailAddress(email.Address)
                };

                //mail.To is your recipient(s) ex. rudnan.chavez@vxi.com
                foreach (string t in email.To)
                {
                    mail.To.Add(t);
                }

                //mail.Cc is your carbon copy recipient(s) ex. ramon.santos@vxi.com
                foreach (string t in email.Cc)
                {
                    mail.CC.Add(t);
                }

                //Put your own email subject here
                //mail.Subject = "iTRACK Online Work Request";
                mail.Subject = email.Subject;

                //Put body or message of email here
                mail.Body = Style + email.Header.ToString() + email.Body.ToString();
                mail.IsBodyHtml = true;


                //AlternateView imgView = AlternateView.CreateAlternateViewFromString(mail.Body + "<br/><img src=cid:imgpath height=200 width=200>", null, "text/html");
                //LinkedResource lr = new LinkedResource(email.QrCodeFileName, MediaTypeNames.Image.Jpeg);
                //lr.ContentId = "imgpath";
                //imgView.LinkedResources.Add(lr);
                //mail.AlternateViews.Add(imgView);
                //mail.Body = lr.ContentId + email.Footer.ToString();


                var client = new SmtpClient
                {
                    Host = _credential.Host,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(_credential.Username, _credential.Password, _credential.Domain)
                };
                client.Send(mail);

                return new StatusModels()
                {
                    Flag = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                //Implement your own error handling

                return new StatusModels()
                {
                    Flag = false,
                    Message = "Error: " + ex.Message
                };
            }
        }
    }
}