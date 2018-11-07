using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IEmailGenerateService
    {
        string GetEmailBody();
        string GetSubject();
        string GetGlobalEmailSubject(string hotelCode);
        string GetGlobalEmailBody(string hotelCode, string loginConfirmationNum, string confirmationNum);
        string GetGlobalEmailBccRecipients(string hotelCode);
    }
}
