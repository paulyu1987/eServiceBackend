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
    public  partial class RateTypeRepository: EntityRepository<RateType,ConciergeEntities>,IRateTypeRepository
    {
        public IList<RateTypeSearchResultDTO> GetRateTypeBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo)
        {
            List<RateTypeSearchResultDTO> list = new List<RateTypeSearchResultDTO>();
            try
            {
                string query = @" select ID = r.ID, 
                    HotelCode= r.HotelCode,
                    RateTypeCode = r.RateTypeCode, 
                    RateTypeCodeDescription= r.RateTypeCodeDescription, 
                    IsActive = r.ActiveYN, 
                    InsertDate = r.InsertDate, 
                    UpdateDate= r.UpdateDate,
                    DateFormat = h.DateFormat 
               from RateType r with(nolock) join Hotels h with (nolock) on r.HotelCode = h.Hotel_Code ";

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
	                    Where hm.ActiveYN = 1 and hm.HotelCode = r.HotelCode and hm.ConciergeID = N'{0}') and ", searchCriteria.UserName);
                }

                if (!string.IsNullOrEmpty(subquery))
                {
                    subquery = subquery.Substring(0, subquery.Length - " and ".Length);
                    query += " and (" + subquery + ")";
                }
                list = this.DataContext.Database.SqlQuery<RateTypeSearchResultDTO>(query).ToList();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return list;
        }
    }
}
