using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class BusinessRuleSearchResultDTO
    {
        public string UserName { get; set; }
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BookedRoomTypeCode { get; set; }
        public string UpgradeRoomTypeCode { get; set; }
        public string RoomTypeCodeUpgradeDescription { get; set; }
        public string RoomLongDescription { get; set; }
        public string RateTypeBooked { get; set; }

        public decimal? UpgradeCost { get; set; }
        public bool ActiveYN { get; set; }
        public int BusinessRuleID { get; set; }
        public string UpgradePriceBy { get; set; }
        public string UpgradeWeekDayWeekEnd { get; set; }
        public string DateFormat { get; set; }
        public string CurrencySymbol { get; set; }
        public int? Groupid { get; set; }
    }
}
