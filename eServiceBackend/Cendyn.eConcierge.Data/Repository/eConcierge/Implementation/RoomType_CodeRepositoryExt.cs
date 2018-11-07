using Cendyn.eConcierge.Core.EntityFrameworkRepository;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Implementation
{
    public partial class RoomType_CodeRepository : EntityRepository<RoomType_Code, ConciergeEntities>, IRoomType_CodeRepository
    {
        public IList<RoomTypeSearchResultDTO> GetRateTypeBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            List<RoomTypeSearchResultDTO> list = new List<RoomTypeSearchResultDTO>();
            try
            {
                string query = @" select distinct id = r.ID, 
                    Hotel_Code = r.Hotel_Code,
                    RoomCode = r.RoomCode, 
                    RoomDescription= r.RoomDescription, 
                    RoomLongDescription= r.RoomLongDescription,
                    r.UpgradeType,-- = case when r.AddOnYN ='Y' then  'Add-on' else 'Room' end,
                    t.UpgradeTypeDisplayName,
                    AddOnYN= Case when r.UpgradeType like '%Room%' then 'Y' else 'N'end, --r.AddOnYN,
                    ImageYN= r.ImageYN,
                    TotalRoom=r.TotalRoom,
                    Threshold= r.Threshold,
                    PriceDesc = r.PriceDesc,
                    PerNightCharge=r.PerNightCharge,
                    IsActive = r.IsActive,
                    DateFormat = h.DateFormat 
                    from RoomType_Code r with(nolock)
                    INNER JOIN eUpgradeTransactionCode t WITH ( NOLOCK ) ON r.Hotel_Code = t.HotelCode AND r.UpgradeType = t.UpgradeType
                    join Hotels h with (nolock) on r.Hotel_Code = h.Hotel_Code
                    join Hotels_Languages hl with (nolock) on r.Hotel_Code = hl.hotel_code and r.languageid = hl.LanguageID and hl.sortorder = 1";
                var subquery = "";
                //Hotel Name
                if (!string.IsNullOrWhiteSpace(searchCriteria.HotelName))
                {
                    subquery += string.Format("h.Hotel_Code = '{0}' and ", searchCriteria.HotelName);
                }
                else
                {
                    //Based on user hotel mapping to get back hotel list, then only show those hotels
                    subquery += string.Format(@"exists (select HotelCode from ConciergeHotelAccessMapping hm with (nolock)
	                    Where hm.ActiveYN = 1 and hm.HotelCode = r.Hotel_Code and hm.ConciergeID = N'{0}') and ", searchCriteria.UserName);
                }

                if (!string.IsNullOrEmpty(subquery))
                {
                    subquery = subquery.Substring(0, subquery.Length - " and ".Length);
                    query += " and (" + subquery + ")";
                }
                list = this.DataContext.Database.SqlQuery<RoomTypeSearchResultDTO>(query).ToList();
                
                // add filter feature for the upgrade types page
                if (null != pageInfo)
                {
                    var filterValue = pageInfo.FilterValue.ToLower();
                    if (!string.IsNullOrWhiteSpace(filterValue) && !string.IsNullOrEmpty(filterValue))
                    {
                        list = list.Where(x =>
                                         (x.Hotel_Code??"").ToLower().Contains(filterValue) ||
                                         (x.RoomCode??"").ToLower().Contains(filterValue) ||
                                         (x.RoomDescription??"").ToLower().Contains(filterValue) ||
                                         (x.RoomLongDescription??"").ToLower().Contains(filterValue) ||
                                         (x.UpgradeTypeDisplayName??"").ToLower().Contains(filterValue) ||
                                         (x.ImageYN.ToLower()??"").Contains(filterValue) ||
                                         (x.PriceDesc??"").ToLower().Contains(filterValue)
                                         ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return list;
        }

        public RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode, int languageID)
        {
            RoomType_Code ret = new RoomType_Code();
            languageID = languageID < 1 ? 1 : languageID;
            roomCode = string.IsNullOrWhiteSpace(roomCode) ? string.Empty : roomCode;
            try
            {
                string query = @"select c.ID,c.Hotel_Code,c.RoomCode,c.RoomType,c.[Image],c.InsertDate,c.UpdateDate
	,c.NonRoom,c.PerNightCharge,c.TotalRoom,c.Threshold,c.IsActive,c.AddOnYN,c.ImageYN,c.UpgradeType,c.PackageCode
	,LanguageId=isnull(d.LanguageId,c.LanguageId)
	,RoomDescription=isnull(d.RoomDescription,c.RoomDescription)
	,RoomLongDescription=isnull(d.RoomLongDescription,c.RoomLongDescription)
	,PriceDesc=isnull(d.PriceDesc,c.PriceDesc)
from RoomType_Code c with (nolock)
left join (
		select Hotel_Code,RoomCode,RoomDescription,RoomLongDescription,PriceDesc,cd.LanguageId
		from RoomType_Code cd with (nolock) 
		inner join Languages l with (nolock) on cd.LanguageId= l.LanguageId and l.Active=1
		where cd.LanguageId = @LanguageId
		) d on c.Hotel_Code=d.Hotel_Code and c.RoomCode=d.RoomCode
where c.LanguageId=1 and c.Hotel_Code = @hotelCode and c.RoomCode = @RoomCode";

                ret = this.DataContext.Database.SqlQuery<RoomType_Code>(query,
                    new SqlParameter("@hotelCode", hotelCode),
                    new SqlParameter("@LanguageId", languageID),
                    new SqlParameter("@RoomCode", roomCode)
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return ret;
        }

        public bool IsStoredProcedureExists (string spname)
        {
            bool isExists = false;
            try
            {
                string query = " SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'" + spname + "') AND type IN(N'P', N'PC')";
                var ret = this.DataContext.Database.SqlQuery<int>(query).FirstOrDefault();
                if (ret == 1) isExists = true;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return isExists;
        }
    }
}
