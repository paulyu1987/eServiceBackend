using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public partial class FGuestDTO
    {
        public int ID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string LoginConfirmationNum { get; set; }
        public string ConfirmationNum { get; set; }
        public string HOTEL_CODE { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public string Arrival_Estimated_Time { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public string GPCC_Num { get; set; }
        public string Preferences_Desc { get; set; }
        public Nullable<bool> EmailOptOut { get; set; }
        public Nullable<short> Adults_Num { get; set; }
        public Nullable<short> Children_Num { get; set; }
        public string RateType { get; set; }
        public string Rate_Plan_Text { get; set; }
        public Nullable<double> QuotedRate { get; set; }
        public string Package_Flag { get; set; }
        public string Fax { get; set; }
        public string RoomType { get; set; }
        public string RoomType_Desc { get; set; }
        public string PKG { get; set; }
        public Nullable<System.DateTime> CXL { get; set; }
        public string CXLPolicyType { get; set; }
        public Nullable<System.DateTime> ReservationDate { get; set; }
        public bool ItinerarySent { get; set; }
        public bool SevenDaysSent { get; set; }
        public Nullable<bool> SevenDaysSentQA { get; set; }
        public bool NeedToResend { get; set; }
        public bool IsConfirmSent { get; set; }
        public bool IsConfirmSentQA { get; set; }
        public bool CancellationSent { get; set; }
        public System.DateTime ImportDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public Nullable<bool> NonGuest { get; set; }
        public bool DoNotSend_Confirm { get; set; }
        public string CXLNumber { get; set; }
        public string market { get; set; }
        public Nullable<System.Guid> SourceStayID { get; set; }
        public string Departure_Estimated_Time { get; set; }
        public string eMailType { get; set; }
        public string RateSuppressCode { get; set; }
        public string eMailSuppressCode { get; set; }
        public string AcknowledgementNum { get; set; }
        public string DepPolicy { get; set; }
        public string DepReqAmt { get; set; }
        public string DepReqDate { get; set; }
        public string DepRecdAmt { get; set; }
        public string DepForfeited { get; set; }
        public string ClientStatus { get; set; }
        public string Source_Of_Business { get; set; }
        public string Group_Reservation { get; set; }
        public Nullable<short> Rooms_Num { get; set; }
        public string ClientType { get; set; }
        public int Source { get; set; }
        public string PreferredLanguage { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string ConfirmationNumOld { get; set; }
        public string LoginConfirmationNumOld { get; set; }
        public Nullable<int> CRMStayId { get; set; }
        public Nullable<int> CRMCustomerId { get; set; }
        public string CRMResStatus { get; set; }
        public bool SMSNotification { get; set; }
        public string RoomNumber { get; set; }
        public Nullable<decimal> AverageDailyRate { get; set; }
    }
}
