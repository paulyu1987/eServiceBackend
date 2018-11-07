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
    public partial class EUpgradeRequestRepository : EntityRepository<eUpgradeRequest, ConciergeEntities>, IEUpgradeRequestRepository
    {
        public IList<ReportSearchResultDTO> GetReportBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            List<ReportSearchResultDTO> list = new List<ReportSearchResultDTO>();
            try
            {
                string query = @"select HotelCode = e.HotelCode,
        GuestName = f.FName + ' ' + f.LName,
       ArrivalDate =  f.ArrivalDate,
       FirstName = f.FName,
       LastName = f.LName,
       Email = f.Email,
       UpgradeCost = e.UpgradeCost,
       BookedRoomType = e.BookedRoomType,
       UpgradeRoomType = e.UpgradeRoomType,
       UpgradeStatus = e.UpgradeStatus,
       ConfirmedDate = e.InsertDate,
       NightsOfStay = isnull(f.NightsOfStay,0),
       --ReservationID = f.ConfirmationNum,
       ReservationID = case when (h.CentralResNumYN = 1 and isNull(f.CentralResNum, '') <> '' and f.CentralResNum !='''') then f.CentralResNum else f.ConfirmationNum end,
       ChargePerNight = rt.PerNightCharge,
       IncTotalAmountForStay = case when rt.PerNightCharge='Y' then e.UpgradeCost * isnull(f.NightsOfStay,0) else e.UpgradeCost end,
       DateFormat = h.DateFormat,
       CurrencySymbol = lc.Symbol
--select *
from eUpgradeRequest e with (nolock)
inner join hotels h with (nolock) on e.HotelCode=h.Hotel_Code
inner join Hotels_Languages hl with (nolock) on h.Hotel_Code = hl.Hotel_Code
inner join dbo.L_Currency lc with (nolock) on hl.CurrencyCode = lc.Code
--inner join CurrencyConvert c with (nolock) on h.Hotel_Code = c.Hotel_Code and h.LocalCurrencyCode = c.Code
inner join Fguest f with (nolock) on e.loginConfirmationNum=f.LoginConfirmationNum 
inner join RoomType_Code rt with (nolock) on e.HotelCode = rt.Hotel_Code and e.UpgradeRoomType = rt.RoomCode
where hl.LanguageID=1
-- 1=1
-- and e.id = (select max(id) from eUpgradeRequest e2 with (nolock) 
--  where e.loginConfirmationNum=e2.loginConfirmationNum and e.HotelCode=e2.HotelCode 
--    and e.BookedRoomType=e2.BookedRoomType and e.UpgradeRoomType=e2.UpgradeRoomType)
";

                var subquery = "";

                //Hotel Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
                {
                    subquery += string.Format("e.HotelCode = '{0}' and ", searchCriteria.HotelName);
                }
                else
                {
                    //Based on user hotel mapping to get back hotel list, then only show those hotels
                    subquery += string.Format(@"exists (select HotelCode from ConciergeHotelAccessMapping hm with (nolock)
	Where hm.ActiveYN = 1 and hm.HotelCode = e.HOTELCODE and hm.ConciergeID = N'{0}') and ", searchCriteria.UserName);
                }


                //Email
                if (!string.IsNullOrWhiteSpace(searchCriteria.Email))
                {
                    subquery += string.Format("f.Email like '%{0}%' and ", searchCriteria.Email);
                }

                //First Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.FirstName))
                {
                    subquery += string.Format("f.FName like N'%{0}%' and ", searchCriteria.FirstName.Replace("'", "''"));
                }
                //Last Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.LastName))
                {
                    subquery += string.Format("f.LName like N'%{0}%' and ", searchCriteria.LastName.Replace("'","''"));
                }

                //Booked Room Type
                if (!string.IsNullOrWhiteSpace(searchCriteria.BookedRoomType))
                {
                    subquery += string.Format("e.BookedRoomType = '{0}' and ", searchCriteria.BookedRoomType);
                }
                //Request Status
                if (!string.IsNullOrWhiteSpace(searchCriteria.ReportUpgradeStatus))
                {
                    subquery += string.Format("e.UpgradeStatus = '{0}' and ", searchCriteria.ReportUpgradeStatus);
                }

                //Confirmed Date
                if (searchCriteria.ConfirmedDate != null)
                {
                    var startDate = searchCriteria.ConfirmedDate.Value.Date;
                    var endDate = searchCriteria.ConfirmedDate.Value.Date.AddDays(1);
                    subquery += string.Format("e.InsertDate >= '{0}' and e.InsertDate < '{1}' and ", startDate.ToString("MM/dd/yyyy"), endDate.ToString("MM/dd/yyyy"));
                }

                //Arrival Date From
                if (searchCriteria.ArrivalDateFrom != null)
                {
                    var date = searchCriteria.ArrivalDateFrom.Value.Date;
                    subquery += string.Format("f.ArrivalDate >= '{0}' and ", date.ToString("MM/dd/yyyy"));
                }

                //Arrival Date To
                if (searchCriteria.ArrivalDateTo != null)
                {
                    var date = searchCriteria.ArrivalDateTo.Value.Date.AddDays(1);
                    subquery += string.Format("f.ArrivalDate < '{0}' and ", date.ToString("MM/dd/yyyy"));
                }

                if (!string.IsNullOrEmpty(subquery))
                {
                    subquery = subquery.Substring(0, subquery.Length - " and ".Length);
                    query += " and (" + subquery + ")";
                }
                list = this.DataContext.Database.SqlQuery<ReportSearchResultDTO>(query).ToList();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return list;
        }
        public IList<EUpgradeRequestDTO> GetEUpgradeRequestConfirmed()
        {
            try
            {
                return (from r in this.DataContext.eUpgradeRequests
                             from f in this.DataContext.FGuests
                             where r.loginConfirmationNum == f.LoginConfirmationNum && r.UpgradeStatus == "Confirmed" // && OXIDirty = false; // Check if has been sent to PMS
                             select new EUpgradeRequestDTO
                             {
                                 ID = r.ID,
                                 loginConfirmationNum = r.loginConfirmationNum,
                                 HotelCode = r.HotelCode,
                                 BookedRoomType = r.BookedRoomType,
                                 UpgradeRoomType = r.UpgradeRoomType,
                                 UpgradeCost = r.UpgradeCost,
                                 UpgradeStatus = r.UpgradeStatus,
                                 DecisionPerson = r.DecisionPerson,
                                 OXIDateSent = r.OXIDateSent,
                                 OXIMessageID = r.OXIMessageID,
                                 OXIDirty = r.OXIDirty,
                                 OXITransactioncode = r.OXITransactioncode,
                                 StartDate = f.ArrivalDate,
                                 EndDate = f.DepartureDate,
                             }).ToList();
            }
            catch (Exception ex)
            {
                //TODO Handle exception
                return null;
            }
        }

        public void UpDateeUpgradeRequest(string LoginConfirmationNum, string HotelCode, string BookedRoomType, string UpgradeRoomType, string UpgradeStatus, string DecisionPerson)
        {
            string spName = "spUpdateeUpgradeRequest";
            var pList = new Dictionary<string, object>(){
                {"@LoginConfirmationNum",LoginConfirmationNum},
                {"@HotelCode",HotelCode},
                {"@BookedRoomType",BookedRoomType},
                {"@UpgradeRoomType",UpgradeRoomType},
                {"@UpgradeStatus",UpgradeStatus},
                {"@DecisionPerson",DecisionPerson}
            };

            this.ExecuteStoredProcedure<eUpgradeOptionDTO>(spName, pList);
        }
    }
}
