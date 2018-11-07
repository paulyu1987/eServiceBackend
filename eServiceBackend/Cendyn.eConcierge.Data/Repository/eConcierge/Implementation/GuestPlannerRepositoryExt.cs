using Cendyn.eConcierge.Core.EntityFrameworkRepository;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Implementation
{
    public partial class GuestPlannerRepository : EntityRepository<GuestPlanner, ConciergeEntities>, IGuestPlannerRepository
    {
        public bool UpdatRequestIDs(List<int> guestPlannerIDList)
        {
            try
            {
                var query = (from g in this.DataContext.GuestPlanners
                             where guestPlannerIDList.Contains(g.ID)
                             select g);

                List<GuestPlanner> guestPlanerList = query.ToList();

                if (null != guestPlanerList && guestPlanerList.Count() > 0)
                {
                    string lookupID = guestPlanerList[0].LookupID.ToString();
                    foreach (var item in guestPlanerList)
                    {
                        item.RequestID = lookupID;
                        Update(item);
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
            string requestID = "";

            try
            {
                var query = (from g in this.DataContext.GuestPlanners
                             where guestPlannerIDList.Contains(g.ID)
                             select g);

                List<GuestPlanner> guestPlanerList = query.ToList();

                if (null != guestPlanerList && guestPlanerList.Count() > 0)
                {
                    requestID = guestPlanerList[0].RequestID.ToString();
                }
                return requestID;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return "";
            }
        }

        public IList<RequestSearchResultDTO> GetRequestsBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            List<RequestSearchResultDTO> list = new List<RequestSearchResultDTO>();
            try
            {
                string query = @"select HotelCode = r.HOTEL_CODE,
       CheckInventoryYN = Convert(bit, case when (isNull(h.CenResServiceInterface, '') <> '' and isNull(h.CenResServiceUrl, '') <> '') then 1 else 0 end), 
       AutoRequestProcessYN = h.AutoRequestProcessYN,
       AutoConfirmEmailYN = h.AutoConfirmEmailYN,
       ArrivalDate = r.ArrivalDate,
       BookedRoomTypeCode = pe.RoomTypeCodeBooked,
       LastModifiedDate= gpr.LastModifiedDate,
       DateSubmitted = gpr.CreatedDate,
       Email = r.Email,
       FirstName = r.FName,
       LastName = r.LName,
       VIPCode = r.VIPCode,
       MembershipID = r.MembershipID,
       DepartureDate = r.DepartureDate,
       NumberOfNights = r.NightsOfStay,
       RequestID = gpr.ID,
       RequestStatus = gpr.Status,
       SendEmailYN = gpr.ShowItinerary,
       Reservation = case when (h.CentralResNumYN = 1 and isNull(r.CentralResNum, '') <> '' and r.CentralResNum !='''') then r.CentralResNum else r.ConfirmationNum end,
       UpgradeCost = gpr.USDPrice,
       UpgradeRoomTypeCode = pe.RoomTypeCodeUpgrade,
       ConfirmStatus = gpr.ConfirmStatus,
       ShowSendEmail = Convert(bit,case when (select count(1) from GuestPlanner with (nolock) where LoginConfirmationNum = gpr.LoginConfirmationNum and ConfirmStatus = 0) > 0 then 0 else 1 end),
       TotalRooms = rc.TotalRoom,
       Threshold = rc.Threshold,
       LastEmailSentDate = gpr.LastConfirmEmailSentDate,
       ChargePerNight = rc.PerNightCharge,
       IncTotalAmountForStay = case when rc.PerNightCharge = 'Y' then gpr.USDPrice * isnull(r.NightsOfStay,0) else gpr.USDPrice end,
       Committed = 0,
       LifetimeSpend = 0,
       DateFormat = h.DateFormat,
       /*
       CurrencySymbol = c.Symbol,
       */
       CurrencySymbol =  lc.Symbol,
       PreviousRequests = (select count(1) from GuestPlanner g with (nolock) 
							where g.Event=gpr.Event
							and exists (
							select * from FGuest f1 with (nolock)
							where exists (
								select * from FGuest f2 with (nolock) 
								where f1.LoginConfirmationNum = f2.LoginConfirmationNum and f2.LoginConfirmationNum=gpr.LoginConfirmationNum)
							and g.LoginConfirmationNum =f1.LoginConfirmationNum and g.Hotel_Code=f1.HOTEL_CODE )),
	   ConfirmedRequests = (select sum(Convert(int,ConfirmStatus)) from GuestPlanner g with (nolock) 
							where g.Event=gpr.Event 
							and exists (
							select * from FGuest f1 with (nolock)
							where exists (
								select * from FGuest f2 with (nolock) 
								where f1.LoginConfirmationNum = f2.LoginConfirmationNum and f2.LoginConfirmationNum=gpr.LoginConfirmationNum)
							and g.LoginConfirmationNum =f1.LoginConfirmationNum and g.Hotel_Code=f1.HOTEL_CODE ))
--select * 
from GuestPlanner gpr with (nolock)
inner join Hotels h with (nolock) on gpr.Hotel_Code = h.Hotel_Code
inner join Hotels_Languages hl with (nolock) on h.Hotel_Code = hl.Hotel_Code
inner join dbo.L_Currency lc with (nolock) on hl.CurrencyCode = lc.Code
/*
inner join CurrencyConvert c with (nolock) on h.Hotel_Code = c.Hotel_Code and h.LocalCurrencyCode = c.Code
*/
inner join FGuest r with (nolock) on gpr.LoginConfirmationNum = r.LoginConfirmationNum
inner join PlannerEvent pe with (nolock) on gpr.DescID = pe.ID
inner join RoomType_Code rc with (nolock) on pe.Hotel_Code = rc.Hotel_Code and pe.RoomTypeCodeUpgrade = rc.RoomCode and rc.LanguageID=1
where gpr.Event='RoomUpgrade' and hl.LanguageID=1";

                var subquery = "";
                //if (searchCriteria.HotelName)
                //{

                //}
                //Hotel Name
                //Get all requests based on seach criteria, and map returned DTO to a view model

                if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
                {
                    subquery += string.Format("h.Hotel_Code = '{0}' and ", searchCriteria.HotelName);
                }
                else
                {

                    //Based on user hotel mapping to get back hotel list, then only show those hotels
                    subquery += string.Format(@"exists (select HotelCode from ConciergeHotelAccessMapping hm with (nolock)
	Where hm.ActiveYN = 1 and hm.HotelCode = r.HOTEL_CODE and hm.ConciergeID = N'{0}') and ", searchCriteria.UserName);
                }

                //DateRequest
                if (searchCriteria.DateRequested != null)
                {
                    var startDate = searchCriteria.DateRequested.Value.Date;
                    var endDate = searchCriteria.DateRequested.Value.Date.AddDays(1);
                    subquery += string.Format("gpr.CreatedDate >= '{0}' and gpr.CreatedDate < '{1}' and ", startDate.ToString("MM/dd/yyyy"), endDate.ToString("MM/dd/yyyy"));
                }

                //Arrival Date From
                if (searchCriteria.ArrivalDateFrom != null)
                {
                    var date = searchCriteria.ArrivalDateFrom.Value.Date;
                    subquery += string.Format("r.ArrivalDate >= '{0}' and ", date.ToString("MM/dd/yyyy"));
                }

                //Arrival Date To
                if (searchCriteria.ArrivalDateTo != null)
                {
                    var date = searchCriteria.ArrivalDateTo.Value.Date.AddDays(1);
                    subquery += string.Format("r.ArrivalDate < '{0}' and ", date.ToString("MM/dd/yyyy"));
                }

                //FirstName
                if (!string.IsNullOrWhiteSpace(searchCriteria.FirstName))
                {
                    subquery += string.Format("r.FName like N'%{0}%' and ", searchCriteria.FirstName.Replace("'", "''"));
                }

                //Last name
                if (!string.IsNullOrWhiteSpace(searchCriteria.LastName))
                {
                    subquery += string.Format("r.LName like N'%{0}%' and ", searchCriteria.LastName.Replace("'", "''"));
                }

                //Booked RoomType
                if (!string.IsNullOrWhiteSpace(searchCriteria.BookedRoomType))
                {
                    subquery += string.Format("r.RoomType = '{0}' and ", searchCriteria.BookedRoomType);
                }

                //Request Status
                if (!string.IsNullOrWhiteSpace(searchCriteria.RequestStatus))
                {
                    if (searchCriteria.RequestStatus.Equals("Lapsed"))
                    {
                        subquery += string.Format("gpr.Status = '{0}' and ", "Requested");
                        subquery += string.Format("r.ArrivalDate < '{0}' and ", DateTime.Today.ToString("MM/dd/yyyy"));
                    }
                    else if(searchCriteria.RequestStatus.Equals("Requested"))
                    {
                        subquery += string.Format("gpr.Status = '{0}' and ", searchCriteria.RequestStatus);
                        subquery += string.Format("r.ArrivalDate >= '{0}' and ", DateTime.Today.ToString("MM/dd/yyyy"));
                    }
                    else
                    {
                        subquery += string.Format("gpr.Status = '{0}' and ", searchCriteria.RequestStatus);

                    }
                }

                //Email
                if (!string.IsNullOrWhiteSpace(searchCriteria.Email))
                {
                    subquery += string.Format("r.Email like N'%{0}%' and ", searchCriteria.Email);
                }

                if (!string.IsNullOrEmpty(subquery))
                {
                    subquery = subquery.Substring(0, subquery.Length - " and ".Length);
                    query += " and (" + subquery + ")";
                }
                list = this.DataContext.Database.SqlQuery<RequestSearchResultDTO>(query).ToList();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return list;
        }
    }
}
