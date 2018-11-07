using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IReservationService
    {
        IList<FGuest> GetCancelledReservations();
        //FGuest GetFGuestByConfirmationNum(string confirmationNum, string hotelCode);
        FGuest GetFGuestByLoginConfirmationNum(string logConfirmationNum, string hotelCode);
        FGuest GetFGuestByLogin(string UserID, string Password, string hotelCode);
        //FGuest AuthenicateUser(string UserID, string Password, string confirmationNum, string hotelCode, string logConfirmationNum);
        //FGuest GetAuthenicateUser(string UserID, string Password, string hotelCode, string logConfirmationNum);
        
    }
}
