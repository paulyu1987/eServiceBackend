using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using RepositoryT.Infrastructure;
using Castle.Core.Logging;

namespace Cendyn.eConcierge.Service.Implement
{
    public class ReservationService : ServiceBase, IReservationService
    {
        public IFGuestRepository fgRepo { get; set; }
        public IHotelRepository hotelRepo { get; set;}
        public IHotelService hotelService { get; set; }

        public  IList<FGuest> GetCancelledReservations()
        {
            logger.Info(string.Format("Calling {0} function",GetCurrentClassName()));
            var flist = fgRepo.GetAll().Where(x => x.Status == "CXL").ToList();
            return flist;
        }       

        public FGuest GetFGuestByLoginConfirmationNum(string logConfirmationNum, string hotelCode)
        {
            return fgRepo.Get(x => x.LoginConfirmationNum == logConfirmationNum && x.HOTEL_CODE == hotelCode);
        }
        public FGuest GetFGuestByLogin(string UserID, string Password,  string hotelCode)       
        {
            bool CentralResNumYN = hotelRepo.Get(p => p.Hotel_Code == hotelCode && p.Live == true).CentralResNumYN;
            FGuest guest;

            if (CentralResNumYN)
            {             
                guest = fgRepo.Get(x => x.LName == UserID && (x.CentralResNum !="" && x.CentralResNum!= null && x.CentralResNum !="''") && x.CentralResNum == Password && x.HOTEL_CODE == hotelCode);
                if (null == guest)
                {
                    guest = fgRepo.Get(x => x.LName == UserID && (x.CentralResNum == "" || x.CentralResNum == null || x.CentralResNum=="''") && x.ConfirmationNum == Password && x.HOTEL_CODE == hotelCode);
                }
            }
            else
            {
                guest = fgRepo.Get(x => x.LName == UserID && x.ConfirmationNum == Password && x.HOTEL_CODE == hotelCode);
            }
            return guest;
        }
    }
}
