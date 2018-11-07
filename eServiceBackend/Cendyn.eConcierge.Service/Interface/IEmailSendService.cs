using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IEmailSendService
    {
        bool Send(SendEmailDTO emailDTO);

        Task<bool> SendMailAsync(SendEmailDTO emailDTO);
    }
}
