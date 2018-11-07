using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Core;
using Castle.Core;
using Cendyn.eConcierge.Core.Helper;

namespace Cendyn.eConcierge.Service.Implement
{
    public class RequestConfirmEmailService : IEmailGenerateService
    {
        public ISettingService SettingService { get; set; }
        public IGuestPlannerService GuestPlannerService { get; set; }
        public IHotelService HotelService { get; set; }
        public IHotelSettingService hotelSettingService { get; set; }
        public IGuestPlannerRepository guestPlannerRepo { get; set; }
        public int requestID { get; set; }

        string subject = string.Empty;
        GuestPlanner gp = null;


        public string GetEmailBody()
        {
            //Email Boday
            var emailBody = "";
            var textContext = "";

            var emailBody_grant = "We are pleased to let you know that your requested upgrade has been granted for the defined additional rates: \n[Content]";
            var emailBody_deny = "We regret to inform you that your requested upgrade has been denied for the defined additional rates: \n[Content]";
            
            //TODO: change this call to constructor
            gp = GuestPlannerService.GetGuestPlannerByID(requestID);

            if (gp.Status == "Confirmed")
            {
                var setting_grantEmailBody = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeGrantEmailBody");
                if (!string.IsNullOrWhiteSpace(setting_grantEmailBody))
                {
                    emailBody_grant = setting_grantEmailBody;
                }

                //Requests
                var requests_grant = GuestPlannerService.GetGrantedRequestForEmail(gp.LoginConfirmationNum);

                foreach (var item in requests_grant)
                {
                    string perNight = "";
                    if (!string.IsNullOrWhiteSpace(item.PerNightCharge) && item.PerNightCharge.Equals("Y"))
                    {
                        perNight = "per night";
                    }
                    //textContext += string.Format("{0}, for ${1} {2}\n", item.UpgradedRoomType, (item.UpgradeCost ?? 0).ToString("#,##0.00"), string.IsNullOrWhiteSpace(item.PerNightDesc) ? "" : "per night");
                    textContext += string.Format("{0}, for {1} {2}\n", item.UpgradedRoomType, item.CurrencySymbol+(item.UpgradeCost ?? 0).ToString("#,##0.00"), perNight);
                }
                emailBody = emailBody_grant.Replace("[Content]", textContext);
            }
            else if (gp.Status == "Denied")
            {
                var setting_denialEmailBody = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeDenialEmailBody");
                if (!string.IsNullOrWhiteSpace(setting_denialEmailBody))
                {
                    emailBody_deny = setting_denialEmailBody;
                }

                //Requests
                var requests_deny = GuestPlannerService.GetDeniedRequestForEmail(gp.LoginConfirmationNum);

                foreach (var item in requests_deny)
                {
                    string perNight = "";
                    if (!string.IsNullOrWhiteSpace(item.PerNightCharge) && item.PerNightCharge.Equals("Y"))
                    {
                        perNight = "per night";
                    }
                    //textContext += string.Format("{0}, for ${1} {2}\n", item.UpgradedRoomType, (item.UpgradeCost ?? 0).ToString("#,##0.00"), string.IsNullOrWhiteSpace(item.PerNightCharge) ? "" : "per night");
                    textContext += string.Format("{0}, for {1} {2}\n", item.UpgradedRoomType, item.CurrencySymbol + (item.UpgradeCost ?? 0).ToString("#,##0.00"), perNight);
                }
                emailBody = emailBody_deny.Replace("[Content]", textContext);
            }            

            return emailBody;
        }


        public string GetSubject()
        {
            //Get Subject from setting
            subject = SettingService.GetSettingValueByKey("BackEnd_RequestEmail_Subject");
            gp = GuestPlannerService.GetGuestPlannerByID(requestID);
            Hotel hotelInfo = HotelService.GetHotelByCode(gp.Hotel_Code);

            if (string.IsNullOrWhiteSpace(subject))
            {

                //subject = "Requests - " + gp.ConfirmationNum + " - " 
                //    + (gp.EventDate.HasValue ? gp.EventDate.Value.ToShortDateString() : string.Empty) + " - " + gp.Event;

                subject = "Requests - " + gp.ConfirmationNum + " - " 
                    + (gp.EventDate.HasValue ? gp.EventDate.Value.ToString(hotelInfo.DateFormat.ToLower().Replace("m","M")) : string.Empty) + " - " + gp.Event;
            }
            else
            {
                //[Property Name] - [Category] - [Date] - [Confirmation #]
                subject = subject.Replace(SettingPlaceHolder.RequestConfirmEmailHoder.PROPERTY_NAME.ToString(), hotelInfo.Hotel_Name);
                subject = subject.Replace(SettingPlaceHolder.RequestConfirmEmailHoder.CATEGORY.ToString(), gp.Event);
                subject = subject.Replace(SettingPlaceHolder.RequestConfirmEmailHoder.DATE.ToString(), gp.EventDate.HasValue ? gp.EventDate.Value.ToShortDateString() : string.Empty);
                subject = subject.Replace(SettingPlaceHolder.RequestConfirmEmailHoder.CONFIRMATION_POUND_SIGN.ToString(), gp.ConfirmationNum);
            }

            return subject;
        }

        public string GetGlobalEmailSubject(string hotelCode)
        {
            subject = hotelSettingService.GetHotelSettingValue(hotelCode, "UpgradeGlobalEmailSubject");
            Hotel hotelInfo = HotelService.GetHotelByCode(hotelCode);

            if (!string.IsNullOrWhiteSpace(subject) && subject.Contains("|HotelName|"))
            {
                subject = subject.Replace("|HotelName|", hotelInfo.Hotel_Name);
            }
            else
            {
                subject = hotelInfo.Hotel_Name + " Upgrade Request Status";
            }

            return subject;
        }

        public string GetGlobalEmailBody(string hotelCode, string loginConfirmationNum, string confirmationNum)
        {
            var textContext = "";

            var emailBody = "Please see below for the status of your upgrade request(s): \r\n\r\n[Content]";

            var setting_globalEmailBody = hotelSettingService.GetHotelSettingValue(hotelCode, "UpgradeGlobalEmailBody");
            if (!string.IsNullOrWhiteSpace(setting_globalEmailBody))
            {
                setting_globalEmailBody = setting_globalEmailBody.Replace("[Content]", "\r\n\r\n[Content]");
                emailBody = setting_globalEmailBody;
            }

            //Requests are confirmaed
            var requests_grant = GuestPlannerService.GetGrantedRequestForEmail(loginConfirmationNum);
            if (null != requests_grant && requests_grant.Count > 0)
            {
                foreach (var item in requests_grant)
                {
                    string perNight = "";
                    if (!string.IsNullOrWhiteSpace(item.PerNightCharge) && item.PerNightCharge.Equals("Y"))
                    {
                        perNight = "per night";
                    }
                    textContext += string.Format("{0}, for {1} {2} (Confirmed)\n", item.UpgradedRoomType, item.CurrencySymbol + (item.UpgradeCost ?? 0).ToString("#,##0.00"), perNight);
                }
            }

            //Requests are confirmaed
            var requests_deny = GuestPlannerService.GetDeniedRequestForEmail(loginConfirmationNum);
            if (null != requests_deny && requests_deny.Count > 0)
            {
                foreach (var item in requests_deny)
                {
                    string perNight = "";
                    if (!string.IsNullOrWhiteSpace(item.PerNightCharge) && item.PerNightCharge.Equals("Y"))
                    {
                        perNight = "per night";
                    }
                    textContext += string.Format("{0}, for {1} {2} (Denied)\n", item.UpgradedRoomType, item.CurrencySymbol + (item.UpgradeCost ?? 0).ToString("#,##0.00"), perNight);
                }

            }

            textContext += "\r\nReservation Number: " + confirmationNum;

            emailBody = emailBody.Replace("[Content]", textContext);
            return emailBody;
        }

        public string GetGlobalEmailBccRecipients(string hotelCode)
        {
            string returnEmails = "";
            StringBuilder adminEmails = new StringBuilder();
            string spName = "eUpgrade_Email_GetGlobalEmailBccRecipients";
            var pList = new Dictionary<string, object>(){
                {"@hotelCode",hotelCode}
            };

            List<string> bccEmailList = guestPlannerRepo.ExecuteStoredProcedure<string>(spName, pList);
            if (null != bccEmailList && bccEmailList.Count > 0)
            {
                foreach (var email in bccEmailList)
                {
                    if (!string.IsNullOrWhiteSpace(email) && ValidateHelper.IsEmail(email))
                        adminEmails.Append(email).Append(",");
                }
            }
            if (adminEmails.Length > 0)
                returnEmails = adminEmails.ToString().Trim(',');
            return returnEmails;
        }
    }
}
