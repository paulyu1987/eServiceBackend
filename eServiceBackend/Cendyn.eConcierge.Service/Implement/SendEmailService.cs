using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Core;

namespace Cendyn.eConcierge.Service.Implement
{
    public class SendEmailService : ServiceBase
    {
        public IGuestPlannerRepository guestPlannerRepo { get; set; }

        public IFGuestRepository fguestRepo { get; set; }

        public IHotelRepository hotelRepo { get; set; }

        /// <summary>
        /// send email
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public string SendEmail(string requestId)
        {
            var query = from gpr in guestPlannerRepo.GetAll().Where(x => x.RequestID == requestId)
                        join h in hotelRepo.GetAll() on gpr.Hotel_Code equals h.Hotel_Code
                        join r in fguestRepo.GetAll() on gpr.LoginConfirmationNum equals r.LoginConfirmationNum
                        select new
                        {
                            gpr,
                            h,
                            r
                        };

            //Get column that we need
            //var result = query.Select(x => new List<string>()
            //{
            //    string email = x.r
            //});
            

            return "";
        }

    }
}
