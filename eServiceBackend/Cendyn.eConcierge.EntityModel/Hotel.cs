//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cendyn.eConcierge.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hotel
    {
        public int ID { get; set; }
        public bool Live { get; set; }
        public Nullable<bool> International { get; set; }
        public Nullable<bool> locked { get; set; }
        public Nullable<bool> ReadyForQA { get; set; }
        public string Hotel_Code { get; set; }
        public string LaunchDate { get; set; }
        public Nullable<int> HotelBrandID { get; set; }
        public string Hotel_Name { get; set; }
        public string Hotel_Address { get; set; }
        public string Hotel_City { get; set; }
        public string Hotel_Zip { get; set; }
        public string Hotel_State { get; set; }
        public string Hotel_Country { get; set; }
        public string Hotel_Phone { get; set; }
        public string Hotel_Fax { get; set; }
        public string DomainName { get; set; }
        public string HotelConcierge_Email { get; set; }
        public string Hotel_WebSite { get; set; }
        public string Hotel_Map { get; set; }
        public string Hotel_Direction { get; set; }
        public string RoomView { get; set; }
        public string GatekeeperEmail { get; set; }
        public string Hotel5pmEmail { get; set; }
        public string Hotel5pmEmailGreeting { get; set; }
        public Nullable<double> Hotel5pmTime { get; set; }
        public Nullable<System.DateTime> Hotel5pmDT { get; set; }
        public Nullable<bool> Hotel5pmEmail_IsSent { get; set; }
        public string CutoffTime { get; set; }
        public string ImgFolder { get; set; }
        public string EmailSignature { get; set; }
        public string EmailSignatureManager { get; set; }
        public string EmailSignatureEmail { get; set; }
        public string LeftBGColor { get; set; }
        public string ItineraryGreeting { get; set; }
        public string ReminderEmailGreeting { get; set; }
        public string NewEmailGreeting { get; set; }
        public string STPEmailGreeting { get; set; }
        public string DTPEmailGreeting { get; set; }
        public string ITPEmailGreeting { get; set; }
        public string CancelEmailGreeting { get; set; }
        public string Call800 { get; set; }
        public string Interface { get; set; }
        public Nullable<bool> UsingEConcierge { get; set; }
        public string ConfirmationFrom { get; set; }
        public string HTMLTemplateName { get; set; }
        public string HTMLTemplateName_Reminder { get; set; }
        public string NEConTxtTemplateName { get; set; }
        public string TxtTemplateName_Reminder { get; set; }
        public string NEConTxtGreetingNew { get; set; }
        public string NEConTxtGreetingCHG { get; set; }
        public string NEConTxtGreetingCXL { get; set; }
        public string TxtGreetingReminder { get; set; }
        public Nullable<bool> SendInternalConfirmationBCC { get; set; }
        public string SpecialistPhone { get; set; }
        public Nullable<System.DateTime> DateSubmitted { get; set; }
        public Nullable<int> CendynAdminCompanyID { get; set; }
        public string STA { get; set; }
        public Nullable<int> ReminderDays { get; set; }
        public string CancellationPolicy { get; set; }
        public string AdditionalFeeText { get; set; }
        public string SpecialOfferLink { get; set; }
        public string Signature_Cancel { get; set; }
        public string SignatureManager_Cancel { get; set; }
        public string SignatureEmail_Cancel { get; set; }
        public string ReservationEmail { get; set; }
        public string EmailFooterLink { get; set; }
        public string RqstMsg24hrRule { get; set; }
        public string HTMLTemplateName_Itinerary { get; set; }
        public string lockDate { get; set; }
        public bool ProfileNewspaper { get; set; }
        public bool IsRateTypeString { get; set; }
        public bool HasMultiCancelPolicy { get; set; }
        public bool ShowRoomType { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationComments { get; set; }
        public Nullable<bool> seasonal { get; set; }
        public string LocalCurrencyCode { get; set; }
        public Nullable<decimal> USD_to_Rate { get; set; }
        public string MasterPropertyCode { get; set; }
        public string GuestProfileDomain { get; set; }
        public string eC2HeaderColor { get; set; }
        public string eC2FooterColor { get; set; }
        public string BookNowUrl { get; set; }
        public string BrandCode { get; set; }
        public string ChainCode { get; set; }
        public string CendynPropertyId { get; set; }
        public string CenResServiceUrl { get; set; }
        public string CenResServiceInterface { get; set; }
        public string DateFormat { get; set; }
        public bool AutoRequestProcessYN { get; set; }
        public bool AutoConfirmEmailYN { get; set; }
        public bool CentralResNumYN { get; set; }
    }
}
