using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class NewBusinessRuleSaveDTO
    {
        public int BusinessRuleID { get; set; }
        public string HotelCode { get; set; }
        public string BookedRoomTypeCode { get; set; }
        public string UpgradeRoomTypeCode { get; set; }
        public string RateTypeBooked { get; set; }
        public string UpgradeCost { get; set; }
        public string UserName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UpgradeWeekDayWeekEnd { get; set; }
        public string UpgradePricedBy { get; set; }
    }
}
