using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class ReportSearchResultDTO
    {
        public string HotelCode { get; set; }
        public string ReservationID { get; set; }
        //public DateTime? OXIDateSent { get; set; }
        //public string OXITransactioncode { get; set; }
        public string GuestName { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal? UpgradeCost { get; set; }
        public string BookedRoomType { get; set; }
        public string UpgradeRoomType { get; set; }
        public string UpgradeStatus { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public int NightsOfStay { get; set; }
        public string ChargePerNight { get; set; }
        public decimal? IncTotalAmountForStay { get; set; }
        public string DateFormat { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
