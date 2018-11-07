using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class ReportExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string HotelCode { get; set; }

        [Column(Index = 1, Title = "Reservation")]
        public string ReservationID { get; set; }

        [Column(Index = 2, Title = "Guest Name")]
        public string GuestName { get; set; }

        [Column(Index = 3, Title = "Email")]
        public string Email { get; set; }

        [Column(Index = 4, Title = "Arrival Date")]
        public string ArrivalDate { get; set; }

        [Column(Index = 5, Title = "Date Confirmed")]
        public string ConfirmedDate { get; set; }

        [Column(Index = 6, Title = "Booked Code")]
        public string BookedRoomType { get; set; }

        [Column(Index = 7, Title = "Upgrade Code")]
        public string UpgradeRoomType { get; set; }

        [Column(Index = 8, Title = "Confirmed or Denied Status")]
        public string UpgradeStatus { get; set; }

        [Column(Index = 9, Title = "Cost")]
        public string UpgradeCost { get; set; }

        [Column(Index = 10, Title = "Nights")]
        public int NightsOfStay { get; set; }

        [Column(Index = 11, Title = "Upgrade Total")]
        public string IncTotalAmountForStay { get; set; }
    }
}
