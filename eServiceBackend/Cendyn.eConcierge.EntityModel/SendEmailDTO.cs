using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class SendEmailDTO
    {
        private int _priority = 1;

        public string FromUserEmail { get; set; }
        public string FromUserDisplayName { get; set; }
        public string ToUserEmail { get; set; }
        public string ToUserDisplayName { get; set; }
        public string EmailSubject { get; set; }
        public List<LinkedResource> ListLinkedResource { get; set; }

        /// <summary>
        /// Html body for the email, if you specified html body, the text body will be ignore.
        /// </summary>
        public string EmailBodyHtml { get; set; }
        public string EmailBodyText { get; set; }

        public string BccEmails { get; set; }

        /// <summary>
        /// Email's Priority, 0 - Low, 1 - Mid, 2 - High. Default is 1;
        /// </summary>
        public int MailPriority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        /// <summary>
        /// The Recepints List, the format for each item is a dictionary, key is email, value is display name.
        /// </summary>
        public Dictionary<string, string> ToUsers { get; set; }
    }
}
