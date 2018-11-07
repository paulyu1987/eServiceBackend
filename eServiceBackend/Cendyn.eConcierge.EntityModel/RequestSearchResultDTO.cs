using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RequestSearchResultDTO
    {
        public int RequestID { get; set; }
        public string VIPCode { get; set; }
        public string MembershipID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string Email { get; set; }
        public string Reservation { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string BookedRoomTypeCode { get; set; }
        public string UpgradeRoomTypeCode { get; set; }
        public decimal? UpgradeCost { get; set; }
        public string RequestStatus { get; set; }
        public bool ConfirmStatus { get; set; }
        public string HotelCode { get; set; }
        public int? TotalRooms { get; set; }
        public short? Threshold { get; set; }
        public int? Committed { get; set; }
        public int? LifetimeSpend { get; set; }
        public int? NumberOfNights { get; set; }
        public bool ShowSendEmail { get; set; }
        public DateTime? LastEmailSentDate { get; set; }
        public string ChargePerNight { get; set; }
        public decimal? IncTotalAmountForStay { get; set; }
        public int PreviousRequests { get; set; }
        public int ConfirmedRequests { get; set; }
        public string DateFormat { get; set; }
        public bool CheckInventoryYN { get; set; }
        public bool AutoRequestProcessYN {get; set;}
        public bool AutoConfirmEmailYN { get; set; }
        public string EmailStatus
        {
            get
            {
                if (LastEmailSentDate != null && LastEmailSentDate.ToString() != "")
                {
                    return "Sent";
                }
                else
                {
                    return "Not Sent";
                }
            }
        }
        public string CurrencySymbol { get; set; }
        public string SendEmailYN { get; set; }
    }
}
