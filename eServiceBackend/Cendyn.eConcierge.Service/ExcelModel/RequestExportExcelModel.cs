using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.Extension;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class RequestExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string HotelCode { get; set; }
        [Column(Index = 1, Title = "First Name")]
        public string FirstName { get; set; }

        [Column(Index = 2, Title = "Last Name")]
        public string LastName { get; set; }

        [Column(Index = 3, Title = "Date Submitted")]
        public string DateSubmitted { get; set; }

        [Column(Index = 4, Title = "Email")]
        public string Email { get; set; }

        [Column(Index = 5, Title = "Reservation")]
        public string Reservation { get; set; }

        [Column(Index = 6, Title = "Arrival Date")]
        public string ArrivalDate { get; set; }

        [Column(Index = 7, Title = "Departure Date")]
        public string DepartureDate { get; set; }

        [Column(Index = 8, Title = "Booked Code")]
        public string BookedRoomTypeCode { get; set; }

        [Column(Index = 9, Title = "Upgraded Code")]
        public string UpgradeRoomTypeCode { get; set; }

        [Column(Index = 10, Title = "No. of Nights")]
        public int NumberOfNights { get; set; }

        [Column(Index = 11, Title = "Cost")]
        public string UpgradeCost { get; set; }

        [Column(Index = 12, Title = "Total Upgrade")]
        public string IncTotalAmountForStay { get; set; }

        [Column(Index = 13, Title = "Request Status")]
        public string RequestStatus { get; set; }

        [Column(Index = 14, Title = "Previous Requests")]
        public int PreviousRequests { get; set; }

        [Column(Index = 15, Title = "Confirmed Requests")]
        public int ConfirmedRequests { get; set; }

        [Column(Index = 16, Title = "LastEmailSentDate")]
        public string LastEmailSentDate { get; set; }
        public bool CheckInventoryYN { get; set; }
        public bool AutoRequestProcessYN { get; set; }
        public bool AutoConfirmEmailYN { get; set; }
    }
}
