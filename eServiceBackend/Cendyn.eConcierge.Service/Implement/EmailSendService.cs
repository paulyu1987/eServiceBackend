using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;
using System.Web.Configuration;
using System.Net.Mime;

namespace Cendyn.eConcierge.Service.Implement
{
    public class EmailSendService : ServiceBase, IEmailSendService
    {
        public async Task<bool> SendMailAsync(SendEmailDTO emailDTO)
        {
            return await Task.Run(() => Send(emailDTO));
        }

        public bool Send(SendEmailDTO emailDTO)
        {
            SmtpClient smtpServer;
            MailMessage mailMessage;
            if (string.IsNullOrWhiteSpace(emailDTO.ToUserEmail))
            {
                throw new ArgumentNullException("ToUsreEmail");
            }

            if (string.IsNullOrWhiteSpace(emailDTO.EmailBodyText) && string.IsNullOrWhiteSpace(emailDTO.EmailBodyHtml))
            {
                throw new ArgumentNullException("EmailBodyText & EmailBodyHtml");
            }

            //Create SMTP client, the configration is in the web.config file, system.net/mailSettings/smtp
            smtpServer = new SmtpClient();
            //smtpServer.Host = WebConfigurationManager.AppSettings["SmtpHost"].ToString();

            //Generate Email Message based on passed in informatin
            mailMessage = new MailMessage();
            mailMessage.BodyTransferEncoding = TransferEncoding.Base64;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;

            //If From Email is not , then setup the from email, else use the one from web.config system.net/mailSettings/smtp/from
            if (!string.IsNullOrWhiteSpace(emailDTO.FromUserEmail))
            {
                mailMessage.From = new MailAddress(emailDTO.FromUserEmail, emailDTO.FromUserDisplayName);
            }

            //To email
            if (!string.IsNullOrWhiteSpace(emailDTO.ToUserEmail))
            {
                mailMessage.To.Add(new MailAddress(emailDTO.ToUserEmail, emailDTO.ToUserDisplayName));
            }

            //if we have more recepins, add it here
            if (emailDTO.ToUsers != null && emailDTO.ToUsers.Count() > 0)
            {
                foreach (var item in emailDTO.ToUsers)
                {
                    mailMessage.To.Add(new MailAddress(item.Key, item.Value));
                }
            }

            //Add BCC emails
            if (!string.IsNullOrWhiteSpace(emailDTO.BccEmails))
                mailMessage.Bcc.Add(emailDTO.BccEmails);


            //Priority
            mailMessage.Priority = (MailPriority)emailDTO.MailPriority;

            //Email Subject
            if (!string.IsNullOrWhiteSpace(emailDTO.EmailSubject))
            {
                mailMessage.Subject = emailDTO.EmailSubject;
            }

            //Email Body
            //Text
            mailMessage.Body = emailDTO.EmailBodyText;

            //if contains HTML body , will use html body
            if (!string.IsNullOrWhiteSpace(emailDTO.EmailBodyHtml))
            {
                //mailMessage.Body = emailDTO.EmailBodyHtml;
                mailMessage.IsBodyHtml = true;

                AlternateView htmlBody = AlternateView.CreateAlternateViewFromString(emailDTO.EmailBodyHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
                
                //Attach image
                if (emailDTO.ListLinkedResource != null && emailDTO.ListLinkedResource.Count() > 0)
                {
                    foreach(var res in emailDTO.ListLinkedResource)
                    {
                        if (res != null) htmlBody.LinkedResources.Add(res);
                    }   
                }
                mailMessage.AlternateViews.Add(htmlBody);
            }


           smtpServer.Send(mailMessage);

            return true;
        }

    }
}
