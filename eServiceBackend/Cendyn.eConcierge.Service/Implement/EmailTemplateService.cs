using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Implement
{
    public class EmailTemplateService : ServiceBase, IEmailTemplateService
    {
        public IEmailTemplateRepository emailTemplateRepo { get; set; }

        public bool AddNewEmailTemplate(EmailTemplate emailTemplate)
        {
            try
            {
                emailTemplateRepo.Add(emailTemplate);
                unitOfWork.Commit();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }

}
