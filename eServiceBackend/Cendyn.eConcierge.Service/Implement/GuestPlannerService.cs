using AutoMapper;
using Cendyn.eConcierge.Core.Extensions;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.ExcelModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Linq.Dynamic;
using NPOI.Extension;
using EContact.Infrastructure.Encryption;
using System.Web.Configuration;
using System.Web;
using System.Net.Mail;

namespace Cendyn.eConcierge.Service.Implement
{
    public class GuestPlannerService : ServiceBase, IGuestPlannerService
    {
        public IConciergeHotelAccessMappingRepository conAccMapRepo { get; set; }
        public IGuestPlannerRepository guestPlannerRepo { get; set; }
        public IGuestPlanner_LogRepository guestPlannerLogRepo { get; set; }
        public IHotelRepository hotelRepo { get; set; }
        public IConciergeLoginRepository conciergeLoginRepo { get; set; }
        public IFGuestRepository fguestRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IPlannerEventService plannerEventService { get; set; }
        public IRoomType_CodeRepository roomCodeRepo { get; set; }
        public ISentEmailLogRepository sentEmailLogRepo { get; set; }
        public IEUpgradeRequestRepository upgradeRequestRepo { get; set; }
        public ISettingRepository settingsRepo { get; set; }
        public IHotelSettingService hotelSettingService { get; set; }
        public IEmailGenerateService requestConfirmEmailService { get; set; }
        public ICurrencyConvertRepository currencyConvertRepo { get; set; }
        public IEmailTemplateRepository emailTemplateRepo { get; set; }
        public IRoomImageRepository roomImageRepo { get; set; }
        public IHotels_LanguagesRepository hotelLanguageRepo { get; set; }
        public IEUpgradeBrandCssMappingRepository brandCssRepo { get; set; }
        public IMapper mapper { get; set; }
        public IEmailSendService emailSendService { get; set; }
        public IRateTypeService rateTypeService { get; set; }
        public IL_CurrencyRepository lCurrencyRepository { get; set; }

        public IEUpgradeTransactionCodeRepository transactionCodeRepo { get; set; }

        public IAppConfig appConfig { get; set; }


        public int AddNewEUpgradeRequest(eUpgradeRequest eUpgradeRequest)
        {
            try
            {
                upgradeRequestRepo.Add(eUpgradeRequest);
                unitOfWork.Commit();

                return eUpgradeRequest.ID;
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        public int AddNewGuestPlanner(GuestPlanner guestPlanner)
        {
            try
            {
                guestPlannerRepo.Add(guestPlanner);
                unitOfWork.Commit();

                return guestPlanner.ID;
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        public GuestPlanner GetGuestPlannerByID(int ID)
        {
            return guestPlannerRepo.GetById(ID);
        }

        public bool UpdatRequestIDs(List<int> guestPlannerIDList)
        {
            bool succeed = guestPlannerRepo.UpdatRequestIDs(guestPlannerIDList);

            unitOfWork.Commit();

            return succeed;
        }

        public bool UpdatRequestIDsOfeUpgradeRequest(List<int> eUpgradeRequestIDList, string requestID)
        {
            try
            {
                var eUpgradeRequestQueryable = upgradeRequestRepo.GetAll();
                var query = (from g in eUpgradeRequestQueryable
                             where eUpgradeRequestIDList.Contains(g.ID)
                             select g);

                List<eUpgradeRequest> eUpgradeRequestList = query.ToList();

                if (null != eUpgradeRequestList && eUpgradeRequestList.Count() > 0)
                {
                    foreach (var item in eUpgradeRequestList)
                    {
                        item.RequestID = requestID;
                        upgradeRequestRepo.Update(item);

                        unitOfWork.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string GetRequestID(List<int> guestPlannerIDList)
        {
            string requestID = guestPlannerRepo.GetRequestID(guestPlannerIDList);

            return requestID;
        }
        public string GetPackageCode(string RoomTypeCodeUpgrade, string hotelCode)
        {
            string packageCode;
            var query = (from rc in roomCodeRepo.GetAll()
                         where rc.RoomCode == RoomTypeCodeUpgrade && rc.Hotel_Code == hotelCode
                         select rc).FirstOrDefault();

            RoomType_Code roomType_Code = query;


            if (roomType_Code != null)
            {
                packageCode = roomType_Code.PackageCode;
            }
            else
            {
                packageCode = "";
            }

            return packageCode;
        }


        public GuestPlanner GetGuestPlannerByLookupID(Guid lookUpID)
        {
            return guestPlannerRepo.Get(x => x.LookupID == lookUpID);
        }

        public GuestPlanner GetUnconfirmedRequest(string confirmationNumber, string requestId)
        {
            var gusetPlannerQueryable = guestPlannerRepo.GetAll();
            var g = (from gpr in gusetPlannerQueryable
                     where (gpr.LoginConfirmationNum.Equals(confirmationNumber)
                         && gpr.DescID.Equals(requestId)
                         && gpr.ConfirmStatus == false
                         && gpr.Status.Equals("Requested")
                         && gpr.NewConfirmStatus.Equals("Pending"))
                     orderby gpr.LastModifiedDate descending
                     select gpr).FirstOrDefault();
            return g;
        }

        public void updateUnconfirmedRequest(GuestPlanner unconfirmedRequest)
        {
            guestPlannerRepo.Update(unconfirmedRequest);
            unitOfWork.Commit();
        }

        public GuestPlanner_Log GetUnconfirmedRequestLog(string confirmationNumber, string requestId)
        {
            var gusetPlannerLogQueryable = guestPlannerLogRepo.GetAll();
            var gp_log = (from gprl in gusetPlannerLogQueryable
                          where (gprl.LoginConfirmationNum.Equals(confirmationNumber)
                              && gprl.DescID.Equals(requestId)
                              && gprl.ConfirmStatus == false)
                          orderby gprl.LastModifiedDate descending
                          select gprl).FirstOrDefault();
            return gp_log;
        }

        public void updateUnconfirmedRequestLog(GuestPlanner_Log unconfirmedRequestLog)
        {
            guestPlannerLogRepo.Update(unconfirmedRequestLog);
            unitOfWork.Commit();
        }

        public int AddGuestPlannerLog(GuestPlanner_Log guestPlannerLog)
        {
            try
            {
                guestPlannerLogRepo.Add(guestPlannerLog);
                unitOfWork.Commit();
                return guestPlannerLog.LogID;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public GuestPlanner_Log GetGuestPlannerLogByID(int ID)
        {
            return guestPlannerLogRepo.GetById(ID);
        }

        public int GetRequestCommitted()
        {
            var gusetPlannerQueryable = guestPlannerRepo.GetAll();
            var query = (from gpr in gusetPlannerQueryable
                         select new
                         {
                             gpr,
                             AllRequestHandled = !gusetPlannerQueryable.Any(x => x.LoginConfirmationNum == gpr.LoginConfirmationNum && x.ConfirmStatus == false)
                         });
            int counter = query.Count(p => p.gpr.ConfirmStatus == false);
            return counter;
        }

        /// <summary>
        /// Get all requests based on search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public IList<RequestSearchResultDTO> GetRequestsBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            IList<RequestSearchResultDTO> list = guestPlannerRepo.GetRequestsBySearchCriteria(searchCriteria, pageInfo);
            var query = list.AsEnumerable();
            //Get all committed requests
            pageInfo.RequestCommitted = query.Count(p => p.RequestStatus.Equals("Requested"));
            pageInfo.RecordsTotal = query.Count();

            //Setup Order, the order that passed in like "columnname asc, columnname desc"

            if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
            {
                var orderString = TransformOrder(pageInfo.OrderColumn);
                query = query.OrderBy(orderString);
            }
            else
            {
                //query = query.OrderByDescending(p => p.gpr.CreatedDate);
                //query = query.OrderByDescending(p => p.DateSubmitted);
                query = query.OrderByDescending(p => p.DateSubmitted).OrderBy(p => p.HotelCode);
            }

            //Setup paging
            if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
            {

            }
            else
            {
                query = query.Skip(pageInfo.StartIndex).Take(pageInfo.PageSize);
            }

            list = query.ToList();
            return list;

        }

        /// <summary>
        /// Get all business rules based on search criteria
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        //public IList<BusinessRuleSearchResultDTO> GetBusinessRuleBySearchCriteria(BusinessRuleSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        //{
        //    var query = (from x in plannerEventRepo.GetAll()
        //                 join rp in roomCodeRepo.GetAll() on new { Key1 = x.RoomTypeCodeUpgrade, Key2 = x.Hotel_Code } equals new { Key1 = rp.RoomCode, Key2 = rp.Hotel_Code }
        //                 select new BusinessRuleSearchResultDTO()
        //                 {
        //                     HotelCode = x.Hotel_Code, //r.HOTEL_CODE,
        //                     BookedRoomTypeCode = x.RoomTypeCodeBooked, //r.RoomType,
        //                     UpgradePriceBy = x.UpgradePricedBy,
        //                     BusinessRuleID = x.ID,
        //                     StartDate = x.DateStart,
        //                     EndDate = x.DateEnd,
        //                     UpgradeWeekDayWeekEnd = x.UpgradeWeekDayWeekEnd,
        //                     UpgradeRoomTypeCode = x.RoomTypeCodeUpgrade,
        //                     RateTypeBooked = x.RateTypeBooked,
        //                     UpgradeCost = x.USDPrice,
        //                     RoomTypeCodeUpgradeDescription = rp.RoomLongDescription,
        //                     ActiveYN = x.ActiveYN
        //                 });

        //    //by Hotel Name
        //    if (!string.IsNullOrWhiteSpace(searchCriteria.HotelCode))
        //    {
        //        query = query.Where(p => p.HotelCode == searchCriteria.HotelCode);
        //    }
        //    else
        //    {
        //        var hotelList = conAccMapRepo.GetAll()
        //            .Where(x => x.ConciergeID == searchCriteria.UserName && x.ActiveYN == true)
        //            .Select(x => x.HotelCode).ToList<string>();
        //        query = query.Where(x => hotelList.Any(h => h == x.HotelCode));
        //    }

        //    //by Booked RoomType
        //    if (!string.IsNullOrWhiteSpace(searchCriteria.BookedRoomTypeCode))
        //    {
        //        query = query.Where(p => p.BookedRoomTypeCode == searchCriteria.BookedRoomTypeCode);
        //    }

        //    pageInfo.RecordsTotal = query.Count();

        //    //Setup Order, the order that passed in like "columnname asc, columnname desc"
        //    if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
        //    {
        //        var orderString = pageInfo.OrderColumn.TrimEnd(',');
        //        query = query.OrderBy(orderString);
        //    }
        //    else
        //    {
        //        query = query.OrderByDescending(p => p.BookedRoomTypeCode);
        //    }

        //    //Setup paging
        //    if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
        //    {

        //    }
        //    else
        //    {
        //        //pageInfo.PageSize = pageInfo.RecordsTotal;
        //        query = query.Skip(pageInfo.StartIndex).Take(pageInfo.PageSize);
        //    }


        //    return query.ToList();

        //}

        public IList<BusinessRuleSearchResultDTO> GetBusinessRuleBySearchCriteria(BusinessRuleSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            IList<string> hotelCodeList = userAccountService.GetUserMappedHotelCodes(searchCriteria.UserName);
            if (hotelCodeList != null && hotelCodeList.Any())
            {
                var query = (from x in plannerEventRepo.GetAll()
                             join rp in roomCodeRepo.GetAll() on new { Key1 = x.RoomTypeCodeUpgrade, Key2 = x.Hotel_Code } equals new { Key1 = rp.RoomCode, Key2 = rp.Hotel_Code }
                             join h in hotelRepo.GetAll() on x.Hotel_Code equals h.Hotel_Code
                             //join c in currencyConvertRepo.GetAll() on new { A = h.Hotel_Code, B = h.LocalCurrencyCode } equals new { A = c.Hotel_Code, B = c.Code }
                             join hl in hotelLanguageRepo.GetAll() on x.Hotel_Code equals  hl.Hotel_Code
                             join lc in lCurrencyRepository.GetAll() on hl.CurrencyCode equals lc.Code
                             where rp.LanguageId == 1 && hl.LanguageID==1
                             select new BusinessRuleSearchResultDTO()
                             {
                                 HotelCode = x.Hotel_Code, //r.HOTEL_CODE,
                                 BookedRoomTypeCode = x.RoomTypeCodeBooked, //r.RoomType,
                                 UpgradePriceBy = x.UpgradePricedBy,
                                 BusinessRuleID = x.ID,
                                 StartDate = x.DateStart,
                                 EndDate = x.DateEnd,
                                 UpgradeWeekDayWeekEnd = x.UpgradeWeekDayWeekEnd,
                                 UpgradeRoomTypeCode = x.RoomTypeCodeUpgrade,
                                 RateTypeBooked = x.RateTypeBooked,
                                 UpgradeCost = x.USDPrice,
                                 RoomTypeCodeUpgradeDescription = rp.RoomLongDescription,
                                 ActiveYN = x.ActiveYN,
                                 DateFormat = h.DateFormat,
                                 CurrencySymbol = lc.Symbol,
                                 Groupid = x.Groupid
                             }).ToList();
                //Group by Groupid
                var querygroup = query.Where(p => p.Groupid != null).GroupBy(p => p.Groupid).ToList();
                query.RemoveAll(p => p.HotelCode != null);

                //The first row of each group is the main rule,combine ratetypebooked to the main rule
                //Only keep main rules,delete others
                foreach (var itemquery2 in querygroup)
                {
                    var rateTypeList = rateTypeService.GetRateTypeListByHotelCode(itemquery2.ElementAt(0).HotelCode).Select(p => p.Value.Split(',')[1]).ToList();

                    if (itemquery2.ElementAt(0).RateTypeBooked == null)
                    {
                        string rateTypeAll = string.Join(",", rateTypeList.ToArray());
                        itemquery2.ElementAt(0).RateTypeBooked = rateTypeAll;
                    }
                    else
                    {
                        if (itemquery2.Count() > 1)
                        {
                            bool groupactive = false;
                            string ratetypejoin = "";
                            for (int i = 0; i < itemquery2.Count(); i++)
                            {
                                if (itemquery2.ElementAt(i).ActiveYN == true)
                                {
                                    groupactive = true;
                                }
                                if (rateTypeList.Exists(p => p == itemquery2.ElementAt(i).RateTypeBooked))
                                {
                                    ratetypejoin += itemquery2.ElementAt(i).RateTypeBooked;
                                    ratetypejoin += ",";
                                }
                            }
                            itemquery2.ElementAt(0).ActiveYN = groupactive;
                            itemquery2.ElementAt(0).RateTypeBooked = ratetypejoin.EndsWith(",") ? ratetypejoin.Substring(0, ratetypejoin.Length - 1) : "";
                        }
                        else
                        {
                            if (!rateTypeList.Exists(p => p == itemquery2.ElementAt(0).RateTypeBooked))
                            {
                                itemquery2.ElementAt(0).RateTypeBooked = "";
                            }
                        }
                    }


                    query.Add(itemquery2.ElementAt(0));
                }

                //by Hotel Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.HotelCode))
                {
                    query = query.Where(p => p.HotelCode == searchCriteria.HotelCode).ToList();
                }
                else
                {
                    var hotelList = conAccMapRepo.GetAll()
                        .Where(x => x.ConciergeID == searchCriteria.UserName && x.ActiveYN == true)
                        .Select(x => x.HotelCode).ToList<string>();
                    query = query.Where(x => hotelList.Any(h => h == x.HotelCode)).ToList();
                }

                //by Booked RoomType
                if (!string.IsNullOrWhiteSpace(searchCriteria.BookedRoomTypeCode))
                {
                    query = query.Where(p => p.BookedRoomTypeCode == searchCriteria.BookedRoomTypeCode).ToList();
                }

                var filterValue = pageInfo.FilterValue.ToLower();
                if (!string.IsNullOrWhiteSpace(filterValue) && !string.IsNullOrEmpty(filterValue))
                {
                    query = query.Where(p =>
                                     (p.HotelCode ?? "").ToLower().Contains(filterValue) ||
                                     (p.BookedRoomTypeCode ?? "").ToLower().Contains(filterValue) ||
                                     (p.UpgradePriceBy ?? "").ToLower().Contains(filterValue) ||
                                     (p.UpgradeRoomTypeCode.ToLower() ?? "").Contains(filterValue) ||
                                     (p.RateTypeBooked ?? "").ToLower().Contains(filterValue) ||
                                     (p.UpgradeCost != null && p.UpgradeCost.ToString().Contains(filterValue)) ||
                                     (p.CurrencySymbol ?? "").ToLower().Contains(filterValue)
                                     ).ToList();
                }

                pageInfo.RecordsTotal = query.Count();

                //Setup Order, the order that passed in like "columnname asc, columnname desc"
                if (!string.IsNullOrWhiteSpace(pageInfo.OrderColumn))
                {
                    string orderString = pageInfo.OrderColumn.TrimEnd(',');
                    query = query.AsQueryable().OrderBy(orderString).ToList();
                }
                else
                {
                    //query = query.AsQueryable().OrderByDescending(p => p.BookedRoomTypeCode).ToList();
                    query = query.AsQueryable().OrderBy(p => p.BookedRoomTypeCode).OrderBy(p => p.HotelCode).ToList();
                }

                //Setup paging
                if (pageInfo.PageSize == -1 && pageInfo.StartIndex == -1)
                {

                }
                else
                {
                    //pageInfo.PageSize = pageInfo.RecordsTotal;
                    query = query.AsQueryable().Skip(pageInfo.StartIndex).Take(pageInfo.PageSize).ToList();
                }

                return query.ToList();
            }
            else
                return null;

        }

        /// <summary>
        /// Transform the order string to column name
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string TransformOrder(string p)
        {
            //Get each order
            var Orders = p.Split(',');
            //var orderIndex = 0;
            var orderString = string.Empty;

            foreach (var order in Orders)
            {
                if (!string.IsNullOrWhiteSpace(order))
                {
                    //Get column name and order type, array[0] is column name , array[1] is order type
                    var array = order.Split(' ');
                    switch (array[0])
                    {
                        case "HotelCode":
                            orderString += string.Format("HotelCode {0},", array[1]);
                            break;
                        case "GuestName":
                            orderString += string.Format("FirstName {0},", array[1]);
                            orderString += string.Format("LastName {0},", array[1]);
                            break;
                        case "DateSubmitted":
                            orderString += string.Format("DateSubmitted {0},", array[1]);
                            break;
                        case "Email":
                            orderString += string.Format("Email {0},", array[1]);
                            break;
                        case "Reservation":
                            orderString += string.Format("Reservation {0},", array[1]);
                            break;
                        case "ArrivalDate":
                            orderString += string.Format("ArrivalDate {0},", array[1]);
                            break;
                        case "NumberOfNights":
                            orderString += string.Format("NumberOfNights {0},", array[1]);
                            break;
                        case "BookedRoomTypeCode":
                            orderString += string.Format("BookedRoomTypeCode {0},", array[1]);
                            break;
                        case "UpgradeRoomTypeCode":
                            orderString += string.Format("UpgradeRoomTypeCode {0},", array[1]);
                            break;
                        case "UpgradeCost":
                            orderString += string.Format("UpgradeCost {0},", array[1]);
                            break;
                        case "RequestStatus":
                            orderString += string.Format("RequestStatus {0},", array[1]);
                            break;
                        case "TotalRooms":
                            orderString += string.Format("TotalRooms {0},", array[1]);
                            break;
                        case "Threshold":
                            orderString += string.Format("Threshold {0},", array[1]);
                            break;
                        case "LastEmailSentDate":
                            orderString += string.Format("LastEmailSentDate {0},", array[1]);
                            break;
                        case "IncrementalTotalAmountForStay":
                            orderString += string.Format("IncTotalAmountForStay {0},", array[1]);
                            break;
                        case "PreviousRequests":
                            orderString += string.Format("PreviousRequests {0},", array[1]);
                            break;
                        case "ConfirmedRequests":
                            orderString += string.Format("ConfirmedRequests {0},", array[1]);
                            break;
                        case "EmailStatus":
                            orderString += string.Format("LastEmailSentDate {0},", array[1]);
                            break;
                        case "LastModifiedDate":
                            orderString += string.Format("LastModifiedDate {0},", array[1]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return orderString.TrimEnd(',');
        }

        public string ConfirmUpgradeByRequestID(int requestID, string userName, int operation)
        {
            //Check if current logged in usser has permission to do this action
            //Based on Hotel code
            //Get current request
            var request = guestPlannerRepo.GetById(requestID);

            if (request == null)
            {
                return "404";
            }

            //Check if the hotelcode current user's access mapping
            if (!userAccountService.CheckUserHasPermissionByHotelCode(userName, request.Hotel_Code))
            {
                //Current user doesn't have permission
                return "403";
            }

            //Update GuestPlanner Table
            request.ConfirmDate = DateTime.Now;
            request.ConfirmStatus = true; //Grant
            request.NewConfirmStatus = "Confirmed";
            request.Status = operation == 1 ? "Confirmed" : "Denied";
            //request.ConciergeInitial = "eUpgrade"; //TODO: for now hardcode to eUpgrade.
            request.ConfirmConcierge = userName;
            guestPlannerRepo.Update(request);

            //Insert GuestPlanner_Log table
            var log = mapper.Map<GuestPlanner_Log>(request);
            guestPlannerLogRepo.Add(log);


            #region New logic Manual process: Update eUpgradeRequest table when request is confirmed or denied

            string bookedRoomType = plannerEventRepo.GetAll().Where(p => p.ID.ToString() == request.DescID).Select(p => p.RoomTypeCodeBooked).FirstOrDefault();
            if (string.IsNullOrEmpty(bookedRoomType))
            {
                bookedRoomType = fguestRepo.Get(p => p.LoginConfirmationNum == request.LoginConfirmationNum).RoomType;
            }

            string upgradedRoomType = plannerEventRepo.GetAll().Where(p => p.ID.ToString() == request.DescID).Select(p => p.RoomTypeCodeUpgrade).FirstOrDefault();

            upgradeRequestRepo.UpDateeUpgradeRequest(request.LoginConfirmationNum, request.Hotel_Code, bookedRoomType, upgradedRoomType, request.Status, request.ConfirmConcierge);

            #endregion

            unitOfWork.Commit();

            return string.Empty;

        }


        public IList<GrantedRequestEmailGenerateDTO> GetGrantedRequestForEmail(string loginConfirmationNum)
        {
            var list = (from gp in guestPlannerRepo.GetAll()
                        join fg in fguestRepo.GetAll() on gp.LoginConfirmationNum equals fg.LoginConfirmationNum
                        join pe in plannerEventRepo.GetAll() on gp.DescID equals pe.ID.ToString()
                        join brc in roomCodeRepo.GetAll() on new { K1 = fg.RoomType, K2 = fg.HOTEL_CODE } equals new { K1 = brc.RoomCode, K2 = brc.Hotel_Code }
                        join urc in roomCodeRepo.GetAll() on new { K1 = pe.RoomTypeCodeUpgrade, K2 = pe.Hotel_Code } equals new { K1 = urc.RoomCode, K2 = urc.Hotel_Code }
                        join h in hotelRepo.GetAll() on gp.Hotel_Code equals h.Hotel_Code
                        join c in currencyConvertRepo.GetAll() on new { A = h.Hotel_Code, B = h.LocalCurrencyCode } equals new { A = c.Hotel_Code, B = c.Code }
                        where gp.LoginConfirmationNum == loginConfirmationNum && gp.Status == "Confirmed" && urc.LanguageId == 1 && brc.LanguageId == 1
                        select new GrantedRequestEmailGenerateDTO()
                        {
                            BookedRoomType = brc.RoomDescription,
                            PerNightCharge = urc.PerNightCharge,
                            PriceDesc = urc.PriceDesc,
                            UpgradeCost = gp.USDPrice,
                            UpgradedRoomType = urc.RoomDescription,
                            CurrencySymbol = c.Symbol,
                        });

            return list.ToList();
        }

        public IList<DeniedRequestEmailGenerateDTO> GetDeniedRequestForEmail(string loginConfirmationNum)
        {
            var list = (from gp in guestPlannerRepo.GetAll()
                        join fg in fguestRepo.GetAll() on gp.LoginConfirmationNum equals fg.LoginConfirmationNum
                        join pe in plannerEventRepo.GetAll() on gp.DescID equals pe.ID.ToString()
                        join brc in roomCodeRepo.GetAll() on new { K1 = fg.RoomType, K2 = fg.HOTEL_CODE } equals new { K1 = brc.RoomCode, K2 = brc.Hotel_Code }
                        join urc in roomCodeRepo.GetAll() on new { K1 = pe.RoomTypeCodeUpgrade, K2 = pe.Hotel_Code } equals new { K1 = urc.RoomCode, K2 = urc.Hotel_Code }
                        join h in hotelRepo.GetAll() on gp.Hotel_Code equals h.Hotel_Code
                        join c in currencyConvertRepo.GetAll() on new { A = h.Hotel_Code, B = h.LocalCurrencyCode } equals new { A = c.Hotel_Code, B = c.Code }
                        where gp.LoginConfirmationNum == loginConfirmationNum && gp.Status == "Denied" && urc.LanguageId == 1 && brc.LanguageId == 1
                        select new DeniedRequestEmailGenerateDTO()
                        {
                            BookedRoomType = brc.RoomDescription,
                            PerNightCharge = urc.PerNightCharge,
                            PriceDesc = urc.PriceDesc,
                            UpgradeCost = gp.USDPrice,
                            UpgradedRoomType = urc.RoomDescription,
                            CurrencySymbol = c.Symbol,
                        });

            return list.ToList();
        }

        public bool SendRequestConfirmEmail(RequestConfirmEmailSendDTO data)
        {
            //Get Recepints
            var gp = guestPlannerRepo.GetById(data.RequestID);

            var userEmailinfo = (from fg in fguestRepo.GetAll()
                                 where fg.LoginConfirmationNum == gp.LoginConfirmationNum
                                 select new { fg.Email, fg.LName, fg.FName, fg.LoginConfirmationNum, fg.HOTEL_CODE }).FirstOrDefault();

            //Get Html Body
            var htmlBody = data.EmailBody.Replace("\n", "<br/>");
            var settings = settingsRepo.GetAll().Where(s => s.settingKey.Equals("Cendyn supporting email")).FirstOrDefault();
            //Send Email
            var emailDTO = new SendEmailDTO()
            {
                EmailSubject = data.Subject,
                MailPriority = 2,
                ToUserEmail = userEmailinfo.Email,
                ToUserDisplayName = userEmailinfo.FName + " " + userEmailinfo.LName,
                EmailBodyHtml = htmlBody,
                FromUserDisplayName = "eUpgrade Support",
                FromUserEmail = string.IsNullOrEmpty(settings.settingValue) ? "eUpgrade-support@cendyn.com" : settings.settingValue
            };

            var fromUserEmail = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeEmailFrom");
            if (!string.IsNullOrWhiteSpace(fromUserEmail)) emailDTO.FromUserEmail = fromUserEmail;

            var fromUserDisplayName = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeEmailFromDisplayName");
            if (!string.IsNullOrWhiteSpace(fromUserDisplayName)) emailDTO.FromUserDisplayName = fromUserDisplayName;

            var result = emailSendService.Send(emailDTO);

            if (result)
            {
                //Add send email log
                var sendEmailEntity = new SentEmailLog();
                sendEmailEntity.Subject = data.Subject;
                sendEmailEntity.Body = htmlBody;
                sendEmailEntity.Category = "RequstConfirmEmail";
                sendEmailEntity.EmailTo = userEmailinfo.Email;
                sendEmailEntity.LoginConfirmationNum = userEmailinfo.LoginConfirmationNum;
                sendEmailEntity.Hotel_Code = userEmailinfo.HOTEL_CODE;
                sendEmailEntity.SentDate = DateTime.Now;
                sendEmailEntity.SendStatus = "Success";
                sendEmailEntity.Source = "WebSite -- Admin Site";
                sentEmailLogRepo.Add(sendEmailEntity);

                //Update timestamp
                var gpList = guestPlannerRepo.GetAll().Where(p => p.LoginConfirmationNum == userEmailinfo.LoginConfirmationNum
                    && (p.Status == "Confirmed" || p.Status.Equals("Denied")));

                foreach (var item in gpList)
                {
                    item.LastConfirmEmailSentDate = DateTime.Now;
                    guestPlannerRepo.Update(item);
                }

                unitOfWork.Commit();
            }

            return result;
        }

        public bool SendGlobalEmail(RequestConfirmEmailSendDTO data)
        {
            //Get Recepints
            var gp = guestPlannerRepo.GetById(data.RequestID);

            var userEmailinfo = (from fg in fguestRepo.GetAll()
                                 where fg.LoginConfirmationNum == gp.LoginConfirmationNum
                                 select new { fg.Email, fg.ConfirmationNum, fg.LName, fg.FName, fg.LoginConfirmationNum, fg.HOTEL_CODE }).FirstOrDefault();

            //Get Html Body
            var htmlBody = data.EmailBody.Replace("\n", "<br/>");

            var hasEmailTemplate = hotelSettingService.GetHotelSettingValue(userEmailinfo.HOTEL_CODE, "HasEmailTemplate");
            List<LinkedResource> lstRes = new List<LinkedResource>();
            if (hasEmailTemplate == "1")
            {
                data.EmailBody = GetEmailTempate(userEmailinfo.HOTEL_CODE, userEmailinfo.FName, userEmailinfo.ConfirmationNum, userEmailinfo.LoginConfirmationNum, out lstRes);
                htmlBody = data.EmailBody;
            }

            var settings = settingsRepo.GetAll().Where(s => s.settingKey.Equals("Cendyn supporting email")).FirstOrDefault();
            //Send Email
            var emailDTO = new SendEmailDTO()
            {
                ListLinkedResource = lstRes,
                EmailSubject = data.Subject,
                MailPriority = 2,
                ToUserEmail = userEmailinfo.Email,
                ToUserDisplayName = userEmailinfo.FName + " " + userEmailinfo.LName,
                EmailBodyHtml = htmlBody,
                FromUserDisplayName = "eUpgrade Support",
                FromUserEmail = string.IsNullOrEmpty(settings.settingValue) ? "eUpgrade-support@cendyn.com" : settings.settingValue,
            };

            var fromUserEmail = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeEmailFrom");
            if (!string.IsNullOrWhiteSpace(fromUserEmail))
                emailDTO.FromUserEmail = fromUserEmail;

            var fromUserDisplayName = hotelSettingService.GetHotelSettingValue(gp.Hotel_Code, "UpgradeEmailFromDisplayName");
            if (!string.IsNullOrWhiteSpace(fromUserDisplayName))
                emailDTO.FromUserDisplayName = fromUserDisplayName;

            var bccEmails = requestConfirmEmailService.GetGlobalEmailBccRecipients(userEmailinfo.HOTEL_CODE);
            if (!string.IsNullOrWhiteSpace(bccEmails))
                emailDTO.BccEmails = bccEmails;

            var result = emailSendService.Send(emailDTO);

            if (result)
            {
                //Add send email log
                var sendEmailEntity = new SentEmailLog();
                sendEmailEntity.Subject = data.Subject;
                sendEmailEntity.Body = htmlBody;
                sendEmailEntity.Category = "RequstConfirmGlobalEmail";
                sendEmailEntity.EmailTo = userEmailinfo.Email;
                sendEmailEntity.LoginConfirmationNum = userEmailinfo.LoginConfirmationNum;
                sendEmailEntity.Hotel_Code = userEmailinfo.HOTEL_CODE;
                sendEmailEntity.SentDate = DateTime.Now;
                sendEmailEntity.SendStatus = "Success";
                sendEmailEntity.Source = "WebSite -- Admin Site";
                sendEmailEntity.BccEmails = bccEmails;
                sentEmailLogRepo.Add(sendEmailEntity);

                //Update timestamp
                var gpList = guestPlannerRepo.GetAll().Where(p => p.LoginConfirmationNum == userEmailinfo.LoginConfirmationNum
                    && (p.Status == "Confirmed" || p.Status.Equals("Denied")));

                foreach (var item in gpList)
                {
                    item.LastConfirmEmailSentDate = DateTime.Now;
                    guestPlannerRepo.Update(item);
                }

                unitOfWork.Commit();
            }

            return result;
        }

        private string GetBackendLoginLink(string token)
        {
            //Encrypter
            using (TripleDESEncryption tde = new TripleDESEncryption(WebConfigurationManager.AppSettings["TripleDESEncryptionKey"].ToString()))
            {
                token = tde.Encrypt(token);
            }

            token = System.Web.HttpUtility.UrlEncode(token);
            var linkurl = "{2}://{0}/Admin/Account/Login?token={1}";
            string domainName = HttpContext.Current.Request.Url.Host;
            string scheme = HttpContext.Current.Request.Url.Scheme;
            return string.Format(linkurl, domainName, token, scheme);
        }

        public void SendRequestNotificationEmail(int gpid)
        {
            //Get Recepints
            var gp = guestPlannerRepo.GetById(gpid);

            var userEmailinfo = (from fg in fguestRepo.GetAll()
                                 join h in hotelRepo.GetAll() on fg.HOTEL_CODE equals h.Hotel_Code
                                 where fg.LoginConfirmationNum == gp.LoginConfirmationNum
                                 select new { fg.Email, fg.LName, fg.FName, fg.LoginConfirmationNum, fg.HOTEL_CODE, fg.ArrivalDate, h.Hotel_Name, h.DateFormat, h.CentralResNumYN, fg.ConfirmationNum, fg.CentralResNum }).FirstOrDefault();

            var decisionPerson = (from cl in conciergeLoginRepo.GetAll()
                                  join mapping in conAccMapRepo.GetAll() on cl.ConciergeID equals mapping.ConciergeID
                                  where (mapping.HotelCode == userEmailinfo.HOTEL_CODE && mapping.ReqstEmailFlg && cl.Email.Contains("@") && cl.Active)
                                  select new { cl.ConciergeID, cl.password1, cl.Email, cl.FName, cl.LName }).ToList();

            string dateFormat = userEmailinfo.DateFormat.ToLower().Replace("m", "M");
            string arrivalDate = userEmailinfo.ArrivalDate.HasValue ? userEmailinfo.ArrivalDate.Value.ToString(dateFormat) : string.Empty;
            string todayDate = DateTime.Today.ToString(dateFormat);

            bool isToday = ((arrivalDate == todayDate || string.IsNullOrWhiteSpace(arrivalDate)) ? true : false);
            string subject_date = (isToday ? "Today - " + todayDate : arrivalDate);
            var settings = settingsRepo.GetAll().Where(s => s.settingKey.Equals("Cendyn supporting email")).FirstOrDefault();
            string guestname = userEmailinfo.FName + " " + userEmailinfo.LName;
            string reservationNum = userEmailinfo.ConfirmationNum;
            if (userEmailinfo.CentralResNumYN && !string.IsNullOrEmpty(userEmailinfo.CentralResNum) && userEmailinfo.CentralResNum != "''")
            {
                reservationNum = userEmailinfo.CentralResNum;
            }
            for (int i = 0; i < decisionPerson.Count; i++)
            {
                if (string.IsNullOrEmpty(decisionPerson[i].Email)) continue;
                //uid and password are admin info
                string uid = decisionPerson[i].Email;
                string password = decisionPerson[i].password1;
                DateTime? arrival_date = userEmailinfo.ArrivalDate;
                var token = string.Format("uid={0}&pwd={1}&arrivaldate={2}&status={3}", decisionPerson[i].ConciergeID, password, arrival_date.Value.ToString("MM/dd/yyyy"), "pending");

                //Send Email
                var emailDTO = new SendEmailDTO()
                {
                    EmailSubject = string.Format("eUpgrade New Request in Queue for Guest Arriving {0}", subject_date),
                    MailPriority = 2, //high priority
                    ToUserEmail = uid,
                    FromUserEmail = string.IsNullOrEmpty(settings.settingValue) ? "eUpgrade-support@cendyn.com" : settings.settingValue,
                    FromUserDisplayName = "eUpgrade Support",
                    ToUserDisplayName = decisionPerson[i].FName + " " + decisionPerson[i].LName,
                    EmailBodyHtml =
                        "<html><body><div>Please be informed that the property <b>" + userEmailinfo.Hotel_Name + "</b> has an unfulfilled upgrade requests. Please check <a href=\""
                        + GetBackendLoginLink(token)
                        + "\">the eUpgrade system</a> at your earliest convenience and process the request(s) accordingly. Thank you for your assistance and enjoy a wonderful day.</div>"
                        + "<div>&nbsp;</div><div>Reservation Number: " + reservationNum + "</div>"
                        + "<div>Guest Name: " + guestname + "</div>"
                        //+ "\r\n\r\nReservation Number: " + reservationNum + "\r\nGuest Name: " + guestname
                        + "</body></html>"
                };

                var result = emailSendService.Send(emailDTO);

                if (result)
                {
                    //Add send email log
                    var sendEmailEntity = new SentEmailLog();
                    sendEmailEntity.Subject = emailDTO.EmailSubject;
                    sendEmailEntity.Body = emailDTO.EmailBodyHtml;
                    sendEmailEntity.Category = "RequstNotificationEmail";
                    sendEmailEntity.EmailTo = emailDTO.ToUserEmail;
                    sendEmailEntity.LoginConfirmationNum = userEmailinfo.LoginConfirmationNum;
                    sendEmailEntity.Hotel_Code = userEmailinfo.HOTEL_CODE;
                    sendEmailEntity.SentDate = DateTime.Now;
                    sendEmailEntity.SendStatus = "Success";
                    sendEmailEntity.Source = "WebSite -- Admin Site";
                    sentEmailLogRepo.Add(sendEmailEntity);

                    unitOfWork.Commit();
                }
            }
        }

        public string UpdateActiveYNByBusinessRuleID(int businessRuleID, string userName, int operation)
        {
            //Get businessRule
            var businessRule = plannerEventRepo.GetById(businessRuleID);

            if (businessRule == null)
            {
                return "404";
            }

            //Check if the hotelcode current user's access mapping
            if (!userAccountService.CheckUserHasPermissionByHotelCode(userName, businessRule.Hotel_Code))
            {
                //Current user doesn't have permission
                return "403";
            }

            int? Groupid = businessRule.Groupid;
            plannerEventService.UpdateUpgradeRuleByGroupID(Groupid, 0, "", operation);

            //businessRule.UpdateDate = DateTime.Now;
            //businessRule.ActiveYN = operation == 1 ? true : false;
            //plannerEventRepo.Update(businessRule);
            //unitOfWork.Commit();

            return string.Empty;

        }

        public byte[] GenerateRequestExcelBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {

            //For excel export, we need to export all records.
            //So set the pageindex and pagesize to -1
            pageInfo.StartIndex = -1;
            pageInfo.PageSize = -1;

            var list = GetRequestsBySearchCriteria(searchCriteria, pageInfo);

            //if (searchCriteria.HotelName == null)
            //{
            //    //Hotel List
            //    var hotelList = userAccountService.GetUserMappedHotels(searchCriteria.UserName);

            //    if (hotelList.Count > 1) {
            //        var secondHotelValue = hotelList[0].Value;
            //        int index = secondHotelValue.IndexOf('_');
            //        var secondDateFormat = secondHotelValue.Substring(index + 1);
            //        secondDateFormat = secondDateFormat.ToLower().Replace("m", "M");
            //        for (var i = 0; i < list.Count; i++)
            //        {
            //            list[i].DateFormat = secondDateFormat;
            //        }
            //    }
            //}
            //Get excel export list
            var excelList = mapper.Map<IList<RequestExportExcelModel>>(list);

            return excelList.ToExcelContent();

        }

        public byte[] GenerateBusinessRuleExcelBySearchCriteria(BusinessRuleSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {

            //For excel export, we need to export all records.
            //So set the pageindex and pagesize to -1
            pageInfo.StartIndex = -1;
            pageInfo.PageSize = -1;

            var list = GetBusinessRuleBySearchCriteria(searchCriteria, pageInfo);
            //if (searchCriteria.HotelName == null)
            //{
            //    //Hotel List
            //    var hotelList = userAccountService.GetUserMappedHotels(searchCriteria.UserName);

            //    if (hotelList.Count > 1)
            //    {
            //        var secondHotelValue = hotelList[0].Value;
            //        int index = secondHotelValue.IndexOf('_');
            //        var secondDateFormat = secondHotelValue.Substring(index + 1);
            //        secondDateFormat = secondDateFormat.ToLower().Replace("m", "M");
            //        for (var i = 0; i < list.Count; i++)
            //        {
            //            list[i].DateFormat = secondDateFormat;
            //        }
            //    }
            //}
            //Get excel export list
            var excelList = mapper.Map<IList<BusinessRuleExportExcelModel>>(list);

            return excelList.ToExcelContent();

        }

        public int CheckNewRequest(int interval)
        {
            DateTime dt = DateTime.Now.AddMilliseconds(-(Double)interval);
            var g = (from gpr in guestPlannerRepo.GetAll()
                     where (gpr.CreatedDate > dt && gpr.ConfirmStatus == false)
                     orderby gpr.LastModifiedDate descending
                     select gpr).ToList();
            return g.Count;
        }

        public bool CheckAllRequestsProcessed(int reqId, out GlobalEmailGuestDTO guestInfo)
        {
            var g = (from g1 in guestPlannerRepo.GetAll()
                     join g2 in guestPlannerRepo.GetAll() on g1.LoginConfirmationNum equals g2.LoginConfirmationNum
                     where g2.ID == reqId && g1.ConfirmStatus == false
                     select g1).ToList();
            if (null != g && g.Count > 0)
            {
                guestInfo = null;
                return false;
            }
            else
            {
                guestInfo = (from x in guestPlannerRepo.GetAll()
                             join r in fguestRepo.GetAll() on x.LoginConfirmationNum equals r.LoginConfirmationNum
                             where x.ID == reqId && x.ShowItinerary == "Y"
                             select new GlobalEmailGuestDTO()
                             {
                                 RequestID = x.ID,
                                 Email = r.Email,
                                 HotelCode = r.HOTEL_CODE,
                                 GuestName = r.LName + " " + r.FName

                             }).FirstOrDefault();
                if (null != guestInfo)
                {
                    return true;
                }
                return false;
            }
        }

        public string GetEmailTempate(string hotelCode, string firstName, string confirmNum, string loginConfirmNum, out List<LinkedResource> lstLinkResource)
        {
            LinkedResource res = null;
            lstLinkResource = new List<LinkedResource>();
            var gpList = guestPlannerRepo.GetAll().Where(p => p.LoginConfirmationNum == loginConfirmNum
                    && (p.Status == "Confirmed" || p.Status.Equals("Denied"))).OrderBy(p => p.Status).ToList();

            var languageID = GetLanguageID(gpList);
            var emailTempate = emailTemplateRepo.GetAll().Where(p => p.Hotel_Code == hotelCode && p.eMailType == "RequestConfirmEmail" && p.ActiveYN == true && p.languageID == languageID).FirstOrDefault();

            string emailBody = emailTempate.Template;

            if (!string.IsNullOrEmpty(emailBody))
            {
                var hotel = hotelRepo.GetAll().Where(p => p.Hotel_Code == hotelCode).FirstOrDefault();
                string hotelDomain = hotel.DomainName;
                emailBody = emailBody.Replace("|HEADERBGCOLOR|", GetHeaderBackgroundColor(hotelCode, hotel.BrandCode));
                emailBody = emailBody.Replace("|RESERVATION|", confirmNum);
                emailBody = emailBody.Replace("|FIRSTNAME|", firstName);
                emailBody = emailBody.Replace("|HOTELNAME|", hotel.Hotel_Name);
                emailBody = emailBody.Replace("|HOTELWEBSITE|", hotel.Hotel_WebSite);

                string defaultLogoPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\logo.png");
                string defaultHeaderImagePath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\header.jpg");

                var brandCss = brandCssRepo.GetAll().Where(p => p.IsActive == true && p.HotelCode == hotelCode).Select(p => p.BrandCss).FirstOrDefault();
                string logoPath = HttpContext.Current.Server.MapPath("~" + @"/PropertyFiles/Shared/Email/" + brandCss + "_logo.png");
                string headerImagePath = HttpContext.Current.Server.MapPath("~" + @"/PropertyFiles/" + hotel.ChainCode + "/" + hotel.BrandCode + "/" + hotel.Hotel_Code + "/_design/Hero.jpg");
                if (!System.IO.File.Exists(logoPath))
                {
                    logoPath = defaultLogoPath;
                }

                if (!System.IO.File.Exists(headerImagePath))
                {
                    headerImagePath = defaultHeaderImagePath;
                }

                emailBody = emailBody.Replace("|LOGOPATH|", GetEmbeddedImage(logoPath, out res));
                lstLinkResource.Add(res);
                emailBody = emailBody.Replace("|HEADERIMAGEPATH|", GetEmbeddedImage(headerImagePath, out res));
                lstLinkResource.Add(res);

                string roomDetailPlaceHolder = "";
                var roomDetailTemplate = emailTemplateRepo.GetAll().Where(p => p.Hotel_Code == hotelCode && p.eMailType == "EmailRoomDetail" && p.ActiveYN == true && p.languageID == languageID).FirstOrDefault();

                int firstConfirmRequestID = gpList.Where(p => p.Status == "Confirmed") == null ? 0 : gpList.Where(p => p.Status == "Confirmed").ToList().First().ID;
                int firstDenyRequestID = gpList.Where(p => p.Status == "Denied") == null ? 0 : gpList.Where(p => p.Status == "Denied").ToList().First().ID;

                foreach (var gp in gpList)
                {
                    var roomDetail = roomDetailTemplate != null ? roomDetailTemplate.Template : "";

                    if (gp.ID == firstConfirmRequestID)
                    {
                        roomDetail = roomDetail.Replace("|APPROVEDDISPLAYATTR|", "block");
                        roomDetail = roomDetail.Replace("|DENYDISPLAYATTR|", "none");
                    }
                    else if (gp.ID == firstDenyRequestID)
                    {
                        roomDetail = roomDetail.Replace("|APPROVEDDISPLAYATTR|", "none");
                        roomDetail = roomDetail.Replace("|DENYDISPLAYATTR|", "block");
                    }
                    else
                    {
                        roomDetail = roomDetail.Replace("|APPROVEDDISPLAYATTR|", "none");
                        roomDetail = roomDetail.Replace("|DENYDISPLAYATTR|", "none");
                    }

                    string upgradedRoomType = plannerEventRepo.GetAll().Where(p => p.ID.ToString() == gp.DescID).Select(p => p.RoomTypeCodeUpgrade).FirstOrDefault();
                    var roomCode = roomCodeRepo.GetAll().Where(p => p.RoomCode == upgradedRoomType && p.Hotel_Code == hotelCode && p.LanguageId == languageID).FirstOrDefault();
                    if (roomCode == null)
                    {
                        roomCode = roomCodeRepo.GetAll().Where(p => p.RoomCode == upgradedRoomType && p.Hotel_Code == hotelCode && p.LanguageId == 1).FirstOrDefault();
                    }

                    string roomImagePath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\image-none.png");
                    var roomImageURL = roomImageRepo.GetAll().Where(p => p.RoomCode == upgradedRoomType && p.HotelCode == hotelCode).Select(p => p.ImageUrl).ToList();
                    if (roomImageURL.Count() > 0)
                    {
                        string tempPath = HttpContext.Current.Server.MapPath("~" + @roomImageURL[0]);
                        if (System.IO.File.Exists(tempPath)) roomImagePath = tempPath;
                    }

                    string statusIconPath = "";
                    string requestStatus = "";
                    if (gp.Status == "Confirmed")
                    {
                        statusIconPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\icon-approved.png");
                        requestStatus = "Confirmed";
                    }
                    else
                    {
                        statusIconPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\icon-denied.png");
                        requestStatus = "Unavailable";
                    }

                    roomDetail = roomDetail.Replace("|ROOMIMAGEPATH|", GetEmbeddedImage(roomImagePath, out res));
                    lstLinkResource.Add(res);
                    roomDetail = roomDetail.Replace("|ROOMNAME|", roomCode == null ? "" : roomCode.RoomDescription);
                    roomDetail = roomDetail.Replace("|PRICE|", gp.USDPrice.ToString());
                    roomDetail = roomDetail.Replace("|PRICEINFORMATION|", roomCode.PriceDesc);
                    roomDetail = roomDetail.Replace("|STATUSICON|", GetEmbeddedImage(statusIconPath, out res));
                    lstLinkResource.Add(res);
                    roomDetail = roomDetail.Replace("|STATUS|", requestStatus);

                    roomDetailPlaceHolder += roomDetail;
                }

                var emailFooterTemplate = emailTemplateRepo.GetAll().Where(p => p.Hotel_Code == hotelCode && p.eMailType == "EmailFooter" && p.ActiveYN == true).FirstOrDefault();
                string emailFooter = emailFooterTemplate != null ? emailFooterTemplate.Template : "";
                if (!string.IsNullOrEmpty(emailFooter))
                {
                    string tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\footer-logo.png");
                    emailFooter = emailFooter.Replace("|FOOTERLOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\footer-logo.png");
                    emailFooter = emailFooter.Replace("|FOOTERLOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\social-facebook.png");
                    emailFooter = emailFooter.Replace("|SOCIALFACEBOOKIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\social-instagram.png");
                    emailFooter = emailFooter.Replace("|SOCIALINSTAGRAMIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\newadina3.png");
                    emailFooter = emailFooter.Replace("|NEWADINA3IMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\vibe-logo.png");
                    emailFooter = emailFooter.Replace("|VIBELOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\travelodge-logo.png");
                    emailFooter = emailFooter.Replace("|TRAVELODGELOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\rendevous-logo-footer.png");
                    emailFooter = emailFooter.Replace("|RENDEVOUSLOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);

                    tempImgPath = HttpContext.Current.Server.MapPath("~" + @"\PropertyFiles\Shared\Email\tfecolletion-logo.png");
                    emailFooter = emailFooter.Replace("|TFECOLLECTIONLOGOIMG|", GetEmbeddedImage(tempImgPath, out res));
                    lstLinkResource.Add(res);
                }

                emailBody = emailBody.Replace("|ROOMDETAIL|", roomDetailPlaceHolder);
                emailBody = emailBody.Replace("|FOOTER|", emailFooter);
            }

            return emailBody;
        }

        private string GetEmbeddedImage(String filePath, out LinkedResource res)
        {
            string result = "";
            res = null;
            if (System.IO.File.Exists(filePath))
            {
                res = new LinkedResource(filePath);
                res.ContentId = Guid.NewGuid().ToString();
                result = "cid:" + res.ContentId;
            }
            return result;
        }

        private string GetHeaderBackgroundColor(string hotelCode, string brandCode)
        {
            string brandCss = brandCssRepo.GetAll().Where(p => p.IsActive == true && p.HotelCode == hotelCode && p.BrandCode == brandCode).Select(p => p.BrandCss).FirstOrDefault();
            switch (brandCss)
            {
                case "HK_05":
                    return "#bd9c69";
                case "TC_06":
                    return "#ffffff";
                case "AS_07":
                    return "#004750";
                case "HK_04":
                    return "#000000";
                case "VH_02":
                    return "#00b5e2";
                case "TH_03":
                    return "#da291c";
                case "AA_01":
                    return "#b41f23";
                default:
                    return "#f1f1f1";
            }
        }

        private int GetLanguageID(List<GuestPlanner> lstGuestPlanner)
        {
            int languageID = 1;
            if (lstGuestPlanner != null)
            {
                string langId = "en-US";
                //if requests have multiple languages, then use the language of latest request
                if (lstGuestPlanner.Select(p => p.LangId).ToList().Distinct().ToList().Count() > 1)
                {
                    var gpList = lstGuestPlanner.OrderBy(p => p.LastModifiedDate);
                    langId = gpList.Last().LangId;
                }
                else
                {
                    langId = lstGuestPlanner.Select(p => p.LangId).First();
                }
                languageID = hotelLanguageRepo.GetAll().Where(p => p.CultureID == langId).FirstOrDefault().LanguageID;
            }
            return languageID;
        }
    }
}
