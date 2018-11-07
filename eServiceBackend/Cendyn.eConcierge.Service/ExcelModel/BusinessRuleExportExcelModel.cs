using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class BusinessRuleExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string HotelCode { get; set; }

        [Column(Index = 1, Title = "Booked Code")]
        public string BookedRoomTypeCode { get; set; }

        [Column(Index = 2, Title = "Upgrade Price By")]
        public string UpgradePriceBy { get; set; }

        [Column(Index = 3, Title = "Start Date")]
        public string StartDate { get; set; }

        [Column(Index = 4, Title = "End Date")]
        public string EndDate { get; set; }

        //[Column(Index = 5, Title = "Weekday/Weekend")]
        //public string UpgradeWeekDayWeekEnd { get; set; }

        [Column(Index = 5, Title = "Upgrade Code")]
        public string upgradeRoomTypeCode { get; set; }

        [Column(Index = 6, Title = "Rate Type")]
        public string RateTypeBooked { get; set; }

        [Column(Index = 7, Title = "Upgrade Cost")]
        public string UpgradeCost { get; set; }

        [Column(Index = 8, Title = "Active Y/N")]
        public bool ActiveYN { get; set; }
    }
}
