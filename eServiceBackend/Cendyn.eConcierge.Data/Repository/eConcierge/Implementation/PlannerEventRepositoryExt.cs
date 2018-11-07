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
    public partial class PlannerEventRepository : EntityRepository<PlannerEvent, ConciergeEntities>, IPlannerEventRepository
    {

        //public List<PlannerEvent> GetRequestDesc_New(string hotelCode, string eventCategory, List<string> requestIDs)
        //{
        //    if (string.IsNullOrEmpty(hotelCode) || string.IsNullOrEmpty(eventCategory) || (null == requestIDs || requestIDs.Count <= 0))
        //        return null;

        //    var query = (from p in this.DataContext.PlannerEvents
        //                 where p.Hotel_Code == hotelCode && p.ActiveYN == true
        //                 && p.EventCategory == eventCategory && requestIDs.Contains(p.ID.ToString()) && p.languageID == 1
        //                 select p);

        //    return query.ToList();

        //}

        //public List<PlannerEvent> GetPlannerEvents(List<string> requestIDs)
        //{
        //    if ( (null == requestIDs || requestIDs.Count <= 0))
        //        return null;

        //    var query = (from p in this.DataContext.PlannerEvents
        //                 where requestIDs.Contains(p.ID.ToString()) && p.ActiveYN ==true
        //                 select p);

        //    return query.ToList();
        //}
        public IList<HotelCurrencyItem> GetHotelActiveCurrency(string hotelCode)
        {
            List<HotelCurrencyItem> list = new List<HotelCurrencyItem>();
            try
            {
                list = (from v in this.DataContext.Hotels_Currency
                        join c in this.DataContext.L_Currency on v.Code equals c.Code
                        where v.Hotel_Code == hotelCode && v.Active == true
                        select new HotelCurrencyItem {
                            HotelCurrencyID = v.ID,
                            Hotel_Code = v.Hotel_Code,
                            CurrencyName = c.Name,
                            CurrencyCode = v.Code,
                            CurrencySymbol = c.Symbol,
                            USD_to_Rate = v.USD_to_Rate,
                            CurrencySymbolPosition = v.SymbolPosition,
                            CurrencyCultureID = c.CultureID,
                            InsertDate = v.InsertDate,
                            UpdateDate = v.UpdateDate
                        }).ToList();
            }
            catch (Exception ex)
            {
                var msg = ex.ToString();
            }
            return list;
        }
    }
}
