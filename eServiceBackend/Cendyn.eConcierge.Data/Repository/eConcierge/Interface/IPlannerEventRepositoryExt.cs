using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public partial interface IPlannerEventRepository : IRepository<PlannerEvent>
    {
        //List<PlannerEvent> GetRequestDesc_New(string hotelCode, string eventCategory, List<string> requestIDs);
        //List<PlannerEvent> GetPlannerEvents(List<string> requestIDs);
        IList<HotelCurrencyItem> GetHotelActiveCurrency(string hotelCode);
    }
}
