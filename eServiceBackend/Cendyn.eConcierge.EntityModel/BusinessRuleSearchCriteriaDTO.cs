using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class BusinessRuleSearchCriteriaDTO
    {
        public string UserName { get; set; }
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public DateTime? DateRequested { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BookedRoomTypeCode { get; set; }
        public string UpgradeRoomTypeCode { get; set; }
        public string RoomTypeCodeUpgradeDescription { get; set; }
        public string RoomLongDescription { get; set; }
        public decimal? UpgradeCost { get; set; }
        //public string UpgradeCost_Display { get { return UpgradeCost.ToString("#,##0.00"); } }
    }
}
