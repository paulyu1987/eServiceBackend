using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RequestNotificationEmailSendDTO
    {
        public int RequestID { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string PropertyManagerName { get; set; }
    }
}
