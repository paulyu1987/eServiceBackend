using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.CenResModel;
using AutoMapper;

namespace Cendyn.eConcierge.Service.Implement
{
    public class PlannerEventService : ServiceBase, IPlannerEventService
    {
        public IGuestPlannerService guestPlannerService { get; set; }
        public IRoomType_CodeRepository roomTypeRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }
        public IRoomImageRepository imageRepo { get; set; }
        public IReservationService reservationService { get; set; }
        public ICenResService cenResService { get; set; }
        public IHotelService hotelService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IMapper mapper { get; set; }
        public IEUpgradeRequestRepository upgradeRequestRepo { get; set; }
        public IHotels_LanguagesRepository hotelLangRepo { get; set; }

        public IEUpgradeTransactionCodeRepository transactionCodeRepo { get; set; }

        public RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode)
        {
            return roomTypeRepo.Get(x => x.Hotel_Code == hotelCode && x.RoomCode == roomCode);
        }

        public RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode, int languageID)
        {
            return roomTypeRepo.GetRoomTypeCode(hotelCode, roomCode, languageID);
        }

        public List<RoomImage> GetRoomTypeImages(string hotelCode, string roomCode)
        {
            return imageRepo.GetAll().Where(x => x.HotelCode == hotelCode && x.RoomCode == roomCode).OrderBy(x=>x.SortOrder).ToList();
        }

        public List<eUpgradeOptionDTO> GetEventDetail(string hotelCode, string loginComfirmationNum, string roomtype, string ratetype, DateTime? guestArrivaldate, DateTime? guestDeparturedate, int languageId, bool multiLanguageEnabled)
        {
            
            string eventCategory = "RoomUpgrade";
            int serviceTemplateID = 3;
            int languageID = languageId;
            string spName = "eUpgrade_GetEventDetail";

            if (!roomTypeRepo.IsStoredProcedureExists(spName) || !multiLanguageEnabled)
            {
                spName = "spGetEventDetail";
                languageID = 1;
            }

            DateTime? arrivaldate = null;
            arrivaldate = guestArrivaldate;
            DateTime? departuredate = null;
            departuredate = guestDeparturedate;
            var pList = new Dictionary<string,object>(){
                {"@eventCategory",eventCategory},
                {"@hotelCode",hotelCode},
                {"@loginConfirmationNum",loginComfirmationNum},
                {"@serviceTemplateID",serviceTemplateID},
                {"@languageID",languageID},
                {"@ArrivalDate",arrivaldate},
                {"@RoomCode",string.IsNullOrWhiteSpace(roomtype) ? string.Empty : roomtype},
                {"@RateType",string.IsNullOrWhiteSpace(ratetype) ? string.Empty : ratetype}
            };

            // Store options that comply to business rule but haven't done availability check
            
            List<eUpgradeOptionDTO> eUpgradeOptionsRaw = roomTypeRepo.ExecuteStoredProcedure<eUpgradeOptionDTO>(spName, pList);
            // Store final options
            List<eUpgradeOptionDTO> eUpgradeOptions = new List<eUpgradeOptionDTO>();

            if(eUpgradeOptionsRaw == null || eUpgradeOptionsRaw.Count == 0)
                return eUpgradeOptionsRaw;

            var hotel = hotelService.GetHotelByCode(hotelCode);

            if (string.IsNullOrWhiteSpace(hotel.CendynPropertyId) || string.IsNullOrEmpty(hotel.CenResServiceUrl) || string.IsNullOrEmpty(hotel.CenResServiceInterface))
            {
                // If we do not have a Available API parameters, resume the old logic
                foreach (var it in eUpgradeOptionsRaw)
                {
                    it.RoomTypeImages = GetRoomTypeImages(hotelCode, it.EventName);
                    it.OptionPrices = GetOptionPrices(it.ID.Value);
                }
                return eUpgradeOptionsRaw;
            }
            else
            {
                #region check Roon availability
                List<string> roomTypes = new List<string>();

                //Only chack rooms availability
                foreach (eUpgradeOptionDTO euo in eUpgradeOptionsRaw)
                {
                    // If this is an addon, it does not need to check availability
                    var addon = GetRoomTypeCode(hotelCode, euo.EventName).AddOnYN.ToString();

                    if (addon == "Y")
                    {                      
                        euo.RoomTypeImages = GetRoomTypeImages(hotelCode, euo.EventName);
                        eUpgradeOptions.Add(euo);
                    }
                    else if (addon == "N")
                    {
                        roomTypes.Add(euo.EventName);
                    }
                }

                CenResParamsDTO cenResParams = new CenResParamsDTO() { ServiceUrl = hotel.CenResServiceUrl, Interface = hotel.CenResServiceInterface, CendynPropertyId = hotel.CendynPropertyId, };

                //new logic --2 ways

                List<RoomInfo> roomInfo = cenResService.GetRoomInfos(roomTypes.ToArray(), (DateTime)arrivaldate, (DateTime)departuredate, cenResParams);
                if (null != roomInfo && roomInfo.Count > 0)
                {
                    //check Threshold 
                    foreach (var option in eUpgradeOptionsRaw)
                    {
                        List<bool> okInDateRange = new List<bool>();
                        for (DateTime day = (DateTime)arrivaldate; day < (DateTime)departuredate; day = day.AddDays(1))
                        {
                            bool oneDayOK = false;
                            foreach (var room in roomInfo)
                            {
                                if (option.EventName == room.RoomType &&
                                    ((option.Threshold.HasValue && option.Threshold < room.AvailableRooms) || option.Threshold.HasValue == false && room.AvailableRooms >= 1) &&
                                    day == room.Date)
                                {
                                    oneDayOK = true;
                                }
                            }
                            okInDateRange.Add(oneDayOK);
                        }

                        if (okInDateRange.Contains(false) == false)
                        {
                            option.RoomTypeImages = GetRoomTypeImages(hotelCode, option.EventName);
                            eUpgradeOptions.Add(option);
                        }
                    }
                }

                return eUpgradeOptions;
                #endregion 
            }

        }

        public List<RoomInfo> GetRoomEventDetail(string hotelCode, string loginComfirmationNum, string roomtype, string ratetype, DateTime? guestArrivaldate, DateTime? guestDeparturedate)
        {

            string eventCategory = "RoomUpgrade";
            int serviceTemplateID = 3;
            int languageID = 1;
            string spName = "spGetEventDetail";
            DateTime? arrivaldate = null;
            arrivaldate = guestArrivaldate;
            DateTime? departuredate = null;
            departuredate = guestDeparturedate;

            var pList = new Dictionary<string, object>(){
                {"@eventCategory",eventCategory},
                {"@hotelCode",hotelCode},
                {"@loginConfirmationNum",loginComfirmationNum},
                {"@serviceTemplateID",serviceTemplateID},
                {"@languageID",languageID},
                {"@ArrivalDate",arrivaldate},
                {"@RoomCode",string.IsNullOrWhiteSpace(roomtype) ? string.Empty : roomtype},
                {"@RateType",string.IsNullOrWhiteSpace(ratetype) ? string.Empty : ratetype}
            };

            // Store options that comply to business rule but haven't done availability check
            List<eUpgradeOptionDTO> eUpgradeOptionsRaw = roomTypeRepo.ExecuteStoredProcedure<eUpgradeOptionDTO>(spName, pList);
            // Store final options
            List<eUpgradeOptionDTO> eUpgradeOptions = new List<eUpgradeOptionDTO>();
            //if (eUpgradeOptionsRaw == null || eUpgradeOptionsRaw.Count == 0)
            //    return eUpgradeOptionsRaw;

            var hotel = hotelService.GetHotelByCode(hotelCode);
            // If we do not have a Availability API to call, resume the old logic
            //if (string.IsNullOrEmpty(hotel.CenResServiceUrl))
            //    return eUpgradeOptionsRaw;

            // Store roomtypes that need to check availability
            List<string> roomTypes = new List<string>();
            foreach (eUpgradeOptionDTO euo in eUpgradeOptionsRaw)
            {
                // If this is an addon, it does not need to check availability
                if (GetRoomTypeCode(hotelCode, euo.EventName).AddOnYN == "Y")
                    eUpgradeOptions.Add(euo);
                else
                    roomTypes.Add(euo.EventName);
            }

            CenResParamsDTO cenResParams = new CenResParamsDTO() { ServiceUrl = hotel.CenResServiceUrl, Interface = hotel.CenResServiceInterface, CendynPropertyId = hotel.CendynPropertyId, };

            string cendynPropertyId = hotel.CendynPropertyId;
            //cenResService = new CenResService();
            //string[] availableRooms = cenResService.GetAvailableRoomTypes(roomTypes.ToArray(), (DateTime)arrivaldate, (DateTime)departuredate, cenResParams);
            List<RoomInfo> GetRoomInfos = cenResService.GetRoomInfos(roomTypes.ToArray(), (DateTime)arrivaldate, (DateTime)departuredate, cenResParams);

            List<RoomInfo> GetRoomInfo= new List<RoomInfo>();
            //Remove RoomInfo following by threshold criteria
            foreach (var room in GetRoomInfos) 
            {
                List<bool> okInDateRange = new List<bool>();
                   bool oneRTCode = false;
                   foreach (var option in eUpgradeOptionsRaw)
                    {
                        if (option.EventName == room.RoomType &&
                            ((option.Threshold.HasValue && option.Threshold < room.AvailableRooms) || option.Threshold.HasValue == false && room.AvailableRooms >= 1))
                        {
                            oneRTCode = true;
                        }
                    }
                   okInDateRange.Add(oneRTCode);                

                if (okInDateRange.Contains(false) == false)
                {
                    GetRoomInfo.Add(room);             
                }
            }
            return GetRoomInfo;
        }

        public bool SaveRoomOptions(string loginConfirmationNum, List<string> requestIDs, string hotelCode, int languageID, bool multiLanguageEnabled, ref bool roomUnavailable,string langForSave,string currencyForSave)
        {
            bool succeed = false;
            List<int> guestPlannerIDList = new List<int>();
            List<int> eUpgradeRequestIDList = new List<int>();
            List<string> deniedList = new List<string>();

            FGuest guest = reservationService.GetFGuestByLoginConfirmationNum(loginConfirmationNum, hotelCode);
            if (null == guest || guest.Status == "CXL")
                return false;
            Hotel hotel = hotelService.GetHotelByCode(hotelCode);

            if (!string.IsNullOrWhiteSpace(hotel.CendynPropertyId) && !string.IsNullOrWhiteSpace(hotel.CenResServiceInterface)
                && !string.IsNullOrWhiteSpace(hotel.CenResServiceUrl))
            {
                #region Check Inventory before save data

                List<eUpgradeOptionDTO> OptionList = GetEventDetail(guest.HOTEL_CODE, guest.LoginConfirmationNum, guest.RoomType, guest.RateType, guest.ArrivalDate, guest.DepartureDate, languageID, multiLanguageEnabled);

                if (null != OptionList && OptionList.Count > 0)
                {
                    foreach (var it in requestIDs)
                    {
                        int id = 0;
                        Int32.TryParse(it, out id);
                        PlannerEvent pe = plannerEventRepo.GetAll().Where(x => x.ParentID == id && x.Hotel_Code == guest.HOTEL_CODE).FirstOrDefault();
                        var availableOption = OptionList.Where(x => x.EventName == pe.RoomTypeCodeUpgrade).FirstOrDefault();
                        if (availableOption == null)
                        {
                            deniedList.Add(it);

                        }
                    }


                    if (null != deniedList && deniedList.Count > 0)
                    {
                        succeed = false;
                        roomUnavailable = true;
                        return succeed;
                    }
                    else
                    {
                        //insert data 
                        foreach (var item in requestIDs)
                        {
                            succeed = InsertData(guest, item, ref guestPlannerIDList, ref eUpgradeRequestIDList, langForSave, currencyForSave);
                        }
 
                    }
                }
                else
                {
                    succeed = false;
                    roomUnavailable = true;
                    return succeed;
                }

                #endregion
            }
            else
            {
                #region  Do not check inventory
                foreach (var item in requestIDs)
                {
                    //insert data
                    succeed = InsertData(guest, item, ref guestPlannerIDList, ref eUpgradeRequestIDList, langForSave, currencyForSave);
                }

                #endregion
            }

            #region Update RequestID 
            if (null != guestPlannerIDList && guestPlannerIDList.Count > 0)
            {
                succeed = guestPlannerService.UpdatRequestIDs(guestPlannerIDList);

                //update coloumn requestID of database table 'eUpgradeRequest'
                string requestID = guestPlannerService.GetRequestID(guestPlannerIDList);
                if (!string.IsNullOrEmpty(requestID))
                {
                    if (null != eUpgradeRequestIDList && eUpgradeRequestIDList.Count > 0)
                    {

                        succeed = guestPlannerService.UpdatRequestIDsOfeUpgradeRequest(eUpgradeRequestIDList, requestID);
                    }
                }
            }
            #endregion

            #region send out request notification email
            if (succeed)
            {
                foreach (var gpid in guestPlannerIDList)
                {
                    guestPlannerService.SendRequestNotificationEmail(gpid);
                }
            }
            #endregion

            return succeed;
        }
        public bool IsBussinessRuleValid(NewBusinessRuleSaveDTO data)
        {
            if (data.UpgradePricedBy != "Arrival Date") return true;
            var list = plannerEventRepo.Get(p => 
                p.Hotel_Code == data.HotelCode 
                && p.UpgradeWeekDayWeekEnd == data.UpgradeWeekDayWeekEnd
                && p.UpgradePricedBy == "Arrival Date" 
                && p.RoomTypeCodeBooked == data.BookedRoomTypeCode 
                && p.RoomTypeCodeUpgrade == data.UpgradeRoomTypeCode
                //&& (p.DateStart <data.EndDate && p.DateEnd>data.StartDate || p.DateEnd<data.EndDate && p.DateEnd > data.StartDate)
                && p.RateTypeBooked == data.RateTypeBooked
                &&(p.DateEnd>=data.StartDate && data.EndDate>=p.DateStart)
                && p.Groupid != data.BusinessRuleID
                && p.Groupid != null
                );
            return (list==null);
        }

        public bool IsBussinessRuleValid(PlannerEvent data)
        {

            if (data.UpgradePricedBy != "Arrival Date") return true;
            var list = plannerEventRepo.Get(p =>
                p.ActiveYN == true
                && p.Hotel_Code == data.Hotel_Code
                && p.UpgradeWeekDayWeekEnd == data.UpgradeWeekDayWeekEnd
                && p.UpgradePricedBy == "Arrival Date"
                && p.RoomTypeCodeBooked == data.RoomTypeCodeBooked
                && p.RoomTypeCodeUpgrade == data.RoomTypeCodeUpgrade
                && (p.DateStart < data.DateEnd && p.DateEnd > data.DateStart || p.DateEnd < data.DateEnd && p.DateEnd > data.DateStart)
                );
            return (list == null);
        }

        public bool IsExistingBussinessRule(NewBusinessRuleSaveDTO data)
        {
            var list = (dynamic)null;
            bool isDateAll = data.UpgradePricedBy == "All";
            decimal cost = decimal.Parse(data.UpgradeCost);
            DateTime start = (data.StartDate ?? DateTime.Now).Date;
            DateTime end = (data.EndDate ?? DateTime.Now).Date;
            list = plannerEventRepo.Get(p => p.Hotel_Code == data.HotelCode
                    && p.UpgradeWeekDayWeekEnd == data.UpgradeWeekDayWeekEnd
                    && p.UpgradePricedBy == data.UpgradePricedBy
                    && p.RoomTypeCodeBooked == data.BookedRoomTypeCode
                    && p.RoomTypeCodeUpgrade == data.UpgradeRoomTypeCode
                    && p.RateTypeBooked == data.RateTypeBooked
                    && p.USDPrice == cost
                    && p.UpgradeWeekDayWeekEnd == data.UpgradeWeekDayWeekEnd
                    && (isDateAll || (DateTime.Compare(p.DateStart ?? DateTime.Now, start) == 0 && DateTime.Compare(p.DateEnd ?? DateTime.Now, end) == 0))
                    && p.Groupid != data.BusinessRuleID
                    && p.Groupid != null);
            return (list != null);
        }

        public int GetBussinessRuleGroupid(NewBusinessRuleSaveDTO data,int type)
        {
            PlannerEvent plannerEvent = new PlannerEvent();
            int groupid = 0;
            if(type == 0)
            {
                bool isDateAll = data.UpgradePricedBy == "All";
                decimal cost = decimal.Parse(data.UpgradeCost);
                DateTime start = (data.StartDate ?? DateTime.Now).Date;
                DateTime end = (data.EndDate ?? DateTime.Now).Date;
                DateTime today = DateTime.Now.Date;
                plannerEvent = plannerEventRepo.Get(p => p.Hotel_Code == data.HotelCode
                        && p.UpgradeWeekDayWeekEnd == data.UpgradeWeekDayWeekEnd
                        && p.UpgradePricedBy == data.UpgradePricedBy
                        && p.RoomTypeCodeBooked == data.BookedRoomTypeCode
                        && p.RoomTypeCodeUpgrade == data.UpgradeRoomTypeCode
                        && p.USDPrice == cost
                        && p.Groupid != null
                        && (isDateAll || (DateTime.Compare(p.DateStart ?? today, start) == 0 && DateTime.Compare(p.DateEnd ?? today, end) == 0))
                        );
                if ((plannerEvent != null))
                {
                    groupid = Convert.ToInt32(plannerEvent.Groupid);
                }
            }
            else if(type == 1)
            {
                var list = plannerEventRepo.Get(p => p.Groupid != null);
                if ((list != null))
                {
                    plannerEvent = plannerEventRepo.GetAll().OrderByDescending(p => p.Groupid).First();
                    groupid = Convert.ToInt32(plannerEvent.Groupid);
                }
            }

            return groupid;
        }

        public bool UpdateByBusinessRule(NewBusinessRuleSaveDTO data)
        {
            bool succeed = false;
            //TODO: insert new planner event
            #region insert to table PlannerEvent
            PlannerEvent plannerEvent = new PlannerEvent();
            plannerEvent.ID = data.BusinessRuleID;
            plannerEvent.ParentID = 0;
            plannerEvent.Hotel_Code = data.HotelCode;
            plannerEvent.RoomTypeCodeBooked = data.BookedRoomTypeCode;
            plannerEvent.RoomTypeCodeUpgrade = data.UpgradeRoomTypeCode;
            plannerEvent.RateTypeBooked = data.RateTypeBooked;
            plannerEvent.EventCategory = "RoomUpgrade";
            // Price and price display
            plannerEvent.USDPrice = Decimal.Parse(data.UpgradeCost);
            plannerEvent.Price = Decimal.Parse(data.UpgradeCost);
            plannerEvent.PriceDesc = Decimal.Parse(data.UpgradeCost).ToString();
            plannerEvent.DisplayPriceYN = true;
            // Time and Date
            plannerEvent.TimeStart = "0:00";
            plannerEvent.TimeEnd = "24:00";
            plannerEvent.TimeInterval = 30;
            plannerEvent.TimeDisplayType = 1;
            plannerEvent.InsertDate = DateTime.Now;

            plannerEvent.UpdateDate = DateTime.Now;
            plannerEvent.DateStart = data.StartDate;
            plannerEvent.DateEnd = data.EndDate;
            plannerEvent.UpgradePricedBy = data.UpgradePricedBy;
            plannerEvent.UpgradeWeekDayWeekEnd = data.UpgradeWeekDayWeekEnd;

            // non-nullable columns
            plannerEvent.languageID = 1;
            // TODO: EventDetailDesc needs to be defined
            plannerEvent.EventDetailDesc = "updated or new event";
            plannerEvent.ShowOnYN = true;
            plannerEvent.ActiveYN = true;
            plannerEvent.Seasonal = true;
            plannerEvent.reservationYN = true;
            plannerEvent.ContentYN = true;
            plannerEvent.DropDownYN = true;
            plannerEvent.SortOrder = 0;
            plannerEvent.SortOrder2 = 0;
            plannerEvent.RevenuePP = false;

            try
            {
                int groupid = GetBussinessRuleGroupid(data,0);
                if(groupid != 0)
                {
                    if(data.RateTypeBooked == null)
                    {
                        List<PlannerEvent> plannerEventlist = plannerEventRepo.GetAll().Where(p => p.Groupid == groupid).ToList();
                        foreach (var plannerEventitem in plannerEventlist)
                        {
                            plannerEventitem.Groupid = null;
                            plannerEventitem.ActiveYN = false;
                            plannerEventRepo.Update(plannerEventitem);
                            //unitOfWork.Commit();
                            //succeed = true;
                        }
                        plannerEvent.Groupid = groupid;
                        plannerEventRepo.Add(plannerEvent);
                    }
                    else
                    {
                        plannerEvent.Groupid = groupid;
                        plannerEventRepo.Add(plannerEvent);
                    }
                }
                else
                {
                    int maxgroupid = GetBussinessRuleGroupid(data, 1);
                    plannerEvent.Groupid = maxgroupid + 1;
                    plannerEventRepo.Add(plannerEvent);
                }

                //if (data.BusinessRuleID != 0) // update
                //{
                //    plannerEventRepo.Update(plannerEvent);
                //}
                //else // add new
                //{
                //    plannerEventRepo.Add(plannerEvent);
                //}

                unitOfWork.Commit();
                succeed = UpdateBusinessRuleParentID(plannerEvent.Hotel_Code, plannerEvent.RoomTypeCodeBooked, plannerEvent.RoomTypeCodeUpgrade);

            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.StackTrace);
                //{"Validation failed for one or more entities. See 'EntityValidationErrors' property for more details."}
            }
            #endregion
            return succeed;
        }

        public bool UpdateUpgradeRuleByGroupID(int? groupidbefore ,int newgroupid ,string ratetypebooked, int type)
        {
            bool succeed = false;
            List<PlannerEvent> plannerEventlist = plannerEventRepo.GetAll().Where(p => p.Groupid == groupidbefore).ToList();
            #region Change groupid null

            //PlannerEvent plannerEventbefore = plannerEventRepo.Get(p => p.Groupid == groupidbefore);
            //check newgroup
            try
            {
                if (type == 0)
                {
                    foreach (var plannerEventitem in plannerEventlist)
                    {
                        plannerEventitem.Groupid = null;
                        plannerEventitem.ActiveYN = false;
                        plannerEventRepo.Update(plannerEventitem);
                        unitOfWork.Commit();
                        succeed = true;
                    }
                }
                else if (type == 1)
                {
                    foreach (var plannerEventitem in plannerEventlist)
                    {
                        plannerEventitem.ActiveYN = true;
                        plannerEventitem.UpdateDate = DateTime.Now;
                        plannerEventRepo.Update(plannerEventitem);
                        unitOfWork.Commit();
                        succeed = true;
                    }
                }
                else if (type == 2)
                {
                    foreach (var plannerEventitem in plannerEventlist)
                    {
                        plannerEventitem.ActiveYN = false;
                        plannerEventitem.UpdateDate = DateTime.Now;
                        plannerEventRepo.Update(plannerEventitem);
                        unitOfWork.Commit();
                        succeed = true;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.StackTrace);
            }
            
            #endregion
            return succeed;
        }

        public bool UpdateBusinessRuleParentID(string hotel_code, string room_code, string room_upgrade_code)
        {
            bool succeed = false;
            #region update ParentID of the new PlannerEvent
            PlannerEvent plannerEvent = plannerEventRepo.Get(p => p.Hotel_Code.Equals(hotel_code) 
                                                && p.RoomTypeCodeBooked.Equals(room_code) 
                                                && p.RoomTypeCodeUpgrade.Equals(room_upgrade_code)
                                                && p.ParentID == 0);
            if (null == plannerEvent) return succeed;  
            plannerEvent.ParentID = plannerEvent.ID;

            try
            {
                plannerEventRepo.Update(plannerEvent);
                unitOfWork.Commit();
                succeed = true;
            }
            catch (Exception e)
            {
                logger.Error("Exception: " + e.StackTrace);
            }
            #endregion
            return succeed;
        }

        public PlannerEvent GetPlannerEvent(int plannerEventID, string cultureID)
        {
            PlannerEvent plannerEvent = plannerEventRepo.GetAnyById(plannerEventID);
            
            List<PriceItem> plannerEventPrices = currencyService.GetHotelActiveCurrency(plannerEvent.Hotel_Code, plannerEvent.USDPrice);
            PriceItem plannerEventPrice = plannerEventPrices.Where(x => x.CurrencyCultureID == cultureID).FirstOrDefault();

            string currencyCode = "";
            decimal? price = null;
            if (plannerEventPrice != null)
            {
                price = plannerEventPrice.Price ?? plannerEvent.USDPrice;
                currencyCode = plannerEventPrice.CurrencyCode ?? plannerEvent.CurrencyName;
            }

            plannerEvent.CurrencyName = currencyCode;
            plannerEvent.Price = price;

            return plannerEvent;
        }

        private bool InsertData(FGuest guest, string plannerEventID, ref List<int> guestPlannerIDList, ref List<int> eUpgradeRequestIDList,string langForSave,string currencyForSave)
        {
            bool succeed = false;

            #region Insert data

            GuestPlanner unconfirmedRequest = guestPlannerService.GetUnconfirmedRequest(guest.LoginConfirmationNum, plannerEventID);
            if (unconfirmedRequest != null)
            {
                unconfirmedRequest.LastModifiedDate = DateTime.Now;
                unconfirmedRequest.ShowItinerary = "Y";

                if(!string.IsNullOrEmpty(langForSave))
                    unconfirmedRequest.LangId = langForSave;
                if (!string.IsNullOrEmpty(currencyForSave))
                    unconfirmedRequest.CurrencyName = currencyForSave;

                guestPlannerService.updateUnconfirmedRequest(unconfirmedRequest);
                guestPlannerIDList.Add(unconfirmedRequest.ID);
                succeed = true;
            }
            else
            {
                #region insert to Table GuestPlanner

                int id = 0;
                Int32.TryParse(plannerEventID, out id);
                string cultureID = "en-US";
                PlannerEvent plannerEvent = GetPlannerEvent(id, cultureID);
                if (null == plannerEvent)
                    return false;
                GuestPlanner guestPlanner = new GuestPlanner();
                guestPlanner.LookupID = Guid.NewGuid();
                guestPlanner.RequestID = "";
                guestPlanner.ConfirmationNum = guest.ConfirmationNum;
                guestPlanner.LoginConfirmationNum = guest.LoginConfirmationNum;
                guestPlanner.Hotel_Code = guest.HOTEL_CODE;
                guestPlanner.Event = plannerEvent.EventCategory;
                guestPlanner.Description = plannerEvent.EventDetailDesc;
                guestPlanner.DescID = plannerEvent.ID.ToString();
                guestPlanner.EventDate = guest.ArrivalDate;
                guestPlanner.EventTime = DateTime.Now.ToString("HH:MM");
                guestPlanner.Name = "";
                guestPlanner.NumberPeople = 1;
                guestPlanner.VendorID = null;
                guestPlanner.RequestTypeID = null;
                guestPlanner.Other1 = null;
                guestPlanner.Other2 = null;

                guestPlanner.Status = "Requested";
                guestPlanner.ConfirmStatus = false;
                guestPlanner.NewConfirmStatus = "Pending";
                guestPlanner.ConfirmConcierge = null;
                guestPlanner.ConfirmDate = null;

                guestPlanner.ConciergeInitial = "eUpgrade";
                guestPlanner.Removed = false;
                guestPlanner.ShowItinerary = "Y";
                guestPlanner.Comments = "";
                guestPlanner.CreatedDate = DateTime.Now;
                guestPlanner.LastModifiedDate = DateTime.Now;
                guestPlanner.AdditionalInfo = null;

                if (!string.IsNullOrEmpty(currencyForSave))
                    guestPlanner.CurrencyName = currencyForSave;
                else
                   guestPlanner.CurrencyName = plannerEvent.CurrencyName;

                guestPlanner.Price = plannerEvent.Price;
                guestPlanner.USDPrice = plannerEvent.USDPrice;
                guestPlanner.RevenuePP = plannerEvent.RevenuePP;
                guestPlanner.ActionTaken = false;
                guestPlanner.IsNotificationSent = false;
                guestPlanner.RequestSessionID = Guid.NewGuid();

                if (!string.IsNullOrEmpty(langForSave))
                    guestPlanner.LangId = langForSave;
                else
                    guestPlanner.LangId = cultureID;

                guestPlanner.SMSNotification = false;
                guestPlanner.USDGratuity = null;
                guestPlanner.SMSNotifyCell = "";

                int guestPlannerID = 0;
                guestPlannerID = guestPlannerService.AddNewGuestPlanner(guestPlanner);
                guestPlannerIDList.Add(guestPlannerID);

                #endregion

                #region insert to table GustPlanner_Log
                if (guestPlannerID > 0 && null != guestPlanner)
                {
                    int guestPlannerLogID = 0;
                    var log = mapper.Map<GuestPlanner_Log>(guestPlanner);
                    guestPlannerLogID = guestPlannerService.AddGuestPlannerLog(log);
                }
                #endregion

                #region insert to Table eUpgradeRequest

                var eRequest = new eUpgradeRequest();
                eRequest.InsertDate = DateTime.Now;
                eRequest.loginConfirmationNum = guest.LoginConfirmationNum;
                eRequest.HotelCode = guest.HOTEL_CODE;
                eRequest.BookedRoomType = plannerEvent.RoomTypeCodeBooked;
                eRequest.UpgradeRoomType = plannerEvent.RoomTypeCodeUpgrade;
                eRequest.UpgradeStatus = "Pending";
                eRequest.UpgradeCost = guestPlanner.USDPrice;
                RoomType_Code roomType_Code = roomTypeRepo.Get(x => x.Hotel_Code == guest.HOTEL_CODE && x.RoomCode == plannerEvent.RoomTypeCodeUpgrade);
                eRequest.UpgradeType = roomType_Code.UpgradeType;
                eRequest.PerNightCharge = (roomType_Code.PerNightCharge == "Y") ? true : false;
                string transactionCode = transactionCodeRepo.GetAll().Where(t => t.HotelCode == guest.HOTEL_CODE && t.PerNightCharge == eRequest.PerNightCharge && t.UpgradeType == roomType_Code.UpgradeType).Select(t => t.TransactionCode).FirstOrDefault();
                eRequest.TransactionCode = transactionCode;
                Hotel hotel = hotelService.GetHotelByCode(guest.HOTEL_CODE);
                if (!string.IsNullOrEmpty(hotel.CenResServiceInterface) && !string.IsNullOrEmpty(hotel.CenResServiceUrl)
                    && !string.IsNullOrEmpty(hotel.CendynPropertyId) && hotel.AutoRequestProcessYN)
                    eRequest.TotalUpgradeFees = guest.UpgradeFees + guestPlanner.USDPrice.GetValueOrDefault();
                else
                    eRequest.TotalUpgradeFees = 0;

                eRequest.PackageCode = guestPlannerService.GetPackageCode(plannerEvent.RoomTypeCodeUpgrade, guest.HOTEL_CODE);


                int eUpgradeRequestID = 0;
                eUpgradeRequestID = guestPlannerService.AddNewEUpgradeRequest(eRequest);
                eUpgradeRequestIDList.Add(eUpgradeRequestID);

                #endregion
            }

            #endregion


            return succeed;
        }

        public IList<PriceItem> GetOptionPrices(int parentID)
        {
            List<PriceItem> list = new List<PriceItem>();

            var plannerEvent = plannerEventRepo.GetAll().Where(x => x.ParentID == parentID && x.languageID == 1).FirstOrDefault();
            if (plannerEvent != null)
                list = currencyService.GetHotelActiveCurrency(plannerEvent.Hotel_Code, plannerEvent.USDPrice);

            return list;
        }
    }
}
